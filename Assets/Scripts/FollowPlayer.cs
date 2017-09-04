using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    Rigidbody rb;

    public Transform target; // The transform that the camera will follow
    public Vector2 distance; // The distance the camera will have from the target
    float originDistance; // The original distance in X the player should keep from the camera
    public float minHeight;  // The minimum allowed height the camera can have
    Vector3 newPos; // The variable that keeps the camera's new position

    static int nrOfPlayers; // The amount of players

    public RaycastHit cameraHitInfo; // The variable that keeps the info from the raycast 
    int layerMask = 1 << 9; // A layermask to make the raycast checking if the target is near a wall only interact with certain objects

    void Start()
    {
        // Gets the Camera component so that it can be used in this script
        Camera thisCamera = GetComponent<Camera>();

        // Sets the amount of players by finding objects with the tag "Player"
        nrOfPlayers = MenuManager.playerAmount;

        // Sets the viewport for all the cameras depending on how many players there are
        if (nrOfPlayers == 2)
            if (name == "Main Camera")
                thisCamera.rect = new Rect(0f, 0.5f, 1, 1);
            else if (name == "Main Camera 2")
                thisCamera.rect = new Rect(0f, -0.5f, 1, 1);

        if (nrOfPlayers >= 3)
            if (name == "Main Camera")
                thisCamera.rect = new Rect(-0.5f, 0.5f, 1, 1);
            else if (name == "Main Camera 2")
                thisCamera.rect = new Rect(0.5f, 0.5f, 1, 1);
            else if (name == "Main Camera 3")
                thisCamera.rect = new Rect(0f, -0.5f, 1, 1);

        if (nrOfPlayers == 4)
            if (name == "Main Camera")
                thisCamera.rect = new Rect(-0.5f, 0.5f, 1, 1);
            else if (name == "Main Camera 2")
                thisCamera.rect = new Rect(0.5f, 0.5f, 1, 1);
            else if (name == "Main Camera 3")
                thisCamera.rect = new Rect(-0.5f, -0.5f, 1, 1);
            else if (name == "Main Camera 4")
                thisCamera.rect = new Rect(0.5f, -0.5f, 1, 1);

        originDistance = distance.x;    // Keeps the original distance from the target

        layerMask = ~layerMask;
    }


    void Update()
    {
        if (Physics.Raycast(target.position, target.forward, out cameraHitInfo, originDistance, layerMask))   // Checks if the target is near a wall
            distance.x = cameraHitInfo.distance; // Changes the distance in X so the camera doesn't go outside of the wall
        else
            distance.x = originDistance;   // If the target is not near a wall, the camera will keep it's original distance

        newPos = target.position + (target.forward * distance.x);   // Calculates where the camera should be depending on the target's posittion
        newPos.y = Mathf.Max(newPos.y + distance.y, minHeight); // Makes it so the camera doesn't go through the ground
        transform.position = newPos;    // Sets the new position

        transform.LookAt(target.position);  // Makes camera look at the target
    }
}
