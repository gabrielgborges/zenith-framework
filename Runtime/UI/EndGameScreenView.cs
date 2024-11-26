using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreenView : ScreenViewBase
{
    [SerializeField] private Image _screenLayout;
    [SerializeField] private TextMeshProUGUI _playerWinnerText;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _characterSelectionButton;
    [SerializeField] private Button _quitGameButton;

    private IEventService _eventService;
    private ILoadingService _loadingService;

    private async void Awake()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.AddListener<OnEndGameEvent>(OpenScreen, GetHashCode());

        _loadingService = await ServiceLocator.GetService<ILoadingService>();
        
        _retryButton.onClick.AddListener(HandleRetryGame);
        _characterSelectionButton.onClick.AddListener(HandleCharacterSelection);
        _quitGameButton.onClick.AddListener(HandleQuitGame);
    }

    private void OpenScreen(OnEndGameEvent endGameEvent)
    {
        _playerWinnerText.text = endGameEvent.WinnerPlayer + " WINS!";
        _screenLayout.gameObject.SetActive(true);
        _retryButton.Select();
    }

    private void CloseScreen()
    {
        _screenLayout.gameObject.SetActive(false);
    }

    private async void HandleRetryGame()
    {
        CloseScreen();
        await _loadingService.StartLoading();
        _eventService.TryInvokeEvent(new PrepareForNextRoundEvent());
        await _loadingService.EndLoading();
        _eventService.TryInvokeEvent(new StartMatchAnnounceEvent());
    }

    private void HandleCharacterSelection()
    {
        _eventService.TryInvokeEvent(new GoToCharacterSelectionEvent());
        CloseScreen();
    }

    private void HandleQuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<OnEndGameEvent>(GetHashCode());
    }
}
