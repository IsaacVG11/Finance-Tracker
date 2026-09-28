using System.Net;
using System.Text.Json;
using FinanceTracker.Web.Exceptions;

namespace FinanceTracker.Web.Services.Http;

public static class ApiResponseHandler
{
    public static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        var mensaje = ObtenerMensajePorStatusCode(response.StatusCode);

        Dictionary<string, string[]> errores = [];

        if (!string.IsNullOrWhiteSpace(contenido))
        {
            try
            {
                using var document = JsonDocument.Parse(contenido);

                var root = document.RootElement;

                if (root.TryGetProperty("mensaje", out var mensajeElement))
                {
                    mensaje = mensajeElement.GetString() ?? mensaje;
                }
                else if (root.TryGetProperty("detail", out var detailElement))
                {
                    mensaje = detailElement.GetString() ?? mensaje;
                }
                else if (root.TryGetProperty("title", out var titleElement))
                {
                    mensaje = titleElement.GetString() ?? mensaje;
                }

                if (root.TryGetProperty("errors", out var errorsElement))
                {
                    foreach (var property in errorsElement.EnumerateObject())
                    {
                        errores[property.Name] = property.Value
                                .EnumerateArray()
                                .Select(
                                    e => e.GetString()
                                         ?? string.Empty)
                                .Where(
                                    e => !string.IsNullOrWhiteSpace(e))
                                .ToArray();
                    }
                }
            }
            catch (JsonException)
            {
                // El body no tenía JSON válido.
            }
        }

        if (errores.Count > 0)
        {
            mensaje = "Uno o más campos contienen errores.";
        }

        throw new ApiException(mensaje, response.StatusCode, errores);
    }

    private static string ObtenerMensajePorStatusCode(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest =>
                "La solicitud contiene datos inválidos.",

            HttpStatusCode.NotFound =>
                "El recurso solicitado no existe.",

            HttpStatusCode.Conflict =>
                "La operación entra en conflicto con el estado actual del recurso.",

            HttpStatusCode.InternalServerError =>
                "Ocurrió un error interno del servidor.",

            HttpStatusCode.Unauthorized =>
                "Debes autenticarte para realizar esta operación.",

            HttpStatusCode.Forbidden =>
                "No tienes permisos para realizar esta operación.",

            _ =>
                "Ocurrió un error procesando la solicitud."
        };
    }
}