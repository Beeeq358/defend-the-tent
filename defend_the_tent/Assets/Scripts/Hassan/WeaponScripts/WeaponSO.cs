using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponRange
    {
        Short,
        Medium,
        Long
    }

    public GameObject weaponPrefab;
    public string weaponName;
    public int weaponDamage;
    public int weaponHealth;
    public float attackSpeed;
    public WeaponRange weaponRangeType;
    public float weaponRange;
}
