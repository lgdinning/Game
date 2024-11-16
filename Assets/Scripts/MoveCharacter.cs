using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public Material plains;
    public Material water;
    public Material wall;
    public Material attackable;
    public Material available;
    public Material unmoved;
    public List<List<GameObject>> map;
    public List<GameObject> validTiles;
    public GameObject playerStatus;
    public bool status = true;
    public Dictionary<int,int> traversalGraph;
    public bool hasMoved;
    public int movementDistance;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;
        traversalGraph = new Dictionary<int, int>();
        validTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {

        if ((!hasMoved) && (!playerStatus.GetComponent<ActionStatus>().pieceSelected)) {
            //QueueUpdate(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
            playerStatus.GetComponent<ActionStatus>().character = gameObject;
            playerStatus.GetComponent<ActionStatus>().validTiles = traversalGraph;
            playerStatus.GetComponent<ActionStatus>().validTileList = validTiles;
            playerStatus.GetComponent<ActionStatus>().Toggle();

        }
    }

    void OnMouseEnter() {
        if (!hasMoved && !playerStatus.GetComponent<ActionStatus>().pieceSelected) {
            QueueUpdate(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
        }
    }

    void OnMouseExit() {
        if (!playerStatus.GetComponent<ActionStatus>().pieceSelected) {
            foreach (GameObject valid in validTiles) {
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

    public int DictOrDefault(Dictionary<int,int> a, int key) {
        int remMove;
        if (a.TryGetValue(key, out remMove)) {
            return remMove;
        } else {
            return -5;
        }
    }

    public void QueueUpdate(int i, int j, int movement) {
        validTiles = new List<GameObject>();
        traversalGraph.Clear();
        //remainingMove.Clear();
        Queue<List<int>> q = new Queue<List<int>>();
        validTiles.Add(map[i][j]);
        traversalGraph[map[i][j].GetInstanceID()] = movement;
        q.Enqueue(new List<int> {i, j, movement});
        int tries = 0;
        List<int> curr;
        while (q.Count > 0) {
            tries += 1;

            curr = q.Dequeue(); //Read current node
            //Debug.Log(q.Count);
            //Convert for readability
            int x = curr[0]; 
            int y = curr[1];
            int m = curr[2];

            //Show current node as readable
            map[x][y].GetComponent<MeshRenderer>().material = available;
            if (m > 0) { //If node not out of range
                //Check if left node is off map or if it's a wall
                if ((x != 0) && (map[x-1][y].GetComponent<TileBehaviour>().status != 3)) {
                    
                    if (m > DictOrDefault(traversalGraph,map[x-1][y].GetInstanceID())) {
                        if (map[x-1][y].GetComponent<TileBehaviour>().status == 2) {
                            if (m > 1) {
                                validTiles.Add(map[x-1][y]);
                                traversalGraph[map[x-1][y].GetInstanceID()] = m-2;
                                q.Enqueue(new List<int> {x-1, y, m-2});
                            }
                        //If left node is plains
                        } else {
                            validTiles.Add(map[x-1][y]);
                            traversalGraph[map[x-1][y].GetInstanceID()] = m-1;
                            q.Enqueue(new List<int> {x-1, y, m-1});
                        }
                    }
                }
                if ((y != 0) && (map[x][y-1].GetComponent<TileBehaviour>().status != 3)) {
                    if (m > DictOrDefault(traversalGraph,map[x][y-1].GetInstanceID())) {
                        if (map[x][y-1].GetComponent<TileBehaviour>().status == 2) {
                            if (m > 1) {
                                validTiles.Add(map[x][y-1]);
                                traversalGraph[map[x][y-1].GetInstanceID()] = m-2;
                                q.Enqueue(new List<int> {x, y-1, m-2});
                            }
                        } else {
                            validTiles.Add(map[x][y-1]);
                            traversalGraph[map[x][y-1].GetInstanceID()] = m-1;
                            q.Enqueue(new List<int> {x, y-1, m-1});
                        }
                    }
                }
                if ((x != (map.Count-1)) && (map[x+1][y].GetComponent<TileBehaviour>().status != 3)) {
                    if (m > DictOrDefault(traversalGraph,map[x+1][y].GetInstanceID())) {
                        if (map[x+1][y].GetComponent<TileBehaviour>().status == 2) {
                            if (m > 1) {
                                validTiles.Add(map[x+1][y]);
                                traversalGraph[map[x+1][y].GetInstanceID()] = m-2;
                                q.Enqueue(new List<int> {x+1, y, m-2});
                            }
                        } else {
                            validTiles.Add(map[x+1][y]);
                            traversalGraph[map[x+1][y].GetInstanceID()] = m-1;
                            q.Enqueue(new List<int> {x+1, y, m-1});
                        }
                    }
                }
                if ((y != (map[0].Count-1)) && (map[x][y+1].GetComponent<TileBehaviour>().status != 3)) {
                    if (m > DictOrDefault(traversalGraph,map[x][y+1].GetInstanceID())) {
                        if (map[x][y+1].GetComponent<TileBehaviour>().status == 2) {
                            if (m > 1) {
                                validTiles.Add(map[x][y+1]);
                                traversalGraph[map[x][y+1].GetInstanceID()] = m-2;
                                q.Enqueue(new List<int> {x, y+1, m-2});
                            }
                        } else {
                            validTiles.Add(map[x][y+1]);
                            traversalGraph[map[x][y+1].GetInstanceID()] = m-1;
                            q.Enqueue(new List<int> {x, y+1, m-1});
                        }
                    }
                }
            }
        } 
        //map[i][j].GetComponent<MeshRenderer>().material = available;
    }

    public void StartTileUpdate(int i, int j, int movement) {
        validTiles = new List<GameObject>();
        traversalGraph.Clear();
        validTiles.Add(map[i][j]);
        traversalGraph[map[i][j].GetInstanceID()] = movement;
        map[i][j].GetComponent<MeshRenderer>().material = available;
        if ((i > 0) && (map[i-1][j].GetComponent<TileBehaviour>().status != 3)) {
            RecTileUpdate(i-1, j, movement-1);
        }
        if ((i < (map.Count-1)) && (map[i+1][j].GetComponent<TileBehaviour>().status != 3)) {
            RecTileUpdate(i+1, j, movement-1);
        }
        if ((j > 0) && (map[i][j-1].GetComponent<TileBehaviour>().status != 3)) {
            RecTileUpdate(i, j-1, movement-1);
        }
        if ((j < (map[0].Count-1)) && (map[i][j+1].GetComponent<TileBehaviour>().status != 3)) {
            RecTileUpdate(i, j+1, movement-1);
        }
    }

    public void RecTileUpdate(int i, int j, int movement) {
        if (map[i][j].GetComponent<TileBehaviour>().status == 2) {
            movement -= 1;
        }
        if (movement > (-1)) {
            validTiles.Add(map[i][j]);
            traversalGraph[map[i][j].GetInstanceID()] = movement;
            map[i][j].GetComponent<MeshRenderer>().material = available;
        }
        if (movement > 0) {
            int traversal;
            if ((i > 0) && (map[i-1][j].GetComponent<TileBehaviour>().status != 3)) {
                if (traversalGraph.TryGetValue(map[i-1][j].GetInstanceID(), out traversal)) {
                    if (movement > traversal) {
                        RecTileUpdate(i-1, j, movement-1);
                    }
                } else {
                    RecTileUpdate(i-1, j, movement-1);
                }
            }
            if ((i < (map.Count-1)) && (map[i+1][j].GetComponent<TileBehaviour>().status != 3)) {
                if (traversalGraph.TryGetValue(map[i+1][j].GetInstanceID(), out traversal)) {
                    if (movement > traversal) {
                        RecTileUpdate(i+1, j, movement-1);
                    }
                } else {
                    RecTileUpdate(i+1, j, movement-1);
                }
            }
            if ((j > 0) && (map[i][j-1].GetComponent<TileBehaviour>().status != 3)) {
                if (traversalGraph.TryGetValue(map[i][j-1].GetInstanceID(), out traversal)) {
                    if (movement > traversal) {
                        RecTileUpdate(i, j-1, movement-1);
                    }
                } else {
                    RecTileUpdate(i, j-1, movement-1);
                }
            }
            if ((j < (map[0].Count-1)) && (map[i][j+1].GetComponent<TileBehaviour>().status != 3)) {
                if (traversalGraph.TryGetValue(map[i][j+1].GetInstanceID(), out traversal)) {
                    if (movement > traversal) {
                        RecTileUpdate(i, j+1, movement-1);
                    }
                } else {
                    RecTileUpdate(i, j+1, movement-1);
                }
            }
        }
    }
}
