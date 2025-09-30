using BSB.src.Domain.Entities;
using Newtonsoft.Json;
using System.Dynamic;

namespace BSB.src.Common
{
    public class Utils
    {
        public static T? TransformTo<T>(object? obj)
        {
            if (obj is null || obj is DBNull)
            {
                throw new ArgumentNullException($"Could not convert null object {nameof(obj)}");
            }

            string json = JsonConvert.SerializeObject(obj);
            T? result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }

        public static string GenerateHash(string password)
        {
            return password;
        }

        public static string ResolveUserName(string firstName, string lastName)
        {
            return firstName + "-" + lastName;
        }

        public static object? ResolveUserName(string username)
        {
            dynamic fullName = new { firstName = username, lastName = string.Empty };

            if (!string.IsNullOrWhiteSpace(username) && username.Contains('-'))
            {
                string[] name = username.Split("-");

                if (string.IsNullOrWhiteSpace(name[1]))
                {
                    fullName = new { firstName = name[0] };
                }

                fullName = new { firstName = name[0], lastName = name[1] };
            }
            
            return fullName;
        }
    }
}
