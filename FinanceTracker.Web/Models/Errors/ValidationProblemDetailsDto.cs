namespace FinanceTracker.Web.Models.Errors
{
    public class ValidationProblemDetailsDto :ApiProblemDetails
    {
        public Dictionary<string, string[]> Errors { get; set; } = [];
    }
}
