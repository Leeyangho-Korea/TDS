using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public Zombie zombiePrefab;
    public Transform spawnPoint;

    private ObjectPool<Zombie> zombiePool;

    void Start()
    {
        // 풀 초기화
        zombiePool = new ObjectPool<Zombie>(
            prefab: zombiePrefab,
            initialSize: 50,
            parent: spawnPoint // 부모 Transform 
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 예시: 스페이스바를 누르면 좀비 생성
        {
            SpawnZombie();
        }
    }

    public void SpawnZombie()
    {
        Zombie zombie = zombiePool.Get();
        zombie.transform.position = spawnPoint.position;
    }

    public void ReleaseZombie(Zombie zombie)
    {
        zombiePool.Return(zombie);
    }
}
