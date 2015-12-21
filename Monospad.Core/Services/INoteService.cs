using Monospad.Core.Bootstrapping.Bootstrappers;
using Monospad.Core.Models.Messages;
using Taga.Framework.Hosting;

namespace Monospad.Core.Services
{
    public interface INoteService
    {
        Response DeleteNote(DeleteNoteRequest request);

        Response SaveNote(SaveNoteRequest request);

        Response GetContent(GetContentRequest request);

        [NoAuth]
        Response GetNoteByAccessToken(GetNoteByAccessTokenRequest request);
    }
}
