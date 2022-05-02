namespace LOGIC.Services.Models
{
    public class GenericResultSet<T> : StandardObjectResult
    {
        public T ResultSet { get; set; }
    }
}
