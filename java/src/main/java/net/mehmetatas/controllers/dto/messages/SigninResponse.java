package net.mehmetatas.controllers.dto.messages;

import net.mehmetatas.controllers.dto.models.NoteSummary;
import net.mehmetatas.entities.Login;

import java.util.UUID;

/**
 * Created by mehmet on 08.05.2016.
 */
public class SigninResponse {
    public UUID loginToken;
    public long userId;
    public NoteSummary note;

    public SigninResponse(Login login) {
        loginToken = login.getToken();
        userId = login.getUser().getId();
    }
}
