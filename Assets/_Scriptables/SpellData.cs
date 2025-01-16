using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
public class SpellData : ScriptableObject
{
    public int damage;
    public float range;
    public float cooldown;
    public int numberOfProjectile;
}
