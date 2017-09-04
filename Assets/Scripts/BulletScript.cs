using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
    public float bulletSpeed = 20f;
    public float maxDist = 50f; // The maximum distance the bullet is allowed to travel
    Vector3 originPos; // The positions it spawns from
    public Vector3 dir;
    public GameObject owner; // The object that shoots the bullet
    public GameObject sparksEffect; // A spark particel effect

	void Awake () 
	{
        originPos = transform.position;
        dir = transform.forward;
	}
	
	// Update is called once per frame
	void Update () 
	{    
        transform.position -= dir * bulletSpeed * Time.deltaTime; // Makes the bullet move forwards

        if (maxDist <= Vector3.Distance(transform.position, originPos))  // Destroys the bullet if it reaches the maxDist
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider otherObj)
    {
        // If the bullet collides with something, it get's destroyed
        if (otherObj.gameObject.tag != "Bullet" && otherObj.gameObject.name != "Field of View" && otherObj.tag != "Ground")
            if (otherObj.gameObject.name != owner.gameObject.name)
            {
                // If the bullet hits a player or an enemy, a spark is created
                if (otherObj.tag == "Enemy" || otherObj.tag == "Player")
                {
                    GameObject sparkObj = Instantiate(sparksEffect, transform.position, transform.rotation) as GameObject;
                    Destroy(sparkObj, sparksEffect.GetComponent<ParticleSystem>().startLifetime);
                }
                Destroy(gameObject);
            }           
    }
}
