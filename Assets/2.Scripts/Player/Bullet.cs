using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private int _damage;
    private float _maxDistance;

    private Vector2 _moveDirection;
    private Vector2 _startPosition;
    private System.Action<Bullet> _returnToPool;

    public void Init(Vector2 direction, System.Action<Bullet> returnToPool, float speed, float maxDistance, int damage)
    {
        this._moveDirection = direction.normalized;
        this._returnToPool = returnToPool;
        this._speed = speed;
        this._maxDistance = maxDistance;
        this._damage = damage;
        this._startPosition = transform.position;
    }

    void Update()
    {
        transform.position += (Vector3)(_moveDirection * _speed * Time.deltaTime);

        // 거리 제한 (sqrMagnitude로 최적화)
        float sqrDist = ((Vector2)transform.position - _startPosition).sqrMagnitude;
        float maxSqrDist = _maxDistance * _maxDistance;
        if (sqrDist >= maxSqrDist)
        {
            _returnToPool?.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Zombie zombie = collision.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(_damage);
            _returnToPool?.Invoke(this);
            return;
        }

        if (collision.CompareTag("Ground") || collision.CompareTag("DeadLine"))
        {
            _returnToPool?.Invoke(this);
        }
    }
}
