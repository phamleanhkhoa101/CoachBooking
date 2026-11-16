package com.coachbooking.config;

import com.coachbooking.domain.Account;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

@Component
public class DataInitializer implements CommandLineRunner {

  private static final Logger log = LoggerFactory.getLogger(DataInitializer.class);

  private final AccountRepository accountRepository;
  private final EmployeeRepository employeeRepository;
  private final CustomerRepository customerRepository;
  private final PasswordEncoder passwordEncoder;

  public DataInitializer(
      AccountRepository accountRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      PasswordEncoder passwordEncoder) {
    this.accountRepository = accountRepository;
    this.employeeRepository = employeeRepository;
    this.customerRepository = customerRepository;
    this.passwordEncoder = passwordEncoder;
  }

  @Override
  @Transactional
  public void run(String... args) {
    Account admin = ensureAccount("admin", "admin@gmail.com", "Abc@123", 1);
    Account nv1 = ensureAccount("nv1", "nv1@gmail.com", "Abc@123", 2);
    Account nv2 = ensureAccount("nv2", "nv2@gmail.com", "Abc@123", 2);
    Account kh1 = ensureAccount("kh1", "kh1@gmail.com", "Abc@123", 3);
    Account kh2 = ensureAccount("kh2", "kh2@gmail.com", "Abc@123", 3);

    linkEmployee("0912345678", admin);
    linkEmployee("0912345679", nv1);
    linkEmployee("0912345680", nv2);
    linkCustomer("0981111111", kh1);
    linkCustomer("0981111112", kh2);
  }

  private Account ensureAccount(String username, String email, String password, int roleId) {
    return accountRepository
        .findByUsername(username)
        .orElseGet(
            () -> {
              Account account = new Account();
              account.setUsername(username);
              account.setEmail(email);
              account.setPassword(passwordEncoder.encode(password));
              account.setRoleId(roleId);
              account.setIsActive(true);
              Account saved = accountRepository.save(account);
              log.info("Đã tạo tài khoản mẫu: {}", username);
              return saved;
            });
  }

  private void linkEmployee(String phone, Account account) {
    employeeRepository
        .findByPhone(phone)
        .ifPresent(
            employee -> {
              if (employee.getAccountId() == null) {
                employee.setAccountId(account.getAccountId());
                employeeRepository.save(employee);
              }
            });
  }

  private void linkCustomer(String phone, Account account) {
    customerRepository
        .findByPhone(phone)
        .ifPresent(
            customer -> {
              if (customer.getAccountId() == null) {
                customer.setAccountId(account.getAccountId());
                customerRepository.save(customer);
              }
            });
  }
}
