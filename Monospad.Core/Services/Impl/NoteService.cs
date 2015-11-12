using DummyOrm.Db;
using Monospad.Core.Models.Database;
using Monospad.Core.Models.Messages;
using Monospad.Core.Providers;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.WebApi;

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
    }
}
