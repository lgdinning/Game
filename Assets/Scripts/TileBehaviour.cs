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

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if ((playerStatus.GetComponent<ActionStatus>().pieceSelected) && (gameObject.transform.childCount == 0) && (playerStatus.GetComponent<ActionStatus>().validTiles.ContainsKey(gameObject.GetInstanceID()))) {
            playerStatus.GetComponent<ActionStatus>().character.transform.SetParent(gameObject.transform, false);
            playerStatus.GetComponent<ActionStatus>().Toggle();
            foreach (GameObject valid in playerStatus.GetComponent<ActionStatus>().validTileList) {
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
        }
    }
}
