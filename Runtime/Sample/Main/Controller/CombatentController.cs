using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Combatent : ControllerBase, ICombatent
{
    public Action<Combatent> OnDie;
    public Action<int, int> OnTookDamage;

    [SerializeField] private InputEventQueue _inputQueue;
    [SerializeField] private ComboComponent _combo;
    [SerializeField] private MovementComponent _movement;

    private CombatentViewComponent _view;
    private CombatentPhysicsComponent _physics;

    private int _currentLife;
    private int _maxLife;
    private int _damage;

    private CombatentData _data;

    private IState _currentState;

    public IState CurrentState { get => _currentState; }

    private void Awake()
    {
        _currentState = new StandardState();
        _currentState.OnChangeState = UpdateState;
        _combo.OnStartComboChance = StartComboBufferingState;
        _combo.OnEndComboChance = FinishComboBufferingState;
    }

    public void SetType(CombatentData combatentData)
    {
        _data = combatentData;
        GameObject prefabInstantiated = Instantiate(combatentData.Prefab, transform);
        _physics = prefabInstantiated.GetComponent<CombatentPhysicsComponent>();
        _physics.OnTakeDamage = HandleTakeDamageInCurrentState;

        _view = prefabInstantiated.GetComponent<CombatentViewComponent>();
        _view.Initialize();
        _view.OnFinishAwaitableAnimations = ReturnToIdle;

        _movement = prefabInstantiated.GetComponent<MovementComponent>();
        _movement.Initialize(prefabInstantiated.transform);
        //_movement.Initialize(prefabInstantiated.GetComponent<CharacterController>(), prefabInstantiated.GetComponent<m>());

        SetupInitialState();
    }

    public void SetupInitialState()
    {
        _physics.SetTakeDamageEnable(true);
        _physics.SetDamageEnable(false);

        _currentLife = _data.MaxLife;
        _maxLife = _data.MaxLife;
        _damage = _data.Damage;

        _view.PlayAnimationSignature(_data.animationSignature);
    }

    public void OnUpdate()
    {
        ICommand nextCommand = _inputQueue.GetNextCommand();
        if (nextCommand == null)
        {
            return;
        }
        _currentState.HandleCommand(nextCommand, this);
    }

    public void Attack(string context)
    {
        HitData hitData = _combo.GetHitFromCurrentCombo(context);
     
        _view.PlayAnimation(hitData.Animation);
        _physics.SetTakeDamageEnable(true);
        _physics.SetDamageEnable(false, hitData.DamageData);
    }

    public void Defend(string context)
    {
        _combo.ResetCombo();

        _view.ResetAnimations();
        _view.PlayAnimation(context);

        _physics.SetTakeDamageEnable(true);
        _physics.SetDamageEnable(false);
        Debug.Log("Defended");
    }

    public void ExitDefense(string context)
    {
        _combo.ResetCombo();

        _view.PlayAnimation(context);
      
        _physics.SetTakeDamageEnable(true);
        _physics.SetDamageEnable(false);
        Debug.Log("Exited defense");
    }

    public void Move(string context, Vector2 moveVector)
    {
        if (moveVector.x > 0.4f || moveVector.x < -0.4f)
        {
            _view.SetAnimationValue(context, moveVector.sqrMagnitude);
            _movement.Move(moveVector);
        }
        else
        {
            _view.SetAnimationValue("Idle", 0);
        }
    }

    public void StopMove()
    {
        Debug.Log("#33 Stop moved");
        _view.StopContiguousAnimation();
    }

    public void Jump(string context)
    {
        Debug.Log("Jumped");
    }

    public void TakeDamage(int damage, string context)
    {
        Debug.Log("Took " + damage + " life is: " + (_currentLife -= damage));
        _view.ResetAnimations();
        _view.PlayAnimation(context);
        _physics.SetDamageEnable(false);
        
        OnTookDamage?.Invoke(_currentLife, _maxLife);

        if (_currentLife <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        _view.PlayAnimation("KnockOut");
        OnDie?.Invoke(this);
        Debug.Log(gameObject.name + " died");
    }

    private void ReturnToIdle()
    {
        Debug.Log("#33 ReturnedToIdle");
        UpdateState(new StandardState());
    }

    private void HandleTakeDamageInCurrentState(DamageData damage)
    {
        _currentState.HandleTakeHit(damage, this);
    }

    private void UpdateState(IState newState)
    {
            Debug.Log("Novo estad: " + newState.GetType().FullName);
        
        _currentState = newState;
        _currentState.OnChangeState = UpdateState;
    }

    private void StartComboBufferingState()
    {
        UpdateState(new ComboBufferingState());
    }
    
    private void FinishComboBufferingState()
    {
        UpdateState(new StandardState());
    }
}
