package com.coachbooking.config;

import jakarta.annotation.PostConstruct;
import java.util.Set;
import lombok.Getter;
import lombok.Setter;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.core.env.Environment;
import org.springframework.core.env.Profiles;
import org.springframework.core.env.StandardEnvironment;
import org.springframework.stereotype.Component;

@Component
@ConfigurationProperties(prefix = "app.sepay")
@Getter
@Setter
public class SepayProperties {
  private boolean enabled;
  private String environment = "sandbox";
  private String merchantId = "";
  private String secretKey = "";
  private String ipnSecret = "";
  private String frontendUrl = "";
  private String confirmationMode = "IPN";
  private String localReturnUrl = "http://localhost:3000";
  private Environment runtimeEnvironment = new StandardEnvironment();

  @Autowired
  public void setRuntimeEnvironment(Environment environment) {
    this.runtimeEnvironment = environment;
  }

  public boolean isPollingEnabled() {
    return "POLLING".equals(confirmationMode)
        && "sandbox".equals(environment)
        && runtimeEnvironment.acceptsProfiles(Profiles.of("local"));
  }

  @PostConstruct
  public void validateConfirmationMode() {
    if (!Set.of("IPN", "POLLING").contains(confirmationMode)
        || ("POLLING".equals(confirmationMode)
            && (!isPollingEnabled()
                || !Set.of("http://localhost:3000", "http://localhost:5173")
                    .contains(localReturnUrl)))) {
      throw new IllegalStateException(
          "SePay POLLING chỉ dùng với profile local, sandbox và URL localhost:3000 hoặc localhost:5173.");
    }
  }
}
