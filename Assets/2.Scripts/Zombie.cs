using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Rigidbody2D rigid;
    public float moveSpeed = 1f;
    public float jumpPower = 5f;
    public float jumpCooldown = 1f; // 점프 쿨타임(초)
    private float lastJumpTime = -999f; // 마지막 점프 시각

    [SerializeField] LayerMask layerMask; // Zombie 레이어 마스크

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        #region 레이어 설정

        List<string> allLayers = new List<string> { DEF.Tag_Zombie1, DEF.Tag_Zombie2, DEF.Tag_Zombie3 };

        int random = Random.Range(0, allLayers.Count);

        string selectedLayer = allLayers[random];
        transform.name = selectedLayer;
        gameObject.layer = LayerMask.NameToLayer(selectedLayer);

        // 포함할 레이어
        int includeMask = LayerMask.GetMask(selectedLayer);

        // 제외할 레이어: 나머지 두 개
        allLayers.Remove(selectedLayer);
        int excludeMask = LayerMask.GetMask(allLayers.ToArray());
        if (rigid != null)
        {
            rigid.includeLayers = includeMask;   // 이 레이어만 포함
            rigid.excludeLayers = excludeMask;   // 나머지는 제외
        }

        // Raycast도 같은 레이어만 감지하도록 동기화
        layerMask = includeMask;
        #endregion

        #region 쿨타임 설정
        jumpCooldown = Random.Range(1f, 4f); // 1초에서 4초 사이의 랜덤 쿨타임
        #endregion
    }
    void FixedUpdate()
    {
        // 위에 좀비가 있다면
        if (IsZombieAbove())
        {
            // 뒤로 이동
            rigid.velocity = new Vector2(moveSpeed * 0.1f, rigid.velocity.y);
        }
        else
        {
            // 앞으로 이동
            rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
        }

        float offset = 0.55f; // Collider 크기보다 조금 크게 설정
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.left * offset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, 0.5f, layerMask);
        if (hit.collider != null
            && hit.collider.gameObject != this.gameObject
            && Time.time - lastJumpTime >= jumpCooldown
            && IsZombieAbove() == false) // 쿨다운 체크
        {
            Debug.Log(transform.name + " : Jumping!");
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            // 점프 직후 y속도 제한 (슈퍼점프 방지)
            if (rigid.velocity.y > jumpPower)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            }

            lastJumpTime = Time.time; // 점프 시각 갱신
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(DEF.Tag_Truck))
        {
            rigid.velocity = Vector2.zero;
        }
    }

    bool IsZombieAbove()
    {
        float checkRadius = 0.5f; // 콜라이더 크기에 맞게 조정
        Vector2 aboveCenter = (Vector2)transform.position + Vector2.up * checkRadius;

        int myLayer = gameObject.layer;
        int layerMask = 1 << myLayer;

        Collider2D[] hits = Physics2D.OverlapCircleAll(aboveCenter, checkRadius, layerMask);

        foreach (var hit in hits)
        {
            if (hit.gameObject != this.gameObject && hit.transform.position.y > transform.position.y)
            {
                return true; // 한 마리라도 있으면 true 반환
            }
        }
        return false;
    }
}
