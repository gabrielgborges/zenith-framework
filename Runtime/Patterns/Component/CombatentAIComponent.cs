using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatentAIComponent : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private InputEventQueue _inputQueue;

    private InputCommandMap _inputMap = new InputCommandMap();

    private string[] _inputs; 
    private bool _isEnabled = false;
    private InputType _inputType = InputType.COMBAT;
    private IEventService _eventService;

    private async void Awake()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.AddListener<EnableInputsEvent>(HandleEnableInputs, GetHashCode());

        _inputMap.Initialize();

        _inputs = new string[_inputActions.Count()];
        int counter = 0;

        foreach(InputAction inputAction in _inputActions)
        {
            Debug.Log(inputAction.name);
            _inputs[counter] = inputAction.name;
            counter++;
        }
    }

    private void HandleEnableInputs(EnableInputsEvent inputEvent)
    {
        if (inputEvent.Player != _inputType)
        {
            return;
        }

        if (inputEvent.Enable)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    private void Enable()
    {
        _isEnabled = true;
       // StartCoroutine(TakeRandomDecisions());
    }

    private void Disable()
    {
        _isEnabled = false;
    }

    private IEnumerator TakeRandomDecisions()
    {
        ICommand choosenCommand = _inputMap.GetCommand(_inputs[Random.Range(0, _inputs.Length)]);

        _inputQueue.AddCommand(choosenCommand);

        if (_isEnabled)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(TakeRandomDecisions());
        }
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<EnableInputsEvent>(GetHashCode());
    }
}
