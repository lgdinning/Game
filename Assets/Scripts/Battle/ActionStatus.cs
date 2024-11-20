using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStatus : MonoBehaviour
{
    public Material shade;
    public bool pieceSelected = false;
    public GameObject character;
    public Dictionary<int,int> validTiles;
    public HashSet<GameObject> validTileList;
    public HashSet<GameObject> attackableTiles;
    public GameObject phaseManager;
    public bool playerMoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle() {
        //Debug.Log(pieceSelected);
        pieceSelected = !pieceSelected;
        //Debug.Log(pieceSelected);
        if (!pieceSelected) {
            character.GetComponent<MoveCharacter>().hasMoved = true;
            character.GetComponent<MeshRenderer>().material = shade;
            phaseManager.GetComponent<PhaseManager>().CheckPlayerDone();
            playerMoving = false;
        }
    }
}
