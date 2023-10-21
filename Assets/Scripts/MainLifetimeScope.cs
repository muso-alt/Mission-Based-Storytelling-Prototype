using Unfrozen.Configs;
using Unfrozen.Controllers;
using Unfrozen.Models;
using Unfrozen.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unfrozen
{
    public class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainConfig _mainConfig;
        [SerializeField] private HeroesConfig _heroesConfig;
        [SerializeField] private MainScreenView _mainScreenView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_mainScreenView);
            builder.RegisterComponent(_mainConfig);
            builder.RegisterComponent(_heroesConfig);
            
            builder.RegisterEntryPoint<MissionsMapController>();
            builder.RegisterEntryPoint<MissionController>();
            builder.RegisterEntryPoint<HeroesController>();
            
            builder.Register<MissionsModel>(Lifetime.Singleton);
            builder.Register<HeroesModel>(Lifetime.Singleton);
        }
    }
}