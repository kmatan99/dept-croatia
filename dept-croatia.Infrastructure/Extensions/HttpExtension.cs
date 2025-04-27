using dept_croatia.Infrastructure.Models;
using System.Reflection;
using System.Text.Json;
using System.Web;

namespace dept_croatia.Infrastructure.Extensions
{
    public static class HttpExtension
    {
        public async static Task<HttpResponseMessage> GetWithFiltersAsync<T>(this HttpClient client, string requestUrl, T filterOptions, string? apiKey = null)
        {
            var queryParams = ConstructQueryParams(filterOptions, apiKey);

            if (queryParams.Count > 0)
            {
                var queryString = string.Join("&", queryParams);
                requestUrl += $"?{queryString}";
            }

            return await client.GetAsync(requestUrl);
        }

        public async static Task<T> ReadFromStream<T>(this HttpContent httpContent) where T : new()
        {
            if (httpContent == null)
                return new T();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var stream = await httpContent.ReadAsStreamAsync();

                if (stream == null || stream.Length == 0)
                {
                    return new T();
                }

                return await JsonSerializer.DeserializeAsync<T>(stream, options);
            }
            catch
            {
                return new T();
            }
        }

        private static List<string> ConstructQueryParams<T>(T filterOptions, string? apiKey) 
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                queryParams.Add($"key={apiKey}");
            }

            if (filterOptions is not null) 
            {
                foreach (PropertyInfo property in filterOptions.GetType().GetProperties())
                {
                    var value = property.GetValue(filterOptions);

                    if (value == null)
                        continue;

                    if (property.Name.ToLower() == "page")
                        continue;

                    if (value is string paramValue && string.IsNullOrWhiteSpace(paramValue))
                        continue;

                    var query = value switch
                    {
                        string strValue => strValue,
                        _ => value.ToString()
                    };

                    queryParams.Add($"{property.Name.ToLower()}={HttpUtility.UrlEncode(query)}");
                }
            }

            return queryParams;
        }
    }
}
