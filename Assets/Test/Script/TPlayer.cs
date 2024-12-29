using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TPlayer : MonoBehaviour
{
    //----------------------------������ ����---------------------------------------------------
    public float interval = 1f; // �Լ� ȣ�� ���� (��)
    private float nextCallTime;
    //---------------------------�޸��� �� ������-------------------------------------------
    public float lineCreationInterval = 0.5f; // ���� ���� ���� (��)
    private float lastLineCreationTime;
    //------------------------------------------------------------------------------------------

    public float moveSpeed = 5f;
    //private Rigidbody2D rb;
    //private Vector2 movement;

    //---------------------����----------------------------------------------------------------
    public enum MonsterState
    {
        Sleep,
        Find
    }
    private Rigidbody2D rb;
    private MonsterState currentState;
    //-----------------------------------------------------------------------------------------
    private Vector2 target;

    private PlayerController player;
    private Coroutine moveCoroutine;
    private MultiLineDraw line;

    //����
    private AudioSource audioSource;
    public AudioClip EnemySound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        line = GetComponent<MultiLineDraw>();
        currentState = MonsterState.Sleep;
        lastLineCreationTime = Time.time;
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        switch (currentState)
        {
            case MonsterState.Sleep:
                SleepState();
                break;
            case MonsterState.Find:
                // FindState();
                break;
        }
    }

    private void SleepState()
    {
        //if (Time.time >= nextCallTime)
        //{
        //   //���� ���� �ʿ� 
        //   line.CreateNewLineGroup();

        //    nextCallTime = Time.time + interval;
        //}
        //�⺻ ���� �̵� ���� ����
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Find Player");
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                currentState = MonsterState.Find;
            }
            moveCoroutine = StartCoroutine(MoveToPrePos());
        }
    }

    private IEnumerator MoveToPrePos()
    {
        if ((player.prePos == null) || (player == null))
        {
            // Debug.LogError("Player or prePos is null");
            yield break;
        }

        while (true)
        {
            Vector2 startPos = rb.position; // Rigidbody2D�� ���� ��ġ
            Vector2 targetPos = player.prePos;
            float journeyLength = Vector2.Distance(startPos, targetPos);
            float startTime = Time.time;

            while ((Vector2)rb.position != targetPos)
            {
                float currentTime = Time.time;

                // ���� �������� ���� ����
                if (currentTime - lastLineCreationTime >= lineCreationInterval)
                {
                    PlaySound(EnemySound);
                    line.CreateNewLineGroup();
                    lastLineCreationTime = currentTime;
                }

                // ��ǥ���� ����Ǿ����� Ȯ��
                if (targetPos != player.prePos)
                {
                    break; // ���� while ������ �������� ���ο� ��ǥ������ �̵� ����
                }

                float distanceCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distanceCovered / journeyLength;

                // Rigidbody2D�� ����Ͽ� �̵�
                Vector2 newPosition = Vector2.Lerp(startPos, targetPos, fractionOfJourney);
                rb.MovePosition(newPosition);

                yield return new WaitForFixedUpdate(); // FixedUpdate�� ����ȭ
            }

            // ��ǥ�� �����߰ų� ��ǥ���� ����Ǿ��� ��
            if ((Vector2)rb.position == player.prePos)
            {
                break; // ���� ��ǥ�� ���������� �ڷ�ƾ ����
            }
        }
        moveCoroutine = null;
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource�� �����ϴ�. Ȯ�����ּ���.");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("����� AudioClip�� �������� �ʾҽ��ϴ�.");
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}