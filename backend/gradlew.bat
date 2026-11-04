@rem Gradle wrapper bootstrap for Windows.
@echo off
setlocal
set "DIRNAME=%~dp0"
if "%DIRNAME%"=="" set "DIRNAME=."
set "WRAPPER_JAR=%DIRNAME%gradle\wrapper\gradle-wrapper.jar"

if not exist "%WRAPPER_JAR%" (
    echo Khong tim thay gradle\wrapper\gradle-wrapper.jar.
    echo Tao mot lan bang Docker:
    echo   docker run --rm -v "%CD%":/home/gradle/project -w /home/gradle/project gradle:9.1.0-jdk25 gradle wrapper --gradle-version 9.1.0
    exit /b 1
)

if defined JAVA_HOME (
    set "JAVACMD=%JAVA_HOME%\bin\java.exe"
) else (
    set "JAVACMD=java"
)

"%JAVACMD%" %DEFAULT_JVM_OPTS% %JAVA_OPTS% %GRADLE_OPTS% "-Dorg.gradle.appname=gradlew" -classpath "%WRAPPER_JAR%" org.gradle.wrapper.GradleWrapperMain %*
endlocal
