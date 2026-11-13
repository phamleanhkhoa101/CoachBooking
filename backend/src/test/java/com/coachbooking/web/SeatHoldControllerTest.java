package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.assertSame;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.domain.Account;
import com.coachbooking.dto.hold.SeatHoldRequest;
import com.coachbooking.dto.hold.SeatHoldResponse;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.SeatHoldService;
import java.time.OffsetDateTime;
import java.util.List;
import java.util.UUID;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class SeatHoldControllerTest {

  private SeatHoldService seatHoldService;
  private CurrentUserService currentUserService;
  private SeatHoldController controller;
  private Account account;

  @BeforeEach
  void setUp() {
    seatHoldService = mock(SeatHoldService.class);
    currentUserService = mock(CurrentUserService.class);
    controller = new SeatHoldController(seatHoldService, currentUserService);
    account = new Account();
    account.setAccountId(7);
    when(currentUserService.requireAccount()).thenReturn(account);
  }

  @Test
  void createDelegatesWithCurrentAccount() {
    SeatHoldRequest request = new SeatHoldRequest(11, List.of(1, 2));
    SeatHoldResponse response =
        new SeatHoldResponse(UUID.randomUUID(), 11, List.of(1, 2), OffsetDateTime.now());
    when(seatHoldService.create(account, request)).thenReturn(response);

    assertSame(response, controller.create(request));
    verify(seatHoldService).create(account, request);
  }

  @Test
  void getDelegatesWithCurrentAccount() {
    UUID token = UUID.randomUUID();
    SeatHoldResponse response = new SeatHoldResponse(token, 11, List.of(1), OffsetDateTime.now());
    when(seatHoldService.get(account, token)).thenReturn(response);

    assertSame(response, controller.get(token));
    verify(seatHoldService).get(account, token);
  }

  @Test
  void releaseDelegatesWithCurrentAccount() {
    UUID token = UUID.randomUUID();

    controller.release(token);

    verify(seatHoldService).release(account, token);
  }
}
