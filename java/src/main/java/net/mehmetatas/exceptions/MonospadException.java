package net.mehmetatas.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * Created by mehmet on 28.04.2016.
 */
@ResponseStatus(HttpStatus.BAD_REQUEST)
public abstract class MonospadException extends RuntimeException {
    public MonospadException(String message) {
        super(message);
    }
}
