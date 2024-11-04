using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0); // Set initial horizontal speed
    }

    void FixedUpdate()
    {
        // Maintain horizontal speed
        rb.velocity = new Vector2(speed, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        // Reverse speed based on the collision normal
        if (collision.contacts.Length > 0)
        {
            // Get the normal of the collision to determine direction
            Vector2 normal = collision.contacts[0].normal;

            // Check if the collision is horizontal or vertical
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                // Horizontal collision
                speed = -speed; // Reverse speed
            }
            // You can also add more conditions here if you want specific behaviors
        }
    }
}
