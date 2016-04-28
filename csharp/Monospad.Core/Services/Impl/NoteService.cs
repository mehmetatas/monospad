using Monospad.Core.Exceptions;
using Taga.Orm.Db;
using Monospad.Core.Models.Database;
using Monospad.Core.Models.Messages;
using Monospad.Core.Providers;
using Taga.Framework.Hosting;
using Taga.Orm.UnitOfWork;

namespace Monospad.Core.Services.Impl
{
    public class NoteService : INoteService
    {
        private readonly IRepository _repository;

        public NoteService(IRepository repository)
        {
            _repository = repository;
        }
        
        public Response GetContent(GetContentRequest request)
        {
            var note = _repository.Select<Note>()
                .Include(n => n.Content)
                .FirstOrDefault(n => n.Id == request.Id && n.User.Id == MonospadContext.Current.User.Id);

            return Response.Success.WithData(new
            {
                note.Content
            });
        }

        public Response DeleteNote(DeleteNoteRequest request)
        {
            var note = _repository.Select<Note>()
                .Include(n => new { n.Id })
                .FirstOrDefault(n => n.Id == request.Id && n.User.Id == MonospadContext.Current.User.Id);

            if (note != null)
            {
                _repository.Delete(note);
            }

            return Response.Success;
        }

        public Response SaveNote(SaveNoteRequest request)
        {
            var note = _repository.SaveNote(request.Content, MonospadContext.Current.User, request.Id);

            return Response.Success.WithData(note.ToItem());
        }

        public Response GetNoteByAccessToken(GetNoteByAccessTokenRequest request)
        {
            var note = _repository.Select<Note>()
                .Include(n => n.Content)
                .FirstOrDefault(n => n.AccessToken == request.AccessToken);

            return note == null
                ? Response.Error(Errors.Note_NotFound)
                : Response.Success.WithData(new
                {
                    note.Content
                });
        }
    }
}
