using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Collider2D collider;

    public Vector3 prePos;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            prePos = transform.position;
            this.gameObject.GetComponent<MultiLineDraw>().CreateNewLineGroup();
            this.collider.enabled = false;
        }
    }
}
