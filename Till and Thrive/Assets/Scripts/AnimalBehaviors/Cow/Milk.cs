using UnityEngine;

public class Milk : MonoBehaviour
{
    private CowMilkSpawner spawner;
    private Transform cowTransform;

    public void Initialize(CowMilkSpawner spawner)
    {
        this.spawner = spawner;
        this.cowTransform = spawner.transform; // Link the milk to the cow's transform
    }

    void Update()
    {
        if (cowTransform != null)
        {
            // Keep the milk positioned directly above the cow
            transform.position = cowTransform.position + Vector3.up;
        }

        HandleProximityCollection();
    }

    private void HandleProximityCollection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float proximityDistance = 2f; // Define the proximity range
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= proximityDistance && Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Milk collected by pressing Q!");

                if (spawner != null)
                {
                    spawner.MilkCollected(); // Notify the spawner that the milk is collected
                }

                AddMoneyToPlayer(player); // Add money to the player

                Destroy(gameObject); // Destroy the milk object
            }
        }
    }

    private void AddMoneyToPlayer(GameObject player)
    {
        MoneyComponent moneyComponent = player.GetComponent<MoneyComponent>();
        if (moneyComponent != null)
        {
            moneyComponent.AddMoney(15); // Add money (e.g., 15 for each milk collected)
            Debug.Log("Money added for milk collection!");
        }
    }

    void OnMouseDown()
    {
        // Logic when the milk is clicked by the player
        Debug.Log("Milk collected!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            AddMoneyToPlayer(player); // Add money to the player
        }

        if (spawner != null)
        {
            spawner.MilkCollected(); // Notify the spawner that the milk is collected
        }

        Destroy(gameObject); // Destroy the milk object
    }

    private void OnMouseOver()
    {
        // Ensure the object can be clicked by providing visual feedback or debug messages
        Debug.Log("Mouse is over the milk object.");
    }
}
