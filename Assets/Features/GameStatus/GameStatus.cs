using UnityEngine;
using Zenject;

public class GameOverMessage
{

}

public class GameStatus : ITickable
{
    private readonly SignalBus _signalBus;
    private readonly FallingObjectConfig _fallingObjectConfig;
    private readonly FallingObjectController.Pool _pool;
    private readonly TickableManager _tickableManager;
    private readonly UIService _uIService;
    private readonly System.Random _random = new System.Random();    

    private readonly float _spawnDelay;
    private readonly int _leftX = -7;
    private readonly int _righttX = 7;
    private readonly float _y = 5f;

    private float _currentTime;    


    public GameStatus(
        SignalBus signalBus,
        FallingObjectConfig fallingObjectConfig,
        FallingObjectController.Pool pool, 
        TickableManager tickableManager, 
        UIService uIService,
        FallingObjectsFallingConfig fallConfig)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<StartNewGameMessage>(StartGame);
        _signalBus.Subscribe<HealthChangeMessage>(OnHealthChangeHandler);
        _fallingObjectConfig = fallingObjectConfig;
        _pool = pool;
        _tickableManager = tickableManager;
        _uIService = uIService;
        _spawnDelay = fallConfig.SpawnDelay;
    }

    private void OnHealthChangeHandler(HealthChangeMessage value)
    {
        if(value.Value == 0)
        {
            GameOver();
        }
    }

    public void Tick()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= _spawnDelay)
        {
            var modelID = _random.Next(0, _fallingObjectConfig.Models.Length);
            var x = _random.Next(_leftX, _righttX);

            var model = _fallingObjectConfig.Models[modelID];

            var obj = _pool.Spawn(new FallingObjectControllerProtocol(
                model.Speed,
                new Vector3(x, _y, 0),
                model.Sprite,
                model.PlayerMessageOnClickModel,
                model.PlayerMessageEndOfScreenModel,
                model.OnClickSound,
                model.OnEoSSound));            

            _currentTime = 0;
        }
    }

    private void StartGame()
    {
        _tickableManager.Add(this);        
    }

    private void GameOver()
    {
        _tickableManager.Remove(this);
        _signalBus.Fire<GameOverMessage>();
        _uIService.Hide<UIHud>();
        _uIService.Show<UILoseWindow>();
    }
}
