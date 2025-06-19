using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    #region 좀비 풀 관련 변수
    public Zombie zombiePrefab;
    public Transform spawnPoint;
    private ObjectPool<Zombie> _zombiePool;
    #endregion

    #region 총알 풀 관련 변수
    public Bullet bulletPrefab;
    public Transform bulletParent;
    private ObjectPool<Bullet> _bulletPool;
    #endregion

    PlayerAttack hero; // 플레이어 캐릭터
    [SerializeField] private Button _btnTransButton; // 무기 전환 버튼
    [SerializeField] Image[] _weaponImages; // 무기 이미지 UI 배열
    [SerializeField] Sprite[] _weaponSprites; // 무기 이미지 스프라이트 배열


    public static InGame Instance { get; private set; }

    private void Awake()
    {
        hero = FindObjectOfType<PlayerAttack>();
        Instance = this;
    }


    void Start()
    {
        Init(); // 초기화 메소드 호출
        _btnTransButton.onClick.AddListener(SetWeaponData); // 무기 전환 버튼 클릭 시 SetWeaponData 호출
    }


    private void Init()
    {
        // 좀비 풀 초기화
        _zombiePool = new ObjectPool<Zombie>(
            prefab: zombiePrefab,
            initialSize: 30,
            parent: spawnPoint 
            );

        _bulletPool = new ObjectPool<Bullet>(
            prefab: bulletPrefab, 
            initialSize: 30,
            parent: bulletParent
            );

        if (hero == null)
        {
            Debug.LogError("PlayerAttack 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        hero.SetWeapon(0); // 플레이어의 무기 설정
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 예시: 스페이스바를 누르면 좀비 생성
        {
            SpawnZombie();
        }
    }

    #region 좀비 풀 관련 메소드
    public void SpawnZombie()
    {
        Zombie zombie = _zombiePool.Get();
        zombie.transform.position = spawnPoint.position;
    }

    public void ReleaseZombie(Zombie zombie)
    {
        _zombiePool.Return(zombie);
    }

    #endregion

    #region 총알 풀 관련 메소드
    public Bullet GetBullet()
    {
        return _bulletPool.Get();
    }

    public void ReturnBullet(Bullet bullet)
    {
        _bulletPool.Return(bullet);
    }
    #endregion

    #region 무기 관련 메소드

    public void SetWeaponData()
    {
        int i = GameManager.Instance.WeaponType == WeaponType.ShotGun ? 1 : 0;
        hero.SetWeapon(i, () => {
            // 여기서 총 이미지 교체, UI 업데이트 등 원하는 동작 실행
            UpdateWeaponUI();
        });
    }

    private void UpdateWeaponUI()
    {
        // 예시: 총 이미지 교체 코드
        _weaponImages[0].sprite = _weaponSprites[GameManager.Instance.WeaponType == WeaponType.ShotGun ? 0 : 1];
        _weaponImages[1].sprite = _weaponSprites[GameManager.Instance.WeaponType == WeaponType.ShotGun ? 1 : 0];
    }

    #endregion
}
