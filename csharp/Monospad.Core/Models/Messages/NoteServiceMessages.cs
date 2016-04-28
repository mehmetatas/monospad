﻿using System;

namespace Monospad.Core.Models.Messages
{
    public class DeleteNoteRequest
    {
        public long Id { get; set; }
    }

    public class SaveNoteRequest
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }

    public class GetContentRequest
    {
        public long Id { get; set; }
    }

    public class GetNoteByAccessTokenRequest
    {
        public Guid AccessToken { get; set; }
    }
}
