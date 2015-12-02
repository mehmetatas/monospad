using Monospad.Core.Models.Messages;
using TagKid.Framework.Hosting;

namespace Monospad.Core.Services
{
    public interface INoteService
    {
        Response DeleteNote(DeleteNoteRequest request);
        Response SaveNote(SaveNoteRequest request);
        Response GetContent(GetContentRequest request);
        Response GetNote(GetNoteRequest request);
    }
}
