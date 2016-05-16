package net.mehmetatas.context;

import net.mehmetatas.entities.Login;
import net.mehmetatas.entities.User;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;

/**
 * Created by mehmet on 08.05.2016.
 */
public class RequestContext {
    private final static String Key = "MonospadRequestContext";

    private RequestContext() {

    }

    public static RequestContext current() {
        RequestAttributes requestAttributes = RequestContextHolder.currentRequestAttributes();
        RequestContext ctx = (RequestContext) requestAttributes.getAttribute(Key, RequestAttributes.SCOPE_REQUEST);

        if (ctx == null) {
            ctx = new RequestContext();
            requestAttributes.setAttribute(Key, ctx, RequestAttributes.SCOPE_REQUEST);
        }

        return ctx;
    }

    private Login _login;
    private User _user;

    public void init(Login login) {
        _login = login;
        _user = login.getUser();
    }
}
