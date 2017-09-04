using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour 
{
    public float spawnTimer = 0f; // A timer
    public float timerEnd = 5f; // The end of the timer 
    public int pickupContent; // A number that will represent what the pickup contains
	
	// Update is called once per frame
	void Update () 
    {
        // If the timer has reached 5 or more it...
        if (spawnTimer >= timerEnd)
        {
            // Makes the pickup visible as if a new has spawned
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            // Activates the Pickup script so that you can pick it up again
            gameObject.GetComponent<Pickup>().enabled = true;

            // Activates the collider
            gameObject.GetComponent<Collider>().enabled = true;

            // Gives pickupContent a random value between 1 and 6
            pickupContent = Random.Range(1, 1);

            // Deactivates this script
            this.enabled = false;
        }
        
        else
            // Makes the timer tick
            spawnTimer += Time.deltaTime;
	}
}
