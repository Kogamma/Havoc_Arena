using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour 
{
    public GameObject explosionEffect; // An explosion particle effect
    public float health = 50f; // The health
    public int maxHealth = 100; // The maximum amount of health allowed

	void Update () 
    {
        if (health <= 0)
        {
            // Deactivates the object
            gameObject.SetActive(false);

            // Instantiates the explosion effect
            GameObject explosionObj = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation) as GameObject;

            // Destroys the effect when it's done
            Destroy(explosionObj, explosionEffect.GetComponent<ParticleSystem>().startLifetime);
        }
	}

    void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject.tag == "Bullet" && otherObj.gameObject.name != "Field of View" && otherObj.gameObject.GetComponent<BulletScript>().owner.name != gameObject.name)
        {
            // Decreases HP
            health -= 5;
        }
    }
}
