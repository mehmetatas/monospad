using Monospad.Core.Models.Messages;
using TagKid.Framework.WebApi;

namespace Monospad.Core.Services
{
    public interface INoteService
    {
        Response DeleteNote(DeleteNoteRequest request);
        Response SaveNote(SaveNoteRequest request);
        Response GetContent(GetContentRequest request);
    }
}
