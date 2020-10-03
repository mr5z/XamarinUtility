using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XamarinUtility.Extensions;

namespace XamarinUtility
{
    public static class QueryStringHelper
    {
        private const string TokenSeparator = "&";

        public static string ToQueryString(object model, JsonSerializer serializer = null)
        {
            if (model is string stringValue)
                return stringValue;

            var jsonObject = Deserialize(model, serializer);
            return string.Join(
                TokenSeparator,
                jsonObject.Children()
                    .Cast<JProperty>()
                    .Where(e => IsNotDefault(e.Value))
                    .Select(e => ToKeyValuePair(e, serializer))
            );
        }

        public static T ToObject<T>(Uri uri)
            where T : class, new()
        {
            if (IsRelativeUri(uri))
            {
                var dummyUrl = new Uri("http://localhost");
                uri = new Uri(dummyUrl, uri);
            }
            return ToObject<T>(uri.Query);
        }

        public static T ToObject<T>(string queryString)
            where T : class, new()
        {
            var keyValue = ParseQueryString(queryString);
            var dictionary = ToDictionary(keyValue);
            return dictionary.ToObject<T>();
        }

        // Taken from https://stackoverflow.com/a/68803/2304737
        private static NameValueCollection ParseQueryString(string queryString)
        {
            var queryParameters = new NameValueCollection();
            var querySegments = queryString.Split('&');
            foreach (string segment in querySegments)
            {
                var parts = segment.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim('?', ' ');
                    var val = parts[1].Trim();
                    queryParameters.Add(key, val);
                }
                else if (parts.Length == 1)
                {
                    queryParameters.Add(parts.First(), string.Empty);
                }
            }
            return queryParameters;
        }

        private static bool IsRelativeUri(Uri uri)
        {
            return !Uri.TryCreate(uri.OriginalString, UriKind.Absolute, out var _);
        }

        private static IDictionary<string, string> ToDictionary(NameValueCollection nameValue)
        {
            return nameValue.AllKeys.ToDictionary(key => key, value => nameValue[value]);
        }

        private static JsonSerializerSettings ToSerializerSettings(JsonSerializer serializer)
        {
            if (serializer == null)
                return null;

            return new JsonSerializerSettings
            {
                DateFormatString = serializer.DateFormatString,
                DateFormatHandling = serializer.DateFormatHandling,
                DateParseHandling = serializer.DateParseHandling,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling
            };
        }

        private static string Serialize(object model, JsonSerializerSettings settings = null)
        {
            return settings == null ?
                JsonConvert.SerializeObject(model) :
                JsonConvert.SerializeObject(model, settings);
        }

        private static JObject Deserialize(object model, JsonSerializer serializer)
        {
            var result = serializer == null ?
                JObject.FromObject(model) :
                JObject.FromObject(model, serializer);
            return result;
        }

        private static JObject Deserialize(string model, JsonSerializerSettings settings = null)
        {
            var result = settings == null ?
                JsonConvert.DeserializeObject(model) :
                JsonConvert.DeserializeObject(model, settings);
            return result as JObject;
        }

        private static string ToKeyValuePair(JProperty property, JsonSerializer serializer = null)
        {
            return property.Value.Type switch
            {
                JTokenType.Array => ToQueryStringArray(property.Name, property.Value),
                // TODO
                // check all possible structs
                JTokenType.Object => string.Empty,
                _ => $"{property.Name}={ToEscapedValue(property.Value, serializer)}",
            };
        }

        private static string ToQueryStringArray(string name, JToken token, JsonSerializer serializer = null)
        {
            var builder = new List<string>();
            var index = 0;
            foreach(var value in token.Values())
            {
                builder.Add($"{name}[{index++}]={ToEscapedValue(value, serializer)}");
            }
            return string.Join(TokenSeparator, builder);
        }

        private static string ToEscapedValue(JToken token, JsonSerializer serializer = null)
        {
            var stringValue = JTokenConvert(token, serializer: serializer)?.ToString();
            return string.IsNullOrEmpty(stringValue) ? string.Empty : Uri.EscapeDataString(stringValue);
        }

        private static object JTokenConvert(JToken token, int index = 0, JsonSerializer serializer = null)
        {
            return token.Type switch
            {
                JTokenType.Boolean => token.ToObject<bool>(),
                JTokenType.Float => token.ToObject<float>(),
                JTokenType.Integer => token.ToObject<int>(),
                JTokenType.String => token.ToObject<string>(),
                JTokenType.TimeSpan => token.ToObject<TimeSpan>(),
                JTokenType.Guid => token.ToObject<Guid>(),
                JTokenType.Date => ToDateTime(token, serializer),
                JTokenType.Object => JTokenConvert(token.Values().ToList()[index], index + 1),
                _ => default
            };
        }

        private static object ToDateTime(JToken token, JsonSerializer serializer = null)
        {
            if (serializer == null)
                return token.ToObject<DateTime>();
            return token.ToObject<DateTime>().ToString(serializer.DateFormatString, serializer.Culture);
        }

        private static bool IsNotDefault(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Boolean => ObjectHelper.IsNotDefault((bool)token),
                JTokenType.Integer => ObjectHelper.IsNotDefault((bool)token),
                JTokenType.Float => ObjectHelper.IsNotDefault((float)token),
                JTokenType.String => !string.IsNullOrEmpty((string)token),
                _ => token.Type != JTokenType.Null,
            };
        }
    }
}

