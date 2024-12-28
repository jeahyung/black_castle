using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRange : MonoBehaviour
{

    //���Ŀ� ���ڱ�
    public GameObject projectilePrefab;
    //public GameObject stepPrefab;

    //���� ����
    public float Walk_lifetime;

    //���� ��
    public float Walk_Shoot_CoolTime;
    private float lastShootTime;



 


    // public event Action slow;

    void Start()
    {
    }
    void Update()
    {

        //�޸��� : �÷��̾� �ӵ� ����, ���� ���� ����, �߻� �ֱ� ����.
        float LifeTime = Walk_lifetime;
        float Current_Shoot_CoolTime = Walk_Shoot_CoolTime;


        //���������� ���� �߻���
        if ( Time.time > lastShootTime + Current_Shoot_CoolTime)
        {
            Shoot(LifeTime); //���Ĺ߻�
            
            lastShootTime = Time.time;
        }

    }  

    public void Shoot(float LifeTime)
    {
        //Debug.Log("player stepin");

        //���ڱ�
        //Instantiate(stepPrefab, transform.position, Quaternion.identity);
        
        //Debug.Log(prePos);
        // ������ ����ü ���� ���� ����
        float angleStep = 360f / 6;
        float currentAngle = 15f;

        // 12�߾� ���� ����
        for (int i = 0; i < 6; i++)
        {
            // ������ ���� ���� ���� ����
            Vector3 projectileMoveDirection = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;

            // ���� ����
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = proj.GetComponent<Projectile>();
            projectileScript.direction = projectileMoveDirection;
            projectileScript.lifetime = LifeTime;

            // ���� ���� ���� ����
            currentAngle += angleStep;
        }
    }



}
