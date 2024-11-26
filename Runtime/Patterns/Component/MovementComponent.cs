using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _velocity = 2;
    [SerializeField] private bool _enable;
    [SerializeField] private MovementOrientation _orientation;

    [SerializeField] private CharacterController _character;

    private Transform _transform;

    public void Initialize(Transform combatentTransform)
    {
        if (_enable)
        {
            _transform = combatentTransform;
            _orientation.OnChangeOrientation = TurnAround;
        }
    }

    public void Move(Vector2 moveInput)
    {
        int modifier = -1;
        Vector3 moveValue = new Vector3(modifier * moveInput.x, 0, 0);
        _character.Move(moveValue * _velocity * Time.deltaTime);
    }

    private void TurnAround(bool isTurnedRight)
    {
        Vector3 eulers = new Vector3(0, 0, 0);
        if(isTurnedRight)
        {
            eulers.y = 180;
        }
        _transform.localEulerAngles = eulers;
    }
}
