using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public List<GameObject> playerPieces;
    public List<GameObject> enemyPieces;
    public bool playerPhase;
    // Start is called before the first frame update
    void Start()
    {
        playerPhase = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool CheckPlayerDone() {
        foreach (GameObject p in playerPieces) {
            if (!p.GetComponent<MoveCharacter>().hasMoved) {
                return false;
            }
        }
        foreach (GameObject p in playerPieces) {
            p.GetComponent<MoveCharacter>().hasMoved = false;
            p.GetComponent<MeshRenderer>().material = p.GetComponent<MoveCharacter>().unmoved;
        }
        return true;
    }
}
