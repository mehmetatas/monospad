package net.mehmetatas.exceptions.common;

import net.mehmetatas.exceptions.MonospadException;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * Created by mehmet on 07.05.2016.
 */
@ResponseStatus(HttpStatus.UNPROCESSABLE_ENTITY)
public class PasswordValidationException extends MonospadException {
    public PasswordValidationException(String reason) {
        super("The password you entered does not match our password policy: " + reason);
    }
}
