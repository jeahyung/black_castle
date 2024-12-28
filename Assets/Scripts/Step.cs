using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour //발자국
{
    public float lifetime;
    public float lifeTimer;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut());
        lifeTimer = lifetime;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FadeOut()
    {
        // 시작부터 투명까지
        float startAlpha = spriteRenderer.color.a;
        float targetAlpha = 0f;
        float fadeDuration = lifetime;
        float elapsedTime = 0f;

        // 투명도를 서서히 감소
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, currentAlpha);
            yield return null;
        }
    }
}
