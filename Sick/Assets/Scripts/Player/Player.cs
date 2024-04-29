using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Walk Properties")]
    [SerializeField] private float velocity;

    [Header("Jump properties")]
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private int maxQtdeJump;
    private bool isJumping = false;
    private int qtdeJump = 0;

    [Header("Dash Properties")]
    [SerializeField] private float dashForce = 24f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolDown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Components")]
    private Animator anim;
    private Rigidbody2D rig;

    #region "Start And Update"

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDashing)
        {
            Move();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z) && (canDash || !isJumping))
            StartCoroutine(Dash());
    }

    #endregion

    #region "Movements"

    private void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");

        rig.velocity = new Vector2(dirX * velocity, rig.velocity.y);

        if (rig.velocity.x > 0)
        {
            anim.SetFloat("Horizontal", 1);
        }
        else if (rig.velocity.x < 0)
        {
            anim.SetFloat("Horizontal", -1);
        }
        else
        {
            anim.SetFloat("Horizontal", 0);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && qtdeJump < maxQtdeJump)
        {
            rig.velocity = new Vector2(0, jumpStrength);
            qtdeJump++;
        }

        anim.SetFloat("Vertical", rig.velocity.y);
        anim.SetBool("isJumping", isJumping);
    }

    private IEnumerator Dash()
    {
        float direction;
        float originalGravity = rig.gravityScale;

        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", isDashing);
        rig.gravityScale = 0;

        if (rig.velocity.x > 0)
            direction = 1;
        else
            direction = -1;

        rig.velocity = new Vector2(direction * dashForce, rig.velocity.y);
        
        yield return new WaitForSeconds(dashDuration);
        rig.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashing", isDashing);
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    #endregion

    #region "Collisions"

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            qtdeJump = 0;
            isJumping = false;
            anim.SetBool("isJumping", isJumping);
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
            anim.SetBool("WallJump", true);
            qtdeJump = 0;

            ContactPoint2D contact = collision.GetContact(0);

            //player (otherCollider) is to the right of the wall
            if (contact.otherCollider.gameObject.transform.position.x > contact.point.x)
            {
                anim.SetFloat("WallJumpValue", -1);
            }
            else
            {
                anim.SetFloat("WallJumpValue", 1);
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            anim.SetBool("WallJump", false);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = true;
        }
    }

    #endregion
}
