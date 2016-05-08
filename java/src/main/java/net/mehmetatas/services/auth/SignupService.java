package net.mehmetatas.services.auth;

import net.mehmetatas.entities.User;

/**
 * Created by mehmet on 28.04.2016.
 */
public interface SignupService {
    User signup(String email, String password);
}
