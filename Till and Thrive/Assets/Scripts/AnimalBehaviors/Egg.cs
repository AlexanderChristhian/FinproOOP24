using UnityEngine;

public class Egg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player picked up the egg.");
            Destroy(gameObject);
        }
    }
}