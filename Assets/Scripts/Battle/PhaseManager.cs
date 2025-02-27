using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public GameObject mc;
    public GameObject actionStatus;
    public ActionStatus state;
    public GameObject enemyDisplay;
    public List<GameObject> playerPieces;
    public List<GameObject> enemyPieces;
    public bool playerPhase;



    // Start is called before the first frame update
    void Start()
    {
        playerPhase = true;
        state = actionStatus.GetComponent<ActionStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPhase && (state.state == 1) && Input.GetKeyDown(KeyCode.Backspace)) {
            playerPhase = false;
            SetPlayerTurn(false);
            CheckPlayerDone();
        }
    }

    public void ClearUnclear() {
        foreach (GameObject e in enemyPieces) {
            if (e.GetComponent<MoveEnemy>().displaying) {
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, false, 1);
                e.GetComponent<MoveEnemy>().UpdateAttackables();
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, true, 1);
            }
        }
    }

    public void Clear(bool total) {
        foreach (GameObject e in enemyPieces) {
            if (e.GetComponent<MoveEnemy>().displaying) {
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, false, 1);
            }
            if (total) {
                e.GetComponent<MoveEnemy>().displaying = false;
            }
        }
    }

    public void UnClear() {
        foreach (GameObject e in enemyPieces) {
            if (e.GetComponent<MoveEnemy>().displaying) {
                e.GetComponent<MoveEnemy>().UpdateAttackables();
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, true, 1);
            }
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
                if (p.GetComponent<MoveCharacter>().isMC) {
                    UpdateTarget(p.transform.parent.GetComponent<TileBehaviour>().x, p.transform.parent.GetComponent<TileBehaviour>().y);
                }
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
        SetPlayerTurn(false);
        StartCoroutine(EnemyTurn());
        return true;
    }

    IEnumerator EnemyTurn() {
        foreach (GameObject e in enemyPieces) {
            
            if (e.GetComponent<MoveEnemy>().displaying) {
                e.GetComponent<MoveEnemy>().BFS(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y, e.GetComponent<MoveEnemy>().movementDistance);
                //Debug.Log(e.GetComponent<MoveEnemy>().attackableTiles.Count);
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, false, 1);
            }
            e.GetComponent<MoveEnemy>().DoTurn(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y); 
            while (e.GetComponent<MoveEnemy>().isMoving) {
                yield return null;
            }
            if (e.GetComponent<MoveEnemy>().displaying) {
                e.GetComponent<MoveEnemy>().BFS(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y, e.GetComponent<MoveEnemy>().movementDistance);
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, true, 1);
            }
        }
        SetPlayerTurn(true);
        playerPhase = true;
    }
}
