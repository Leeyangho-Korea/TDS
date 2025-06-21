using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private int _damage;
    private float _maxDistance;
    private Vector2 _moveDirection;
    private Vector2 _startPosition;
    private System.Action<Bullet> _returnToPool;
    private bool _hasCollided = false; // 충돌 여부 추적

    public void Init(Vector2 direction, System.Action<Bullet> returnToPool,
                    float speed, float maxDistance, int damage)
    {
        this._moveDirection = direction.normalized;
        this._returnToPool = returnToPool;
        this._speed = speed;
        this._maxDistance = maxDistance;
        this._damage = damage;
        this._startPosition = transform.position;
        _hasCollided = false; // 초기화 시 충돌 상태 리셋
    }

    void Update()
    {
        if (_hasCollided) return; // 이미 충돌한 경우 이동 중지

        transform.position += (Vector3)(_moveDirection * _speed * Time.deltaTime);

        float sqrDist = ((Vector2)transform.position - _startPosition).sqrMagnitude;
        float maxSqrDist = _maxDistance * _maxDistance;
        if (sqrDist >= maxSqrDist)
        {
            _returnToPool?.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasCollided) return; // 이미 충돌 처리된 경우 무시


        // 좀비에 부딪혔을 때
        if (collision.TryGetComponent<Zombie>(out Zombie zombie))
        {
            _hasCollided = true; // 충돌 플래그 설정
            zombie.TakeDamage(_damage);

            gameObject.SetActive(false);
            _returnToPool?.Invoke(this);
            return;
        }
        // 좀비 커맨드에 부딪혔을 때
        else if(collision.TryGetComponent<ZombieCommand>(out ZombieCommand zombieCommand))
        {
            _hasCollided = true; // 충돌 플래그 설정
            zombieCommand.TakeDamage(_damage);

            gameObject.SetActive(false);
            _returnToPool?.Invoke(this);
            return;
        }


        if (collision.CompareTag(DEF.TAG_Ground) || collision.CompareTag(DEF.TAG_DeadLine))
        {
            _hasCollided = true; // 충돌 플래그 설정
            _returnToPool?.Invoke(this);
        }
    }
}
