package net.mehmetatas.services.auth.impl;

import net.mehmetatas.TestBase;
import net.mehmetatas.entities.Login;
import net.mehmetatas.entities.User;
import net.mehmetatas.exceptions.auth.LoginFailedException;
import net.mehmetatas.repositories.LoginRepository;
import net.mehmetatas.repositories.UserRepository;
import net.mehmetatas.services.common.CryptoProvider;
import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.Date;

import static org.hamcrest.MatcherAssert.assertThat;
import static org.hamcrest.Matchers.is;
import static org.junit.Assert.assertNotNull;
import static org.mockito.Matchers.any;
import static org.mockito.Matchers.eq;
import static org.mockito.Mockito.*;

/**
 * Created by mehmet on 08.05.2016.
 */
public class SigninServiceImplTest extends TestBase {

    private final static String email = "mail@mail.com";
    private final static String password = "123456";
    private final static String sha256HashOf123456 = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"; // http://www.xorbin.com/tools/sha256-hash-calculator

    @InjectMocks
    SigninServiceImpl service;

    @Mock
    CryptoProvider crypto;

    @Mock
    LoginRepository loginRepo;

    @Mock
    UserRepository userRepo;

    @Before
    public void setUp() throws Exception {
        MockitoAnnotations.initMocks(this);
    }

    @Test
    public void should_throw_exception_when_user_not_found() {
        // act & assert
        assertThrows(LoginFailedException.class,
                () -> service.signin(email, password));

        // verify
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, never()).encryptPassword(password);
    }

    @Test
    public void should_throw_exception_when_password_not_match() {
        // arrange
        User user = new User(email, sha256HashOf123456, new Date());

        when(userRepo.findByEmail(email))
                .thenReturn(user);

        when(crypto.encryptPassword(password))
                .thenReturn("something wrong");

        // act & assert
        assertThrows(LoginFailedException.class,
                () -> service.signin(email, password));

        // verify
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, once()).encryptPassword(password);
        verify(loginRepo, never()).findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(any(Long.class), any(Date.class));
    }

    @Test
    public void should_not_create_new_login_when_existing_login_found() {
        // arrange
        User user = new User(email, sha256HashOf123456, new Date());
        Login login = Login.newLogin(user);

        when(userRepo.findByEmail(email))
                .thenReturn(user);

        when(crypto.encryptPassword(password))
                .thenReturn(sha256HashOf123456);

        when(loginRepo.findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(eq(user.getId()), any(Date.class)))
                .thenReturn(login);

        // act
        Login result = service.signin(email, password);

        // assert
        assertThat(result, is(login));

        // verify
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, once()).encryptPassword(password);
        verify(loginRepo, once()).findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(any(Long.class), any(Date.class));
        verify(loginRepo, never()).save(any(Login.class));
    }

    @Test
    public void should_create_new_login_when_existing_login_not_found() {
        // arrange
        User user = new User(email, sha256HashOf123456, new Date());

        when(userRepo.findByEmail(email))
                .thenReturn(user);

        when(crypto.encryptPassword(password))
                .thenReturn(sha256HashOf123456);

        when(loginRepo.findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(eq(user.getId()), any(Date.class)))
                .thenReturn(null);

        // act
        Login login = service.signin(email, password);

        // assert
        assertNotNull(login);
        assertThat(login.getUser(), is(user));

        // verify
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, once()).encryptPassword(password);
        verify(loginRepo, once()).findFirstByUserIdAndExpireDateAfterOrderByExpireDateDesc(any(Long.class), any(Date.class));
        verify(loginRepo, once()).save(any(Login.class));
    }
}