using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private GameObject _battleground;
    [SerializeField] private GameObject _battleUI;
    [SerializeField] private Transform[] _combatentsSpawnPosition = new Transform[2];

    private Combatent[] _combatents = new Combatent[2];

    private bool _fightIsOn = false;

    private IEventService _eventService;
    private ILoadingService _loadingService;

    private async void Start()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
        _loadingService = await ServiceLocator.GetService<ILoadingService>();
        _eventService.AddListener<ConfirmedCharacterEvent>(SetupCombatent, GetHashCode());
        _eventService.AddListener<GoToCharacterSelectionEvent>(HandleSelectCharacters, GetHashCode());
        _eventService.AddListener<RetryMatchEvent>(HandleRetryMatch, GetHashCode());
        _eventService.AddListener<FinishMatchAnnounceEvent>(HandleMatchAnnounceEnded, GetHashCode());
        _eventService.AddListener<PrepareForNextRoundEvent>(RestorePlayersToInitialState, GetHashCode());
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<ConfirmedCharacterEvent>(GetHashCode());
        _eventService.RemoveListener<GoToCharacterSelectionEvent>(GetHashCode());
        _eventService.RemoveListener<RetryMatchEvent>(GetHashCode());
        _eventService.RemoveListener<FinishMatchAnnounceEvent>(GetHashCode());
        _eventService.RemoveListener<PrepareForNextRoundEvent>(GetHashCode());
    }

    private async void SetupCombatent(ConfirmedCharacterEvent confirmedCharacter)
    {
        _battleUI.SetActive(true);

        for (int i = 0; i < _combatents.Length; i++)
        {
            if (_combatents[i] == null)
            {
                Combatent combatent = Instantiate(confirmedCharacter.CombatentModule, _combatentsSpawnPosition[i]);
                combatent.SetType(confirmedCharacter.CombatentData);
                combatent.OnDie = HandleCombatentDied;
                _combatents[i] = combatent;
                _eventService.TryInvokeEvent(new OnSetUpPlayerEvent(i + 1, combatent));

                if(i + 1 == _combatents.Length)
                {
                    PrepareBattleground();
                    await _loadingService.EndLoading();
                    _eventService.TryInvokeEvent(new StartMatchAnnounceEvent());
                }
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_fightIsOn)
        {
            UpdateCombatentInputs();
        }
    }

    private void HandleMatchAnnounceEnded(FinishMatchAnnounceEvent matchAnnounceEvent)
    {
        StartMatch();
    }

    private void PrepareBattleground()
    {
        _battleground.SetActive(true);
    }
    
    private async void StartMatch()
    {
        await Task.Delay(1000);
        _eventService.TryInvokeEvent(new EnableInputsEvent(InputType.COMBAT, true));
        _eventService.TryInvokeEvent(new EnableInputsEvent(InputType.NAVIGATION, false));
        _fightIsOn = true;
    }

    private void UpdateCombatentInputs()
    {
        foreach (Combatent combatent in _combatents)
        {
            combatent.OnUpdate();
        }
    }
    
    private void HandleCombatentDied(Combatent deadCombatent)
    {
        _eventService.TryInvokeEvent(new EnableInputsEvent(InputType.COMBAT, false));

        for (int i = 0; i < _combatents.Length; i++)
        {
            if (_combatents[0] == deadCombatent)
            {
                continue;
            }
            _eventService.TryInvokeEvent(new OnPlayerWin("PLAYER " + i));
        }
    }

    private void HandleSelectCharacters(GoToCharacterSelectionEvent gameEvent)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RestorePlayersToInitialState(PrepareForNextRoundEvent gameEvent)
    {
        for (int i = 0; i < _combatents.Length; i++)
        {
            Combatent combatent = _combatents[i];
            combatent.SetupInitialState();
            combatent.transform.position = _combatentsSpawnPosition[i].position;
            _eventService.TryInvokeEvent(new OnSetUpPlayerEvent(i + 1, combatent));
        }
    }
    
    private void HandleRetryMatch(RetryMatchEvent retryEvent)
    {
        StartMatch();
    }
}
