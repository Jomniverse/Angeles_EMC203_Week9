
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    [Header("References")]
    public PlayerInput_Manager playerInput;
    public PlayerUI_Manager playerUI;

    [Header("Player Sprites")]
    public SpriteRenderer playerSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Lane Movement")]
    public int currentLane = 1;
    public float laneWidth = 2f;
    public float laneSwitchDuration = 0.2f;

    private float startX;
    private float targetX;
    private float laneTimer;

    [Header("Jump")]
    public float jumpHeight = 2.5f;
    public float jumpDuration = 0.8f;

    private bool isJumping;
    private float jumpTimer;
    private float groundY;

    [Header("Manual Hitbox")]
    public Vector3 playerHalfSize =
        new Vector3(0.4f, 0.5f, 0f);

    public Vector3 centerOffset = Vector3.zero;

    [Header("Health")]
    public float maxHP = 100f;
    public float currentHP;
    public float regenerationRate = 1f;

    private void Start()
    {
        groundY = transform.position.y;

        currentLane = Mathf.Clamp(currentLane, 0, 2);

        targetX = (currentLane - 1) * laneWidth;
        startX = targetX;

        laneTimer = laneSwitchDuration;

        transform.position = new Vector3(
            targetX,
            groundY,
            0f
        );

        currentHP = maxHP;
        UpdateUI();
    }

    private void Update()
    {
        HandleInput();
        HandleLaneMovement();
        HandleJump();
        RegenerateHealth();
    }

    private void HandleInput()
    {
        if (playerInput.laneInput != 0)
        {
            int newLane = Mathf.Clamp(
                currentLane + playerInput.laneInput,
                0,
                2
            );

            if (newLane != currentLane)
            {
                currentLane = newLane;

                startX = transform.position.x;
                targetX = (currentLane - 1) * laneWidth;

                laneTimer = 0f;

                if (playerInput.laneInput == -1)
                {
                    playerSprite.sprite = leftSprite;
                }
                else if (playerInput.laneInput == 1)
                {
                    playerSprite.sprite = rightSprite;
                }
            }
        }

        if (playerInput.jumpInput && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    private void HandleLaneMovement()
    {
        if (laneTimer >= laneSwitchDuration)
            return;

        laneTimer += Time.deltaTime;

        float t = Mathf.Clamp01(
            laneTimer / laneSwitchDuration
        );

        float easedT = Mathf.SmoothStep(0f, 1f, t);

        float newX =
            startX + (targetX - startX) * easedT;

        transform.position = new Vector3(
            newX,
            transform.position.y,
            0f
        );
    }

    private void HandleJump()
    {
        if (!isJumping)
            return;

        jumpTimer += Time.deltaTime;

        float t = Mathf.Clamp01(
            jumpTimer / jumpDuration
        );

        float height =
            4f * jumpHeight * t * (1f - t);

        transform.position = new Vector3(
            transform.position.x,
            groundY + height,
            0f
        );

        if (t >= 1f)
        {
            isJumping = false;
        }
    }

    public bool CheckCollision(Item obstacle)
    {
        Vector3 playerCenter =
            transform.position + centerOffset;

        Vector3 obstacleCenter = obstacle.GetCollisionCenter();

        float xDistance = Mathf.Abs(
            playerCenter.x - obstacleCenter.x
        );

        float yDistance = Mathf.Abs(
            playerCenter.y - obstacleCenter.y
        );

        bool overlapX =
            xDistance <= playerHalfSize.x + obstacle.halfSize.x;

        bool overlapY =
            yDistance <= playerHalfSize.y + obstacle.halfSize.y;

        return overlapX && overlapY;
    }

    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(0f, currentHP - damage);
        UpdateUI();
    }

    private void RegenerateHealth()
    {
        if (currentHP >= maxHP)
            return;

        currentHP = Mathf.Min(
            currentHP + regenerationRate * Time.deltaTime,
            maxHP
        );

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerUI != null)
        {
            playerUI.UpdateHealth(currentHP, maxHP);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            transform.position + centerOffset,
            playerHalfSize * 2f
        );
    }
}
