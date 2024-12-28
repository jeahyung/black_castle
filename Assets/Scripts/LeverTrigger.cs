using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public GameObject goalObject; // ��

    void Start()
    {
        // �� ����
        if (goalObject != null)
        {
            goalObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateLever();
        }
    }

    void ActivateLever() //�� Ű��
    {
        if (goalObject != null)
        {
            goalObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
