package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.dto.profile.ProfileResponse;
import com.coachbooking.dto.profile.UpdateProfileRequest;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import java.time.LocalDate;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class ProfileServiceTest {

  private CustomerRepository customerRepository;
  private EmployeeRepository employeeRepository;
  private CurrentUserService currentUserService;
  private ProfileService service;

  private Account account;

  @BeforeEach
  void setUp() {
    customerRepository = mock(CustomerRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    currentUserService = mock(CurrentUserService.class);

    service = new ProfileService(customerRepository, employeeRepository, currentUserService);

    account = new Account();
    account.setAccountId(1);
    account.setUsername("testuser");
    account.setEmail("test@gmail.com");

    when(currentUserService.roleName(account)).thenReturn("KhachHang");
  }

  @Test
  void getReturnsCustomerProfile() {
    Customer customer = new Customer();
    customer.setCustomerId(10);
    customer.setFullName("Nguyen Customer");
    customer.setPhone("0901234567");
    customer.setDateOfBirth(LocalDate.of(1995, 5, 20));
    customer.setGender(true);
    when(customerRepository.findByAccountId(1)).thenReturn(Optional.of(customer));

    ProfileResponse res = service.get(account);
    assertEquals("CUSTOMER", res.type());
    assertEquals(10, res.customerId());
    assertEquals("Nguyen Customer", res.fullName());
    assertEquals("test@gmail.com", res.email());
  }

  @Test
  void getReturnsEmployeeProfile() {
    Employee employee = new Employee();
    employee.setEmployeeId(20);
    employee.setFullName("Tran Employee");
    employee.setPhone("0907654321");
    employee.setAddress("123 Street");
    when(employeeRepository.findByAccountId(1)).thenReturn(Optional.of(employee));

    ProfileResponse res = service.get(account);
    assertEquals("EMPLOYEE", res.type());
    assertEquals(20, res.employeeId());
    assertEquals("Tran Employee", res.fullName());
    assertEquals("123 Street", res.address());
  }

  @Test
  void getReturnsUnassignedProfile() {
    when(customerRepository.findByAccountId(1)).thenReturn(Optional.empty());
    when(employeeRepository.findByAccountId(1)).thenReturn(Optional.empty());

    ProfileResponse res = service.get(account);
    assertEquals("UNASSIGNED", res.type());
    assertEquals("testuser", res.fullName());
  }

  @Test
  void updateCustomerProfileSuccessAndDuplicatePhone() {
    Customer customer = new Customer();
    customer.setCustomerId(10);
    customer.setFullName("Old Name");
    customer.setPhone("0901234567");
    when(customerRepository.findByAccountId(1)).thenReturn(Optional.of(customer));

    UpdateProfileRequest req =
        new UpdateProfileRequest("New Name", "0909999999", LocalDate.of(1995, 1, 1), true, null);

    // duplicate phone with another customer
    Customer other = new Customer();
    other.setCustomerId(99);
    when(customerRepository.findByPhone("0909999999")).thenReturn(Optional.of(other));
    ApiException conflict = assertThrows(ApiException.class, () -> service.update(account, req));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());

    // success update
    when(customerRepository.findByPhone("0909999999")).thenReturn(Optional.empty());
    ProfileResponse updated = service.update(account, req);
    verify(customerRepository).save(customer);
    assertEquals("New Name", customer.getFullName());
  }

  @Test
  void updateEmployeeProfileSuccessAndAddressValidation() {
    Employee employee = new Employee();
    employee.setEmployeeId(20);
    when(employeeRepository.findByAccountId(1)).thenReturn(Optional.of(employee));

    UpdateProfileRequest reqNoAddress =
        new UpdateProfileRequest("Emp Name", "0907654321", LocalDate.of(1990, 1, 1), false, "");
    ApiException badReq =
        assertThrows(ApiException.class, () -> service.update(account, reqNoAddress));
    assertEquals(HttpStatus.BAD_REQUEST, badReq.getStatus());

    UpdateProfileRequest validReq =
        new UpdateProfileRequest(
            "Emp Name", "0907654321", LocalDate.of(1990, 1, 1), false, "District 1");
    service.update(account, validReq);
    verify(employeeRepository).save(employee);
    assertEquals("District 1", employee.getAddress());
  }

  @Test
  void updateUnassignedAccountCreatesOrLinksCustomer() {
    when(customerRepository.findByAccountId(1)).thenReturn(Optional.empty());
    when(employeeRepository.findByAccountId(1)).thenReturn(Optional.empty());
    when(currentUserService.isCustomer(account)).thenReturn(true);

    UpdateProfileRequest req =
        new UpdateProfileRequest(
            "New Customer", "0901112222", LocalDate.of(1998, 2, 2), true, null);

    // Creates new customer
    when(customerRepository.findByPhone("0901112222")).thenReturn(Optional.empty());
    service.update(account, req);
    verify(customerRepository).save(any(Customer.class));

    // Links existing unattached phone
    Customer unattached = new Customer();
    unattached.setCustomerId(30);
    unattached.setAccountId(null);
    when(customerRepository.findByPhone("0901112222")).thenReturn(Optional.of(unattached));
    service.update(account, req);
    assertEquals(1, unattached.getAccountId());
  }
}
