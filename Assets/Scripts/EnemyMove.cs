using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    public float speed = 2f;              // Velocidade do movimento
    private Vector2 direction = Vector2.left; // Começa indo para cima


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Move o inimigo na direção atual
        transform.Translate(direction * speed * Time.deltaTime);
        if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Inverte a direção ao colidir com uma parede
        direction = -direction;
                       
    }
}
