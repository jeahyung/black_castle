using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Vector2 normalizedDirection;
    public GameObject projectilePrefab; // 투사체 프리팹
    public float projectileSpeed = 10f; // 투사체 속도

    public float cooldownTime = 3f; // 쿨다운 시간 (3초)
    private float lastFireTime; // 마지막으로 발사한 시간

    private MultiLineDraw mt;

    void Start()
    {
        lastFireTime = -cooldownTime; // 게임 시작 시 바로 발사할 수 있도록 설정

        mt = GetComponent<MultiLineDraw>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanFire()) // 마우스 왼쪽 버튼 클릭 시
        {
            CalculateDirectionToMouseClick();
            Debug.Log("Normalized Direction: " + normalizedDirection);
            FireProjectile();
            lastFireTime = Time.time; // 발사 시간 갱신

        }
    }

    bool CanFire()
    {
        return Time.time - lastFireTime >= cooldownTime;
    }


    void CalculateDirectionToMouseClick()
    {
        // 마우스 클릭 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 2D 게임이므로 z 좌표는 0으로 설정

        // 플레이어 위치에서 마우스 클릭 위치로의 방향 벡터 계산
        Vector2 direction = mousePosition - transform.position;

        // 방향 벡터를 정규화하여 노멀 벡터 얻기
        normalizedDirection = direction.normalized;
    }

    void FireProjectile()
    {
        // 투사체 생성
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // 투사체에 Rigidbody2D 컴포넌트가 있다고 가정
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 노멀 벡터 방향으로 속도 설정
            rb.velocity = normalizedDirection * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody2D component!");
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        mt.CreateNewLineGroup();

    //    }
    //}

}
