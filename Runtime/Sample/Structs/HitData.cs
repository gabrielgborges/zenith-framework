using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HitData", menuName = "Data/Hit", order = 0)]
public class HitData : ScriptableObject
{
    [SerializeField] private DamageData _damageData;
    [SerializeField] private string _animation;

    public DamageData DamageData => _damageData;
    public int Damage => _damageData.Damage;
    public Vector3 ImpulseForce => _damageData.ImpulseForce;
    public DamageType DamageType => _damageData.DamageType;
    public string Animation => _animation;
}

[Serializable]
public struct DamageData
{
    public int Damage;
    public Vector3 ImpulseForce;
    public DamageType DamageType;
}
