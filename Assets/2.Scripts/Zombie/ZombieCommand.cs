using UnityEngine;

public class ZombieCommand : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public float stopThreshold = 0.01f; // x=0 근처에서 멈추는 임계값
    public bool isMoving = true; // 이동 중 여부

    private float _startX;
    private float _targetX = 0f;

    [SerializeField] HPBar _hpBar;
    private int _maxHp = 100;
    private int _hp;

    private void Start()
    {
        _hp = _maxHp;
        _hpBar.Init(_maxHp);
    }

    void OnEnable()
    {
        _startX = transform.localPosition.x;
    }

    void Update()
    {
        if (isMoving == false || GameManager.Instance.State != GameState.Playing) return;

        // 트럭에 붙은 좀비가 있으면 이동을 중단
        if (Truck.Instance != null && Truck.Instance.zombieContactCount > 0)
            return;

        // x=0까지 서서히 이동
        float newX = Mathf.MoveTowards(transform.localPosition.x, _targetX, moveSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);

        // x=0에 도달하면 이동 종료
        if (Mathf.Abs(transform.localPosition.x - _targetX) < stopThreshold)
        {
            isMoving = false;
            // 배경 스크롤 멈추기
            Background.Instance.StopScroll(true);
        }
    }

    public float GetProgress()
    {
        // 0(출발)~1(도착) 진행도 반환
        return 1f - Mathf.Clamp01(Mathf.Abs(transform.localPosition.x - _targetX) / Mathf.Abs(_startX - _targetX));
    }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hpBar != null)
            _hpBar.SetHP(_hp);

        if (_hp <= 0)
        {
            GameManager.Instance.SetState(GameState.Win);
        }
    }
}
