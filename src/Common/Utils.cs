using Newtonsoft.Json;

namespace Common
{
    public class Utils
    {
        public static T TransformTo<T>(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"Could not convert null object {nameof(obj)}");
            }

            string json = JsonConvert.SerializeObject(obj);
            T? result = JsonConvert.DeserializeObject<T>(json);

            if (result == null)
            {
                throw new Exception($"Failed to deserialize {nameof(obj)} to {typeof(T).FullName}");
            }

            return result;
        }

    }
}
