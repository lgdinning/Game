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
    public MoveCharacter moveScript;
    public ActionStatus statusScript;


    
    // Start is called before the first frame update
    void Start()
    {
        statusScript = playerStatus.GetComponent<ActionStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && statusScript.pieceSelected) {
            statusScript.pieceSelected = !statusScript.pieceSelected;
        }   
    }

    public bool HasUnit() {
        return (HasAlly() || HasEnemy());
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

        if (statusScript.state > 1) {
            moveScript = statusScript.character.GetComponent<MoveCharacter>();
            if (!statusScript.playerMoving && (statusScript.pieceSelected) && (gameObject.transform.childCount == 0) && (statusScript.validTiles.ContainsKey(gameObject.GetInstanceID()))) {
                foreach (GameObject valid in statusScript.attackableTiles) {
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
                statusScript.playerMoving = true;
                moveScript.BeginMove(x,y);   //.transform.SetParent(gameObject.transform, false);
                StartCoroutine(WaitForMove());
            }
        }
    }

    IEnumerator WaitForMove() {
        while (statusScript.playerMoving) {
            yield return null;
        }
        if (gameObject.transform.GetChild(0).GetComponent<MoveCharacter>().isMC) {
            phaseManager.GetComponent<PhaseManager>().UpdateTarget(x, y);
        }
        statusScript.Shift(); //Move to 3
        moveScript.QueueUpdate(x,y,0);

    }
}
