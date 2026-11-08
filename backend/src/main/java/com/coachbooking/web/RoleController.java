package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/roles", "/api/quyen"})
@PreAuthorize("hasRole('Admin')")
public class RoleController {

  private final RoleRepository roleRepository;
  private final AccountRepository accountRepository;

  public RoleController(RoleRepository roleRepository, AccountRepository accountRepository) {
    this.roleRepository = roleRepository;
    this.accountRepository = accountRepository;
  }

  @GetMapping
  public List<Role> list() {
    return roleRepository.findAll();
  }

  @GetMapping("/{id}")
  public Role get(@PathVariable Integer id) {
    return roleRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Quyền không tồn tại."));
  }

  @PostMapping
  public Role create(@Valid @RequestBody Request request) {
    if (roleRepository.findByRoleName(request.roleName().trim()).isPresent()) {
      throw ApiException.conflict("Tên quyền đã tồn tại.");
    }
    Role role = new Role();
    apply(role, request);
    return roleRepository.save(role);
  }

  @PutMapping("/{id}")
  public Role update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Role role = get(id);
    roleRepository
        .findByRoleName(request.roleName().trim())
        .filter(other -> !other.getRoleId().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Tên quyền đã tồn tại.");
            });
    apply(role, request);
    return roleRepository.save(role);
  }

  @DeleteMapping("/{id}")
  public Map<String, String> delete(@PathVariable Integer id) {
    Role role = get(id);
    if (accountRepository.existsByRoleId(id)) {
      throw ApiException.conflict("Không thể xóa quyền đang được gán cho tài khoản.");
    }
    roleRepository.delete(role);
    return Map.of("message", "Xóa quyền thành công.");
  }

  private void apply(Role role, Request request) {
    role.setRoleName(request.roleName().trim());
    role.setDescription(request.description());
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên quyền.") @JsonAlias({"tenQuyen", "roleName"})
          String roleName,
      @JsonAlias({"moTa", "description"}) String description) {}
}
