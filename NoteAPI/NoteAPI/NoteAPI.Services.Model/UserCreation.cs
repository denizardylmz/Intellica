using System;

namespace NoteAPI.Services.Model
{
    public class UserCreation
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public DateTime CreationDate { get; set; }        

    }
}
