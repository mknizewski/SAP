using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SAP.Web.Infrastructrue.IoC
{
    public class NinjectDepedencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDepedencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        //bindy do IoC
        private void AddBindings()
        {
        }
    }
}