using System;
using Hangfire;
using Unity;

namespace Project.Server.Hangfire
{
    public class HangfireUnityJobActivator : JobActivator
    {
        private readonly IUnityContainer _container;
        public HangfireUnityJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.Resolve(jobType);
        }
    }
}