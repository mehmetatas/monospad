package net.mehmetatas.repositories;

import net.mehmetatas.entities.Note;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

/**
 * Created by mehmet on 07.05.2016.
 */
public interface NoteRepository extends JpaRepository<Note, Long> {
    Note findByAccessToken(UUID accessToken);
}
