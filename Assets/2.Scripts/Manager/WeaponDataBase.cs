using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "New Weapon";
    public float attackCooldown = 0.5f;
    public int bulletCount = 3;
    public float spreadAngle = 15f;
    public float bulletSpeed = 20f;
    public float bulletRange = 10f;
    public int bulletDamage = 10;
}

public enum WeaponType
{
    ShotGun,
    Riple,
}


// 무기 데이터 관리
public class WeaponDataBase : MonoBehaviour
{
    public WeaponData shotGunData;
    public WeaponData ripleData;

    public Dictionary<WeaponType, WeaponData> WeaponDataDict { get; private set; }

    void Awake()
    {
        WeaponDataDict = new Dictionary<WeaponType, WeaponData>
        {
            { WeaponType.ShotGun, shotGunData },
            { WeaponType.Riple, ripleData }
        };
    }
}