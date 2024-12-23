using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;          // Player transform to follow
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);  // Camera offset from player
    
    [Header("Follow Settings")]
    [SerializeField] private Vector2 maxPosition = new Vector2(10f, 10f); // Max camera bounds
    [SerializeField] private Vector2 minPosition = new Vector2(-10f, -10f); // Min camera bounds

    private void Start()
    {
        // If no target is set, try to find the player
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        // Set initial position
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;
        
        // Clamp the camera position within bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
        
        // Instantly move camera
        transform.position = desiredPosition;
    }
}