using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class CharacterSelectionScreen : MonoBehaviour
{
    [SerializeField] private Canvas _background;
    [SerializeField] private Animator _foreground;
    [SerializeField] private PlayerSelection _player1;
    [SerializeField] private PlayerSelection _player2;
    [SerializeField] private GridLayoutGroup _characters;

    [SerializeField] private Combatent[] _combatentModules = new Combatent[2];
    [SerializeField] private GameObject _characterPreviewIlumination;

    private CombatentData[] _choosenCombatents = new CombatentData[2];

    private IEventService _eventService;

    async void Start()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();

        _eventService.AddListener<SelectedCharacterEvent>(HandleCharacterSelected, GetHashCode());
        //_characterPreviewIlumination.SetActive(true);
    }

    private void HandleCharacterSelected(SelectedCharacterEvent eventData)
    {
        if(!_player1.ConfirmedSelection)
        {
            if(eventData.ConfirmedSelection)
            {
                _player1.ConfirmCharacter();
                _choosenCombatents[0] = eventData.Combatent;
                return;
            }

            _player1.SelectCharacter(eventData.Combatent.Prefab, eventData.Combatent.animationSignature);
            return;
        }

        if (eventData.ConfirmedSelection)
        {
            _player2.ConfirmCharacter();
            _choosenCombatents[1] = eventData.Combatent;
            DisplayGameStart();
            return;
        }

        _player2.SelectCharacter(eventData.Combatent.Prefab, eventData.Combatent.animationSignature);
    }

    private async void DisplayGameStart()
    {
        _characters.gameObject.SetActive(false);
        await Task.Delay(1000);
        ILoadingService loadingService = await ServiceLocator.GetService<ILoadingService>();
        await loadingService.StartLoading();
        
        await Task.Delay(2000);
        _background.gameObject.SetActive(false);

        for (int i = 0; i < _combatentModules.Length; i++)
        {
            _eventService.TryInvokeEvent(new ConfirmedCharacterEvent(_choosenCombatents[i], _combatentModules[i]));
        }
        Destroy(_player1.CombatentPrefab);
        Destroy(_player2.CombatentPrefab);
        Destroy(gameObject);
        _characterPreviewIlumination.SetActive(false);
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<SelectedCharacterEvent>(GetHashCode());
    }
}

[Serializable]
internal struct PlayerSelection
{
    public GameObject CombatentPrefab;
    public bool ConfirmedSelection;
    [SerializeField] private Transform _prefabSpawnPosition;
    [SerializeField] private TextMeshProUGUI _nameText;

    public void SelectCharacter(GameObject prefab, AnimationSignature animation)
    {
        if(CombatentPrefab != null)
        {
            GameObject.Destroy(CombatentPrefab);
        }

        CombatentPrefab = GameObject.Instantiate(prefab, _prefabSpawnPosition);
        CombatentPrefab.GetComponent<ISignatureAnimator>().PlayAnimationSignature(animation);
        _nameText.text = prefab.name;
        FilterNamingConventions();
    }

    public void ConfirmCharacter()
    {
        ConfirmedSelection = true;
    }

    private void FilterNamingConventions()
    {
        _nameText.text = _nameText.text.Replace("Combatent", "");
    }
}
