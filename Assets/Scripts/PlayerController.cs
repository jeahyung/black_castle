using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //������
    public float moveSpeed;
    public float runSpeed;
    private bool isRunning = false;
    private bool isMoving = false;
    public Vector2 prePos;
    public event System.Action mob;

    
    //���Ŀ� ���ڱ�
    public GameObject projectilePrefab;
    public GameObject stepPrefab;

    //���� ����
    public float Walk_lifetime;
    public float Run_lifetime;
    public float Clap_lifetime;

    //���� ��
    public float Walk_Shoot_CoolTime;
    public float Run_Shoot_CoolTime;
    private float lastShootTime;

    //�ڼ�
    public float clapCool;
    private float lastClapTime;

    //���� ������
    //public  Transform StartPos;
    public float MoveCool;
    private float lastMoveTime;

    //�Ҹ�
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

        //�޸��� : �÷��̾� �ӵ� ����, ���� ���� ����, �߻� �ֱ� ����.
        float LifeTime = isRunning ? Run_lifetime : Walk_lifetime;
        float Current_Shoot_CoolTime = isRunning ? Run_Shoot_CoolTime : Walk_Shoot_CoolTime;

        //��� �ൿ�� �ൿ�� �����ؾ� ������.
        lastClapTime -= Time.deltaTime;
        lastMoveTime -= Time.deltaTime;

        if (lastMoveTime < 0)
        {
            Move();

            //���������� ���� �߻���
            if (isMoving && Time.time > lastShootTime + Current_Shoot_CoolTime)
            {
                Shoot(LifeTime); //���Ĺ߻�
                PlaySound(stepSound);
                lastShootTime = Time.time;
            }
       
            //Z�� �ڼ�, �ڼ��� ���� ������ �ſ� ��. ���� �ð����� �̵� �Ұ�.
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

            //P�� ġƮŰ
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(AdvanceStageAfterDelay(1f)); // 3�� �Ŀ� ������������
            }

            //���Ӳ���
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // ������ ��忡�� ������ ����
#endif
            }


            //���ʽ���Ʈ �� �ȱ�
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

    void Move() //�����¿�����̱�
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0).normalized;

        //�޸� �� �ӵ���ȭ
        float speed  = moveSpeed;
        if(isRunning )
        {
            speed = runSpeed;
           
        }



        transform.Translate(move * speed * Time.deltaTime);

        // ������ ���¸� ������Ʈ
        isMoving = move.sqrMagnitude > 0;
        
    }

    public void Shoot(float LifeTime)
    {
        //Debug.Log("player stepin");

        //���ڱ�
        Instantiate(stepPrefab, transform.position, Quaternion.identity);
        prePos = transform.position;
        //Debug.Log(prePos);
        // ������ ����ü ���� ���� ����
        float angleStep = 360f / 12;
        float currentAngle = 15f;

        // 12�߾� ���� ����
        for (int i = 0; i < 12; i++)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap")) //������ ó������ ���ư��� ����
        {
            gameObject.transform.position = StageManager.Instance.startPos.position; 
            lastMoveTime = MoveCool;
            PlaySound(trapSound);
        }

        if (other.CompareTag("Goal")) //���� ������ �Ѿ�� ��ǥ��
        {
            PlaySound(goalSound);
            lastMoveTime = 5;
            StartCoroutine(ShootAfterDelay(2f)); //���ݴ� ���Ŀ���
            StartCoroutine(AdvanceStageAfterDelay(5f)); // 3�� �Ŀ� ������������
        }
        if (other.CompareTag("Lever")) //������ ����
        {
            PlaySound(LeverSound);
        }
    }

    //����
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

    void PlaySound(AudioClip clip) //�Ҹ�
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}