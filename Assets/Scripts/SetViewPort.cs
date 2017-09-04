















































































































































































































































































































































using UnityEngine;
using System.Collections;

public class SetViewPort : MonoBehaviour 
{
    Camera thisCamera;
    int nrOfPlayers;
	// Use this for initialization
	void Start () 
    {
        // Gets the Camera component so that it can be used in this script
        thisCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        nrOfPlayers = MenuManager.playerAmount;

        // Scales the camera's viewport according to how many players there are
        if(GameObject.Find("Game Manager").GetComponent<MenuManager>().vehicleSelectionGroup.activeSelf)
        {
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
        }
        else if(name == "Main Camera")
            thisCamera.rect = new Rect(0f, 0f, 1, 1);
	}
}
