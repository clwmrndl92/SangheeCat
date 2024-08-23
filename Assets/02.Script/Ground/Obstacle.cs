using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Player player;
    public Ground ground;
    
    public bool isBad = true;
    public bool isInitialized = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Move the obstacle to the left based on the player's velocity
    private void FixedUpdate()
    {
        if (isInitialized == false)
        {
            return;
        }
        Vector2 pos = transform.position;

        // Move the obstacle to the left based on the player's velocity
        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        
        //TODO: When the obstacle reaches player's x position, play pop up animation
        
        // Destroy the obstacle if it goes off the left edge of the screen
        if (ground is null)
        {
            Destroy(gameObject);
            
            // Destroy the parent if it has no children
            if (transform.parent.childCount == 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        // Update the obstacle's position
        transform.position = pos;
    }
}