using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeEffectSO", menuName = "Scriptable Objects/UpgradeEffectSO")]
public abstract class UpgradeEffectSO : ScriptableObject
{
    public abstract void Apply(GameManager game);
}
