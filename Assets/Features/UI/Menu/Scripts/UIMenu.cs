using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIMenu : UIWindow
{
    [SerializeField] private Button playButton;
    [SerializeField] private UIButtonHoverHandler hoverHander;
    [SerializeField] private AudioClip onButtonHoverSound;
    [SerializeField] private AudioClip onButtonClickSound;    

    private UIService _uIService;
    private SignalBus _signalBus;
    private UIAudioSource _uIAudioSource;

    [Inject]
    public void Inject(
        UIService uIService,
        SignalBus signalBus,
        UIAudioSource uIAudioSource)
    {
        _uIAudioSource = uIAudioSource;
        _uIService = uIService;
        _signalBus = signalBus;
    }
    
    public override void Hide()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        hoverHander.OnHover -= HoverHanderOnHover;
    }

    public override void Show()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        hoverHander.OnHover += HoverHanderOnHover;        
    }

    private void HoverHanderOnHover(object sender, System.EventArgs e)
    {        
        _uIAudioSource.Play(onButtonHoverSound);
    }    

    private void OnPlayButtonClick()
    {
        _uIAudioSource.Play(onButtonClickSound);

        _uIService.Hide<UIMenu>();
        _uIService.Show<UIHud>();

        _signalBus.Fire<StartNewGameMessage>();
    }
}
