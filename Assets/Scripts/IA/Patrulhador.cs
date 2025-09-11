using UnityEngine;

public enum EnemyState_
{
    None, Patrolling, Attacking
}
public class Patrulhador : MonoBehaviour
{
    public EnemyState currentState;
    public GameObject pontoA;
    public GameObject pontoB;
    public float speed;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform PontoAtual;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    int state = 0;

    float cooldown = 5;

    [Header("Sprites")]
    public Sprite spriteNormal;
    public Sprite spriteAtaque;

    public float distanciaAtaque = 1.5f; // Distância para trocar o sprite

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Patrolling;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PontoAtual = pontoB.transform;
        anim.SetBool("Andando", true);
        spriteRenderer.sprite = spriteNormal;
    }
    void Update()
    {
        // Diminui o cooldown a cada frame
        if (cooldown > 0)
            cooldown -= Time.deltaTime;

        switch (state)
        {
            case 0:
                Patrolling();
                break;
            case 1:
                Follow();
                break;
            //case 2:
                //Attack();
                //break;
        }

        Debug.Log("Inimigo está vivo", gameObject);
    }

    void Patrolling()
    {
        Debug.Log("Inimigo está patrulhando.");
        Vector2 point = PontoAtual.position - transform.position;
        if (PontoAtual == pontoB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            spriteRenderer.flipX = false;
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
            spriteRenderer.flipX = true;
        }

        if (Vector2.Distance(transform.position, PontoAtual.position) < 0.5f && PontoAtual == pontoB.transform)
        {
            PontoAtual = pontoA.transform;
        }
        if (Vector2.Distance(transform.position, PontoAtual.position) < 0.5f && PontoAtual == pontoA.transform)
        {
            PontoAtual = pontoB.transform;
        }
    }
    void Follow()
    {
        Debug.Log("Inimigo está seguindo.");
        if (!target) return;
        
        float direction = Mathf.Sign(target.transform.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * speed * 1.4f, rb.velocity.y);

        spriteRenderer.flipX = direction < 0;

        // Se chegar perto o suficiente, muda para ataque
        if (Vector2.Distance(transform.position, target.transform.position) <= distanciaAtaque)
        {
            state = 2;
        }
        
    }

    //void Attack()
    //{
    //    if (!target) return;
//
    //    float distancia = Vector2.Distance(transform.position, target.transform.position);
//
    //    // Para o inimigo durante o ataque
    //    rb.velocity = Vector2.zero;
    //    rb.angularVelocity = 0f;
//
    //    if (distancia <= distanciaAtaque)
    //    {
    //        if (cooldown <= 0f)
    //        {
    //            Debug.Log("Inimigo está atacando.");
    //            anim.SetTrigger("Atacando");
    //            spriteRenderer.sprite = spriteAtaque;
    //            cooldown = 5f;   
    //        }
    //    }
    //    else
    //    {
    //        spriteRenderer.sprite = spriteNormal;
    //        state = 0;
    //    }
    //}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pontoA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pontoB.transform.position, 0.5f);
        Gizmos.DrawLine(pontoA.transform.position, pontoB.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            state = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = 0;
        }
    }
}