package net.mehmetatas.repositories;

import net.mehmetatas.entities.Note;
import net.mehmetatas.entities.views.NoteSummary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.UUID;

/**
 * Created by mehmet on 07.05.2016.
 */
public interface NoteRepository extends JpaRepository<Note, Long> {
    Note findByAccessToken(UUID accessToken);

    @Query("select new net.mehmetatas.entities.views.NoteSummary(n.id, n.title, n.summary, n.accessToken) " +
            "from Note n " +
            "where n.user.id = :id " +
            "order by n.lastUpdateDate")
    Page<NoteSummary> pageNoteSummariesByUserId(@Param("id") long id, Pageable pageable);
}
