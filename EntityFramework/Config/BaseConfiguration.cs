using System.Configuration;

namespace EntityFramework.Config
{
    public class BaseConfiguration
    {
        protected BaseConfiguration()
        {
        }

        protected static object? GetAppSetting(Type expectedType, string key)
        {
            string? value = ConfigurationManager.AppSettings?.Get(key);
            try
            {
                return ConvertValue(expectedType, value);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Config key:{0} was expected to be of type {1} but was not.",
                    key, expectedType), ex);
            }
        }

        protected static object? GetConnectionString(Type expectedType, string key)
        {
            string? value = ConfigurationManager.ConnectionStrings?.Cast<ConnectionStringSettings>()?.FirstOrDefault(x => x.Name == key)?.ConnectionString;
            try
            {
                return ConvertValue(expectedType, value);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Config key:{0} was expected to be of type {1} but was not.",
                    key, expectedType), ex);
            }
        }

        private static object? ConvertValue(Type expectedType, string? value)
        {
            if (value == null)
            {
                return null;
            }
            if (expectedType == typeof(int))
            {
                return int.Parse(value);
            }
            if (expectedType == typeof(string))
            {
                return value;
            }
            throw new Exception("Type not supported.");
        }
    }
}
