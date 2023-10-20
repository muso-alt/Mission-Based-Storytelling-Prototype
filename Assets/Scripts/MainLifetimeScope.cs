using Unfrozen.Controllers;
using VContainer;
using VContainer.Unity;

namespace Unfrozen
{
    public class MainLifetimeScope : LifetimeScope
    {
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TasksController>();
        }
    }
}