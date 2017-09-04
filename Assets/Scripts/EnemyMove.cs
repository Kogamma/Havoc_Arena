using UnityEngine;
using System.Collections;

public class EnemyMove : Enemy 
{
	//Code;Kajsa

	private bool lockOn = false;
	public float moverSpeed = 10f;
	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		iSeeYou = transform.FindChild("Field of View").GetComponent<FieldOfView>().iSeeYou;

        base.Update();

        MoveEnemy ();
	}

	protected virtual void MoveEnemy()
	 {	
		if (!iSeeYou) //If the turret haven't locked on do this
		{
			transform.Translate((transform.forward) * moverSpeed * Time.deltaTime); //Moves the turret in the way it's pointed
		}

	 }
	 


}
