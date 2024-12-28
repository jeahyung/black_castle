using UnityEngine;
using System.Collections;

public class Projectile : Step
{
    //����
    public float speed;
    public Vector3 direction;
   // public new float lifetime;
    //private float lifeTimer;
    private SpriteRenderer sprite;

    // �ܻ�
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

        //����
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        //�ܻ�
        trailTimer -= Time.deltaTime;

        if (trailTimer <= 0)
        {
            CreateTrail();
            trailTimer = trailInterval;
        }
    }

    //����
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
        // ���ۺ��� �������
        float startAlpha = sprite.color.a;
        float targetAlpha = 0f;
        float fadeDuration = lifetime;
        float elapsedTime = 0f;

        // ���� ���ҽ�Ű��
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, currentAlpha);
            yield return null;
        }
    }

    //�ܻ�
    void CreateTrail()
    {
        // �ܻ� ����
        GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        SpriteRenderer trailRenderer = trail.GetComponent<SpriteRenderer>();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trail.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        trailRenderer.color = trailColor;

        //�ð� ������ �����
        Destroy(trail, lifetime / 2);
        StartCoroutine(FadeTrail(trailRenderer));
    }

    IEnumerator FadeTrail(SpriteRenderer trailRenderer)
    {
        if (trailRenderer == null)
            yield break;

        float startAlpha = trailRenderer.color.a;
        float targetAlpha = 0f;
        float fadeDuration = lifetime / 2; // �ܻ��� ���� ���� ���� ����
        float elapsedTime = 0f;

        // ������ ���ҽ�Ű��
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
        if (other.CompareTag("Trap")) //������ ������
        {
            sprite.color = Color.red;
            trailColor = Color.red;
        }

        if (other.CompareTag("Goal")) //�������� �����
        {
            sprite.color = Color.yellow;
            trailColor = Color.yellow;
        }
        if (other.CompareTag("Lever")) //������ �Ķ���
        {
            sprite.color = Color.blue;
            trailColor = Color.blue;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        BackToWhite();
    }

    void BackToWhite() //������ ������� �ǵ�����
    { 
        sprite.color = Color.white;
        trailColor = Color.white;
    }
}