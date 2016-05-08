package net.mehmetatas.controllers.dto.messages;

import net.mehmetatas.controllers.dto.models.NoteSummary;
import net.mehmetatas.entities.Login;
import net.mehmetatas.entities.Note;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

import java.util.UUID;

/**
 * Created by mehmet on 08.05.2016.
 */
public class SignupResponse {
    public UUID loginToken;
    public long userId;
    public NoteSummary note;

    public SignupResponse(Login login) {
        loginToken = login.getToken();
        userId = login.getUser().getId();
    }
}
