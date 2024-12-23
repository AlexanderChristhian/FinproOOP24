using UnityEngine;

public class EggLayingMechanics : MonoBehaviour
{
    [Header("Egg Settings")]
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private float layInterval = 5f;

    private float layTimer;

    void Start()
    {
        layTimer = layInterval;
    }

    void Update()
    {
        HandleEggLaying();
    }

    private void HandleEggLaying()
    {
        layTimer -= Time.deltaTime;

        if (layTimer <= 0f && eggPrefab != null)
        {
            SpawnEgg();
            layTimer = layInterval;
        }
    }

    private void SpawnEgg()
    {
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);

        Collider2D eggCollider = egg.GetComponent<Collider2D>();
        Collider2D chickenCollider = GetComponent<Collider2D>();

        if (eggCollider != null && chickenCollider != null)
        {
            Physics2D.IgnoreCollision(eggCollider, chickenCollider);
        }

        Egg eggComponent = egg.AddComponent<Egg>();
        eggComponent.Initialize();
    }

    private class Egg : MonoBehaviour
    {
        public void Initialize()
        {
            // Additional initialization if needed
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Player picked up the egg.");
                Destroy(gameObject);
            }
        }
    }
}
