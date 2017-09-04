using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;

public class TakeDamage : MonoBehaviour
{
    Rigidbody rb;

    public float health = 100f;    // The amount of HP
    public int lives = 5;          // The amount of lives
    public float maxHealth = 100f; // The max amount of HP
    public Text healthText;        // The text that shows the amount of HP
    public Text livesText;         // The text that shows the amount of lives
    public Text gameOverText;      // The text that shows you that you lost
    public bool isWinner = false;
    float timer = 0;               // A timer
    float lastFrameHealth;

    public GameObject explosionEffect; // An explosion particle effect

    GamePadState state;
    GamePadState prevState;
    PlayerIndex controllerIndex;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Gets the amount of lives set in the menu
        lives = MenuManager.livesAmount;
        print(lives);
        if (lives <= 0)
            lives = 1;

        // Resets UIs
        healthText.text = "Health: " + health;
        livesText.text = "Lives: " + lives;

        controllerIndex = gameObject.GetComponent<PlayerController>().controllerIndex;
    }


    void Update ()
    {
        // Decreases HP to it's maximum allowed level if the HP gets beyond the limit
        if (health > maxHealth)
            health = maxHealth;

        if (health != lastFrameHealth)
            this.healthText.text = "Health: " + this.health;

        lastFrameHealth = health;

        if (health <= 0)
        {
            rb.velocity = Vector3.zero;

            GameObject explosionObj = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            Destroy(explosionObj, explosionEffect.GetComponent<ParticleSystem>().startLifetime);

            // Deactivates the player
            gameObject.SetActive(false);

            // Decreases the players lives by 1
            lives -= 1;
            // Updates the Lives HUD
            livesText.text = "Lives: " + lives;

            // Stops the controllers vibration
            GamePad.SetVibration(controllerIndex, 0f, 0f);

            // Sets what sound to play to deathSound
            GameObject.Find("GameManager").GetComponent<SoundManager>().currentAudioClip = GameObject.Find("GameManager").GetComponent<SoundManager>().deathSound;
            // Plays the sound
            GameObject.Find("GameManager").GetComponent<SoundManager>().PlayCurrentAudioClip();
        } 

        if (lives <= 0)
        {   
            // Sets playerPlaceIndex[playerNr - 1] to the place playersLeft - 1
            MatchStatus.playerPlaceIndex[this.gameObject.GetComponent<PlayerController>().playerNr - 1] = GameObject.Find("GameManager").GetComponent<MatchStatus>().playersLeft - 1;

            // Decreases the amount of players left by 1
            GameObject.Find("GameManager").GetComponent<MatchStatus>().playersLeft -= 1;

            // Deactivates that players HUD
            livesText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);

            // Sets the game over text to GAME OVER
            gameOverText.text = "GAME OVER";

            // An easter egg, sets text to "GET REKT" if that random number between 0 and 1337 hits 69
            if (Random.Range(0, 1337) == 69)
            {
                gameOverText.text = "GET REKT";

                // If the easter egg above happens, and if another random number between 0 and 30 hits 17, the text will say "GET SCHREKT" instead
                if(Random.Range(0, 30) == 17)
                    gameOverText.text = "GET SHREKT";
            }
        }

        // If a player is alive and he's the only one left...
        if (gameObject.activeSelf && GameObject.Find("GameManager").GetComponent<MatchStatus>().playersLeft <= 1)
        {
            GameObject.FindGameObjectWithTag("EnemyObject").SetActive(false);
            GameObject.Find("GameManager").GetComponent<EnemySpawner>().enabled = false;

            // Sets that player to 1st place
            MatchStatus.playerPlaceIndex[this.gameObject.GetComponent<PlayerController>().playerNr - 1] = 0;

            // Deactivates the HUD
            healthText.gameObject.SetActive(false);
            livesText.gameObject.SetActive(false);

            // Changes the text 
            gameOverText.color = Color.green;
            gameOverText.text = "YOU WON!";

            isWinner = true;
        }
    }


    void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject.tag == "Bullet" && otherObj.gameObject.name != "Field of View" && otherObj.gameObject.GetComponent<BulletScript>().owner.name != gameObject.name)
        {
            // Decreases HP
            health -= 5; //otherObj.gameObject.damage;

            // Updates the HP text
            healthText.text = "Health: " + Mathf.Round(health);

            StartCoroutine(hitVibrate());

            // Sets what sound to play to hitSound
            GameObject.Find("GameManager").GetComponent<SoundManager>().currentAudioClip = GameObject.Find("GameManager").GetComponent<SoundManager>().hitSound;
            // Plays the sound
            GameObject.Find("GameManager").GetComponent<SoundManager>().PlayCurrentAudioClip();

            // If the player dies...

        }      
    }

    void OnTriggerStay(Collider otherObj)
    {
        if (otherObj.gameObject.tag == "Fire")
        {
            health -= 0.5f;
            StartCoroutine(hitVibrate());
            // Updates the HP text
            healthText.text = "Health: " + Mathf.Round(health);
        }
    }

    IEnumerator hitVibrate()
    {
        GamePad.SetVibration(controllerIndex, 0.3f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        GamePad.SetVibration(controllerIndex, 0f, 0f);
    }
}
