using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace GroupBrush.Web.Unity
{
    public class UnityDependencyResolver : DefaultDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        IUnityContainer _container = new UnityContainer();
        public UnityDependencyResolver()
        { }
        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }
        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return base.GetService(serviceType);
            }
        }
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                List<object> services = _container.ResolveAll(serviceType).ToList();
                object defaultService = GetService(serviceType);
                if (defaultService != null) services.Add(defaultService);
                return services;
            }
            catch
            {
                return base.GetServices(serviceType);
            }
        }
        public override void Register(Type serviceType, IEnumerable<Func<object>> activators)
        {
            _container.RegisterType(serviceType, new InjectionFactory((c) =>
            {
                object returnObject = null;
                foreach (Func<Object> activator in activators)
                {
                    object tempObject = activator.Invoke();
                    if (tempObject != null)
                    {
                        returnObject = tempObject;
                        break;
                    }
                }
                return returnObject;
            }));
            base.Register(serviceType, activators);
        }
        public void RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers)
        where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(injectionMembers);
        }
        public void RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(lifetimeManager, injectionMembers);
        }
        public override void Register(Type serviceType, Func<object> activator)
        {
            _container.RegisterType(serviceType, new InjectionFactory((c) => activator.Invoke()));
            base.Register(serviceType, activator);
        }
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            _container.RegisterInstance<TInterface>(instance);
        }
        public IDependencyScope BeginScope()
        {
            return new UnityDependencyResolver(_container.CreateChildContainer());
        }
    }
}
