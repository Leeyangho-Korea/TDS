using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    public WeaponType WeaponType; // 현재 무기 타입
    public WeaponData currentWeapon; // 현재 무기 데이터

    private void Awake()
    {
        Instance = this;
    }
}
