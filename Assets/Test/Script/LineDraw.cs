using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class LineDraw : MonoBehaviour
{

    public float lineWidth = 0.1f;
    public float maxLength = 10f;
    public float drawSpeed = 5f;
    public LayerMask reflectLayers;
    public int maxReflections = 5;
    public Material lineMaterial; // 라인 머티리얼

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private LineRenderer currentLineRenderer;
    private float currentLength = 0f;
    private Vector3 currentDirection;
    private Vector3 currentStartPosition;
    private int reflectionCount = 0;
    private bool c = true;


    void Start()
    {
        CreateNewLine();
    }

    void Update()
    {
        DrawLine();

        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateNewLine();
        }
    }

    public void CreateNewLine()
    {
        GameObject lineObj = new GameObject("Line_" + lineRenderers.Count);
        lineObj.transform.SetParent(transform);
        currentLineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderers.Add(currentLineRenderer);

        currentLineRenderer.material = lineMaterial;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        currentLineRenderer.positionCount = 1;
        currentStartPosition = transform.position;
        currentDirection = transform.right;
        currentLineRenderer.SetPosition(0, currentStartPosition);

        currentLineRenderer.startColor = new Color(1f, 1f, 1f, 1f);
        currentLineRenderer.endColor = new Color(1f, 1f, 1f, 1f);

        // 새로운 DOTween 애니메이션 적용
        DOTween.To(() => currentLineRenderer.startColor, x => currentLineRenderer.startColor = x, new Color(1f, 1f, 1f, 0f), 4.5f);
        DOTween.To(() => currentLineRenderer.endColor, x => currentLineRenderer.endColor = x, new Color(1f, 1f, 1f, 0f), 4.5f);

        currentLength = 0f;
        reflectionCount = 0;
    }

    public void DrawLine()
    {
        
        if (currentLength < maxLength && reflectionCount < maxReflections)
        {
            float remainingLength = maxLength - currentLength;
            float distanceThisFrame = drawSpeed * Time.deltaTime;
            distanceThisFrame = Mathf.Min(distanceThisFrame, remainingLength);

            RaycastHit2D hit = Physics2D.Raycast(currentStartPosition, currentDirection, distanceThisFrame, reflectLayers);

            if ((hit.collider != null)&&(c == true))
            {
                c = false;
                float distanceToHit = (hit.point - (Vector2)currentStartPosition).magnitude;
                currentLength += distanceToHit;

                currentLineRenderer.positionCount++;
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, hit.point);

                currentDirection = Vector2.Reflect(currentDirection, hit.normal).normalized;
                currentStartPosition = hit.point;
                reflectionCount++;

                distanceThisFrame -= distanceToHit;
            }
            else
            {
                currentLength += distanceThisFrame;
                Vector3 endPosition = currentStartPosition + currentDirection * distanceThisFrame;
                currentLineRenderer.positionCount++;
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, endPosition);
                currentStartPosition = endPosition;
                
                c = true;
            }
        }
    }

}
