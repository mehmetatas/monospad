package net.mehmetatas.services.common.impl;

import net.mehmetatas.exceptions.common.EmailValidationException;
import net.mehmetatas.services.common.EmailValidator;
import org.springframework.stereotype.Component;

/**
 * Created by mehmet on 07.05.2016.
 */
@Component
public class EmailValidatorImpl implements EmailValidator {
    @Override
    public void validate(String email) {
        boolean isValidEmail = org.apache.commons.validator.routines.EmailValidator.getInstance().isValid(email);

        if (!isValidEmail) {
            throw new EmailValidationException();
        }
    }
}
