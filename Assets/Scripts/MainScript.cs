using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MainScript : MonoBehaviour 
{
    public GameObject[] players = new GameObject[4]; // An array with Player 3 and 4
    public Color[] colors;

	void Awake () 
    {
        for(int i = 2; i < MenuManager.playerAmount; i++)
            players[i].SetActive(true);
	}

    void OnApplicationQuit()
    {
        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
        GamePad.SetVibration(PlayerIndex.Three, 0f, 0f);
        GamePad.SetVibration(PlayerIndex.Four, 0f, 0f);
    }
}