using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStatus : MonoBehaviour
{
    public Material shade; //Colour of a piece when it has moved
    public bool pieceSelected = false; //MoveCharacter and TileBehaviour check this so they know if a piece has been clicked on to move it yet
    public GameObject character; //The piece that has been clicked
    public Dictionary<int,int> validTiles; //The traversal graph (Dict<tile instance ID, spaces away from current piece>)
    public HashSet<GameObject> validTileList; //List of tiles the piece can move to
    public HashSet<GameObject> attackableTiles; //List of tiles the piece can attack
    public GameObject phaseManager; //The object that manages phases
    public bool playerMoving; //Checks if a piece is currently in motion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle() { //Switches between the following game states: piece is selected, piece is not selected. 

        pieceSelected = !pieceSelected; //toggles piece selected

        if (!pieceSelected) { //This is used when tilebehaviour confirms a chosen tile to move to. This toggles back to a standard game state
            character.GetComponent<MoveCharacter>().hasMoved = true; //Denotes that the chosen piece has moved
            character.GetComponent<MoveCharacter>().isClicked = false; //Denotes that the chosen piece has not been selected
            character.GetComponent<MeshRenderer>().material = shade; //Greys out used piece
            phaseManager.GetComponent<PhaseManager>().CheckPlayerDone(); //Makes a call to phase manager to check and see if all allied pieces have moved
            playerMoving = false; //Allows players to make the next move now that the piece is done moving.
        }
    }
}
