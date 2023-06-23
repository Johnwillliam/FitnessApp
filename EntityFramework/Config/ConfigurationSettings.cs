namespace EntityFramework.Config
{
    public class ConfigurationSettings : BaseConfiguration
    {
        #region App setting

        public static string DefaultConnectionStrings
        {
            get
            {
                var connectionString = GetConnectionString(typeof(string), "FitnessApp");
                if (connectionString == null)
                {
                    return string.Empty;
                }
                return connectionString.ToString();
            }
        }

        #endregion App setting
    }
}
