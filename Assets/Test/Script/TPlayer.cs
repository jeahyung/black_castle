using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TPlayer : MonoBehaviour
{
    //----------------------------숨쉬기 간격---------------------------------------------------
    public float interval = 1f; // 함수 호출 간격 (초)
    private float nextCallTime;
    //---------------------------달리기 중 숨쉬기-------------------------------------------
    public float lineCreationInterval = 0.5f; // 라인 생성 간격 (초)
    private float lastLineCreationTime;
    //------------------------------------------------------------------------------------------

    public float moveSpeed = 5f;
    //private Rigidbody2D rb;
    //private Vector2 movement;

    //---------------------상태----------------------------------------------------------------
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

    //사운드
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
        //   //길이 조정 필요 
        //   line.CreateNewLineGroup();

        //    nextCallTime = Time.time + interval;
        //}
        //기본 순찰 이동 상태 구현
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
            Vector2 startPos = rb.position; // Rigidbody2D의 현재 위치
            Vector2 targetPos = player.prePos;
            float journeyLength = Vector2.Distance(startPos, targetPos);
            float startTime = Time.time;

            while ((Vector2)rb.position != targetPos)
            {
                float currentTime = Time.time;

                // 일정 간격으로 라인 생성
                if (currentTime - lastLineCreationTime >= lineCreationInterval)
                {
                    PlaySound(EnemySound);
                    line.CreateNewLineGroup();
                    lastLineCreationTime = currentTime;
                }

                // 목표점이 변경되었는지 확인
                if (targetPos != player.prePos)
                {
                    break; // 내부 while 루프를 빠져나가 새로운 목표점으로 이동 시작
                }

                float distanceCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distanceCovered / journeyLength;

                // Rigidbody2D를 사용하여 이동
                Vector2 newPosition = Vector2.Lerp(startPos, targetPos, fractionOfJourney);
                rb.MovePosition(newPosition);

                yield return new WaitForFixedUpdate(); // FixedUpdate와 동기화
            }

            // 목표에 도달했거나 목표점이 변경되었을 때
            if ((Vector2)rb.position == player.prePos)
            {
                break; // 최종 목표에 도달했으면 코루틴 종료
            }
        }
        moveCoroutine = null;
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 없습니다. 확인해주세요.");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("재생할 AudioClip이 설정되지 않았습니다.");
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}