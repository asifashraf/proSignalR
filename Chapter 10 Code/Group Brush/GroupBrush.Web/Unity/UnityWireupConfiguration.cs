using GroupBrush.BL.Canvases;
using GroupBrush.BL.Storage;
using GroupBrush.BL.Users;
using GroupBrush.DL.Canvases;
using GroupBrush.DL.Users;
using GroupBrush.Entity;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.Web.Unity
{
    public class UnityWireupConfiguration
    {
        public static void WireUp(UnityDependencyResolver dependencyResolver)
        {
            string groupBrushRedisHostname = CloudConfigurationManager.GetSetting("GroupBrushRedisHostname");
            string groupBrushRedisPassword = CloudConfigurationManager.GetSetting("GroupBrushRedisPassword");
            string strUseRedis = CloudConfigurationManager.GetSetting("UseRedis") ?? "false";
            bool useRedis = bool.Parse(strUseRedis);
            RedisConfiguration redisConfiguration = new RedisConfiguration(groupBrushRedisHostname, groupBrushRedisPassword, useRedis);
            dependencyResolver.RegisterInstance<RedisConfiguration>(redisConfiguration);
            string groupBrushSQLConnectionString = CloudConfigurationManager.GetSetting("GroupBrushDB");
            dependencyResolver.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            dependencyResolver.RegisterType<ICanvasService, CanvasService>();
            dependencyResolver.RegisterType<ICanvasRoomService, CanvasRoomService>();
            if (useRedis)
            {
                dependencyResolver.RegisterType<IMemStorage, RedisStorage>(new ContainerControlledLifetimeManager(), new InjectionConstructor(redisConfiguration));
            }
            else
            {
                dependencyResolver.RegisterType<IMemStorage, MemoryStorage>(new ContainerControlledLifetimeManager());
            }
            dependencyResolver.Register(typeof(IGetUserNameFromIdData), () => new GetUserNameFromIdData(groupBrushSQLConnectionString));
            dependencyResolver.Register(typeof(IGetCanvasDescriptionData), () => new GetCanvasDescriptionData(groupBrushSQLConnectionString));
            dependencyResolver.Register(typeof(ICreateUserData), () => new CreateUserData(groupBrushSQLConnectionString));
            dependencyResolver.Register(typeof(IValidateUserData), () => new ValidateUserData(groupBrushSQLConnectionString));
            dependencyResolver.Register(typeof(ICreateCanvasData), () => new CreateCanvasData(groupBrushSQLConnectionString));
            dependencyResolver.Register(typeof(ILookUpCanvasData), () => new LookUpCanvasData(groupBrushSQLConnectionString));
        }
    }
}
