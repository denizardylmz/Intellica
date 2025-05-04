using System;

namespace NoteAPI.API.DataContracts.Requests
{
    /// <summary>
    /// User creation request
    /// This follows a flow seggregation pattern to distingish commands from queries (ex: CQRS, etc).
    /// No specific framework has been used to avoid additional technical complexity.
    /// </summary>
    public class UserCreationRequest
    {
        /// <summary>
        /// User creation request date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// User creation data
        /// </summary>
        public User User { get; set; }
    }
}
