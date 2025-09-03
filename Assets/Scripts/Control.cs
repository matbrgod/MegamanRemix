using UnityEngine;

public class Control : MonoBehaviour
{
    [Header("Refs")]
    public Animator anima;
    public Rigidbody2D rdb;
    public ParticleSystem fire;

    [Header("Move")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 8f;

    [Header("Ground / Height")]
    [SerializeField] LayerMask groundMask;          // Marque aqui as layers do chão (ex.: Ground, Platform)
    [SerializeField] float groundRayLength = 0.25f; // Alcance curto
    [SerializeField] float groundedEpsilon = 0.05f; // Distância para considerar “no chão”

    float xmov;
    bool wantJump;
    bool grounded;

    Collider2D[] cols;

    void Awake()
    {
        if (!rdb)   rdb   = GetComponent<Rigidbody2D>();
        if (!anima) anima = GetComponent<Animator>();
        cols = GetComponents<Collider2D>(); // pega todos (se tiver 2 Capsules, ok)
    }

    void Update()
    {
        xmov = Input.GetAxisRaw("Horizontal");

        // Só permite pular se estiver no chão (evita pulo infinito)
        if (Input.GetButtonDown("Jump") && grounded)
            wantJump = true;

        anima.SetBool("Fire", false);
        if (Input.GetButtonDown("Fire1"))
        {
            if (fire) fire.Emit(1);
            anima.SetBool("Fire", true);
        }
    }

    void FixedUpdate()
    {
        // 1) Movimento horizontal estável (sem AddForce acumulando)
        rdb.velocity = new Vector2(xmov * moveSpeed, rdb.velocity.y);

        // 2) HEIGHT: raycast saindo do ponto mais baixo de TODOS os colisores
        float bottom = float.PositiveInfinity;
        foreach (var c in cols)
            if (c && c.enabled)
                bottom = Mathf.Min(bottom, c.bounds.min.y);
        if (float.IsInfinity(bottom)) bottom = transform.position.y;

        Vector2 origin = new Vector2(transform.position.x, bottom + 0.02f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundRayLength, groundMask);
        Debug.DrawRay(origin, Vector2.down * groundRayLength, Color.red);

        // Normaliza Height: 0 (no chão) → 1 (no ar)
        float heightNorm = 1f;
        if (hit.collider != null)
        {
            heightNorm = Mathf.Clamp01(hit.distance / groundRayLength);
            grounded   = hit.distance <= groundedEpsilon;
        }
        else
        {
            grounded = false;
        }

        anima.SetFloat("Height", heightNorm);
        anima.SetFloat("Velocity", Mathf.Abs(xmov));

        // 3) Pulo (impulso único)
        if (wantJump)
        {
            rdb.velocity = new Vector2(rdb.velocity.x, jumpForce);
            wantJump = false;
            grounded = false; // até o próximo raycast confirmar chão
        }

        PhisicalReverser();
    }

    void PhisicalReverser()
    {
        if (rdb.velocity.x >  0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
            LevelManager.instance.LowDamage();
    }
}
