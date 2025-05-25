using System;
using System.Threading.Tasks;

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
        /// Constructer
        /// </summary>
        public Response(T request, Task<R> task = null)
        {
            CorrelationId = Guid.NewGuid().ToString();
            Request = request;

            RequestDate = DateTime.Now;
            ResultTask = task;
        }

        /// <summary>
        /// Execute Task
        /// </summary>
        public async Task ExecuteTask()
        {
            if (ResultTask != null)
            {
                try
                {
                    ResponseContent = await ResultTask;
                    IsSuccessfull = true;
                    ResponseDate = DateTime.Now;
                }
                catch (Exception e)
                {
                    IsSuccessfull = false;
                    Error = e.Message;
                }
            }
        }

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


        private Task<R> ResultTask { get; set; }
    }


    public class Response<R>
    {
        /// <summary>
        /// Constructer
        /// </summary>
        public Response(Task<R> task = null)
        {
            CorrelationId = Guid.NewGuid().ToString();
            
            RequestDate = DateTime.Now;
            ResultTask = task;
        }

        /// <summary>
        /// Execute Task
        /// </summary>
        public async Task ExecuteTask()
        {
            if (ResultTask != null)
            {
                try
                {
                    ResponseContent = await ResultTask;
                    IsSuccessfull = true;
                    ResponseDate = DateTime.Now;
                }
                catch (Exception e)
                {
                    IsSuccessfull = false;
                    Error = e.Message;
                }
            }
        }
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


        private Task<R> ResultTask { get; set; }
    }
}