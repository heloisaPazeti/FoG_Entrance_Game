using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [HideInInspector] public int damage = 0;

    private Vector2 direction = new Vector2(0,0);
    private float speed = 0;

    [Header("References")]
    private Animator anim;
    private SpriteRenderer sprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetUp(Vector2 direction, Vector3 position, int damage, float speed)
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;
        this.transform.position = position;

        if(direction.x < 0)
            sprite.flipX = true;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private IEnumerator Destroy()
    {
        anim.SetBool("destroy", true);
        yield return new WaitForSeconds(0.16f);
        GameManager.instance.EnqueueShot(gameObject);
        anim.SetBool("destroy", false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((!other.gameObject.CompareTag("Player")) && (!other.gameObject.CompareTag("Door")))
            StartCoroutine(Destroy());       
    }
}
