using System;
using UnityEngine;

public class MovementOrientation : MonoBehaviour
{
    public Action<bool> OnChangeOrientation;

    private bool _isTurnedRight = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ICombatent>() != null)
        {
            _isTurnedRight = !_isTurnedRight;
            OnChangeOrientation?.Invoke(_isTurnedRight);
        }  
    }
}
