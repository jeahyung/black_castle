using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLineDraw : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float lineLifeTime = 1f;
    public float lineWidth = 0.1f;
    public float maxLength = 10f;
    public float drawSpeed = 5f;
    public LayerMask reflectLayers;
    public int maxReflections = 5;
    public int lineCount = 18;
    public Material lineMaterial;
    private LinePool linePool;

    private List<LineGroup> lineGroups = new List<LineGroup>();
    private bool check = true;

    private class LineGroup
    {
        public List<LineRenderer> lines = new List<LineRenderer>();
        public List<float> currentLengths = new List<float>();
        public List<Vector3> currentDirections = new List<Vector3>();
        public List<Vector3> currentStartPositions = new List<Vector3>();
        public List<int> reflectionCounts = new List<int>();
        public float creationTime;
    }

    void Start()
    {
        linePool = FindObjectOfType<LinePool>();

        if (linePool == null)
        {
            Debug.LogError("LinePool is not assigned!");
            return;
        }
       // CreateNewLineGroup();
    }

    public void CreateNewLineGroup()
    {
        int angleCnt = 360 / lineCount;
        LineGroup newGroup = new LineGroup();
        newGroup.creationTime = Time.time;
        
        for (int i = 0; i < lineCount; i++)
        {
            GameObject lineObj = linePool.GetLine();
            if (lineObj == null) continue;

            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            SetupLineRenderer(lr);
            newGroup.lines.Add(lr);
            newGroup.currentLengths.Add(0f);
            newGroup.currentStartPositions.Add(transform.position);
            newGroup.reflectionCounts.Add(0);

            float angle = (i * angleCnt + 30) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            newGroup.currentDirections.Add(direction);
            
            ResetLine(lr, direction);
        }

        lineGroups.Add(newGroup);
    }

    void SetupLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 1;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.material = lineMaterial;

        float fadeStartTime = Time.time;
        DOTween.To(() => lr.startColor, x => lr.startColor = x, new Color(startColor.r, startColor.g, startColor.b, 0f), lineLifeTime)
               .SetDelay(fadeStartTime - Time.time);
        DOTween.To(() => lr.endColor, x => lr.endColor = x, new Color(endColor.r, endColor.g, endColor.b, 0f), lineLifeTime)
               .SetDelay(fadeStartTime - Time.time);
    }

    void Update()
    {
        for (int i = lineGroups.Count - 1; i >= 0; i--)
        {
            LineGroup group = lineGroups[i];
            for (int j = 0; j < group.lines.Count; j++)
            {
                DrawLine(group, j);
            }

            if (Time.time - group.creationTime > lineLifeTime)
            {
                foreach (var lr in group.lines)
                {
                    linePool.ReturnLine(lr.gameObject);
                }
                lineGroups.RemoveAt(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateNewLineGroup();
        }
    }

    void DrawLine(LineGroup group, int index)
    {
        LineRenderer lr = group.lines[index];
        if ((group.currentLengths[index] < maxLength) && (group.reflectionCounts[index] < maxReflections))
        {
            float remainingLength = maxLength - group.currentLengths[index];
            float distanceThisFrame = drawSpeed * Time.deltaTime;
            distanceThisFrame = Mathf.Min(distanceThisFrame, remainingLength);

            RaycastHit2D hit = Physics2D.Raycast(group.currentStartPositions[index], group.currentDirections[index], distanceThisFrame, reflectLayers);

            if ((hit.collider != null) && (check))
            {
                check = false;
                float distanceToHit = (hit.point - (Vector2)group.currentStartPositions[index]).magnitude;
                group.currentLengths[index] += distanceToHit;

                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, hit.point);

                group.currentDirections[index] = Vector2.Reflect(group.currentDirections[index], hit.normal).normalized;
                group.currentStartPositions[index] = hit.point;               
                group.reflectionCounts[index]++;

              
            }
            else
            {
          
                group.currentLengths[index] += distanceThisFrame;
                Vector3 endPosition = group.currentStartPositions[index] + group.currentDirections[index] * distanceThisFrame;
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, endPosition);
                group.currentStartPositions[index] = endPosition;
                check = true;
            }
        }
    }

    void ResetLine(LineRenderer lr, Vector3 direction)
    {
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
    }
}
