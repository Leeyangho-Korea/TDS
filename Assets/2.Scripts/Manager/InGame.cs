using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    #region 좀비 풀 관련 변수
    [Header("좀비 풀 관련")]
    public Zombie zombiePrefab;
    public Transform spawnPoint;
    private ObjectPool<Zombie> _zombiePool;

    private Coroutine _zombieSpawnRoutine;

    float _zombieSpawnInterval = 1.5f; // 좀비 생성 간격 (초)
    #endregion

    #region 총알 풀 관련 변수
    [Header("총알 풀 관련")]
    public Bullet bulletPrefab;
    public Transform bulletParent;
    private ObjectPool<Bullet> _bulletPool;
    #endregion

    PlayerAttack hero; // 플레이어 캐릭터
    #region 무기 관련   
    [Header("무기 관련")]
    [SerializeField] private Button _btnTransButton; // 무기 전환 버튼
    [SerializeField] Image[] _weaponImages; // 무기 이미지 UI 배열
    [SerializeField] Sprite[] _weaponSprites; // 무기 이미지 스프라이트 배열
    #endregion

    #region UI관련 (대량의 UI 요소를 관리하게 되면 UI 클래스를 따로 만들어 관리)
    [Header("UI 관련")]
    [SerializeField] private GameObject _panelBlack;
    [SerializeField] private Button _btnStart; // 게임 시작 버튼
    [SerializeField] private GameObject _gameUI; // 게임 UI 오브젝트
    [SerializeField] Text _tKillCount;// 잡은 좀비 수 텍스트
    string _killCountText = "잡은 좀비 수 : {0}"; // 잡은 좀비 수 텍스트 포맷
    [SerializeField] private RectTransform _poopupWin; // 승리 팝업 UI
    [SerializeField] private Text _tWinKillCount; // 승리 팝업의 잡은 좀비 수 텍스트
    [SerializeField] private Text _tTime; // 승리 팝업의 시간 텍스트
    #endregion

    int _killCount = 0; // 잡은 좀비 수 카운트
    float _gameTime = 0f; // 게임 시간 카운트

    public static InGame Instance { get; private set; }

    private void Awake()
    {
        hero = FindObjectOfType<PlayerAttack>();
        Instance = this;
    }


    void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        Init(); // 초기화 메소드 호출
        _btnStart.onClick.AddListener(() => GameManager.Instance.SetState(GameState.Playing));  // 게임 시작 버튼 클릭 시 애니메이션 재생
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

        _killCount = 0;
        _tKillCount.text = string.Format(_killCountText, 0); // 초기 잡은 좀비 수 텍스트 설정
    }

    private void Update()
    {
       if(GameManager.Instance.State != GameState.Playing)
            return;

        // 게임 시간 업데이트
        _gameTime += Time.deltaTime;

    }

    private void HandleGameStateChanged(GameState state)
    {
        if (_zombieSpawnRoutine != null)
        {
            StopCoroutine(_zombieSpawnRoutine);
            _zombieSpawnRoutine = null;
        }

        if (state == GameState.Playing)
        {
            GameStart();
        }
        else if (state == GameState.Win)
        {
            GameOver();
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

    private IEnumerator ZombieSpawnRoutine()
    {
        while (GameManager.Instance.State == GameState.Playing)
        {
            SpawnZombie();
            yield return new WaitForSeconds(_zombieSpawnInterval); // 원하는 간격
        }
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

    public void GameStart()
    {
        _panelBlack.SetActive(false); // 검은색 패널 숨김
        _btnStart.gameObject.SetActive(false); // 게임 시작 버튼 숨김
        _gameUI.SetActive(true); // 게임 UI 활성화
        _zombieSpawnRoutine = StartCoroutine(ZombieSpawnRoutine());
    }

    public void GameOver()
    {
      _tWinKillCount.text = string.Format(_killCountText, _killCount); // 승리 팝업의 잡은 좀비 수 텍스트 설정
        _tTime.text = string.Format("게임 시간: {0:F2}초", _gameTime); // 승리 팝업의 시간 텍스트 설정
        ShowWinPopup(); // 팝업 표시
        _gameUI.SetActive(false); // 게임 UI 비활성화
    }

    // 팝업을 화면 중앙으로 바운스하며 등장시키는 함수
    public void ShowWinPopup()
    {
        // 검은색 패널 보이기
        _panelBlack.SetActive(true);

        // 팝업 활성화
        _poopupWin.gameObject.SetActive(true);

        // 1. 시작 위치: 화면 위(밖)로 이동
        Vector2 startPos = new Vector2(0, Screen.height * 0.7f); // 화면 위 70% 지점(조절 가능)
        _poopupWin.anchoredPosition = startPos;

        // 2. 중앙으로 바운스하며 이동 (Ease.OutBounce)
        _poopupWin.DOAnchorPos(Vector2.zero, 0.7f)
            .SetEase(Ease.OutBounce); // 바운스 효과
    }


    public void IncrementKillCount()
    {
        _killCount++;
        _tKillCount.text = string.Format(_killCountText, _killCount); // 잡은 좀비 수 텍스트 업데이트
    }
}
