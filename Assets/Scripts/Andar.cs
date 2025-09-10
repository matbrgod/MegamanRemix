using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andar : MonoBehaviour
{
    public GameObject pontoA; //precisa criar um ponto A e B
    public GameObject pontoB;
    private Rigidbody2D rb;
    private Animator anim; //coisas de animação
    private Transform PontoAtual;
    public float speed;
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject missileReference;
    [SerializeField]
    private GameObject firepoint;

    [SerializeField]
    private GameObject cannon;

    [SerializeField]
    int state = 0;
    float cooldown = 0;

    public GameObject player; // Referência ao jogador

    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PontoAtual = pontoB.transform;
        anim.SetBool("Andando", true);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 point = PontoAtual.position - transform.position;
        if (PontoAtual == pontoB.transform)
        {
            flip();
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            flip();
            rb.velocity = new Vector2(-speed, 0);
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
        if (Vector2.Distance(transform.position, player.transform.position) < 3f)
        {
            // Troca para o script de perseguição
            GetComponent<SimpleFollow>().enabled = true;
            this.enabled = false;
        }
    }

    private void flip() //inverte o sentido do inimigo
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos() //Linhas para vizualizar patrulha
    {
        Gizmos.DrawWireSphere(pontoA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pontoB.transform.position, 0.5f);
        Gizmos.DrawLine(pontoA.transform.position, pontoB.transform.position);
    }
}
