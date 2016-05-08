package net.mehmetatas.exceptions.auth;

import net.mehmetatas.exceptions.MonospadException;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * Created by mehmet on 28.04.2016.
 */
@ResponseStatus(HttpStatus.CONFLICT)
public class EmailAlreadyExistsException extends MonospadException {
    public EmailAlreadyExistsException(String email) {
        super(String.format("Email (%s) already exists in the system!", email));
    }
}
