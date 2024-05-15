using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [Header("References")]
    private SpriteRenderer sprite;

    [HideInInspector] public float damage = 0;

    private int direction = 0;
    private float speed = 0;

    [Header("References")]
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetUp(int direction, Vector3 position, float damage, float speed)
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;
        this.transform.position = position;

        if (direction < 1)
            sprite.flipX = true; 
    }

    private void FixedUpdate()
    {
        Vector2 dir = new Vector2(direction, 0);
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private IEnumerator Destroy()
    {
        anim.SetBool("destroy", true);
        yield return new WaitForSeconds(0.16f);
        GameManager.instance.EnqueueShot(gameObject);
        anim.SetBool("destroy", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            StartCoroutine(Destroy());
    }
}
