package net.mehmetatas.services.auth.impl;

import net.mehmetatas.entities.User;
import net.mehmetatas.exceptions.auth.EmailAlreadyExistsException;
import net.mehmetatas.repositories.UserRepository;
import net.mehmetatas.services.auth.SignupService;
import net.mehmetatas.services.common.CryptoProvider;
import net.mehmetatas.services.common.EmailValidator;
import net.mehmetatas.services.common.PasswordValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.Date;

/**
 * Created by mehmet on 28.04.2016.
 */
@Component
public class SignupServiceImpl implements SignupService {
    private final UserRepository userRepo;
    private final CryptoProvider crypto;
    private final PasswordValidator pwdValidator;
    private final EmailValidator emailValidator;

    @Autowired
    public SignupServiceImpl(UserRepository userRepo, CryptoProvider crypto, PasswordValidator pwdValidator, EmailValidator emailValidator) {
        this.userRepo = userRepo;
        this.crypto = crypto;
        this.pwdValidator = pwdValidator;
        this.emailValidator = emailValidator;
    }

    @Override
    public User signup(String email, String password) {
        emailValidator.validate(email);
        pwdValidator.validate(password);

        User user = userRepo.findByEmail(email);

        if (user != null) {
            throw new EmailAlreadyExistsException(email);
        }

        String encryptedPwd = crypto.encryptPassword(password);

        user = new User(email, encryptedPwd, new Date());

        userRepo.save(user);

        return user;
    }
}
