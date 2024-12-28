using UnityEngine;
using System.Collections;

public class Projectile : Step
{
    //음파
    public float speed;
    public Vector3 direction;
   // public new float lifetime;
    //private float lifeTimer;
    private SpriteRenderer sprite;

    // 잔상
    public GameObject trailPrefab;
    public float trailInterval = 0.1f;
    private float trailTimer;
    private Color trailColor;

    private PlayerController player;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(FadeOut());
        lifeTimer = lifetime;


        trailColor = sprite.color;
    }

    void Update()
    {
        MoveProjectile();

        //음파
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        //잔상
        trailTimer -= Time.deltaTime;

        if (trailTimer <= 0)
        {
            CreateTrail();
            trailTimer = trailInterval;
        }
    }

    //음파
    void MoveProjectile()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            ReflectProjectile(collision.contacts[0].normal);
        }
    }

    void ReflectProjectile(Vector3 normal)
    {
        direction = Vector3.Reflect(direction, normal);
    }
    IEnumerator FadeOut()
    {
        // 시작부터 투명까지
        float startAlpha = sprite.color.a;
        float targetAlpha = 0f;
        float fadeDuration = lifetime;
        float elapsedTime = 0f;

        // 투명도 감소시키기
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, currentAlpha);
            yield return null;
        }
    }

    //잔상
    void CreateTrail()
    {
        // 잔상 생성
        GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        SpriteRenderer trailRenderer = trail.GetComponent<SpriteRenderer>();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trail.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        trailRenderer.color = trailColor;

        //시간 지나면 사라짐
        Destroy(trail, lifetime / 2);
        StartCoroutine(FadeTrail(trailRenderer));
    }

    IEnumerator FadeTrail(SpriteRenderer trailRenderer)
    {
        if (trailRenderer == null)
            yield break;

        float startAlpha = trailRenderer.color.a;
        float targetAlpha = 0f;
        float fadeDuration = lifetime / 2; // 잔상의 수명에 따라 투명도 조절
        float elapsedTime = 0f;

        // 투명도를 감소시키기
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            if (trailRenderer == null)
                yield break;

            trailRenderer.color = new Color(trailRenderer.color.r, trailRenderer.color.g, trailRenderer.color.b, currentAlpha);
            yield return null;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap")) //함정은 빨간색
        {
            sprite.color = Color.red;
            trailColor = Color.red;
        }

        if (other.CompareTag("Goal")) //도착점은 노란색
        {
            sprite.color = Color.yellow;
            trailColor = Color.yellow;
        }
        if (other.CompareTag("Lever")) //레버는 파란색
        {
            sprite.color = Color.blue;
            trailColor = Color.blue;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        BackToWhite();
    }

    void BackToWhite() //색깔을 원래대로 되돌리기
    { 
        sprite.color = Color.white;
        trailColor = Color.white;
    }
}