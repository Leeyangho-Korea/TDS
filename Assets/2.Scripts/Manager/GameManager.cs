using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Ready,      // 시작 전(애니메이션 등)
    Playing,    // 게임 진행 중(좀비 자동 생성)
    GameOver    // 게임 종료
}
public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    public WeaponType WeaponType; // 현재 무기 타입
    public WeaponData currentWeapon; // 현재 무기 데이터
    [SerializeField] GameState _State;
    public GameState State
    {
        get => _State;
        private set => _State = value;
    }
    public event Action<GameState> OnGameStateChanged;

    public void SetState(GameState newState)
    {
        State = newState;
        OnGameStateChanged?.Invoke(State);
    }
    private void Awake()
    {
        Instance = this;
    }
}
