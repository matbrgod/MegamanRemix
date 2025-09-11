using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject missileReference;
    [SerializeField]
    private GameObject firepoint;
    [SerializeField]
    private GameObject firepoint2;

    [SerializeField]
    private GameObject cannon;

    [SerializeField]
    int state = 0;

    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    float cooldown = 0;
    public Animator anima;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anima = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (state)
        {
            case 0:
                Idle();
                break;
            case 1:
                Aim();
                break;
            case 2:
                Follow();
                break;

        }

    }

    void Idle()
    {

    }

    void Aim()
    {
        anima.SetBool("Shooting", true);
        Vector3 dif = target.transform.position + Vector3.up - transform.position;
        //cannon.transform.up = -dif;
        float value = Vector3.Dot(dif, cannon.transform.right);
        //cannon.transform.Rotate(value, value, value);
        
        if (cooldown <= 0)
        {
            Instantiate(missileReference, firepoint.transform.position, firepoint.transform.rotation);
            cooldown = 2;
            
        }
        cooldown -= Time.deltaTime;

    }


    void Follow()
    {
        if (!target) return;
        if (target.transform.position.x > transform.position.x)
        {
            rb.AddForce(Vector2.right * 100);
        }
        if (target.transform.position.x < transform.position.x)
        {
            rb.AddForce(Vector2.right * -100);
        }
        anima.SetBool("Shooting", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            state = 1;
            float direction = Mathf.Sign(target.transform.position.x - transform.position.x);
            if (direction < 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, -180, 0);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = 2;
        }
    }

    
}
