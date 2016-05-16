package net.mehmetatas.config.interceptors;

import net.mehmetatas.config.annotations.NoAuth;
import net.mehmetatas.context.RequestContext;
import net.mehmetatas.entities.Login;
import net.mehmetatas.repositories.LoginRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Component;
import org.springframework.web.method.HandlerMethod;
import org.springframework.web.servlet.handler.HandlerInterceptorAdapter;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.util.Date;
import java.util.UUID;

import static net.mehmetatas.utils.Utils.isNullOrWhitespace;

/**
 * Created by mehmet on 08.05.2016.
 */
@Component
public class AuthInterceptor extends HandlerInterceptorAdapter {
    private final LoginRepository loginRepo;

    @Autowired
    public AuthInterceptor(LoginRepository loginRepo) {
        this.loginRepo = loginRepo;
    }

    @Override
    public boolean preHandle(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse, Object o) throws Exception {
        HandlerMethod method = (HandlerMethod) o;

        boolean noAuth = method.getMethod().getAnnotation(NoAuth.class) != null;

        if (noAuth) {
            return true;
        }

        String tokenStr = httpServletRequest.getHeader("x-monospad-auth-token");

        if (isNullOrWhitespace(tokenStr)) {
            httpServletResponse.sendError(HttpStatus.UNAUTHORIZED.value(), "Authentication token required!");
            return false;
        }

        UUID token = UUID.fromString(tokenStr);

        Login login = loginRepo.findByTokenAndIsPasswordRecoveryFalse(token);

        if (login == null) {
            httpServletResponse.sendError(HttpStatus.UNAUTHORIZED.value(), "Invalid authentication token!");
            return false;
        }

        if (login.getExpireDate().compareTo(new Date()) < 0) {
            httpServletResponse.sendError(HttpStatus.UNAUTHORIZED.value(), "Authentication token expired!");
            return false;
        }

        login.extend();
        loginRepo.save(login);

        RequestContext.current().init(login);

        return true;
    }
}
