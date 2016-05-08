package net.mehmetatas.services.common.impl;

import net.mehmetatas.exceptions.common.PasswordValidationException;
import net.mehmetatas.services.common.PasswordValidator;
import org.springframework.stereotype.Component;

/**
 * Created by mehmet on 07.05.2016.
 */
@Component
public class PasswordValidatorImpl implements PasswordValidator {
    public void validate(String password) {
        if (password == null || password.length() < 6 || password.length() > 16) {
            throw new PasswordValidationException("Password must be 6 - 16 characters long.");
        }
    }
}
