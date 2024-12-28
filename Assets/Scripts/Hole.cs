using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    public GameObject[] createObjects;
    public GameObject[] disappearObjects;

    void Start()
    {
        RegisterObjects();
    }

    void RegisterObjects()
    {
        Transform createTransform = transform.Find("Create");
        Transform disappearTransform = transform.Find("Disappear");

        if (createTransform != null)
        {
            createObjects = GetChildObjects(createTransform);
        }
        else
        {
            Debug.LogWarning("Create object not found under Hole");
        }

        if (disappearTransform != null)
        {
            disappearObjects = GetChildObjects(disappearTransform);
        }
        else
        {
            Debug.LogWarning("Disappear object not found under Hole");
        }
    }

    GameObject[] GetChildObjects(Transform parent)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i).gameObject);
        }
        return children.ToArray();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ActivateCreateObjects();

        }
    }

    private void ActivateCreateObjects()
    {
        if (createObjects != null)
        {
            foreach (GameObject obj in createObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
        if (disappearObjects != null)
        {
            foreach (GameObject obj in disappearObjects)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

                if ((obj != null) && (obj.activeSelf) && (spriteRenderer.enabled))
                {
                    obj.GetComponent<MultiLineDraw>().CreateNewLineGroup();

                    Collider2D collider = obj.GetComponent<Collider2D>();

                    if (spriteRenderer != null)
                    {
                        spriteRenderer.enabled = false;
                    }
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }

    }
}
