using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected Vector2 velocity;
    public float gravityFactor = 1f;
    protected bool grounded;
    public float desiredx;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        velocity += Physics2D.gravity * gravityFactor * Time.deltaTime;
        velocity.x = desiredx;

        Vector2 move = velocity * Time.deltaTime;
        grounded = false;
        Movement(new Vector2(move.x, 0), true);
        Movement(new Vector2(0, move.y), false);

    }

    public void Movement(Vector2 move, bool movex)
    {
        if (move.magnitude < 0.00001f) return;
        RaycastHit2D[] results = new RaycastHit2D[16];
        int cnt = GetComponent<Rigidbody2D>().Cast(move, results, move.magnitude + 0.01f);
        if (cnt > 0)
        {
            for (int i = 0; i < cnt; ++i)
            {
                CollideWith(results[i].collider);
                
                if (results[i].collider.isTrigger)
                {
                    CollideWithTrigger(results[i].collider);
                }
                else
                {
                    if (Mathf.Abs(results[i].normal.x) > 0.5 && movex)
                    {
                        move.x = 0;
                        velocity.x = 0;
                        CollideWithHorizontal(results[i].collider);
                    }
                    else if (!movex)
                    {
                        move.y = 0;
                        velocity.y = 0;
                        grounded = true;
                        CollideWithVertical(results[i].collider);
                    }
                }
            }
        }
        transform.position += (Vector3)(move);
    }

    virtual public void CollideWith(Collider2D other)
    {

    }

    virtual public void CollideWithTrigger(Collider2D other)
    {

    }

    virtual public void CollideWithHorizontal(Collider2D other)
    {

    }

    virtual public void CollideWithVertical(Collider2D other)
    {

    }
}
