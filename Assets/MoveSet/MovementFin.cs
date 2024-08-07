using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovementFin : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer, enemyLayer;
    LayerMask pustwarstwa;

    PlayerStatsFin playerStats;

    [SerializeField] GameObject vignette;
    byte vinetteVisible = 255;

    AudioSource audio;
    [SerializeField] AudioClip[] runningClips;
    [SerializeField] float audioSpeed = 0.35f;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] AudioClip getHitAudio;
    [SerializeField] AudioClip dashAudio;
    bool playingSoundOn;
    bool soundCorutineisOn;

    [SerializeField] float speed = 4f;
    [SerializeField] float jumpingPower = 7f;
    [SerializeField] float dashPower = 5f;
    [SerializeField] float leftWallJump = 0.5f;
    [SerializeField] float rightWallJump = 0.5f;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Animator animator;
    Transform sword;

    float horizontal;
    float lastDirection;

    bool isJumping, willJump;
    bool isGrounded, wasGrounded;
    bool coyoteTime;
    [SerializeField]float coyoteTimeDuration = 1;
    bool isDashing;
    bool sideJump;

    bool isJumpingRight, isJumpingLeft;

    KeyCode jumpKey = KeyCode.Z;
    KeyCode dashKey = KeyCode.LeftShift;
    //KeyCode spellKey = KeyCode.X;
    //KeyCode meleeKey = KeyCode.C;
    private bool getDamageFromEnemy;
    [SerializeField]
    private float pushBackForce;

    private void Start()
    {
        playingSoundOn = false;
        soundCorutineisOn = false;
        Application.targetFrameRate = 120;

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        sword = transform.GetChild(0);

        audio = GetComponent<AudioSource>();
        StartCoroutine(runningSound());

        //StartCoroutine(running());

        playerStats = GetComponent<PlayerStatsManager>().playerStats;
        UpdateAllStats();
        
    }

    void Update()
    {
        GetInput();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetComponent<PlayerStatsManager>().ResetStats();
        }
        if(gameObject.transform.position.y < -5)
        {
            gameObject.transform.position = Vector3.zero;
        }
        MoveAccordingly();
    }


    private void FixedUpdate()
    {
        // moving player horizontally:
        if(!isDashing && !sideJump && !getDamageFromEnemy)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (Mathf.Abs(horizontal * speed) > 0.1f)
        {
            animator.SetBool("isRunning", true);

            if (animator.GetBool("isRunning") && !animator.GetBool("isGrounded"))
                playingSoundOn = false;

            if (!playingSoundOn)
            {
                if(!soundCorutineisOn)
                    StartCoroutine(runningSound());
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            playingSoundOn = false;
        }

        if (isJumpingLeft)
        {
            rb.velocity = new Vector2(-jumpingPower, jumpingPower);
            isJumpingLeft = false;
            Invoke(nameof(EndSideJump), 0.1f);
            audio.PlayOneShot(jumpAudio, 0.8f);
        }
        else if (isJumpingRight)
        {
            rb.velocity = new Vector2(jumpingPower, jumpingPower);
            isJumpingRight = false;
            Invoke(nameof(EndSideJump), 0.1f);
            audio.PlayOneShot(jumpAudio, 0.8f);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    void GetInput()
    {
        bool ZPressed = false;
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
            lastDirection = horizontal;

        isGrounded = IsGrounded();
        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(jumpKey))
            ZPressed = true;

        if (Input.GetKeyDown(dashKey))
        {
            isDashing = true;
            audio.PlayOneShot(dashAudio, 0.8f);
        }


        // coyoteTime
        if (!isGrounded && wasGrounded && !Input.GetKey(jumpKey))
        {
            coyoteTime = true;
            Invoke(nameof(EndCoyoteTime), coyoteTimeDuration);
        }

        #region JUMP

        if (ZPressed && !isGrounded && !coyoteTime)
        {
            if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.5f, groundLayer))
                willJump = true;
        }
        else if (ZPressed && (isGrounded || coyoteTime))
        {
            isJumping = true;
            coyoteTime = false;
        }

        if (willJump && isGrounded)
        {
            isJumping = true;
            willJump = false;
        }

        if (Input.GetKeyUp(KeyCode.Z) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        #endregion

        //wall jump
        if (Physics2D.OverlapBox(new Vector2(transform.position.x + 0.410643f, transform.position.y + 1f), new Vector2(0.320549f, 1.392045f), 0, groundLayer) && !isGrounded && ZPressed)
        {
            isJumpingLeft = true;
            sideJump = true;
        }
        
        if (Physics2D.OverlapBox(new Vector2(transform.position.x + -0.4013805f, transform.position.y + 1f), new Vector2(0.2293253f, 1.392045f), 0, groundLayer) && !isGrounded && ZPressed)
        {
            isJumpingRight = true;
            sideJump = true;
        }

        wasGrounded = isGrounded;
    }


    void MoveAccordingly()
    {
        if(horizontal > 0)
            transform.GetComponent<SpriteRenderer>().flipX = true;
        else if (horizontal < 0)
            transform.GetComponent<SpriteRenderer>().flipX = false;

        //if(horizontal == 0 && rb.velocity.x > 0.1f)
        //    transform.GetComponent<SpriteRenderer>().flipX = true;
        //else if (horizontal == 0 && rb.velocity.x < -0.1f)
        //    transform.GetComponent<SpriteRenderer>().flipX = false;

        if (isJumping)
        {
            audio.PlayOneShot(jumpAudio, 0.8f);

            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isJumping=false;
        }

        if (isDashing)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(lastDirection * dashPower, 0),ForceMode2D.Impulse);
            boxCollider.excludeLayers = enemyLayer;
            Invoke(nameof(EndDash), 0.1f);
            Invoke(nameof(EndDashArmor), 0.5f);
        }
    }

    public void UpdateAllStats()
    {
        foreach (var stat in playerStats.stats)
        {
            switch (stat.statName)
            {
                case PlayerStatEnum.moveSpeed:
                    speed = stat.value;
                    break;
                case PlayerStatEnum.dashPower:
                    dashPower = stat.value;
                    break;
                case PlayerStatEnum.jumpPower:
                    jumpingPower = stat.value;
                    break;
            }
        }
    }

    void EndCoyoteTime()
    {
        coyoteTime = false;
    }

    void EndDash()
    {
        isDashing = false;
    }

    void EndDashArmor()
    {
        boxCollider.excludeLayers = pustwarstwa;
    }

    void EndSideJump()
    {
        sideJump = false;
    }

    public float GetJump() { return jumpingPower; }

    public float GetDash() { return dashPower; }

    public float GetMovement() { return speed; }

    IEnumerator ChangeDamageEnterFlagAfterDelay(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        getDamageFromEnemy = value;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            DamageOnCollision(collision.collider);  
        }
        
    }

    public void DamageOnCollision(Collider2D coll)
    {
        getDamageFromEnemy = true;

        audio.PlayOneShot(getHitAudio, 1f);

        vignette.SetActive(true);
        StartCoroutine(HideBlood());

        if (coll.transform.position.x > transform.position.x)
            rb.AddForce(new Vector2(-1, 1) * pushBackForce, ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(1, 1) * pushBackForce, ForceMode2D.Impulse);

        StartCoroutine(ChangeDamageEnterFlagAfterDelay(false, 0.5f));
    }


    IEnumerator HideBlood()
    {
        vinetteVisible = 255;
        
        while (vinetteVisible > 20)
        {
            vignette.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,0,0, vinetteVisible);
            vinetteVisible -= 15;
            yield return new WaitForSeconds(0.01f);
        }

        vignette.SetActive(false);
    }

    IEnumerator runningSound()
    {
        int max = runningClips.Length;
        int num = 0;
        int oldNum = 1;

        soundCorutineisOn = true;

        while (animator.GetBool("isRunning") && animator.GetBool("isGrounded")) // while isRunning
        {
            while (num == oldNum)
            {
                num = Random.Range(0, max);
            }

            audio.clip = runningClips[num];
            audio.Play();

            oldNum = num;
            playingSoundOn = true;
            yield return new WaitForSeconds(audioSpeed);
        }

        soundCorutineisOn = false;
        yield return null;
    }
}
