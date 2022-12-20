using Zenject;
using UnityEngine;

public readonly struct FallingObjectControllerProtocol
{
    public readonly Sprite Sprite;
    public readonly Vector3 Position;
    public readonly float Speed;
    public readonly PlayerMessageModel OnClickMessage;
    public readonly PlayerMessageModel OnEndOfScreenMessage;
    public readonly AudioClip ClickSound;
    public readonly AudioClip EoSSound;

    public FallingObjectControllerProtocol(
        float speed,
        Vector3 position,
        Sprite sprite,
        PlayerMessageModel onClickMessage,
        PlayerMessageModel onEndOfScreenMessage,
        AudioClip clickSound,
        AudioClip eoSSound) : this()
    {
        Speed = speed;
        Position = position;
        Sprite = sprite;
        OnClickMessage = onClickMessage;
        OnEndOfScreenMessage = onEndOfScreenMessage;
        ClickSound = clickSound;
        EoSSound = eoSSound;
    }
}

public class FallingObjectController : ITickable
{
    private readonly Pool _controllerPool;
    private readonly FallingObjectView.Pool _viewPool;
    private readonly TickableManager _tickableManager;
    private readonly SignalBus _signalBus;
    private readonly GameplayAudioSource _gameplayAudioSource;


    private IFallingObjectView _view;
    private float _speed;

    private int _onClickScoreDelta;
    private int _onClickHealthDelta;
    private int _onEoSScoreDelta;
    private int _onEoSHealthDelta;

    private AudioClip _clickSound;
    private AudioClip _eosSound;

    public FallingObjectController(
        FallingObjectView.Pool viewPool,
        TickableManager tickableManager,
        SignalBus signalBus,
        Pool controllerPool, GameplayAudioSource gameplayAudioSource)
    {
        _viewPool = viewPool;
        _tickableManager = tickableManager;
        _signalBus = signalBus;
        _controllerPool = controllerPool;
        _gameplayAudioSource = gameplayAudioSource;
    }

    /// <summary>
    /// Âûחûגאועסÿ ךאזהûי ךאהנ
    /// </summary>
    public void Tick()
    {
        _view.BodyTransform.position += Vector3.down * Time.deltaTime * _speed;
    }

    private void Init(FallingObjectControllerProtocol protocol)
    {
        _view = _viewPool.Spawn(new FallingObjectViewProtocol(protocol.Sprite, protocol.Position));
        _view.OnClickEvent += ViewOnClickEvent;
        _view.EndOfScreenEvent += ViewEndOfScreenEvent;

        _speed = protocol.Speed;
        _onClickHealthDelta = protocol.OnClickMessage.DeltaHealth;
        _onClickScoreDelta = protocol.OnClickMessage.DeltaScore;
        _onEoSHealthDelta = protocol.OnEndOfScreenMessage.DeltaHealth;
        _onEoSScoreDelta = protocol.OnEndOfScreenMessage.DeltaScore;

        _clickSound = protocol.ClickSound;
        _eosSound = protocol.EoSSound;

        _signalBus.Subscribe<GameOverMessage>(Kill);

        _tickableManager.Add(this);
    }

    private void Dispose()
    {
        _tickableManager.Remove(this);

        _signalBus.Unsubscribe<GameOverMessage>(Kill);
        _view.OnClickEvent -= ViewOnClickEvent;
        _view.EndOfScreenEvent -= ViewEndOfScreenEvent;        

        _viewPool.Despawn(_view as FallingObjectView);
    }

    private void ViewEndOfScreenEvent(object sender, System.EventArgs e)
    {
        Kill();
        _signalBus.Fire(new PlayerDataMessage(_onEoSScoreDelta, _onEoSHealthDelta));
        PlaySound(_eosSound);        
    }

    private void ViewOnClickEvent(object sender, System.EventArgs e)
    {
        Kill();
        _signalBus.Fire(new PlayerDataMessage(_onClickScoreDelta, _onClickHealthDelta));
        PlaySound(_clickSound);        
    }

    private void Kill()
    {        
        _controllerPool.Despawn(this);
    }

    private void PlaySound(AudioClip clip)
    {
        _gameplayAudioSource.Play(clip);
    }        

    public class Pool : MemoryPool<FallingObjectControllerProtocol, FallingObjectController>
    {
        protected override void Reinitialize(FallingObjectControllerProtocol p1, FallingObjectController item)
        {
            item.Init(p1);
        }

        protected override void OnDespawned(FallingObjectController item)
        {
            item.Dispose();
        }
    }
}
