namespace NoteAPI.Repo.SqlDatabase.DTO
{
    public class User
    {

        public string? Id { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public Address? Address { get; set; }
    }
}
