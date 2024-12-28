using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public GameObject goalObject; // °ñ

    void Start()
    {
        // °ñ ²ô±â
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

    void ActivateLever() //°ñ Å°±â
    {
        if (goalObject != null)
        {
            goalObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
