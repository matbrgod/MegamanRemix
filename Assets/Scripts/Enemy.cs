using UnityEngine;

public enum EnemyState
{
    None, Patrolling, Attacking
}
public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public GameObject pontoA; //precisa criar um ponto A e B
    public GameObject pontoB;
    public float speed;
    private Rigidbody2D rb;
    private Animator anim; //coisas de animação
    private Transform PontoAtual;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    int state = 0;

    float cooldown = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Patrolling;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PontoAtual = pontoB.transform;
        anim.SetBool("Andando", true);
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                Patrolling();
                break;
            case 1:
                Follow();
                break;
                        
        }
        

        Debug.Log("Inimigo está vivo", gameObject);
    }


    void Patrolling()
    {
        Debug.Log("Inimigo está patrulhando.");
        Vector2 point = PontoAtual.position - transform.position;
        if (PontoAtual == pontoB.transform)
        {
            //flip();
            rb.velocity = new Vector2(speed, 0);
            spriteRenderer.flipX = true;
        }
        else
        {
            //flip();
            rb.velocity = new Vector2(-speed, 0);
            spriteRenderer.flipX = false;
        }

        if (Vector2.Distance(transform.position, PontoAtual.position) < 0.5f && PontoAtual == pontoB.transform)
        {
            PontoAtual = pontoA.transform;
        }
        if (Vector2.Distance(transform.position, PontoAtual.position) < 0.5f && PontoAtual == pontoA.transform)
        {
            PontoAtual = pontoB.transform;
        }

        // Verifica se o jogador está próximo para perseguir
        // (Vector2.Distance(transform.position, player.transform.position) < 3f)
       // {
            // Troca para o script de perseguição
         //   GetComponent<SimpleFollow>().enabled = true;
        //    this.enabled = false;
       // }

    }
    void Follow()
    {
        Debug.Log("Inimigo está seguindo.");
        if (!target) return;
        if (target.transform.position.x > transform.position.x)
        {
            rb.AddForce(Vector2.right * 100);
            spriteRenderer.flipX = true;
        }
        if (target.transform.position.x < transform.position.x)
        {
            rb.AddForce(Vector2.right * -100);
            spriteRenderer.flipX = false;
        }
    }
       
    void OnDrawGizmos() //Linhas para vizualizar patrulha
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