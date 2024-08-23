using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    public float genScreenOffset = 10;
    
    public bool isGapped = false;
    public bool isInitiallized = false;
    
    BoxCollider2D collider;

    bool didGenerateGround = false;
    

    public GroundTemplate[] groundTemplates;
    public GameObject[] obstacleTemplates;
    public float[] obstacleProbabilities;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y / 2) + (collider.offset.y / 2);
        screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
    }

    private void FixedUpdate()
    {
        if (player.isCinema)
        {
            return;
        }
        
        var position = transform.position;
        
        groundRight = position.x + (collider.size.x / 2);
        
        Vector2 pos = position;
        
        // Move the ground to the left based on the player's velocity
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        // If the ground is off the screen, destroy it
        if (groundRight < -1 * screenRight)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = pos;

        if (!didGenerateGround && isInitiallized)
        {
            // Don't generate ground if the ground is not visible
            if (groundRight > screenRight + genScreenOffset)
            {
                return;
            }
            
            generateGround();
            didGenerateGround = true;
        }
    }

    void generateGround()
    {
        Vector2 pos;
        
        // Randomly select a ground template
        int groundIndex = Random.Range(0, groundTemplates.Length + 1);
        Debug.Log("Ground index: " + groundIndex);

        GameObject go;
        Ground ground;
        BoxCollider2D goCollider;
        pos.y = transform.position.y;
        
        if (groundIndex == groundTemplates.Length && !isGapped)
        {
            go = Instantiate(groundTemplates[1].gameObject);
            ground = go.GetComponent<Ground>();
            goCollider = go.GetComponent<BoxCollider2D>();
            
            ground.isGapped = true;
            // Blank ground + no obstacles
            float h1 = player.jumpVelocity * player.maxHoldJumpTime;
            float t = player.jumpVelocity / -player.gravity;
            float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
            float maxJumpHeight = h1 + h2;
            
            float t1 = t + player.maxHoldJumpTime;
            float t2 = Mathf.Sqrt((2.0f * maxJumpHeight * 0.7f) / -player.gravity);
            float totalTime = t1 + t2;
            float maxX = totalTime * player.velocity.x;
            maxX *= 0.6f;
            float gapX = Random.Range(maxX * 0.25f, maxX);
            
            pos.x = groundRight + goCollider.size.x / 2 + gapX;
        }
        else
        {
            if (groundIndex == groundTemplates.Length) groundIndex = 1;
            go = Instantiate(groundTemplates[groundIndex].gameObject);
            goCollider = go.GetComponent<BoxCollider2D>();
            
            // 2.11f is the width of cliff collider
            pos.x = groundRight + goCollider.size.x / 2 - (2.11f * 2f);
        }
        
        go.transform.position = pos;
        
        // Randomly select obstacleTemplate based on the probability
        float rand = Random.Range(0.0f, 1.0f);
        for(int index = 0; index < obstacleProbabilities.Length; index++)
        {
            if (rand < obstacleProbabilities[index])
            {
                // If the index is obstacleTemplates.Length, it means no obstacle
                if (index == obstacleTemplates.Length)
                {
                    break;
                }
                GameObject obstacleTemplate = Instantiate(obstacleTemplates[index].gameObject);
                var obstacles = obstacleTemplate.GetComponentsInChildren<Obstacle>();
                foreach (var obstacle in obstacles)
                {
                    obstacle.ground = this;
                    obstacle.isInitialized = true;
                }
                Vector2 obsPos = new Vector2(pos.x, groundHeight);
                obstacleTemplate.transform.position = obsPos;
                break;
            }
        }
        
        ground = go.GetComponent<Ground>();
        ground.isInitiallized = true;
    }

}
