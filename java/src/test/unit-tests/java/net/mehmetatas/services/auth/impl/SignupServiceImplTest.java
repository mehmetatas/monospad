package net.mehmetatas.services.auth.impl;

import net.mehmetatas.TestBase;
import net.mehmetatas.entities.User;
import net.mehmetatas.exceptions.auth.EmailAlreadyExistsException;
import net.mehmetatas.repositories.UserRepository;
import net.mehmetatas.services.common.CryptoProvider;
import net.mehmetatas.services.common.EmailValidator;
import net.mehmetatas.services.common.PasswordValidator;
import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.Date;

import static org.hamcrest.MatcherAssert.assertThat;
import static org.hamcrest.core.Is.is;
import static org.mockito.Mockito.*;

/**
 * Created by mehmet on 28.04.2016.
 */
public class SignupServiceImplTest extends TestBase {

    private final String email = "mail@mail.com";
    private final String password = "123456";

    @InjectMocks
    private SignupServiceImpl service;

    @Mock
    private UserRepository userRepo;

    @Mock
    private CryptoProvider crypto;

    @Mock
    private PasswordValidator pwdValidator;

    @Mock
    private EmailValidator emailValidator;

    @Before
    public void setUp() {
        MockitoAnnotations.initMocks(this);
    }

    @Test
    public void signup_should_fail_when_email_already_exists() {
        // arrange
        when(userRepo.findByEmail(email))
                .thenReturn(new User(email, password, new Date()));

        // act & assert
        assertThrows(EmailAlreadyExistsException.class,
                () -> service.signup(email, password));

        // verify
        verify(emailValidator, once()).validate(email);
        verify(pwdValidator, once()).validate(password);
        verify(userRepo, once()).findByEmail(email);
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, never()).encryptPassword(password);
        verify(userRepo, never()).save(any(User.class));
    }

    @Test
    public void signup_should_succeed_when_email_not_exists() {
        // arrange
        String encryptedPassword = "hash of password";

        when(crypto.encryptPassword(password))
                .thenReturn(encryptedPassword);

        // act
        User user = service.signup(email, password);

        // assert
        assertThat(user.getEmail(), is(email));
        assertThat(user.getPassword(), is(encryptedPassword));

        // verify
        verify(emailValidator, once()).validate(email);
        verify(pwdValidator, once()).validate(password);
        verify(userRepo, once()).findByEmail(email);
        verify(crypto, once()).encryptPassword(password);
        verify(userRepo, once()).save(user);
    }
}