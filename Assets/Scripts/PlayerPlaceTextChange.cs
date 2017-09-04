using UnityEngine;
using System.Collections;

public class PlayerPlaceTextChange : MonoBehaviour 
{
    public RectTransform[] playerTexts = new RectTransform[4]; // The texts that shows the place of the players in the results scene
    
	// Use this for initialization
	void Start () 
    {
        // Activates the texts for player 3 and 4 depending on how many players there are
        for (int i = 2; i < MatchStatus.playerCount; i++)
            playerTexts[i].gameObject.SetActive(true);

        // Repositions the player texts based on their place
        for (int i = 0; i < MatchStatus.playerCount; i++)
            playerTexts[i].anchoredPosition = new Vector2(playerTexts[i].anchoredPosition.x, 170f - 82f * MatchStatus.playerPlaceIndex[i]);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
