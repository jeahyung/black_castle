using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRange : MonoBehaviour
{

    //음파와 발자국
    public GameObject projectilePrefab;
    //public GameObject stepPrefab;

    //음파 수명
    public float Walk_lifetime;

    //음파 쿨
    public float Walk_Shoot_CoolTime;
    private float lastShootTime;



 


    // public event Action slow;

    void Start()
    {
    }
    void Update()
    {

        //달리기 : 플레이어 속도 증가, 음파 수명 증가, 발생 주기 감소.
        float LifeTime = Walk_lifetime;
        float Current_Shoot_CoolTime = Walk_Shoot_CoolTime;


        //걸을때마다 음파 발사함
        if ( Time.time > lastShootTime + Current_Shoot_CoolTime)
        {
            Shoot(LifeTime); //음파발사
            
            lastShootTime = Time.time;
        }

    }  

    public void Shoot(float LifeTime)
    {
        //Debug.Log("player stepin");

        //발자국
        //Instantiate(stepPrefab, transform.position, Quaternion.identity);
        
        //Debug.Log(prePos);
        // 음파의 투사체 생성 각도 설정
        float angleStep = 360f / 6;
        float currentAngle = 15f;

        // 12발씩 음파 생성
        for (int i = 0; i < 6; i++)
        {
            // 각도에 따라 음파 방향 설정
            Vector3 projectileMoveDirection = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;

            // 음파 생성
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = proj.GetComponent<Projectile>();
            projectileScript.direction = projectileMoveDirection;
            projectileScript.lifetime = LifeTime;

            // 다음 음파 각도 설정
            currentAngle += angleStep;
        }
    }



}
