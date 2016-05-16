package net.mehmetatas.controllers.dto.messages;

import net.mehmetatas.entities.views.NoteSummary;
import net.mehmetatas.entities.Login;

import java.util.List;
import java.util.UUID;

/**
 * Created by mehmet on 08.05.2016.
 */
public class SigninResponse {
    public UUID loginToken;
    public long userId;
    public List<NoteSummary> notes;

    public SigninResponse(Login login) {
        loginToken = login.getToken();
        userId = login.getUser().getId();
    }
}
