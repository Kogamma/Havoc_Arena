using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	//Script by Kajsa

	public bool iSeeYou = false;        // Used to see if an enemy is nearby
	public GameObject bulletPrefab;     // The prefab for the bullet fired by the enemy
	protected float randomRotate;       // The random rotation the enemy recieves for it's random movement 
	private float randomNum;            // The random number that decides if the enemy should change direction or not
	protected float newRotate;          // The new rotation the enemy recieves
	public float smoothTime = 0.1f;     // How fast the enemy will rotate to it's new direction
	private float yVelocity = 0.0F;     // Used for reference in the SmoothDamp method
    public float shootTimerCD = 0.5f;   // The amount of time the enemy has to wait to shoot
    float shootTimer = 0;               // The timer that counts the shooting cooldown for the enemy
	
	public virtual void Update () 
	{ 
        // Enemy will only change it's direction when not near an enemy
        if (!iSeeYou)  
		    GenRandomRotate ();
        
        // A timer so that the enemy can't spam bullets
        shootTimer += Time.deltaTime;   
        if (shootTimer >= shootTimerCD && iSeeYou == true)
            Attack();
	}


	protected virtual void Attack()
	{
		 GameObject bullet = Instantiate (bulletPrefab, transform.FindChild("BulletSpawn").position, transform.rotation) as GameObject; // Instantiates a new bullet
         bullet.GetComponent<BulletScript>().dir *= -1;
         bullet.GetComponent<BulletScript>().owner = gameObject;
         shootTimer = 0;
	}


	protected void GenRandomRotate()
	{
		randomNum = Random.Range (0, 500); //Generates a random number to make the process of picking out a rotate random and not happen too often

        newRotate = Mathf.SmoothDamp(transform.rotation.y, randomRotate, ref yVelocity, smoothTime); //Makes the roation appear more smooth
        transform.rotation = new Quaternion(transform.rotation.x, newRotate, transform.rotation.z, transform.rotation.w); //Gives turret the new rotation

		if (randomNum < 10)
		{
			randomRotate = Random.rotation.y; //Takes forth a random rotation
		}

	}
}







