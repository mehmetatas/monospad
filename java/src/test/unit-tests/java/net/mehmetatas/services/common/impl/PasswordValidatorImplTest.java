package net.mehmetatas.services.common.impl;

import net.mehmetatas.TestBase;
import net.mehmetatas.exceptions.common.PasswordValidationException;
import org.junit.Before;
import org.junit.Test;

import static org.junit.Assert.*;

/**
 * Created by mehmet on 07.05.2016.
 */
public class PasswordValidatorImplTest extends TestBase {
    private PasswordValidatorImpl validator;

    @Before
    public void setUp() {
        validator = new PasswordValidatorImpl();
    }

    @Test
    public void should_throw_exception_when_password_is_null() {
        // arrange
        String password = null;

        // act & assert
        assertThrows(PasswordValidationException.class,
                () -> validator.validate(password));
    }

    @Test
    public void should_throw_exception_when_password_is_empty() {
        // arrange
        String password = "";

        // act & assert
        assertThrows(PasswordValidationException.class,
                () -> validator.validate(password));
    }

    @Test
    public void should_throw_exception_when_password_is_too_short() {
        // arrange
        String password = "1234";

        // act & assert
        assertThrows(PasswordValidationException.class,
                () -> validator.validate(password));
    }

    @Test
    public void should_throw_exception_when_password_is_too_long() {
        // arrange
        String password = "12345678901234567890";

        // act & assert
        assertThrows(PasswordValidationException.class,
                () -> validator.validate(password));
    }
}