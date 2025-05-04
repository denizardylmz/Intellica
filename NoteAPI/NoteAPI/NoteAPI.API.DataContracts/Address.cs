namespace NoteAPI.API.DataContracts
{
    /// <summary>
    /// Physical address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Zip code
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}
