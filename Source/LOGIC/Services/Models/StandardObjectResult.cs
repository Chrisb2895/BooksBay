namespace LOGIC.Services.Models
{
    public abstract class StandardObjectResult
    {
        public bool Success { get; set; }

        public string UserMessage { get; set; }

        public string InternalMessage { get; set; }

        public Exception Exception { get; set; }

        public StandardObjectResult()
        {
            Success = false;

            UserMessage = string.Empty;

            InternalMessage = string.Empty;

            Exception = null;
        }
    }
}
