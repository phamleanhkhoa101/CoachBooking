package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import jakarta.mail.MessagingException;
import jakarta.mail.internet.MimeMessage;
import java.io.UnsupportedEncodingException;
import java.util.Optional;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpStatus;
import org.springframework.mail.MailException;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;
import org.springframework.web.util.HtmlUtils;

@Service
public class EmailService {

  private static final Logger log = LoggerFactory.getLogger(EmailService.class);

  private final JavaMailSender mailSender;
  private final boolean mailEnabled;
  private final String fromAddress;
  private final String fromName;

  public EmailService(
      Optional<JavaMailSender> mailSender,
      @Value("${app.mail.enabled:false}") boolean mailEnabled,
      @Value("${app.mail.from:no-reply@coachbooking.local}") String fromAddress,
      @Value("${app.mail.from-name:CoachBooking}") String fromName) {
    this.mailSender = mailSender.orElse(null);
    this.mailEnabled = mailEnabled;
    this.fromAddress = fromAddress;
    this.fromName = fromName;
  }

  /** Gửi mã OTP đặt lại mật khẩu qua SMTP. */
  public void sendPasswordResetOtp(
      String toEmail, String username, String otp, int expirationMinutes) {
    if (!mailEnabled || mailSender == null) {
      throw new ApiException(
          HttpStatus.SERVICE_UNAVAILABLE,
          "Dịch vụ gửi email SMTP chưa được bật. Vui lòng liên hệ quản trị viên.");
    }

    try {
      MimeMessage message = mailSender.createMimeMessage();
      MimeMessageHelper helper = new MimeMessageHelper(message, "UTF-8");
      helper.setTo(toEmail);
      helper.setSubject("[CoachBooking] Mã xác nhận đặt lại mật khẩu");
      setFrom(helper);
      helper.setText(buildOtpEmailHtml(username, otp, expirationMinutes), true);
      mailSender.send(message);
    } catch (MessagingException | MailException | UnsupportedEncodingException ex) {
      log.error("Không thể gửi email đặt lại mật khẩu tới {}: {}", toEmail, ex.getMessage());
      throw new ApiException(
          HttpStatus.SERVICE_UNAVAILABLE,
          "Không thể gửi email xác nhận lúc này. Vui lòng thử lại sau.");
    }
  }

  private void setFrom(MimeMessageHelper helper)
      throws MessagingException, UnsupportedEncodingException {
    if (fromName != null && !fromName.isBlank()) {
      helper.setFrom(fromAddress, fromName);
    } else {
      helper.setFrom(fromAddress);
    }
  }

  private String buildOtpEmailHtml(String username, String otp, int expirationMinutes) {
    String safeUsername = HtmlUtils.htmlEscape(username);
    String safeOtp = HtmlUtils.htmlEscape(otp);
    return """
        <div style="font-family: 'Segoe UI', Arial, sans-serif; max-width: 520px; margin: 0 auto; padding: 24px; border: 1px solid #e2e8f0; border-radius: 12px; color: #1e293b;">
          <h2 style="margin: 0 0 12px; color: #0f172a;">Đặt lại mật khẩu CoachBooking</h2>
          <p style="margin: 0 0 16px; line-height: 1.5;">
            Xin chào <strong>%s</strong>, chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.
          </p>
          <p style="margin: 0 0 8px;">Mã xác nhận (OTP) của bạn là:</p>
          <div style="font-size: 28px; font-weight: 700; letter-spacing: 6px; text-align: center; padding: 14px 20px; margin: 12px 0 16px; background: #f1f5f9; border-radius: 8px; color: #0f172a;">
            %s
          </div>
          <p style="margin: 0 0 12px; font-size: 14px; color: #475569;">
            Mã có hiệu lực trong <strong>%d phút</strong> và chỉ sử dụng được một lần.
          </p>
          <p style="margin: 0; font-size: 13px; color: #64748b;">
            Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.
          </p>
        </div>
        """
        .formatted(safeUsername, safeOtp, expirationMinutes);
  }
}
