using UnityEngine;

[CreateAssetMenu(fileName = "AvailableCombatentsData", menuName = "Data/AvailableCombatents", order = 2)]
public class AvaliableCombatentsData : ScriptableObject
{
    [SerializeField] private CombatentData[] _combatents;

    public CombatentData[] Combatents => _combatents;
}
