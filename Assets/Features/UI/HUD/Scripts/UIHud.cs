using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIHud : UIWindow
{
    [SerializeField] private Image[] health;
    [SerializeField] private TMP_Text score;
    [SerializeField] private AudioClip backgroundMusic;

    private SignalBus _signalBus;
    private UIAudioSource _audio;

    [Inject]
    public void Inject(
        SignalBus signalBus,
        UIAudioSource audio)
    {
        _signalBus = signalBus;
        _audio = audio;
    }

    public override void Hide()
    {
        _signalBus.Unsubscribe<HealthChangeMessage>(OnHealthChange);
        _audio.Stop();
        _audio.IsLooped = false;
    }

    public override void Show()
    {
        _signalBus.Subscribe<HealthChangeMessage>(OnHealthChange);
        _signalBus.Subscribe<ScoreChangeMessage>(OnScoreChange);
        Invoke("PlayBackgroundMusic", 1f);
        
    }

    private void PlayBackgroundMusic()
    {
        _audio.IsLooped = true;
        _audio.Play(backgroundMusic);
    }

    private void OnScoreChange(ScoreChangeMessage args)
    {
        score.text = args.Value.ToString();
    }

    private void OnHealthChange(HealthChangeMessage args)
    {
        for (int i = 0; i < health.Length; i++)
        {
            health[i].gameObject.SetActive(i < args.Value);
        }
    }
}
