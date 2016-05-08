package net.mehmetatas.config.interceptors;

import org.springframework.stereotype.Component;
import org.springframework.web.servlet.HandlerInterceptor;
import org.springframework.web.servlet.ModelAndView;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 * Created by mehmet on 08.05.2016.
 */
@Component
public class LogInterceptor implements HandlerInterceptor {
    @Override
    public boolean preHandle(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse, Object o) throws Exception {
        System.out.println("CALLING: " + httpServletRequest.getRequestURI());
        return true;
    }

    @Override
    public void postHandle(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse, Object o, ModelAndView modelAndView) throws Exception {
        System.out.println("CALLED:  " + httpServletRequest.getRequestURI());
    }

    @Override
    public void afterCompletion(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse, Object o, Exception e) throws Exception {
        if (httpServletResponse.getStatus() >= 400 && httpServletResponse.getStatus() < 500) {
            System.err.println("WARNING: " + httpServletRequest.getRequestURI());
        } else if (httpServletResponse.getStatus() >= 500) {
            System.err.println("ERROR:   " + httpServletRequest.getRequestURI());
        } else {
            System.out.println("SUCCESS: " + httpServletRequest.getRequestURI());
        }
    }
}
