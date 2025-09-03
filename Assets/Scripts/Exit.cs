using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.1f,4);
        if (collision != null)
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("Collision detected with" + collision.gameObject.name);
        }
    }
}