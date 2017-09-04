using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    void OnTriggerEnter(Collider otherObj)
    {
        // If the pickup touches a player it...
        if(otherObj.gameObject.tag == "Player")
        {
            // Makes the object invisible as if it's gone
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            // Activates the PickupSpawner script
            gameObject.GetComponent<PickupSpawner>().enabled = true;

            // Deactivates the collider
            gameObject.GetComponent<Collider>().enabled = false;

            // Resets the timer in the PickupSpawner script
            gameObject.GetComponent<PickupSpawner>().spawnTimer = 0f;

            // Deactivates this script so that you can no longer can pick up the pickup
            this.enabled = false;

            // Gives the player a powerup, weapon or health depending on what number the PickupSpawner script has randomized
            GameObject.Find(otherObj.name).GetComponent<PickupEffectApplier>().currentPickupIndex = gameObject.GetComponent<PickupSpawner>().pickupContent;
        }
    }
}
