using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hover : MonoBehaviour 
{
    RaycastHit hitInfo;	// Used to get the distance from the ground to the hoverNodes
    Rigidbody rigidbody;

    List<Transform> hoverNodes = new List<Transform>(); // The transforms for the hoverNodes that push the vehicle upwards

    public float hoverForce = 200f;  // Multiplier for when calculating the hover power
    public float hoverHeight = 3;   // The height the vehicle will hover at
    public bool aboveHoverHeight;
    public bool stopMoving = false; // When the player is above a certain height, he/she can't steer the vehicle any more

    float distancePercentage; // how many percents above the ground the vehicle is relative to the hoverHeight variable
    Vector3 hoverPower; // The total force used for the hover function

    float zRotation;

	void Start () 
    {
	    rigidbody = GetComponent<Rigidbody>();

        for (int i = 0; i < 4; i++)
        {
            hoverNodes.Add(transform.FindChild("hoverNode" + i));   // Finds all the hoverNodes which are childs to the vehicle
        }
	}

    void FixedUpdate()
    {
        float drag = 0f;
        float angularDrag = 0f;

        stopMoving = false;
        aboveHoverHeight = false;

        foreach (Transform hoverNode in hoverNodes)
        {
            if (!Physics.Raycast(hoverNode.position, -hoverNode.up, out hitInfo, hoverHeight * 3))  // Makes a ray to check how high the vehicle is
                stopMoving = true;
            
            
            if (Physics.Raycast(hoverNode.position, -hoverNode.up, out hitInfo, hoverHeight))  // Makes a ray to check how high the vehicle is
            {
                if (hitInfo.collider.tag != "Bullet" &&
                    hitInfo.collider.tag != "Fire")
                {
                    // Raises the drag so the vehicle falls slower when close to the ground
                    drag += 1.5f;
                    // Lowers the drag when close to the ground so the vehicle rotates easier
                    angularDrag += 2.5f;

                    distancePercentage = 1 - (hitInfo.distance / hoverHeight);  // Calculates the percentage the vehicle is above the ground

                    hoverPower = transform.up * hoverForce * distancePercentage;    // Calculates the total force using the different variables

                    hoverPower = hoverPower * Time.deltaTime * rigidbody.mass;  // Applies time and mass of the vehicle in the calculation    

                    rigidbody.AddForceAtPosition(hoverPower, hoverNode.position);   // Adds the force to the vehicle
                }
            }
            else
            {
                aboveHoverHeight = true;

                drag += 1f;
                angularDrag += 3.5f;
                rigidbody.AddForceAtPosition(Vector3.up * -5, hoverNode.position);
            }

            rigidbody.drag = drag / hoverNodes.Count;
            rigidbody.angularDrag = angularDrag / hoverNodes.Count;

            // If vehicle lands on nose, it gets pushed up
            if (transform.eulerAngles.x == 270)
                transform.Rotate(new Vector3(50, 0, 0));

            // If vehicle lands on back, it gets pushed up
            if (transform.eulerAngles.x == 90)
                transform.Rotate(new Vector3(-50, 0, 0));

            if(gameObject.transform.parent.GetChild(1).GetComponent<FollowPlayer>().cameraHitInfo.collider != null)
            {
                rigidbody.AddForce(-transform.forward);
            }
        }

    }

}
