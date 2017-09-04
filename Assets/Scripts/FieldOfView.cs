using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour 
{
	//Code;Kajsa

	public bool iSeeYou = false;
	public Transform bomberTarget;
	public GameObject moverTarget;

	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (moverTarget == null)
            return;

		if (!moverTarget.activeSelf)
			iSeeYou = false;
	}

	void OnTriggerEnter (Collider otherObj)
	{
        if (otherObj.tag == "Player")
        {
            iSeeYou = true; //Marks the player for the turrets
            moverTarget = otherObj.gameObject;
            bomberTarget = otherObj.transform; //Moves the player's transform into variable
        }
	}

	void OnTriggerExit (Collider otherObj)
	{
		if (otherObj.tag == "Player") 
		{
			iSeeYou = false; //Stops marking the player for the turrets
			moverTarget = null;
		} 

	}

}
