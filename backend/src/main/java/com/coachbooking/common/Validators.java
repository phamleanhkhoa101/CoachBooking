package com.coachbooking.common;

import java.util.regex.Pattern;

public final class Validators {

    private static final Pattern EMAIL = Pattern.compile("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
    private static final Pattern PHONE = Pattern.compile("^\\d{10,11}$");
    private static final Pattern SPECIAL = Pattern.compile("[\\W_]");

    private Validators() {
    }

    public static boolean isBlank(String value) {
        return value == null || value.trim().isEmpty();
    }

    public static void requireText(String value, String message) {
        if (isBlank(value)) {
            throw ApiException.badRequest(message);
        }
    }

    public static void requireEmail(String email) {
        requireText(email, "Vui lòng nhập email.");
        if (!EMAIL.matcher(email.trim()).matches()) {
            throw ApiException.badRequest("Email không đúng định dạng.");
        }
    }

    public static void requirePhone(String phone) {
        requireText(phone, "Vui lòng nhập số điện thoại.");
        if (!PHONE.matcher(phone.trim()).matches()) {
            throw ApiException.badRequest("Số điện thoại phải có 10-11 chữ số.");
        }
    }

    public static void requirePassword(String password) {
        if (password == null || password.length() < 8
                || password.chars().noneMatch(Character::isUpperCase)
                || password.chars().noneMatch(Character::isLowerCase)
                || password.chars().noneMatch(Character::isDigit)
                || !SPECIAL.matcher(password).find()) {
            throw ApiException.badRequest(
                    "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
        }
    }
}
