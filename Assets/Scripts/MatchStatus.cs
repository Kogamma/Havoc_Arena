using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MatchStatus : MonoBehaviour 
{
    public List<GameObject> players = new List<GameObject>(); // The players
    public static int playerCount; // The amount of players
    public int playersLeft;        // The amount of players that still has lives
    public static int[] playerPlaceIndex; // An array with all places

	void Start () 
    {
        // Adds all the players
        players.AddRange(GameObject.FindGameObjectsWithTag("Player")); 
        // PlayerCount has now the same value as the amount of players in the list
        playerCount = players.Count;
        // Resets playersLeft
        playersLeft = playerCount;
        
        //playerPlace = new string[playerCount];
        playerPlaceIndex = new int[playerCount];
    }
	
	void Update () 
    {

	}
}
