package net.mehmetatas.exceptions.auth;

import net.mehmetatas.exceptions.MonospadException;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * Created by mehmet on 08.05.2016.
 */
@ResponseStatus(HttpStatus.UNAUTHORIZED)
public class LoginFailedException extends MonospadException {
    public LoginFailedException() {
        super("Incorrect email address or password!");
    }
}
