using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class MenuManager : MonoBehaviour
{
    // All folders for the different sections of the menu
    public GameObject startScreen;
    public GameObject mainMenuGroup;
    public GameObject optionsGroup;
    public GameObject howToPlayGroup;
    public GameObject controllerLayoutGroup;
    public GameObject instructionsGroup;
    public GameObject playerAmountGroup;
    public GameObject vehicleSelectionGroup;
    public GameObject creditsGroup;

    public Text pressStartText; // A text that tells player 1 to press Start

    public Material[] colorHolders = new Material[4]; // The objects that shows the selected color
    public static int[] colorIndex; // An array that tells which color is which
    public Color[] colors = new Color[8]; // The colors
    int[] colorLastFrame = new int[8]; 
    bool[] canChangeColor; // Can you change color?
    bool[] isColorTaken = new bool[8] { false, false, false, false, false, false, false, false }; // Is the color you're going to choose taken?
    bool[] isPlayerReady; // Is the player ready to start?
    public Text[] isReadyTexts = new Text[4]; // The checkboxes showing if the players are ready
    int nrOfPlayersReady = 0; // The amount of players that are ready
    KeyCode[] keyCodes;

    public Slider playerAmountSlider; // A slider that sets the amount of players
    public static int playerAmount; // The amount of players. Is static so the same value can be used in the next scene
    public Slider livesAmountSlider; // A slider that sets the amount of lives
    public Text livesText; // A text that shows the selected amount of lives
    public static int livesAmount; // The amount of lives

    public Slider volumeSlider; // The slider in the Player Amount Group

    public GameObject creditsText; // The credits text in the Credits group

    public EventSystem EventSystem; // The Event System used for navigation in UI

    // Used for button controller navigation
    GamePadState[] state = new GamePadState[4]; 
    GamePadState[] prevState = new GamePadState[4];


    public void Awake()
    {
        playerAmount = 1;
        livesAmount = 1;
    }

    public void Start()
    {
        colorIndex = new int[8];
        canChangeColor = new bool[4];
        keyCodes = new KeyCode[4] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    }

    public void Update ()
    {
        // Fills the array with all the available controllers for every player
        for (int i = 0; i < playerAmount; i++)
            state[i] = GamePad.GetState((PlayerIndex)i);

        // Updates the volume based on the volume sliders value
        AudioListener.volume = volumeSlider.value;

        // Updates the number of lives text in the playeramount group when the group is active
        if(playerAmountGroup.activeSelf)
            livesText.text = livesAmountSlider.value.ToString();

        if(vehicleSelectionGroup.activeSelf)
        {

            // Changes back from the vehicle selection menu to the player amount when you press B, it also makes the game to go back to using one camera only again
            if (state[0].Buttons.B == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 2; i <= playerAmount; i++)
                {
                    GameObject.Find("Main Camera " + i).GetComponent<Camera>().enabled = false;
                }
                ChangeToPlayerAmount();
            }

            // If every player is ready...
            if(nrOfPlayersReady == playerAmount)
            {
                // Enables the text telling player 1 to press start
                pressStartText.enabled = true;

                // Loads the game level if player 1 presses start
                if (state[0].Buttons.Start == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Space))
                    Application.LoadLevel("level1");
            }
            
            for (int i = 0; i < playerAmount; i++)
            {
                // Sets the checkbox to on if the player presses Start button
                if (state[i].Buttons.Start == ButtonState.Pressed && !isPlayerReady[i])
                {
                    isReadyTexts[i].text = "Player " + (i + 1) + " is ready";
                    isPlayerReady[i] = true;
                    nrOfPlayersReady++;
                }
                // Sets the checkbox to off if the player presses Back button
                else if (state[i].Buttons.Back == ButtonState.Pressed && isPlayerReady[i])
                {
                    isReadyTexts[i].text = "Player " + (i + 1) + " is not ready";
                    isPlayerReady[i] = false;
                    nrOfPlayersReady--;
                }

                // Toggle player ready via keyboard numbers
                if(Input.GetKeyDown(keyCodes[i]))
                {
                    if (!isPlayerReady[i])
                    {
                        isReadyTexts[i].text = "Player " + (i + 1) + " is ready";
                        isPlayerReady[i] = true;
                        nrOfPlayersReady++;
                    }
                    else
                    {
                        isReadyTexts[i].text = "Player " + (i + 1) + " is not ready";
                        isPlayerReady[i] = false;
                        nrOfPlayersReady--;
                    }
                }

                // If you can change color and if you're not ready...
                if (canChangeColor[i] && !isPlayerReady[i])
                {
                    // If you press left on the d-pad...
                    if (state[i].DPad.Left == ButtonState.Pressed)
                    {
                        // Decreases colorIndex[i] by 1
                        colorIndex[i]--;

                        while (true)
                        {                          
                            // Sets colorIndex[i] to it's max value if it gets lower that its minimum value
                            if (colorIndex[i] <= -1)
                                colorIndex[i] = 7;
                           
                            // Checks if the color is already taken, otherwise it skips to the next color
                            if (isColorTaken[colorIndex[i]])
                                colorIndex[i]--;
                            else
                                break;
                            
 
                        }

                        // You can no longer change color so that you wont be able to go through the vehicles by holding down the button
                        canChangeColor[i] = false;
                    }

                    // If you presses right on the d-pad...
                    else if (state[i].DPad.Right == ButtonState.Pressed)
                    {
                        // Increases colorIndex[i] by 1
                        colorIndex[i]++;

                        while (true)
                        {
                            // Sets colorIndex[i] to it's minimum value if it gets higher that its max value
                            if (colorIndex[i] >= 8)
                                colorIndex[i] = 0;
                            
                            // Checks if the color is already taken, otherwise it skips to the next color
                            if (isColorTaken[colorIndex[i]])
                                colorIndex[i]++;
                            else
                                break;   
                            

                        }                

                        // You can no longer change color so that you wont be able to go through the vehicles by holding down the button
                        canChangeColor[i] = false;
                    }  
                }              
                // You can change vehicle if both the left and right d-pad buttons are released
                else if (state[i].DPad.Left == ButtonState.Released && state[i].DPad.Right == ButtonState.Released && !isPlayerReady[i])
                    canChangeColor[i] = true;

                // Updates what mesh to show
                colorHolders[i].color = colors[colorIndex[i]];
            } 
        }

        for (int i = 0; i < playerAmount; i++)
        {
            if(colorLastFrame[i] != colorIndex[i])
            {
                isColorTaken[colorLastFrame[i]] = false;
                isColorTaken[colorIndex[i]] = true;
            }
            // Sets the vehicle mesh last frame so it doesn't update next frame if it's the same
            colorLastFrame[i] = colorIndex[i]; 
            prevState[i] = state[i];
        }
    }


    // Changes from Start Screen to Main Menu screen
    public void ChangeToMain()
    {
        startScreen.SetActive(false);
        mainMenuGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Start Game Button"));
    }


    // Changes from Main Menu to Options screen
    public void ChangeToOptions()
    {
        mainMenuGroup.SetActive(false);
        optionsGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Volume Slider"));
    }


    // Changes to the How To Play menu
    public void ChangeToHowToPlay()
    {
        mainMenuGroup.SetActive(false);
        controllerLayoutGroup.SetActive(false);
        instructionsGroup.SetActive(false);
        howToPlayGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Controller Layout Button"));
    }



    // Changes from How To Play Menu to Controller Layout Menu
    public void ChangeToControllerLayout()
    {
        howToPlayGroup.SetActive(false);
        controllerLayoutGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Back Button"));
    }


    // Changes from How To Play Menu to Instructions Menu
    public void ChangeToInstructions()
    {
        howToPlayGroup.SetActive(false);
        instructionsGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Back Button"));
    }


    // Changes from Main Menu to Player Amount Selection screen
    public void ChangeToPlayerAmount()
    {
        mainMenuGroup.SetActive(false);
        vehicleSelectionGroup.SetActive(false);
        playerAmountGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Player Amount Slider"));
    }


    // Changes from Player Amount Selection screen to Vehicle Selection screen
    public void ChangeToColorSelection()
    {
        // Sets playerAmount to the selected number of players
        playerAmount = Mathf.FloorToInt(playerAmountSlider.value);
        state = new GamePadState[Mathf.FloorToInt(playerAmountSlider.value)];

        // Sets livesAmount to the selected number of lives
        livesAmount =  Mathf.FloorToInt(livesAmountSlider.value);

        playerAmountGroup.SetActive(false);
        vehicleSelectionGroup.SetActive(true);

        // Activates the cameras for the selected players
        for (int i = 2; i <= playerAmount; i++)
            GameObject.Find("Main Camera " + i).GetComponent<Camera>().enabled = true;

        // Sets the array with the same amount of values as playerAmount
        isPlayerReady = new bool[playerAmount];

        // Resets if the players are ready
        for (int i = 0; i < playerAmount; i++)
        {
            isPlayerReady[i] = false;
            isReadyTexts[i].text = "Player " + (i + 1) + " is not ready";
            colorHolders[i].color = colors[i];
            colorIndex[i] = i;
            isColorTaken[i] = true;
            colorLastFrame[i] = colorIndex[i];
        }
    }


    // Changes from Main Menu to Credits screen
    public void ChangeToCredits()
    {
        mainMenuGroup.SetActive(false);
        creditsGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Back Button"));
    }
    

    // Loads the "Level1" scene
    public void StartGame()
    {
        Application.LoadLevel("Level1");
    }


    // Returns to Main Menu
    public void BackToMain()
    {
        optionsGroup.SetActive(false);
        creditsGroup.SetActive(false);
        playerAmountGroup.SetActive(false);
        howToPlayGroup.SetActive(false);
        mainMenuGroup.SetActive(true);

        EventSystem.SetSelectedGameObject(GameObject.Find("Start Game Button"));
    }


    // Loads the Menu scene from the Match Results scene
    public void ReturnToMenu()
    {
        Application.LoadLevel("Menu");
    }


    // Exits game
    public void ExitGame()
    {
        Application.Quit();
    }
}
