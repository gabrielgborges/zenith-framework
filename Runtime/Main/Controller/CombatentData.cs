using UnityEngine;

[CreateAssetMenu(fileName = "CombatentData", menuName = "Data/CombatentData", order = 1)]
public class CombatentData : ScriptableObject
{
    [SerializeField] private AnimationSignature _animationSignature;
    [SerializeField] private Sprite _previewSprite;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _maxLife = 100;
    [SerializeField] private int _damage = 5;

    public AnimationSignature animationSignature => _animationSignature;
    public Sprite PreviewSprite => _previewSprite;
    public GameObject Prefab => _prefab;
    public int MaxLife => _maxLife; 
    public int Damage => _damage;
}
