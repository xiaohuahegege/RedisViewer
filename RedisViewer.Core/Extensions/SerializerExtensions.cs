using Newtonsoft.Json;

namespace RedisViewer.Core
{
    public static class SerializerExtensions
    {
        public static T ToJsonObject<T>(this string json)
        {
            try
            {
                if (json != null)
                    return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {

            }

            return default;
        }

        public static string ToJsonString(this object obj, Formatting formatting = Formatting.None)
        {
            try
            {
                if (obj != null)
                    return JsonConvert.SerializeObject(obj, formatting);
            }
            catch
            {

            }

            return default;
        }
    }
}