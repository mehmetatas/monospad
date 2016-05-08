package net.mehmetatas.services.auth.impl;

import net.mehmetatas.entities.Login;
import net.mehmetatas.entities.User;
import net.mehmetatas.exceptions.auth.LoginFailedException;
import net.mehmetatas.repositories.LoginRepository;
import net.mehmetatas.repositories.UserRepository;
import net.mehmetatas.services.auth.SigninService;
import net.mehmetatas.services.common.CryptoProvider;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.Date;
import java.util.List;

/**
 * Created by mehmet on 08.05.2016.
 */
@Component
public class SigninServiceImpl implements SigninService {
    private final UserRepository userRepository;
    private final LoginRepository loginRepo;
    private final CryptoProvider crypto;

    @Autowired
    public SigninServiceImpl(UserRepository userRepository, LoginRepository loginRepo, CryptoProvider crypto) {
        this.userRepository = userRepository;
        this.loginRepo = loginRepo;
        this.crypto = crypto;
    }

    @Override
    public Login signin(String email, String password) {
        User user = userRepository.findByEmail(email);

        if (user == null) {
            throw new LoginFailedException();
        }

        String encryptedPassword = crypto.encryptPassword(password);

        if (!user.getPassword().equals(encryptedPassword)) {
            throw new LoginFailedException();
        }

        Login login = loginRepo.findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(user.getId(), new Date());

        if (login == null) {
            login = Login.newLogin(user);
            loginRepo.save(login);
        }

        return login;
    }
}
