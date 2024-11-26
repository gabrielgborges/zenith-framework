using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GameplayScreenView))]
public class GameplayScreenController : MonoBehaviour, IScreenController
{
    [SerializeField] private GameplayScreenModel _model;

    private GameplayScreenView _view;
    private IEventService _eventService;
    private ILoadingService _loadingService;

    public async void Initialize<T>(T screenView) where T : ScreenViewBase
    {
        _view = screenView as GameplayScreenView;
        if (_view != null)
        {
            _eventService = await ServiceLocator.GetService<IEventService>();
            _eventService.AddListener<StartMatchAnnounceEvent>(AnnounceMatchStart, GetHashCode());
            _eventService.AddListener<OnPlayerWin>(HandleOnPlayerWin, GetHashCode());

            _loadingService = await ServiceLocator.GetService<ILoadingService>();
        }
    }

    private void AnnounceMatchStart(StartMatchAnnounceEvent startMatchAnnounceEvent)
    {
        _model.ResetWins();
        _model.CurrentRound = 1;

        int count = _model.RoundsConfig.StartCountdown;
        StartCoroutine(PlayTimedSequence());
        IEnumerator PlayTimedSequence()
        {
            while (count > 0)
            {
                yield return new WaitForSeconds(1);
                _view.PlayStartGameAnimation(count.ToString());
                count--;
            }

            yield return new WaitForSeconds(1);
            _view.PlayStartGameAnimation("FIGHT!");

            yield return new WaitForSeconds(0.2f);
            _eventService.TryInvokeEvent(new FinishMatchAnnounceEvent());

            yield return new WaitForSeconds(0.8f);
            _view.PlayStartGameAnimation(string.Empty);
        }
    }

    private async void HandleOnPlayerWin(OnPlayerWin onPlayerWinEvent)
    {
        _model.AddWinner(onPlayerWinEvent.WinnerPlayer);
        if(_model.GetPlayerWins(onPlayerWinEvent.WinnerPlayer) >= _model.RoundsConfig.RoundsToWin)
        {
            _eventService.TryInvokeEvent(new OnEndGameEvent(onPlayerWinEvent.WinnerPlayer));
        }
        else
        {
            _view.SetMiddleText("ROUND END");

            await _loadingService.StartLoading();
            _eventService.TryInvokeEvent(new PrepareForNextRoundEvent());
            
            _view.SetMiddleText(string.Empty);
            await _loadingService.EndLoading();
            StartCoroutine(AnnounceNextRound());
        }
    }

    private IEnumerator AnnounceNextRound()
    {
        _view.SetMiddleText($"ROUND {_model.CurrentRound}");
        _model.CurrentRound++;

        yield return new WaitForSeconds(2);

        _view.SetMiddleText("FIGHT!");

        yield return new WaitForSeconds(0.2f);
        _eventService.TryInvokeEvent(new RetryMatchEvent());

        yield return new WaitForSeconds(0.3f);
        _view.SetMiddleText(string.Empty);

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}