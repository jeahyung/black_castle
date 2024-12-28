using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

                if ((obj != null) && (obj.activeSelf)&&(spriteRenderer.enabled))                  //�⺻���� :1.�迭�� ���� ����Ǿ� ������ 2.Ȱ��ȭ �Ǿ� ������ 3.��������Ʈ �������� Ȱ��ȭ �Ǿ� ������
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
