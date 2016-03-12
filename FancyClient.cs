namespace FancyError
{
    public class FancyClient
    {
        public static void Configure(Models.ClientConfiguration config)
        {
            InternalData.Configuration = config;
        }
    }
}
