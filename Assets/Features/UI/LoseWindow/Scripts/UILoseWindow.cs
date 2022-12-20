using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UILoseWindow : UIWindow
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button restartButton;

    private PlayerDataHandler _playerData;

    [Inject]
    public void Inject(PlayerDataHandler playerData)
    {
        _playerData = playerData;
    }

    public override void Hide()
    {
        restartButton.onClick.RemoveListener(OnRestartButtonClickHandler);
    }

    public override void Show()
    {
        scoreText.text = _playerData.CurrentScore.ToString();

        restartButton.onClick.AddListener(OnRestartButtonClickHandler);
    }

    private void OnRestartButtonClickHandler()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
