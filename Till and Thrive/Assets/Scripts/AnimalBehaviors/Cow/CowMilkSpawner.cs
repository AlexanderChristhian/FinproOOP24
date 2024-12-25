using UnityEngine;

public class CowMilkSpawner : MonoBehaviour
{
    [Header("Milk Settings")]
    [SerializeField] private GameObject milkPrefab;
    [SerializeField] private float spawnInterval = 10f;

    private GameObject spawnedMilk;
    private float spawnTimer;
    private bool isMilkCollected = true; // Tracks if milk is collected

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (isMilkCollected) // Only count down if the milk is collected
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnMilk();
                isMilkCollected = false; // Timer stops until milk is collected
            }
        }
    }

    void SpawnMilk()
    {
        Vector3 spawnPosition = transform.position + Vector3.up; // Spawn above the cow
        spawnedMilk = Instantiate(milkPrefab, spawnPosition, Quaternion.identity);
        spawnedMilk.GetComponent<Milk>().Initialize(this); // Link the milk back to the spawner
    }

    public void MilkCollected()
    {
        spawnedMilk = null; // Allow spawning a new milk after the current one is collected
        isMilkCollected = true; // Restart the timer
        spawnTimer = spawnInterval; // Reset the timer
    }
}
