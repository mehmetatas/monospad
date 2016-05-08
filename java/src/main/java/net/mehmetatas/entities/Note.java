package net.mehmetatas.entities;

import org.hibernate.annotations.Type;

import javax.persistence.*;
import java.util.Date;
import java.util.UUID;

/**
 * Created by mehmet on 28.04.2016.
 */
@Entity
public class Note {
    @Id
    @GeneratedValue
    private long id;

    @ManyToOne(fetch = FetchType.LAZY)
    private User user;

    private String title;

    private String summary;

    private String content;

    @Type(type="org.hibernate.type.UUIDCharType")
    private UUID accessToken;

    private Date lastUpdateDate;

    Note() {
    }

    private Note(User user, String title, String summary, String content, UUID accessToken, Date lastUpdateDate) {
        this.user = user;
        this.title = title;
        this.summary = summary;
        this.content = content;
        this.accessToken = accessToken;
        this.lastUpdateDate = lastUpdateDate;
    }

    public long getId() {
        return id;
    }

    public User getUser() {
        return user;
    }

    public String getTitle() {
        return title;
    }

    public String getSummary() {
        return summary;
    }

    public String getContent() {
        return content;
    }

    public UUID getAccessToken() {
        return accessToken;
    }

    public Date getLastUpdateDate() {
        return lastUpdateDate;
    }

    public static Note fromContent(User user, String content) {
        String title = content.split("\n")[0];
        String summary = content;
        UUID accessToken = UUID.randomUUID();
        Date lastUpdate = new Date();

        if (summary.length() > 100) {
            summary = summary.substring(0, 100);
        }

        if (title.length() > 50) {
            title = title.substring(0, 50);
        }

        return new Note(user, title, summary, content, accessToken, lastUpdate);
    }
}
