using System;

namespace NoteAPI.API.DataContracts.Responses
{
    /// <summary>
    /// Geeric response class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    public class Response<T, R>
    {
        /// <summary>
        /// Request
        /// </summary>
        public T Request { get; set; }

        /// <summary>
        /// Correlation Id
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Request date
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Response date
        /// </summary>
        public DateTime ResponseDate { get; set; }
        

        /// <summary>
        /// Response content
        /// </summary>
        public R ResponseContent { get; set; }

        /// <summary>
        /// Defines if the request has been processed successfully
        /// </summary>
        public bool IsSuccessfull { get; set; }

        /// <summary>
        /// Error message(s)
        /// </summary>
        public string Error { get; set; }
    }
}
