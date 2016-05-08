package net.mehmetatas.services.auth;

import net.mehmetatas.entities.Login;

/**
 * Created by mehmet on 08.05.2016.
 */
public interface SigninService {
    Login signin(String email, String password);
}
