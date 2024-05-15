using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Health & Wealth")]
    [SerializeField] private float life;
    [SerializeField] private float defense;
    [SerializeField] private float invunerableCoolDown;
    private bool isInvulnerable = false;
    private float money;

    [Header("Atack")]
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private int ammoQuant;
    [SerializeField] private float shotCooldown;
    [SerializeField] private float damage;
    [SerializeField] private float shotSpeed;
    private bool canShot = true;
    private Queue<GameObject> ammo = new Queue<GameObject>();

    [Header("Components")]
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer sprite;

    #region "Start And Update"

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        SetUpAmmo();
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

        if (Input.GetKeyDown(KeyCode.X) && canShot && ammo.Count > 0)
            StartCoroutine(Shot());
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
            sprite.flipX = false;
        }
        else if (rig.velocity.x < 0)
        {
            anim.SetFloat("Horizontal", -1);
            sprite.flipX = true;
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
        rig.gravityScale = 0;

        if (rig.velocity.x > 0)
            direction = 1;
        else
            direction = -1;

        rig.velocity = new Vector2(direction * dashForce, rig.velocity.y);
        
        yield return new WaitForSeconds(dashDuration);
        rig.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    #endregion

    #region "Damage"

    public void TakeDamage(float damage)
    {
        if (damage < defense || isInvulnerable)
            return;

        life -= damage - defense;
        if (life <= 0)
            Die();
        else
            StartCoroutine(Invulnerable());

    }

    public IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invunerableCoolDown);
        isInvulnerable = false;
    }

    private void Die()
    {
        // Death Anim ... if I had one
        Destroy(gameObject);
        GameManager.instance.GameOver();
    }

    #endregion

    #region "Rewards"

    public void GetReward(float amount)
    {
        money += amount;

        if (money > GameManager.instance.moneyNeeded)
            GameManager.instance.WinLevel();
    }

    #endregion

    #region "Atack"

    private void SetUpAmmo()
    {
        for (int i = 0; i < ammoQuant; i++)
        {
            GameObject shot = Instantiate(shotPrefab);
            shot.SetActive(false);
            ammo.Enqueue(shot);
        }
    }

    private IEnumerator Shot()
    {
        int direction;

        if(sprite.flipX)
            direction = -1;
        else
            direction = 1;

        canShot = false;
        GameObject shot = ammo.Dequeue();
        shot.SetActive(true);
        shot.GetComponent<Shot>().SetUp(direction, this.transform.position, damage, shotSpeed);

        yield return new WaitForSeconds(shotCooldown);
        canShot = true;
    }

    public void EnqueueShot(GameObject shot)
    {
        shot.SetActive(false);
        ammo.Enqueue(shot);
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = true;
        }
    }

    #endregion
}
