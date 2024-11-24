using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int status;
    public GameObject playerStatus;
    public int x;
    public int y;
    public Material plains;
    public Material water;
    public Material wall;
    public GameObject phaseManager;
    public bool playerMoving;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && playerStatus.GetComponent<ActionStatus>().pieceSelected) {
            playerStatus.GetComponent<ActionStatus>().pieceSelected = !playerStatus.GetComponent<ActionStatus>().pieceSelected;
        }   
    }

    public bool HasAlly() {
        if ((!(gameObject.transform.childCount == 0)) && (gameObject.transform.GetChild(0).GetComponent<AllyOrEnemy>().ally)) {
            return true;
        }
        return false;
    }

    public bool HasEnemy() {
        if ((!(gameObject.transform.childCount == 0)) && (!gameObject.transform.GetChild(0).GetComponent<AllyOrEnemy>().ally)) {
            return true;
        }
        return false;
    }

    void OnMouseDown() {
        if (!playerStatus.GetComponent<ActionStatus>().playerMoving && (playerStatus.GetComponent<ActionStatus>().pieceSelected) && (gameObject.transform.childCount == 0) && (playerStatus.GetComponent<ActionStatus>().validTiles.ContainsKey(gameObject.GetInstanceID()))) {
            foreach (GameObject valid in playerStatus.GetComponent<ActionStatus>().attackableTiles) {
                switch (valid.GetComponent<TileBehaviour>().status) {
                    case 1:
                        valid.GetComponent<MeshRenderer>().material = plains;
                        break;
                    case 2:
                        valid.GetComponent<MeshRenderer>().material = water;
                        break;
                    case 3:
                        valid.GetComponent<MeshRenderer>().material = wall;
                        break;
                }
            }
            playerStatus.GetComponent<ActionStatus>().playerMoving = true;
            playerStatus.GetComponent<ActionStatus>().character.GetComponent<MoveCharacter>().BeginMove(x,y);   //.transform.SetParent(gameObject.transform, false);
            StartCoroutine(WaitForMove());
            

        }

    }

    IEnumerator WaitForMove() {
        while (playerStatus.GetComponent<ActionStatus>().playerMoving) {
            yield return null;
        }
        if (gameObject.transform.GetChild(0).GetComponent<MoveCharacter>().isMC) {
            phaseManager.GetComponent<PhaseManager>().UpdateTarget(x, y);
            // Debug.Log(x);
            // Debug.Log(y);
        }
        playerStatus.GetComponent<ActionStatus>().Toggle();
    }
}
