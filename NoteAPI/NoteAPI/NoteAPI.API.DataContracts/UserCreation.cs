using System;
using System.ComponentModel.DataAnnotations;

namespace NoteAPI.API.DataContracts
{
    /// <summary>
    /// User datacontract summary to be replaced
    /// </summary>
    public class UserCreation
    {
        /// <summary>
        /// User Id
        /// </summary>
        [DataType(DataType.Text)]
        public string Id { get; set; }

        /// <summary>
        /// User firstname
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        /// <summary>
        /// User creation date
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
