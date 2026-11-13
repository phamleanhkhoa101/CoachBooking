package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class RoleControllerTest {

  private RoleRepository roleRepository;
  private AccountRepository accountRepository;
  private RoleController controller;

  @BeforeEach
  void setUp() {
    roleRepository = mock(RoleRepository.class);
    accountRepository = mock(AccountRepository.class);
    controller = new RoleController(roleRepository, accountRepository);
  }

  @Test
  void listAndGetSuccess() {
    Role r = new Role();
    r.setRoleId(1);
    r.setRoleName("Admin");
    when(roleRepository.findAll()).thenReturn(List.of(r));
    when(roleRepository.findById(1)).thenReturn(Optional.of(r));

    assertEquals(1, controller.list().size());
    assertEquals("Admin", controller.get(1).getRoleName());

    when(roleRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createUpdateAndDeleteSuccess() {
    RoleController.Request req = new RoleController.Request("Staff", "Staff role");
    when(roleRepository.findByRoleName("Staff")).thenReturn(Optional.empty());
    when(roleRepository.save(any(Role.class))).thenAnswer(i -> i.getArgument(0));

    Role created = controller.create(req);
    assertNotNull(created);
    assertEquals("Staff", created.getRoleName());

    // duplicate role name
    when(roleRepository.findByRoleName("Staff")).thenReturn(Optional.of(created));
    ApiException dup = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.CONFLICT, dup.getStatus());

    // update
    when(roleRepository.findById(1)).thenReturn(Optional.of(created));
    when(roleRepository.findByRoleName("StaffNew")).thenReturn(Optional.empty());
    RoleController.Request updateReq = new RoleController.Request("StaffNew", "New desc");
    Role updated = controller.update(1, updateReq);
    assertEquals("StaffNew", updated.getRoleName());

    // delete success
    when(accountRepository.existsByRoleId(1)).thenReturn(false);
    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa quyền thành công.", res.get("message"));

    // delete conflict if assigned
    when(accountRepository.existsByRoleId(1)).thenReturn(true);
    ApiException assignedConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, assignedConflict.getStatus());
  }
}
