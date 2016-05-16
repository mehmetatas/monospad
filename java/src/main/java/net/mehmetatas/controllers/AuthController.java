package net.mehmetatas.controllers;

import net.mehmetatas.config.annotations.NoAuth;
import net.mehmetatas.controllers.dto.messages.SigninRequest;
import net.mehmetatas.controllers.dto.messages.SigninResponse;
import net.mehmetatas.controllers.dto.messages.SignupRequest;
import net.mehmetatas.controllers.dto.messages.SignupResponse;
import net.mehmetatas.entities.views.NoteSummary;
import net.mehmetatas.entities.Login;
import net.mehmetatas.entities.Note;
import net.mehmetatas.repositories.NoteRepository;
import net.mehmetatas.services.auth.SigninService;
import net.mehmetatas.services.auth.SignupService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;

import static net.mehmetatas.utils.Utils.isNullOrWhitespace;

/**
 * Created by mehmet on 28.04.2016.
 */
@RestController
@RequestMapping("/api/v1")
public class AuthController {
    private final SignupService signupService;
    private final SigninService signinService;
    private final NoteRepository noteRepository;

    @Autowired
    public AuthController(SignupService signupService, SigninService signinService, NoteRepository noteRepository) {
        this.signupService = signupService;
        this.signinService = signinService;
        this.noteRepository = noteRepository;
    }

    @NoAuth
    @Transactional
    @RequestMapping(value = "/signup", method = RequestMethod.POST)
    @ResponseStatus(HttpStatus.CREATED)
    public SignupResponse signup(@RequestBody SignupRequest request) {
        signupService.signup(request.email, request.password);

        LoginAndNote result = signinAndSaveNote(request.email, request.password, request.unsavedNoteContent);

        SignupResponse response = new SignupResponse(result.login);
        response.note = result.note;
        return response;
    }

    @NoAuth
    @Transactional
    @RequestMapping(value = "/signin", method = RequestMethod.POST)
    @ResponseStatus(HttpStatus.FOUND)
    public SigninResponse signin(@RequestBody SigninRequest request) {
        LoginAndNote result = signinAndSaveNote(request.email, request.password, request.unsavedNoteContent);

        SigninResponse response = new SigninResponse(result.login);
        response.notes = noteRepository
                .pageNoteSummariesByUserId(result.login.getUser().getId(), new PageRequest(0, 10))
                .getContent();
        return response;
    }

    @RequestMapping(value = "/test")
    public void test() {
        Note note = noteRepository.findAll().get(0);
        Note note2 = noteRepository.findByAccessToken(note.getAccessToken());
        if (note2 == null) {
            throw new RuntimeException("guids not working");
        }
    }

    private LoginAndNote signinAndSaveNote(String email, String password, String unsavedNoteContent) {
        LoginAndNote result = new LoginAndNote();

        Login login = signinService.signin(email, password);
        result.login = login;

        if (!isNullOrWhitespace(unsavedNoteContent)) {
            Note note = Note.fromContent(login.getUser(), unsavedNoteContent);
            noteRepository.save(note);

            result.note = new NoteSummary(note);
        }

        return result;
    }

    private static class LoginAndNote {
        public Login login;
        public NoteSummary note;
    }
}
