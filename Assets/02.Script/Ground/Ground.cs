using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    
    [Header("Sprite Shape")]
    [SerializeField] private SpriteShapeController _spriteShapeController;

    [SerializeField, Range(3f, 100f)] private int _levelLength = 30;
    [SerializeField, Range(1f, 50f)] private float _xMultiplier = 2f;
    [SerializeField, Range(1f, 50f)] private float _yMultiplier = 2f;
    [SerializeField, Range(0f, 1f)] private float _curveSmoothness = 0.5f;
    [SerializeField] private float _noiseStep = 0.1f;
    [SerializeField] private float _bottom = 20f;

    private Vector3 _lastPos;

    // Initialize the ground when instantiated
    private void Awake()
    {
        player = GameObject.Find("Character").GetComponent<Player>();

        screenRight = Camera.main.transform.position.x * 2;
        
        _spriteShapeController = GetComponent<SpriteShapeController>();
    }
    
    void Update()
    {
        groundHeight = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;


        groundRight = transform.position.x + ((_levelLength - 1) * _xMultiplier);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        // Instantiate a new ground
        GameObject go = Instantiate(gameObject);
        
        // Set the position of the new ground
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.5f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX;
        go.transform.position = pos;
        
        // Generate the ground by calling the generateGround method
        InitializeGround(go.GetComponent<SpriteShapeController>());
        
        //TODO: Move to InitializeGround Method (to know which Point to insert the obstacle)
        // Instantiate obstacles
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y;

        int obstacleNum = Random.Range(0, 4);
        for (int i=0; i<obstacleNum; i++)
        {
            GameObject box = Instantiate(boxTemplate.gameObject);
            float y = goGround.groundHeight + _yMultiplier;
            float halfWidth = (((_levelLength - 1) * _xMultiplier) / 2) - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }
    }

    private void InitializeGround(SpriteShapeController spriteShapeController)
    {
        spriteShapeController.spline.Clear();

        for (int i = 0; i < _levelLength; i++)
        {
            _lastPos = transform.position + new Vector3(i * _xMultiplier, Mathf.PerlinNoise(0, i * _noiseStep) * _yMultiplier);
            spriteShapeController.spline.InsertPointAt(i, _lastPos);

            // Set the tangent mode to continuous, Left/Right tangent to make the curve smooth
            if (i != 0 && i != _levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * (_xMultiplier * _curveSmoothness));
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * (_xMultiplier * _curveSmoothness));
            }
        }
        
        spriteShapeController.spline.InsertPointAt(_levelLength, new Vector3(_lastPos.x, transform.position.y - _bottom));
        
        spriteShapeController.spline.InsertPointAt(_levelLength + 1, new Vector3(transform.position.x, transform.position.y - _bottom));
    }
}
