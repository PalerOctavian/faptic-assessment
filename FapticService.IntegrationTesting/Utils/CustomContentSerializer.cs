using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace FapticService.IntegrationTesting.Utils;

public class CustomContentSerializer : IHttpContentSerializer
{
    private readonly JsonSerializerOptions jsonSerializerOptions;
    
    public CustomContentSerializer() : this(new JsonSerializerOptions(JsonSerializerDefaults.Web))
    {
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        jsonSerializerOptions.WriteIndented = true;
        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
    }
    
    public CustomContentSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        this.jsonSerializerOptions = jsonSerializerOptions;
    }

    public HttpContent ToHttpContent<T>(T item)
    {
        var content = JsonContent.Create(item, options: jsonSerializerOptions);
        return content;
    }

    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
    {
        if (content.Headers.ContentType != null && content.Headers.ContentType.MediaType == "application/json")
        {
            var item = await content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
            return item;
        }

        return default;
    }

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
        {
            throw new ArgumentNullException(nameof(propertyInfo));
        }

#pragma warning disable CS8603
        return propertyInfo.GetCustomAttributes<JsonPropertyNameAttribute>(true)
            .Select(a => a.Name)
            .FirstOrDefault();
#pragma warning restore CS8603
    }
}
