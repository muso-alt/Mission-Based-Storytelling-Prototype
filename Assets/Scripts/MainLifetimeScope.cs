using Unfrozen.Controllers;
using Unfrozen.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unfrozen
{
    public class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainConfig _mainConfig;
        [SerializeField] private MainScreenView _mainScreenView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_mainScreenView);
            builder.RegisterComponent(_mainConfig);
            builder.RegisterEntryPoint<MissionsController>();
        }
    }
}