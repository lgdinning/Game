using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public List<GameObject> playerPieces;
    public List<GameObject> enemyPieces;
    public bool playerPhase;
    public bool playerMoving;
    // Start is called before the first frame update
    void Start()
    {
        playerPhase = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SetPlayerTurn(false);
            CheckPlayerDone();
        }
    }

    public void UpdateTarget(int x, int y) {
        foreach (GameObject e in enemyPieces) {
            e.GetComponent<MoveEnemy>().targetX = x;
            e.GetComponent<MoveEnemy>().targetY = y;
        }
    }

    public void SetPlayerTurn(bool on) {
        if (on) {
            foreach (GameObject p in playerPieces) {
                p.GetComponent<MoveCharacter>().hasMoved = false;
                p.GetComponent<MeshRenderer>().material = p.GetComponent<MoveCharacter>().unmoved;
            } 
        } else {     
            foreach (GameObject p in playerPieces) {
                p.GetComponent<MoveCharacter>().hasMoved = true;
                p.GetComponent<MeshRenderer>().material = p.GetComponent<MoveCharacter>().selected;
            }
        }
    }

    
    public bool CheckPlayerDone() {
        foreach (GameObject p in playerPieces) {
            if (!p.GetComponent<MoveCharacter>().hasMoved) {
                return false;
            }
        }
        StartCoroutine(EnemyTurn());
        return true;
    }

    IEnumerator EnemyTurn() {
        foreach (GameObject e in enemyPieces) {
            e.GetComponent<MoveEnemy>().QueueUpdate(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y); 
            while (e.GetComponent<MoveEnemy>().isMoving) {
                yield return null;
            }
        }
        SetPlayerTurn(true);
        playerPhase = true;
    }
}
