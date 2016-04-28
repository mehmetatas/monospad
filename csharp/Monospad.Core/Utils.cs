using System;
using Monospad.Core.Exceptions;
using Monospad.Core.Models.Database;
using Taga.Orm.UnitOfWork;
using Taga.Orm.Db;

namespace Monospad.Core
{
    public static class Utils
    {
        public static Note SaveNote(this IRepository repo, string content, User user, long id = 0)
        {
            var isNew = id == 0;

            Note note;
            if (isNew)
            {
                note = new Note { AccessToken = Guid.NewGuid() };
            }
            else
            {
                note = repo.Select<Note>()
                    .Include(n => new { n.AccessToken })
                    .FirstOrDefault(n => n.Id == id && n.User.Id == user.Id);

                if (note == null)
                {
                    throw Errors.Note_NotFound;
                }
            }

            if (!string.IsNullOrEmpty(content))
            {
                var i = content.IndexOf("\n", StringComparison.Ordinal);

                if (i > 0)
                {
                    note.Title = content.Substring(0, Math.Min(i, Constants.TitleLength));
                    note.Summary = content.Substring(i, Math.Min(content.Length - i, Constants.SummaryLength));
                }
                else
                {
                    note.Title = content.Substring(0, Math.Min(content.Length, Constants.TitleLength));
                    note.Summary = content.Substring(0, Math.Min(content.Length, Constants.SummaryLength));
                }
            }

            note.User = user;
            note.Content = (content ?? string.Empty).Trim();
            note.Title = note.Title?.Trim();
            note.Summary = note.Summary?.Trim();
            note.LastUpdateDate = DateTime.UtcNow;

            if (isNew)
            {
                repo.Insert(note);
            }
            else
            {
                repo.Update(note);
            }

            return note;
        }

        public static object ToItem(this Note note)
        {
            return new
            {
                note.Id,
                note.Title,
                note.Summary,
                note.AccessToken
            };
        }
    }
}
