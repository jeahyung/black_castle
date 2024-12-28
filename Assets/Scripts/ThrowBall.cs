using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Vector2 normalizedDirection;
    public GameObject projectilePrefab; // ����ü ������
    public float projectileSpeed = 10f; // ����ü �ӵ�

    public float cooldownTime = 3f; // ��ٿ� �ð� (3��)
    private float lastFireTime; // ���������� �߻��� �ð�

    private MultiLineDraw mt;

    void Start()
    {
        lastFireTime = -cooldownTime; // ���� ���� �� �ٷ� �߻��� �� �ֵ��� ����

        mt = GetComponent<MultiLineDraw>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanFire()) // ���콺 ���� ��ư Ŭ�� ��
        {
            CalculateDirectionToMouseClick();
            Debug.Log("Normalized Direction: " + normalizedDirection);
            FireProjectile();
            lastFireTime = Time.time; // �߻� �ð� ����

        }
    }

    bool CanFire()
    {
        return Time.time - lastFireTime >= cooldownTime;
    }


    void CalculateDirectionToMouseClick()
    {
        // ���콺 Ŭ�� ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 2D �����̹Ƿ� z ��ǥ�� 0���� ����

        // �÷��̾� ��ġ���� ���콺 Ŭ�� ��ġ���� ���� ���� ���
        Vector2 direction = mousePosition - transform.position;

        // ���� ���͸� ����ȭ�Ͽ� ��� ���� ���
        normalizedDirection = direction.normalized;
    }

    void FireProjectile()
    {
        // ����ü ����
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // ����ü�� Rigidbody2D ������Ʈ�� �ִٰ� ����
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ��� ���� �������� �ӵ� ����
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
