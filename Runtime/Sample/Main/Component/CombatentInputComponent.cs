using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatentInputComponent : MonoBehaviour
{
    public Action OnPauseGame;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private InputEventQueue _inputEventQueue;

    private InputCommandMap _inputMap;

    private const InputType _inputType = InputType.COMBAT; 
    private IEventService _eventService;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleInputValuePerformed("MoveTest");
    }

    private async void Initialize()
    {
        _inputMap = new InputCommandMap();
        _inputMap.Initialize();
        
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.AddListener<EnableInputsEvent>(HandleEnableInputs, GetHashCode());
    }

    private void HandleEnableInputs(EnableInputsEvent inputEvent)
    {
        if(inputEvent.Player != _inputType)
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
        enabled = true;
        _input.actions["Punch"].performed += HandleInputPerformed;
        _input.actions["Defend"].performed += HandleInputPerformed;
        _input.actions["ReleaseDefense"].canceled += HandleInputPerformed;
        _input.actions["HardPunch"].performed += HandleInputPerformed;
        _input.actions["SoftPunch"].performed += HandleInputPerformed;
        _input.actions["Pause"].performed += HandleGamePaused;
    }

    private void Disable()
    {
        _input.actions["Punch"].performed -= HandleInputPerformed;
        _input.actions["Defend"].performed -= HandleInputPerformed;
        _input.actions["ReleaseDefense"].canceled += HandleInputPerformed;
        _input.actions["HardPunch"].performed -= HandleInputPerformed;
        _input.actions["SoftPunch"].performed -= HandleInputPerformed;
        _input.actions["Pause"].performed -= HandleGamePaused;
        enabled = false;
    }

    private void HandleInputPerformed(InputAction.CallbackContext context)
    {
        _inputEventQueue.AddCommand(_inputMap.GetCommand(context));
    }

    private void HandleInputValuePerformed(string inputAction)
    {
        ICommand commandWithValue = _inputMap.GetCommand(inputAction);
        commandWithValue.Value =  _input.actions[inputAction].ReadValue<Vector2>();
        _inputEventQueue.AddSecondaryCommand(commandWithValue);
    }

    private void HandleGamePaused(InputAction.CallbackContext context)
    {
        OnPauseGame?.Invoke();
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<EnableInputsEvent>(GetHashCode());
    }
}
