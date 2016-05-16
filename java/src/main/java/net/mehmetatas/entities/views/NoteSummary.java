package net.mehmetatas.entities.views;

import net.mehmetatas.entities.Note;

import java.util.UUID;

/**
 * Created by mehmet on 08.05.2016.
 */
public class NoteSummary {
    public long id;
    public String title;
    public String summary;
    public UUID accessToken;

    public NoteSummary(long id, String title, String summary, UUID accessToken) {
        this.id = id;
        this.title = title;
        this.summary = summary;
        this.accessToken = accessToken;
    }

    public NoteSummary(Note note) {
        id = note.getId();
        title = note.getTitle();
        summary = note.getSummary();
        accessToken = note.getAccessToken();
    }
}
