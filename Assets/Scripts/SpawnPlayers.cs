using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnPlayers : MonoBehaviour 
{
    public Transform[] spawnpoints = new Transform[4]; // The positions of the spawnpoints
    public List<GameObject> respawnableObjects = new List<GameObject>(); // The respawnable objects
    public bool[] isTaken = new bool[4]; // Has a player spawned here recently?

    bool[] isPlayerDead; // Is there a dead player?
    float[] spawnTimers; // The timers for spawning
    public Image[] spawnTimerSprites = new Image[4];
    public float spawnDelay = 5f; // The amount of time before you can spawn after you die
    
    float[] isTakenTimer = new float[4]; // Timers for each spawn point for when the spawn point no longer is taken
    public float isTakenTimerDelay = 5; // The amount of time before a spawn point no longer is taken
    
    
	void Start () 
    {
        // Adds the players to the list
        respawnableObjects.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        //Adds the amounts of timers based on how many players there are
        spawnTimers = new float[respawnableObjects.Count];

        // Adds the amounts of bools based on how many players there are
        isPlayerDead = new bool[respawnableObjects.Count];
	}
	

	void Update () 
    {
        for (int i = 0; i < respawnableObjects.Count; i++)
            // Checks for dead players
            if (!isPlayerDead[i] && respawnableObjects[i].GetComponent<TakeDamage>().lives != 0)
            {
                // If the player is killed...
                if (!respawnableObjects[i].activeSelf)
                {
                    // ...it's dead
                    respawnableObjects[i].transform.eulerAngles = Vector3.zero;
                    isPlayerDead[i] = true;
                    continue;
                }
                // If not...
                else
                    // ...it's alive
                    isPlayerDead[i] = false;
            }

        for (int i = 0; i < respawnableObjects.Count; i++)
        {
            // Sets a random spawnpoint
            int rndSpawnPoint = Random.Range(0, spawnpoints.Length);

            // If there is a dead player...
            if (isPlayerDead[i])
            {
                spawnTimerSprites[i].fillAmount += 0.00395f;

                // Checks for a spawnpoint that's not taken and if the timer has reached the limit...
                if (!isTaken[rndSpawnPoint] && spawnTimers[i] >= spawnDelay)
                {
                    // Resets the timer
                    spawnTimers[i] = 0;
                    spawnTimerSprites[i].fillAmount = 0f;
                    // Repositions the player
                    respawnableObjects[i].transform.position = spawnpoints[rndSpawnPoint].position;
                    // Reactivates the player
                    respawnableObjects[i].SetActive(true);
                    // Resets the health
                    respawnableObjects[i].GetComponent<TakeDamage>().health = respawnableObjects[i].GetComponent<TakeDamage>().maxHealth;
                    // The spawnpoint is now taken
                    isTaken[rndSpawnPoint] = true;
                    // The player is now alive again
                    isPlayerDead[i] = false;
                }

                // If the timer hasn't reached the limit the timer will count
                else if (spawnTimers[i] < spawnDelay)
                    spawnTimers[i] += Time.deltaTime;
            }
        }

        for (int i = 0; i < spawnpoints.Length; i++)
        {
            // If the spawnpoint is taken...
            if (isTaken[i])
            {
                // If the timer has reached its limit...
                if (isTakenTimer[i] >= isTakenTimerDelay)
                {
                    // The spawnpoint is no longer taken
                    isTaken[i] = false;
                    // Resets the timer
                    isTakenTimer[i] = 0;
                }
                else
                    // Counts the timer
                    isTakenTimer[i] += Time.deltaTime;
            }
        }
    }
}

