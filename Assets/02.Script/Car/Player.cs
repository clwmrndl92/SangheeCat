using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject completePanel;
    public TimelineManager timelineManager;
    [Header("입력")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] public InputActionReference _jumpAction;
    [SerializeField] public InputActionReference _pickupAction;
    [Header("Physics Variables")]
    public Rigidbody2D rb;
    public float gravity;
    public Vector2 velocity;
    public float maxXVelocity = 50;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 50;
    //TODO: groundHeight 상수로 변경
    public float groundHeight = -2;
    public bool isGrounded = true;

    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float jumpGroundThreshold = 1;

    public bool isDead = false;
    public bool isCinema = true;

    public Transform[] groundRayOrigins;
    public Transform wallRayOrigin;
    public Transform[] obstacleForwardRayOrigins;
    public Transform[] obstacleDownwardRayOrigins;
    

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask pickupLayerMask;
    
    private Animator _animator;
    public Transform pulloutEnd;
    public Transform pulloutBezier;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void OnEnable()
    {
        _inputActionAsset.Enable();
    }
	
    private void OnDisable()
    {
        _inputActionAsset.Disable();
    }

    void Update()
    {
        Vector2 pos = groundRayOrigins.First().position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (_jumpAction.action.triggered)
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                rb.velocity = Vector2.up * jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (_jumpAction.action.triggered)
        {
            isHoldingJump = false;
        }
        
        if(_pickupAction.action.triggered)
        {
            Debug.Log("Pickup");
            checkPickup();
        }
    }
    
    IEnumerator Dead(float delay)
    {
        yield return new WaitForSeconds(delay);
        completePanel.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (isCinema)
        {
            return;
        }

        if (distance > 1000)
        {
            isCinema = true;
            velocity.x = 0;
            StartCoroutine(Dead(8f));
            timelineManager.EpilogueStart();
            transform.GetChild(0).gameObject.SetActive(false);
        }

        Vector2 pos = groundRayOrigins.First().position;

        // Check if player is dead
        if (pos.y < -20)
        {
            velocity.x = 0;
            isDead = true;
            Debug.Log("Dead");
            StartCoroutine(Dead(1f));
        }

        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
                rb.velocity += Vector2.up * (gravity * Time.fixedDeltaTime);
            }

            // groundRayOrigins들을 순회하며 레이캐스트를 쏴서 땅에 닿았는지 확인, 하나라도 닿았으면 빠져나와 break
            foreach (var groundRayOrigin in groundRayOrigins)
            {
                Vector2 rayOrigin = groundRayOrigin.position;
                Vector2 rayDirection = Vector2.up;
                float rayDistance = velocity.y * Time.fixedDeltaTime;
                RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
                Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
                if (hit2D.collider != null)
                {
                    Ground ground = hit2D.collider.GetComponent<Ground>();
                    if (ground != null)
                    {
                        //TODO: groundHeight 상수로 변경
                        if (pos.y >= ground.groundHeight)
                        {
                            groundHeight = ground.groundHeight;
                            pos.y = groundHeight;
                            velocity.y = 0;
                            rb.velocity = Vector2.zero;
                            isGrounded = true;
                            break;
                        }
                    }
                }
            }
            
            Vector2 wallOrigin = wallRayOrigin.position;
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }


            Vector2 rayOrigin = groundRayOrigins.Last().position;
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }

        // obstacleRayOrigins들을 순회하며 레이캐스트를 쏴서 장애물에 닿았는지 확인
        // 전방
        foreach (var obstacleRayOrigin in obstacleForwardRayOrigins)
        {
            Vector2 obstOrigin = obstacleRayOrigin.position;
            // Check forward collision
            RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
            if (obstHitX.collider != null)
            {
                Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    hitObstacle(obstacle);
                }
            }
        }
        // 하단
        foreach (var obstacleRayOrigin in obstacleDownwardRayOrigins)
        {
            Vector2 obstOrigin = obstacleRayOrigin.position;
            // Check downward collision
            RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
            if (obstHitY.collider != null)
            {
                Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    hitObstacle(obstacle);
                }
            }
        }
    }
    
    // 충돌 시 이벤트 작성
    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        // 오브젝트 충돌 시 70% 감속
        velocity.x *= 0f;
        // Hurt 트리거로 Hurt 애니메이션 재생
        _animator.SetTrigger("Hurt");
    }

    void checkPickup()
    {
        // 하단
        foreach (var obstacleRayOrigin in obstacleDownwardRayOrigins)
        {
            Vector2 obstOrigin = obstacleRayOrigin.position;
            // Check downward collision
            RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, pickupLayerMask);
            if (obstHitY.collider != null)
            {
                Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    pickupCrop(obstacle);
                }
            }
        }
    }
    
    void pickupCrop(Obstacle obstacle)
    {
        if (obstacle.isBad)
        {
            // 오브젝트 충돌 시 70% 감속
            velocity.x *= 0f;
            // Hurt 트리거로 Hurt 애니메이션 재생
            _animator.SetTrigger("Hurt");
            //TODO: Obstacle 애니메이션 재생
            obstacle.pullOut();
        }
        else
        {
            Debug.Log("Good Carrot");
            //TODO: 점수 증가
            
            //TODO: Obstacle 애니메이션 재생
            obstacle.pullOut();
        }
    }
}
