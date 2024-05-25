using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float speed;
    private int direction = 1;

    [Header("Life And Points")]
    [SerializeField] protected int life;
    [SerializeField] protected int defense;
    [SerializeField] protected float reward;

    [Header("Damage")]
    [SerializeField] protected int damage;

    [Header("References")]
    private Rigidbody2D rig;
    private SpriteRenderer sprite;

    protected void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }


    protected void FixedUpdate()
    {
        Move();
    }

    #region "Movement"

    protected void Move()
    {   
        rig.velocity = new Vector2(direction * speed, 0);
    }

    #endregion

    #region "Damage"

    protected void TakeDamage(int damage)
    {
        if (damage < defense)
            return;    
            
        life -= (damage - defense);

        if (life <= 0)
            Die();
    }

    protected void Die()
    {
        //Set Die Anim
        GameManager.instance.GrantReward(reward);
        Destroy(gameObject);
    }


    #endregion

    #region "Collision"

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            direction *= -1;
            sprite.flipX = !sprite.flipX;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.HurtPlayer(damage);
        }

        
            
    }

    protected void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("PlayerShot"))
            TakeDamage(other.gameObject.GetComponent<Shot>().damage);
    }

    #endregion
}
