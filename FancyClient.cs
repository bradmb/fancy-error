namespace FancyError
{
    public class FancyClient
    {
        public void Configure(Models.ClientConfiguration config)
        {
            InternalData.Configuration = config;
        }
    }
}
