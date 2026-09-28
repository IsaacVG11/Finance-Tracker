using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinanceTracker.Web.Services.Http
{
    public static class ApiJsonOptions
    {
        public static JsonSerializerOptions Default { get; } = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)
            }
        };
    }
}
