using UnityEngine;
using System.Collections;

public class BomberMove : EnemyMove 
{
	public float bomberSpeed = 3f;
	private bool bombLockOn = false;
	public Transform target;

	void Update () 
	{
		target = gameObject.GetComponentInChildren<FieldOfView> ().bomberTarget;
		iSeeYou = true;

		if(target == null)
			MoveEnemy ();

		if (iSeeYou == true) 
		{
			Attack ();
			bombLockOn = true;
		} 
		else 
			bombLockOn = false;
	}


	//Override enemy Attack with starts moving towards player and then exploding
	protected override void Attack ()
	{
		//transform.position = Vector3.Lerp (transform.position, target.transform.position, bomberSpeed * Time.deltaTime);
		if(target != null)
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, bomberSpeed * Time.deltaTime);
			
	}


	void OnTriggerEnter (Collider otherObj)
	{
		if (otherObj.gameObject.tag == "Player") 
		{
			Debug.Log ("BOOM");
			Destroy(gameObject);
			Explode();

		}
	}

	void Explode()
	{


	}
}
