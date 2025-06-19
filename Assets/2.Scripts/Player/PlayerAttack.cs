using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponData _currentWeapon;
    [SerializeField] private Transform _firePoint;

    private float _lastAttackTime = -999f;
    WeaponDataBase weaponDB;

    private void Awake()
    {
        weaponDB = FindAnyObjectByType<WeaponDataBase>();
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time - _lastAttackTime >= _currentWeapon.attackCooldown)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDirection = (mouseWorldPos - _firePoint.position).normalized;

            // 퍼짐 각도/발사 개수 적용
            for (int i = 0; i < _currentWeapon.bulletCount; i++)
            {
                float angle = 0f;
                if (_currentWeapon.bulletCount > 1)
                    angle = -_currentWeapon.spreadAngle + (_currentWeapon.spreadAngle * 2f) * i / (_currentWeapon.bulletCount - 1);

                Vector2 shotDirection = Quaternion.Euler(0, 0, angle) * fireDirection;
                ShootBullet(shotDirection);
            }
            _lastAttackTime = Time.time;
        }
    }

    void ShootBullet(Vector2 direction)
    {
        Bullet bullet = InGame.Instance.GetBullet();
        bullet.transform.position = _firePoint.position;
        bullet.Init(
            direction,
            InGame.Instance.ReturnBullet,
            _currentWeapon.bulletSpeed,
            _currentWeapon.bulletRange,
            _currentWeapon.bulletDamage
        );
    }

    // 무기 교체 기능 예시
    public void SetWeapon(int i, Action onSuccess = null)
    {
        // 1. GameManager 인스턴스 체크
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance가 null입니다.");
            return;
        }

        // 2. i가 Enum 범위 내에 있는지 체크
        if (!Enum.IsDefined(typeof(WeaponType), i))
        {
            Debug.LogError($"WeaponType에 {i} 값이 없습니다.");
            return;
        }

        WeaponType weaponType = (WeaponType)i;

        // 4. 정상적으로 무기 교체
        _currentWeapon = weaponDB.WeaponDataDict[weaponType];
        GameManager.Instance.currentWeapon = weaponDB.WeaponDataDict[weaponType];
        GameManager.Instance.WeaponType = weaponType;
        Debug.Log("무기 교체 성공");

        // 5. 성공 콜백 실행
        onSuccess?.Invoke();
    }
}