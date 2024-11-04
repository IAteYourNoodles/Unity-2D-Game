using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : PhysicsObject
{
    public int speed = 3;
    public SpriteRenderer spriteRenderer;
    public Vector2 startpos;
    public int score;
    public Text scoreText;
    public Tilemap tilemap;
    public GameObject waterzonePrefab;
    public TileBase regularTile;
    public TileBase waterzoneTile;

    // Obstacle Management 
    public int defaultObstacleSpeed = 3;
    public List<MaceBehavior> movingObstacles = new List<MaceBehavior>();
    public List<GameObject> addedObstacles = new List<GameObject>();
    public List<Vector2Int> obstacleLocations = new List<Vector2Int>();
    public List<Vector2Int> tempObstacleLocations;

    // Ground detection
    public float groundCheckDistance;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetButton("Jump") && isGrounded())
        {
            velocity.y = 6.5f;
        }
    }

    // Initializes player properties
    private void InitializePlayer()
    {
        startpos = transform.position;
        score = 0;
        UpdateScoreText();
        tempObstacleLocations = new List<Vector2Int>(obstacleLocations);
    }

    // Handles horizontal movement and sprite flipping
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0)
        {
            desiredx = speed;
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            desiredx = -1 * speed;
            spriteRenderer.flipX = true;
        }
        else desiredx = 0;
        spriteRenderer.flipX = horizontalInput < 0;
    }

    private bool isGrounded()
    {
        float halfWidth = GetComponent<Collider2D>().bounds.extents.x; // Half width of the collider
        Vector2 leftPoint = (Vector2)transform.position + new Vector2(-halfWidth, 0);
        Vector2 rightPoint = (Vector2)transform.position + new Vector2(halfWidth, 0);

        // Check the ground beneath both sides
        RaycastHit2D hitLeft = Physics2D.Raycast(leftPoint, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightPoint, Vector2.down, groundCheckDistance, groundLayer);

        // Return true if either ray hits a collider
        return hitLeft.collider != null || hitRight.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            score = 0;
            scoreText.text = $"Score: {score.ToString()}";
            transform.position = startpos;
            ClearAddedObstacles();
        }
        if (collider.gameObject.GetComponent<Chest>())
        {
            score++;
            scoreText.text = $"Score: {score.ToString()}";
            collider.gameObject.GetComponent<Animator>().SetTrigger("reachchest");
            transform.position = startpos;
            Debug.Log("Next level");
            IncreaseDifficulty();
        }
    }

    private void ClearAddedObstacles()
    {
        foreach (Vector2Int location in obstacleLocations)
        {
            tilemap.SetTile((Vector3Int)location, regularTile);
        }
        foreach (GameObject obstacle in addedObstacles)
        {
            Destroy(obstacle);
        }
        tempObstacleLocations = new List<Vector2Int>(obstacleLocations);

        foreach (MaceBehavior obstacle in movingObstacles)
        {
            obstacle.speed = defaultObstacleSpeed;
        }
    }

    private void IncreaseDifficulty()
    {
        for (int i = 0; i < 2; i++)
        {
            if (tempObstacleLocations.Count <= 0)
            {
                return;
            }
            int rndIndex = Random.Range(0, tempObstacleLocations.Count);
            Debug.Log($"Added at {tempObstacleLocations[rndIndex]}");
            Vector2Int location = tempObstacleLocations[rndIndex];
            tempObstacleLocations.RemoveAt(rndIndex);
            tilemap.SetTile((Vector3Int)location, waterzoneTile);
            addedObstacles.Add(Instantiate(waterzonePrefab, tilemap.CellToWorld((Vector3Int)location) + new Vector3(0.5f, 0.5f, 0f), Quaternion.identity));
        }
        foreach (MaceBehavior obstacle in movingObstacles)
        {
            if (obstacle.speed > 0)
                obstacle.speed++;
            else
                obstacle.speed--;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}
