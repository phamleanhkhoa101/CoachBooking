import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import ts from 'typescript';

const source = await readFile(new URL('../src/api/client.ts', import.meta.url), 'utf8');
const js = ts.transpileModule(source, { compilerOptions: { module: ts.ModuleKind.ESNext, target: ts.ScriptTarget.ES2020 } }).outputText;
let serial = 0;
const session = (name='customer') => ({ token: `header.${btoa(JSON.stringify({exp:Date.now()/1000+900}))}.${name}`, user:{username:name,role:'KhachHang'}, expiresIn:900 });
function response(data, status=200, expired=false) {
  return new Response(status===204 ? null : JSON.stringify(data), {status, headers: expired ? {'X-Auth-Expired':'1'} : {}});
}
const channels = new Set();
class TestChannel {
  constructor(){channels.add(this);}
  addEventListener(_event, listener){this.listener=listener;}
  postMessage(data){for(const peer of channels)if(peer!==this)queueMicrotask(()=>peer.listener?.({data:structuredClone(data)}));}
}
async function client(fetcher, locks=true, broadcast=false) {
  const storage = new Map([['coachbooking_token','legacy-token']]);
  globalThis.localStorage = {removeItem:key=>storage.delete(key),setItem:()=>assert.fail('must not persist access token')};
  globalThis.document={visibilityState:'visible'};
  globalThis.window={location:{pathname:'/login',replace:()=>assert.fail('unexpected redirect')}};
  globalThis.BroadcastChannel=broadcast?TestChannel:undefined;
  let tail=Promise.resolve();
  Object.defineProperty(globalThis,'navigator',{configurable:true,value:{locks:locks?{request:(_name,action)=>{const next=tail.then(action);tail=next.catch(()=>{});return next;}}:undefined}});
  globalThis.fetch=fetcher;
  const api=await import(`data:text/javascript;base64,${Buffer.from(js+`\n// ${serial++}`).toString('base64')}`);
  assert.equal(storage.size,0); return api;
}

test('login carries rememberMe and keeps access token only in memory', async()=>{
  let sent;
  const data=session(); const api=await client(async(url,options)=>{sent={url,options};return response(data);});
  await api.loginSession('customer','password','user',true);
  assert.equal(sent.url,'/api/auth/user/login'); assert.equal(JSON.parse(sent.options.body).rememberMe,true);
  assert.equal(sent.options.headers['X-Auth-Request'],'1'); assert.equal(sent.options.credentials,'same-origin');
  assert.equal(api.getToken(),data.token);
});
test('parallel requests share one refresh and retry only filter rejections', async()=>{
  let refreshes=0,requests=0; const fresh=session();
  const api=await client(async(url,options)=>{
    if(url==='/api/auth/refresh'){refreshes++;return response(fresh);}
    requests++; return options.headers.get('Authorization')===`Bearer ${fresh.token}`?response({ok:true}):response({},401,true);
  });
  const results=await Promise.all([api.apiRequest('/protected'),api.apiRequest('/protected')]);
  assert.equal(refreshes,1); assert.equal(requests,4); assert.deepEqual(results,[{ok:true},{ok:true}]);
});
test('403 and business 401 never refresh or retry mutations', async()=>{
  for(const status of [401,403]) {
    let calls=0; const api=await client(async()=>{calls++;return response({message:'denied'},status);});
    await assert.rejects(api.apiRequest('/booking',{method:'POST',body:'{}'}),e=>e.status===status);
    assert.equal(calls,1);
  }
});
test('refresh network and server errors retain the current session', async()=>{
  for(const status of [0,500]) {
    const data=session();
    const api=await client(async(url)=>{
      if(url.endsWith('/login'))return response(data);
      if(status===0)throw new TypeError('offline'); return response({},status);
    });
    await api.loginSession('customer','password','user',false);
    await assert.rejects(api.refreshSession(data.token),e=>e.status===status);
    assert.equal(api.getToken(),data.token);
  }
});
test('logout waits for server and failed logout keeps session for retry', async()=>{
  const data=session(); let fail=true;
  const api=await client(async(url)=>url.endsWith('/login')?response(data):fail?response({},500):response(null,204));
  await api.loginSession('customer','password','user',false);
  await assert.rejects(api.logoutSession()); assert.equal(api.getToken(),data.token);
  fail=false; await api.logoutSession(); assert.equal(api.getToken(),null);
});
test('logout racing refresh cannot restore the session', async()=>{
  let started;const startedPromise=new Promise(resolve=>{started=resolve;});let finish;
  const api=await client(async(url)=>{
    if(url.endsWith('/refresh')){started();await new Promise(resolve=>{finish=resolve;});return response(session());}
    return response(null,204);
  });
  const pending=api.refreshSession(); await startedPromise;
  const loggedOut=api.logoutSession(); finish();
  await assert.rejects(pending,e=>e.status===401); await loggedOut; assert.equal(api.getToken(),null);
});
test('hidden background requests do not extend inactivity window', async()=>{
  let calls=0;const api=await client(async()=>{calls++;return response({},401,true);});document.visibilityState='hidden';
  await assert.rejects(api.apiRequest('/protected'),e=>e.status===401);assert.equal(calls,1);
});
test('unsupported browser fails safely instead of concurrent rotation', async()=>{
  const api=await client(async()=>assert.fail('must not refresh without exclusion'),false);
  await assert.rejects(api.refreshSession(),e=>e.status===0);
});
test('two tabs serialize refresh, share the winner token and propagate logout', async()=>{
  channels.clear();let refreshes=0;const old=session('old'),fresh=session('fresh');
  const fetcher=async(url)=>{
    if(url.endsWith('/login'))return response(old);
    if(url.endsWith('/logout'))return response(null,204);
    refreshes++;return response(fresh);
  };
  const first=await client(fetcher,true,true);await first.loginSession('customer','password','user',true);
  const second=await client(fetcher,true,true);
  await Promise.all([first.refreshSession(old.token),second.refreshSession(old.token)]);
  assert.equal(refreshes,1);assert.equal(first.getToken(),fresh.token);assert.equal(second.getToken(),fresh.token);
  await second.logoutSession();await new Promise(resolve=>setImmediate(resolve));
  assert.equal(first.getToken(),null);assert.equal(second.getToken(),null);channels.clear();
});
