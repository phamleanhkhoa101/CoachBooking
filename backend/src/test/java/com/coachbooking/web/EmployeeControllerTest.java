package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Employee;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.InvoiceRepository;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class EmployeeControllerTest {

  private EmployeeRepository employeeRepository;
  private InvoiceRepository invoiceRepository;
  private AccountRepository accountRepository;
  private EmployeeController controller;

  @BeforeEach
  void setUp() {
    employeeRepository = mock(EmployeeRepository.class);
    invoiceRepository = mock(InvoiceRepository.class);
    accountRepository = mock(AccountRepository.class);
    controller = new EmployeeController(employeeRepository, invoiceRepository, accountRepository);
  }

  @Test
  void listAndGetSuccess() {
    Employee emp = new Employee();
    emp.setEmployeeId(1);
    emp.setFullName("Nguyen Van A");
    when(employeeRepository.findAll()).thenReturn(List.of(emp));
    when(employeeRepository.findById(1)).thenReturn(Optional.of(emp));

    assertEquals(1, controller.list().size());
    assertEquals(1, controller.get(1).getEmployeeId());

    when(employeeRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndValidation() {
    EmployeeController.Request req =
        new EmployeeController.Request(
            "Nguyen Van A",
            LocalDate.now().minusYears(25),
            "123 Ho Chi Minh",
            "0901234567",
            true,
            true,
            10);

    when(employeeRepository.existsByPhone("0901234567")).thenReturn(false);
    when(accountRepository.existsById(10)).thenReturn(true);
    when(employeeRepository.findByAccountId(10)).thenReturn(Optional.empty());
    when(employeeRepository.save(any(Employee.class))).thenAnswer(i -> i.getArgument(0));

    Employee created = controller.create(req);
    assertNotNull(created);
    assertEquals("Nguyen Van A", created.getFullName());
    assertEquals("0901234567", created.getPhone());

    // Phone exists
    when(employeeRepository.existsByPhone("0901234567")).thenReturn(true);
    ApiException phoneEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.CONFLICT, phoneEx.getStatus());

    // Under 18 years old
    when(employeeRepository.existsByPhone("0901234567")).thenReturn(false);
    EmployeeController.Request youngReq =
        new EmployeeController.Request(
            "Nguyen Van A",
            LocalDate.now().minusYears(15),
            "123 Ho Chi Minh",
            "0901234567",
            true,
            true,
            null);
    ApiException ageEx = assertThrows(ApiException.class, () -> controller.create(youngReq));
    assertEquals(HttpStatus.BAD_REQUEST, ageEx.getStatus());
  }

  @Test
  void updateSuccessAndValidation() {
    Employee emp = new Employee();
    emp.setEmployeeId(1);
    emp.setPhone("0901234567");
    when(employeeRepository.findById(1)).thenReturn(Optional.of(emp));
    when(employeeRepository.save(any(Employee.class))).thenAnswer(i -> i.getArgument(0));

    EmployeeController.Request req =
        new EmployeeController.Request(
            "Nguyen Van B",
            LocalDate.now().minusYears(30),
            "456 Ha Noi",
            "0909999999",
            false,
            true,
            null);

    // duplicate phone of other employee
    Employee other = new Employee();
    other.setEmployeeId(2);
    when(employeeRepository.findByPhone("0909999999")).thenReturn(Optional.of(other));
    ApiException phoneDup = assertThrows(ApiException.class, () -> controller.update(1, req));
    assertEquals(HttpStatus.CONFLICT, phoneDup.getStatus());

    // success update
    when(employeeRepository.findByPhone("0909999999")).thenReturn(Optional.empty());
    Employee updated = controller.update(1, req);
    assertEquals("Nguyen Van B", updated.getFullName());
  }

  @Test
  void deleteSuccessAndInvoiceRestriction() {
    Employee emp = new Employee();
    emp.setEmployeeId(1);
    when(employeeRepository.findById(1)).thenReturn(Optional.of(emp));
    when(invoiceRepository.existsByEmployeeId(1)).thenReturn(false);

    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa nhân viên thành công.", res.get("message"));
    verify(employeeRepository).delete(emp);

    // restricted if has invoice
    when(invoiceRepository.existsByEmployeeId(1)).thenReturn(true);
    ApiException invoiceEx = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, invoiceEx.getStatus());
  }
}
