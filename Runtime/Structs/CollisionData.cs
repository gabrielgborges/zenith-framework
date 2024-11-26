using UnityEngine;

[CreateAssetMenu(fileName = "HitData", menuName = "Data/Collision", order = 0)]
public class CollisionData : ScriptableObject
{
    [SerializeField] private VFXTypes _visualEffect;
    [SerializeField] private BodyCollisionType _bodyPosition;

    public VFXTypes VisualEffect => _visualEffect;

    public BodyCollisionType BodyPosition => _bodyPosition;
}
