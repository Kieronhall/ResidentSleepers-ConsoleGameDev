using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Singleton instance of the InteractionManager
    public static InteractionManager Instance { get; set; }

    // Reference to the outlined object
    public Outline outlinedObject = null;

    // Reference to the player GameObject
    GameObject player;

    private void Awake()
    {
        // Ensure only one instance of InteractionManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Find the player GameObject with the "Player" tag
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        // Create a ray from the player's position in the forward direction
        Ray ray = new Ray(player.transform.position + new Vector3(0, 1.5f, 0), player.transform.forward);
        RaycastHit hit;

        // Perform a raycast to detect interactable objects
        if (Physics.Raycast(ray, out hit, 1))
        {
            // Get the GameObject hit by the raycast
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Check if the hit object has an Outline component
            if (objectHitByRaycast.GetComponent<Outline>())
            {
                // Enable the outline effect for the hit object
                outlinedObject = objectHitByRaycast.gameObject.GetComponent<Outline>();
                outlinedObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                // Disable the outline effect if no object is hit or if the hit object has no Outline component
                if (outlinedObject)
                {
                    outlinedObject.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}