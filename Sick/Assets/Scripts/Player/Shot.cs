using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [HideInInspector] public float damage = 0;

    private Vector2 direction = new Vector2(0,0);
    private float speed = 0;

    [Header("References")]
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetUp(Vector2 direction, Vector3 position, float damage, float speed)
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;
        this.transform.position = position;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
       StartCoroutine(Destroy());
    }
}
