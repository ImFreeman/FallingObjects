using System;
using Zenject;

public readonly struct PlayerDataMessage
{
    public readonly int DeltaHealth;
    public readonly int DeltaScore;

    public PlayerDataMessage(int deltaScore, int deltaHealth) : this()
    {
        DeltaScore = deltaScore;
        DeltaHealth = deltaHealth;
    }
}

public readonly struct HealthChangeMessage
{
    public readonly int Value;

    public HealthChangeMessage(int value)
    {
        Value = value;
    }
}

public readonly struct ScoreChangeMessage
{
    public readonly int Value;

    public ScoreChangeMessage(int value)
    {
        Value = value;
    }
}

public class StartNewGameMessage
{

}

public class PlayerDataHandler
{
    public int CurrentScore => _currentScore;

    private readonly PlayerConfig _config;
    private readonly SignalBus _signalBus;

    private int _currentScore;
    private int _currentHealth;

    public PlayerDataHandler(
        PlayerConfig config,
        SignalBus signalBus)
    {
        _config = config;
        _signalBus = signalBus;

        _signalBus.Subscribe<PlayerDataMessage>(OnDataChangeHandler);
        _signalBus.Subscribe<StartNewGameMessage>(StartNewGame);
    }

    private void OnDataChangeHandler(PlayerDataMessage data)
    {
        if(data.DeltaHealth != 0)
        {
            _currentHealth += data.DeltaHealth;
            _signalBus.Fire<HealthChangeMessage>(new HealthChangeMessage(_currentHealth));
        }
        if(data.DeltaScore != 0)
        {
            _currentScore += data.DeltaScore;
            _signalBus.Fire<ScoreChangeMessage>(new ScoreChangeMessage(_currentScore));
        }
    }

    private void StartNewGame()
    {
        _currentHealth = _config.StartHealth;
        _currentScore = 0;
        _signalBus.Fire<ScoreChangeMessage>(new ScoreChangeMessage(_currentScore));
        _signalBus.Fire<HealthChangeMessage>(new HealthChangeMessage(_currentHealth));
    }
}
