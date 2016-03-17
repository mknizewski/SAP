using Ninject;
using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses;
using SAP.BOL.LogicClasses.Managers;
using SAP.DAL.Abstract;
using SAP.DAL.Repositories;
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
            _kernel.Bind<ITournamentRepository>().To<TournamentRepository>();
            _kernel.Bind<ITournamentManager>().To<TournamentManager>();

            _kernel.Bind<ICompilerRespository>().To<CompilerRepository>();
            _kernel.Bind<IProgramManager>().To<ProgramManager>();

            _kernel.Bind<IContactRepository>().To<ContactRepository>();
            _kernel.Bind<IContactManager>().To<ContactManager>();

            _kernel.Bind<IUserRepository>().To<UserRepository>();
            _kernel.Bind<IUserManager>().To<UserManager>();
        }
    }
}