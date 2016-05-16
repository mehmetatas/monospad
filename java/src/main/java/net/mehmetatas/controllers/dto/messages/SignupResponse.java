package net.mehmetatas.controllers.dto.messages;

import net.mehmetatas.entities.views.NoteSummary;
import net.mehmetatas.entities.Login;

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
