using System;
using Monospad.Core.Models.Database;
using TagKid.Framework.UnitOfWork;

namespace Monospad.Core
{
    public static class Utils
    {
        public static Note SaveNote(this IRepository repo, string content, User user, long id = 0)
        {
            var note = new Note
            {
                Id = id,
                User = user,
                Content = (content ?? string.Empty).Trim(),
                LastUpdateDate = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(content))
            {
                var i = content.IndexOf("\n", StringComparison.Ordinal);

                if (i > 0)
                {
                    note.Title = content.Substring(0, Math.Min(i, 40));
                    note.Summary = content.Substring(i, Math.Min(content.Length - i, 150));
                }
                else
                {
                    note.Title = content.Substring(0, Math.Min(content.Length, 40));
                    note.Summary = content.Substring(0, Math.Min(content.Length, 150));
                }
            }

            note.Title = note.Title?.Trim();
            note.Summary = note.Summary?.Trim();

            if (note.Id == 0)
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
                note.Summary
            };
        }
    }
}
