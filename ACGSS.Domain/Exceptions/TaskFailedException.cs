namespace ACGSS.Domain.Exceptions
{
    public class TaskFailedException : Exception
    {
        public TaskFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
