using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWhenDrop : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y < -.5f)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
}
