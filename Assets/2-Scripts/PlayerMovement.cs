using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;
    Animator animator;
    
    [SerializeField] public float runSpeed = 5f;
    [SerializeField] float jumpForce = 18f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float gravity = 6f;
    
    CapsuleCollider2D capsule;
    BoxCollider2D box;
    
    bool isAlive = true;
    [SerializeField] float deathSpeedX = 10f;
    [SerializeField] float deathSpeedY = 8f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] float deathSFXVolume = 0.5f;
    
    GameObject gun;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip bulletSFX;
    [SerializeField] float bulletSFXVolume = 0.5f;
    
    bool isDying = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        box =  GetComponent<BoxCollider2D>();
        gun =  GameObject.FindGameObjectWithTag("Gun");
    }

    void Update()
    {
        if(!isAlive){return;}
        Run();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>(); 
    }

    void OnJump(InputValue value)
    {
        if(!isAlive){return;}
        if (!box.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}
        if (value.isPressed)
        {
            rb.linearVelocity += new Vector2(0, jumpForce);
        }
    }

    void OnAttack(InputValue value)
    {
        if(!isAlive){return;}
        Instantiate(bullet, gun.transform.position, transform.rotation);
        if (bulletSFX != null)
        {
            AudioSource.PlayClipAtPoint(bulletSFX, transform.position, bulletSFXVolume);
        }
    }

    void Run()
    {
        Vector2 playerHorizontalVelocity = new Vector2(moveInput.x*runSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerHorizontalVelocity;
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon);
    }

    void FlipSprite()
    {
       bool hasHorizontalSpeed = Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
       if (hasHorizontalSpeed)
       {
           transform.localScale = new Vector2(Mathf.Sign(rb.linearVelocity.x), 1f);
       }
    }

    void ClimbLadder()
    {
        if (!box.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = gravity;
            return;
        }
        
        rb.gravityScale = 0;
        Vector2 playerVerticalVelocity = new Vector2(rb.linearVelocity.x, moveInput.y*climbSpeed);
        rb.linearVelocity = playerVerticalVelocity;
        animator.SetBool("isClimbing", Mathf.Abs(rb.linearVelocity.y) > Mathf.Epsilon);
    }

    void Die()
    {
        if (isDying) return;

        if (capsule.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spine", "Water")))
        {
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        isDying = true;
        isAlive = false;

        animator.SetTrigger("Dying");
        rb.linearVelocity = new Vector2(-deathSpeedX * Mathf.Sign(transform.localScale.x), deathSpeedY);
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, deathSFXVolume);
        
        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
    }
}
