using Microsoft.Extensions.Configuration;

namespace DAL
{
    public static class Setting
    {
        public static string Connection { get; private set; }

        public static void Init(IConfiguration config)
        {
            Connection = config.GetConnectionString("DefaultConnection");
        }
    }
}