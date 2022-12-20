using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ApplicationInstaller : MonoInstaller<ApplicationInstaller>
{
    public override void InstallBindings()
    {
        //Signals
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerDataMessage>();
        Container.DeclareSignal<HealthChangeMessage>();
        Container.DeclareSignal<ScoreChangeMessage>();
        Container.DeclareSignal<StartNewGameMessage>();
        Container.DeclareSignal<GameOverMessage>();

        //UI
        Container
            .Bind<UIService>()
            .AsSingle();
        Container
            .Bind<UIAudioSource>()
            .AsSingle();

        //Player
        Container
            .Bind<PlayerDataHandler>()
            .AsSingle();
        Container
            .Bind<PlayerConfig>()
            .FromScriptableObjectResource("Player")
            .AsSingle();

        //FallingObjects
        Container
            .Bind<FallingObjectConfig>()
            .FromScriptableObjectResource("FallingObjects/FallingObjectConfig")
            .AsSingle();
        Container
            .Bind<FallingObjectsFallingConfig>()
            .FromScriptableObjectResource("FallingObjects/FallingObjectsFallingConfig")
            .AsSingle();
        Container
            .Bind<GameplayAudioSource>()
            .AsSingle();
        Container
            .BindMemoryPool<FallingObjectController, FallingObjectController.Pool>()
            .WithInitialSize(20);        
        Container
            .BindMemoryPool<FallingObjectView, FallingObjectView.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefabResource("FallingObjects/FallingObjectPrefab");

        Container
            .Bind<GameStatus>()
            .AsSingle()
            .NonLazy();
        Container
            .Bind<ApplicationLauncher>()
            .AsSingle()
            .NonLazy();
    }
}
