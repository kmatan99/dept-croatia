using dept_croatia.Infrastructure.Models;
using System.Reflection;
using System.Text.Json;
using System.Web;

namespace dept_croatia.Infrastructure.Extensions
{
    public static class HttpExtension
    {
        public async static Task<HttpResponseMessage> GetWithFiltersAsync(this HttpClient client, string requestUrl, FilterOptions filterOptions)
        {
            var queryParams = ConstructQueryParams(filterOptions);

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

        private static List<string> ConstructQueryParams(FilterOptions filterOptions) 
        {
            var queryParams = new List<string>();

            if (filterOptions is not null) 
            {
                foreach (PropertyInfo property in filterOptions.GetType().GetProperties())
                {
                    var value = property.GetValue(filterOptions);

                    if (value == null)
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
