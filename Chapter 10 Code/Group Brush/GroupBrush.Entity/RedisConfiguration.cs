using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.Entity
{
    public class RedisConfiguration
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseRedis { get; set; }
        public string EventKey { get; set; }
        public RedisConfiguration(string hostName, string password, bool useRedis)
        {
            HostName = hostName;
            Password = password;
            UseRedis = useRedis;
            Port = 6379;
            EventKey = "GroupBrush";
        }
    }
}
