using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //움직임
    public float moveSpeed;
    public float runSpeed;
    private bool isRunning = false;
    private bool isMoving = false;
    public Vector2 prePos;
    public event System.Action mob;

    
    //음파와 발자국
    public GameObject projectilePrefab;
    public GameObject stepPrefab;

    //음파 수명
    public float Walk_lifetime;
    public float Run_lifetime;
    public float Clap_lifetime;

    //음파 쿨
    public float Walk_Shoot_CoolTime;
    public float Run_Shoot_CoolTime;
    private float lastShootTime;

    //박수
    public float clapCool;
    private float lastClapTime;

    //원래 포지션
    //public  Transform StartPos;
    public float MoveCool;
    private float lastMoveTime;

    //소리
    private AudioSource audioSource;
    public AudioClip clapSound;
    public AudioClip stepSound;
    public AudioClip trapSound;
    public AudioClip goalSound;
    public AudioClip LeverSound;

   // public event Action slow;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

        //달리기 : 플레이어 속도 증가, 음파 수명 증가, 발생 주기 감소.
        float LifeTime = isRunning ? Run_lifetime : Walk_lifetime;
        float Current_Shoot_CoolTime = isRunning ? Run_Shoot_CoolTime : Walk_Shoot_CoolTime;

        //모든 행동은 행동이 가능해야 가능함.
        lastClapTime -= Time.deltaTime;
        lastMoveTime -= Time.deltaTime;

        if (lastMoveTime < 0)
        {
            Move();

            //걸을때마다 음파 발사함
            if (isMoving && Time.time > lastShootTime + Current_Shoot_CoolTime)
            {
                Shoot(LifeTime); //음파발사
                PlaySound(stepSound);
                lastShootTime = Time.time;
            }
       
            //Z로 박수, 박수의 음파 수명은 매우 김. 일정 시간동안 이동 불가.
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                if (lastClapTime < 0)
                {
                    Shoot(Clap_lifetime);
                    lastClapTime = clapCool;
                    lastMoveTime = MoveCool;
                    PlaySound(clapSound);
                }
            }

            //P는 치트키
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(AdvanceStageAfterDelay(1f)); // 3초 후에 다음스테이지
            }

            //게임끄기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 실행을 중지
#endif
            }


            //왼쪽시프트 시 걷기
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
            }     

        }
    }

    void Move() //상하좌우움직이기
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0).normalized;

        //달릴 시 속도변화
        float speed  = moveSpeed;
        if(isRunning )
        {
            speed = runSpeed;
           
        }



        transform.Translate(move * speed * Time.deltaTime);

        // 움직임 상태를 업데이트
        isMoving = move.sqrMagnitude > 0;
        
    }

    public void Shoot(float LifeTime)
    {
        //Debug.Log("player stepin");

        //발자국
        Instantiate(stepPrefab, transform.position, Quaternion.identity);
        prePos = transform.position;
        //Debug.Log(prePos);
        // 음파의 투사체 생성 각도 설정
        float angleStep = 360f / 12;
        float currentAngle = 15f;

        // 12발씩 음파 생성
        for (int i = 0; i < 12; i++)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap")) //닿으면 처음으로 돌아가는 함정
        {
            gameObject.transform.position = StageManager.Instance.startPos.position; 
            lastMoveTime = MoveCool;
            PlaySound(trapSound);
        }

        if (other.CompareTag("Goal")) //다음 레벨로 넘어가는 목표점
        {
            PlaySound(goalSound);
            lastMoveTime = 5;
            StartCoroutine(ShootAfterDelay(2f)); //문닫는 음파연출
            StartCoroutine(AdvanceStageAfterDelay(5f)); // 3초 후에 다음스테이지
        }
        if (other.CompareTag("Lever")) //문여는 레버
        {
            PlaySound(LeverSound);
        }
    }

    //연출
    IEnumerator ShootAfterDelay(float delay) 
    {
        yield return new WaitForSeconds(delay);
        Shoot(Clap_lifetime);
    }

    IEnumerator AdvanceStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StageManager.Instance.AdvanceStage();
        gameObject.transform.position = StageManager.Instance.startPos.position;
        lastMoveTime = MoveCool;
    }

    void PlaySound(AudioClip clip) //소리
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}