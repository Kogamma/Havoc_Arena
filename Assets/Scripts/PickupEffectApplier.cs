using UnityEngine;
using System.Collections;

public class PickupEffectApplier : MonoBehaviour
{
    public int currentPickupIndex;
    string currentPickup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPickupIndex != 0)
        {
            if (currentPickupIndex == 1)
                currentPickup = "Health";
            else if (currentPickupIndex == 2)
                currentPickup = "Shield";
            else if (currentPickupIndex == 3)
                currentPickup = "Double Damage";
            else if (currentPickupIndex == 4)
                currentPickup = "Invisibility";
            else if (currentPickupIndex == 5)
                currentPickup = "Missile";
            else if (currentPickupIndex == 6)
                currentPickup = "Bomb";
            else if (currentPickupIndex == 7)
                currentPickup = "Minigun";
            else if (currentPickupIndex == 8)
                currentPickup = "Flamethrower";
            else if (currentPickupIndex == 9)
                currentPickup = "Homing Missile";
            else if (currentPickupIndex == 10)
                currentPickup = "Lazer Beam";

            currentPickupIndex = 0;
        }
        else
            currentPickup = "null";

        if (currentPickup == "Health")
        {
            this.GetComponent<TakeDamage>().health += 10f ;
        }
    }
}
