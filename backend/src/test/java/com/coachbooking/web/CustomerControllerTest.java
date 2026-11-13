package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Customer;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.InvoiceRepository;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class CustomerControllerTest {

  private CustomerRepository customerRepository;
  private InvoiceRepository invoiceRepository;
  private AccountRepository accountRepository;
  private CustomerController controller;

  @BeforeEach
  void setUp() {
    customerRepository = mock(CustomerRepository.class);
    invoiceRepository = mock(InvoiceRepository.class);
    accountRepository = mock(AccountRepository.class);
    controller = new CustomerController(customerRepository, invoiceRepository, accountRepository);
  }

  @Test
  void listAndGetSuccess() {
    Customer c = new Customer();
    c.setCustomerId(1);
    c.setFullName("Nguyen Van A");
    when(customerRepository.findAll()).thenReturn(List.of(c));
    when(customerRepository.findById(1)).thenReturn(Optional.of(c));

    assertEquals(1, controller.list().size());
    assertEquals(1, controller.get(1).getCustomerId());

    when(customerRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndValidation() {
    CustomerController.Request req =
        new CustomerController.Request(
            "Nguyen Van A", LocalDate.of(1990, 1, 1), "0901234567", true, 10);

    when(customerRepository.existsByPhone("0901234567")).thenReturn(false);
    when(accountRepository.existsById(10)).thenReturn(true);
    when(customerRepository.save(any(Customer.class))).thenAnswer(i -> i.getArgument(0));

    Customer created = controller.create(req);
    assertNotNull(created);
    assertEquals("Nguyen Van A", created.getFullName());

    // phone duplicate
    when(customerRepository.existsByPhone("0901234567")).thenReturn(true);
    ApiException phoneEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.CONFLICT, phoneEx.getStatus());

    // account not found
    when(customerRepository.existsByPhone("0901234567")).thenReturn(false);
    when(accountRepository.existsById(10)).thenReturn(false);
    ApiException accEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.NOT_FOUND, accEx.getStatus());
  }

  @Test
  void updateSuccessAndValidation() {
    Customer c = new Customer();
    c.setCustomerId(1);
    c.setPhone("0901234567");
    when(customerRepository.findById(1)).thenReturn(Optional.of(c));
    when(customerRepository.findByPhone("0909999999")).thenReturn(Optional.empty());
    when(customerRepository.save(any(Customer.class))).thenAnswer(i -> i.getArgument(0));

    CustomerController.Request req =
        new CustomerController.Request(
            "Nguyen Van B", LocalDate.of(1995, 2, 2), "0909999999", false, null);
    Customer updated = controller.update(1, req);
    assertEquals("Nguyen Van B", updated.getFullName());
    assertEquals("0909999999", updated.getPhone());
  }

  @Test
  void deleteSuccessAndRestrictions() {
    Customer c = new Customer();
    c.setCustomerId(1);
    when(customerRepository.findById(1)).thenReturn(Optional.of(c));
    when(invoiceRepository.existsByCustomerId(1)).thenReturn(false);

    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa khách hàng thành công.", res.get("message"));
    verify(customerRepository).delete(c);

    // cannot delete if has invoice
    when(invoiceRepository.existsByCustomerId(1)).thenReturn(true);
    ApiException conflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }
}
