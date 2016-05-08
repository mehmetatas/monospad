package net.mehmetatas.entities;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import java.util.Date;

/**
 * Created by mehmet on 28.04.2016.
 */
@Entity
public class User {
    @Id
    @GeneratedValue
    private long id;

    private String email;

    private String password;

    private Date signupDate;

    User() {
    }

    public User(String email, String password, Date signupDate) {
        this.email = email;
        this.password = password;
        this.signupDate = signupDate;
    }

    public long getId() {
        return id;
    }

    public String getEmail() {
        return email;
    }

    public String getPassword() {
        return password;
    }

    public Date getSignupDate() {
        return signupDate;
    }
}
