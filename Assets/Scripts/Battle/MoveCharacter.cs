using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public bool isMC;
    public Material plains;
    public Material water;
    public Material wall;
    public Material attackable;
    public Material available;
    public Material unmoved;
    public Material selected;
    public List<List<GameObject>> map;
    public HashSet<GameObject> validTiles;
    public HashSet<GameObject> attackableTiles;
    public List<GameObject> hashSet;
    public List<List<int>> validPath;
    public GameObject playerStatus;
    public bool status = true;
    public Dictionary<int,int> traversalGraph;
    public bool hasMoved;
    public bool isMoving;
    public int movementDistance;
    public int attackRange;
    public GameObject phaseManager;
    public int wasX;
    public int wasY;

    // Start is called before the first frame update
    void Start()
    {
        validPath = new List<List<int>>();
        hasMoved = false;
        traversalGraph = new Dictionary<int, int>();
        validTiles = new HashSet<GameObject>();
        attackableTiles = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMC() {
        isMC = true;
    }

    void OnMouseDown() {

        if ((!hasMoved) && (!playerStatus.GetComponent<ActionStatus>().pieceSelected) && phaseManager.GetComponent<PhaseManager>().playerPhase && !playerStatus.GetComponent<ActionStatus>().playerMoving) {
            QueueUpdate(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
            playerStatus.GetComponent<ActionStatus>().character = gameObject;
            playerStatus.GetComponent<ActionStatus>().validTiles = traversalGraph;
            playerStatus.GetComponent<ActionStatus>().validTileList = validTiles;
            playerStatus.GetComponent<ActionStatus>().attackableTiles = attackableTiles;
            playerStatus.GetComponent<ActionStatus>().Toggle();

        }
    }

    void OnMouseEnter() {
        if (!hasMoved && !playerStatus.GetComponent<ActionStatus>().pieceSelected && phaseManager.GetComponent<PhaseManager>().playerPhase && !playerStatus.GetComponent<ActionStatus>().playerMoving) {
            QueueUpdate(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
        }
    }

    void OnMouseExit() {
        if (!playerStatus.GetComponent<ActionStatus>().pieceSelected && !playerStatus.GetComponent<ActionStatus>().playerMoving) {
            foreach (GameObject valid in attackableTiles) {
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

    //
    public int DictOrDefault(Dictionary<int,int> a, int key) {
        int remMove;
        if (a.TryGetValue(key, out remMove)) {
            return remMove;
        } else {
            return -5;
        }
    }

    public void QueueUpdate(int i, int j, int movement) {
        wasX = i;
        wasY = j;
        
        validTiles = new HashSet<GameObject>(); //Stores tiles that we can move to
        traversalGraph.Clear(); //Stores amount of movement it takes to get to each valid tile
        Queue<List<int>> q = new Queue<List<int>>(); //Order of traversal
        validTiles.Add(map[i][j]); //Add root node (where player piece is)
        traversalGraph[map[i][j].GetInstanceID()] = movement; //Root takes 0 movement to get to
        q.Enqueue(new List<int> {i, j, movement}); //Add root to queue, nodes are in form of (x value, y value, movement remaining)
        //int tries = 0; this line was used for testing when i was getting infinite runtime errors
        List<int> curr;
        while (q.Count > 0) { //While there are still things in queue
            //tries += 1;

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
                if (x != 0) {

                    if ((map[x-1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x-1][y].GetComponent<TileBehaviour>().HasEnemy())) { 
                    
                        if (m > DictOrDefault(traversalGraph,map[x-1][y].GetInstanceID())) {  //Check if node has been visited or if it has already been checked in a shorter amount of movement
                            if (map[x-1][y].GetComponent<TileBehaviour>().status == 2) { //if water
                                if (m > 1) { //check if movement is sufficient for water
                                    map[x-1][y].GetComponent<Path>().prevX = x;
                                    map[x-1][y].GetComponent<Path>().prevY = y;
                                    validTiles.Add(map[x-1][y]); //Add to visitable tiles
                                    traversalGraph[map[x-1][y].GetInstanceID()] = m-2; //
                                    q.Enqueue(new List<int> {x-1, y, m-2});
                                } else {
                                    attackableTiles.Add(map[x-1][y]);
                                    q.Enqueue(new List<int> {x-1, y, -1});
                                }
                            } else { //if plains
                                map[x-1][y].GetComponent<Path>().prevX = x;
                                map[x-1][y].GetComponent<Path>().prevY = y;
                                validTiles.Add(map[x-1][y]);
                                traversalGraph[map[x-1][y].GetInstanceID()] = m-1;
                                q.Enqueue(new List<int> {x-1, y, m-1});
                            }
                        }
                    } else {
                        attackableTiles.Add(map[x-1][y]);
                        q.Enqueue(new List<int> {x-1, y, -1});
                    }
                } 
                //Check if bottom node is off map or if it's a wall
                if (y != 0) {
                    
                    if ((map[x][y-1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y-1].GetComponent<TileBehaviour>().HasEnemy())) {
                        if (m > DictOrDefault(traversalGraph,map[x][y-1].GetInstanceID())) {
                            if (map[x][y-1].GetComponent<TileBehaviour>().status == 2) {
                                if (m > 1) {
                                    map[x][y-1].GetComponent<Path>().prevX = x;
                                    map[x][y-1].GetComponent<Path>().prevY = y;
                                    validTiles.Add(map[x][y-1]);
                                    traversalGraph[map[x][y-1].GetInstanceID()] = m-2;
                                    q.Enqueue(new List<int> {x, y-1, m-2});
                                } else {
                                    attackableTiles.Add(map[x][y-1]);
                                    q.Enqueue(new List<int> {x, y-1, -1});
                                }
                            } else {
                                map[x][y-1].GetComponent<Path>().prevX = x;
                                map[x][y-1].GetComponent<Path>().prevY = y;
                                validTiles.Add(map[x][y-1]);
                                traversalGraph[map[x][y-1].GetInstanceID()] = m-1;
                                q.Enqueue(new List<int> {x, y-1, m-1});
                            }
                        }
                    } else {
                        attackableTiles.Add(map[x][y-1]);
                        q.Enqueue(new List<int> {x, y-1, -1});
                    }
                } 
                //Check if right node is off map or if it's a wall
                if (x != (map.Count-1)) {
                    if ((map[x+1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x+1][y].GetComponent<TileBehaviour>().HasEnemy())) {
                        if (m > DictOrDefault(traversalGraph,map[x+1][y].GetInstanceID())) {
                            if (map[x+1][y].GetComponent<TileBehaviour>().status == 2) {
                                if (m > 1) {
                                    map[x+1][y].GetComponent<Path>().prevX = x;
                                    map[x+1][y].GetComponent<Path>().prevY = y;
                                    validTiles.Add(map[x+1][y]);
                                    traversalGraph[map[x+1][y].GetInstanceID()] = m-2;
                                    q.Enqueue(new List<int> {x+1, y, m-2});
                                } else {
                                    attackableTiles.Add(map[x+1][y]);
                                    q.Enqueue(new List<int> {x+1, y, -1});
                                }
                            } else {
                                map[x+1][y].GetComponent<Path>().prevX = x;
                                map[x+1][y].GetComponent<Path>().prevY = y;
                                validTiles.Add(map[x+1][y]);
                                traversalGraph[map[x+1][y].GetInstanceID()] = m-1;
                                q.Enqueue(new List<int> {x+1, y, m-1});
                            }
                        }
                    } else {
                        attackableTiles.Add(map[x+1][y]);
                        q.Enqueue(new List<int> {x+1, y, -1});
                    }
                }
                //Check if right node is off map or if it's a wall
                if (y != (map[0].Count-1)) {
                    if ((map[x][y+1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y+1].GetComponent<TileBehaviour>().HasEnemy())) {
                        if (m > DictOrDefault(traversalGraph,map[x][y+1].GetInstanceID())) {
                            if (map[x][y+1].GetComponent<TileBehaviour>().status == 2) {
                                if (m > 1) {
                                    map[x][y+1].GetComponent<Path>().prevX = x;
                                    map[x][y+1].GetComponent<Path>().prevY = y;
                                    validTiles.Add(map[x][y+1]);
                                    traversalGraph[map[x][y+1].GetInstanceID()] = m-2;
                                    q.Enqueue(new List<int> {x, y+1, m-2});
                                } else {
                                    //map[x][y+1].GetComponent<MeshRenderer>().material = attackable;
                                    attackableTiles.Add(map[x][y+1]);
                                    q.Enqueue(new List<int> {x, y+1, -1});
                                }
                            } else {
                                map[x][y+1].GetComponent<Path>().prevX = x;
                                map[x][y+1].GetComponent<Path>().prevY = y;
                                validTiles.Add(map[x][y+1]);
                                traversalGraph[map[x][y+1].GetInstanceID()] = m-1;
                                q.Enqueue(new List<int> {x, y+1, m-1});
                            }
                        }
                    } else {
                        attackableTiles.Add(map[x][y+1]);
                        q.Enqueue(new List<int> {x, y+1, -1});
                    }
                }
            } else {
                if (m < 0) {
                    map[x][y].GetComponent<MeshRenderer>().material = attackable;
                }
                if (m > -(attackRange)) {
                    if (x > 0 && !(validTiles.Contains(map[x-1][y]))) {
                        attackableTiles.Add(map[x-1][y]);
                        q.Enqueue(new List<int> {x-1, y, m-1});
                    }
                    if (y > 0 && !(validTiles.Contains(map[x][y-1]))) {
                        attackableTiles.Add(map[x][y-1]);
                        q.Enqueue(new List<int> {x, y-1, m-1});
                    }
                    if ((x != map.Count-1) && !(validTiles.Contains(map[x+1][y]))) {
                        attackableTiles.Add(map[x+1][y]);
                        q.Enqueue(new List<int> {x+1, y, m-1});
                    }
                    if ((y != map[0].Count-1) && !(validTiles.Contains(map[x][y+1]))) {
                        attackableTiles.Add(map[x][y+1]);
                        q.Enqueue(new List<int> {x, y+1, m-1});
                    }
                }
            }
        }
        attackableTiles.UnionWith(validTiles);
        //map[i][j].GetComponent<MeshRenderer>().material = available;
    }
 
    public void BeginMove(int targetX, int targetY) {
        int startX = gameObject.transform.parent.GetComponent<TileBehaviour>().x;
        int startY = gameObject.transform.parent.GetComponent<TileBehaviour>().y;
        int tries = 0;
        validPath.Clear();
        int pathX = targetX;
        int pathY = targetY;
        while (!(pathX == startX && pathY == startY) && (tries < 500)) {
            //map[pathX][pathY].GetComponent<MeshRenderer>().material = selected;
            validPath.Add(new List<int> {pathX, pathY});
            int ph = pathX;
            pathX = map[pathX][pathY].GetComponent<Path>().prevX;
            pathY = map[ph][pathY].GetComponent<Path>().prevY;
        }
        Debug.Log(validPath.Count);
        StartCoroutine(Move());
    }
    IEnumerator Move() {
        int len = validPath.Count-1;
        int m = 0;
        while (len - m >= 0) {    
            if (len-m >= 0) {
                gameObject.transform.SetParent(map[validPath[len-m][0]][validPath[len-m][1]].transform, false);
                yield return new WaitForSeconds(0.05f);
            }
            m += 1;
        }
        playerStatus.GetComponent<ActionStatus>().playerMoving = false;
    }

    // public void StartTileUpdate(int i, int j, int movement) {
    //     validTiles = new List<GameObject>();
    //     traversalGraph.Clear();
    //     validTiles.Add(map[i][j]);
    //     traversalGraph[map[i][j].GetInstanceID()] = movement;
    //     map[i][j].GetComponent<MeshRenderer>().material = available;
    //     if ((i > 0) && (map[i-1][j].GetComponent<TileBehaviour>().status != 3)) {
    //         RecTileUpdate(i-1, j, movement-1);
    //     }
    //     if ((i < (map.Count-1)) && (map[i+1][j].GetComponent<TileBehaviour>().status != 3)) {
    //         RecTileUpdate(i+1, j, movement-1);
    //     }
    //     if ((j > 0) && (map[i][j-1].GetComponent<TileBehaviour>().status != 3)) {
    //         RecTileUpdate(i, j-1, movement-1);
    //     }
    //     if ((j < (map[0].Count-1)) && (map[i][j+1].GetComponent<TileBehaviour>().status != 3)) {
    //         RecTileUpdate(i, j+1, movement-1);
    //     }
    // }

    // public void RecTileUpdate(int i, int j, int movement) {
    //     if (map[i][j].GetComponent<TileBehaviour>().status == 2) {
    //         movement -= 1;
    //     }
    //     if (movement > (-1)) {
    //         validTiles.Add(map[i][j]);
    //         traversalGraph[map[i][j].GetInstanceID()] = movement;
    //         map[i][j].GetComponent<MeshRenderer>().material = available;
    //     }
    //     if (movement > 0) {
    //         int traversal;
    //         if ((i > 0) && (map[i-1][j].GetComponent<TileBehaviour>().status != 3)) {
    //             if (traversalGraph.TryGetValue(map[i-1][j].GetInstanceID(), out traversal)) {
    //                 if (movement > traversal) {
    //                     RecTileUpdate(i-1, j, movement-1);
    //                 }
    //             } else {
    //                 RecTileUpdate(i-1, j, movement-1);
    //             }
    //         }
    //         if ((i < (map.Count-1)) && (map[i+1][j].GetComponent<TileBehaviour>().status != 3)) {
    //             if (traversalGraph.TryGetValue(map[i+1][j].GetInstanceID(), out traversal)) {
    //                 if (movement > traversal) {
    //                     RecTileUpdate(i+1, j, movement-1);
    //                 }
    //             } else {
    //                 RecTileUpdate(i+1, j, movement-1);
    //             }
    //         }
    //         if ((j > 0) && (map[i][j-1].GetComponent<TileBehaviour>().status != 3)) {
    //             if (traversalGraph.TryGetValue(map[i][j-1].GetInstanceID(), out traversal)) {
    //                 if (movement > traversal) {
    //                     RecTileUpdate(i, j-1, movement-1);
    //                 }
    //             } else {
    //                 RecTileUpdate(i, j-1, movement-1);
    //             }
    //         }
    //         if ((j < (map[0].Count-1)) && (map[i][j+1].GetComponent<TileBehaviour>().status != 3)) {
    //             if (traversalGraph.TryGetValue(map[i][j+1].GetInstanceID(), out traversal)) {
    //                 if (movement > traversal) {
    //                     RecTileUpdate(i, j+1, movement-1);
    //                 }
    //             } else {
    //                 RecTileUpdate(i, j+1, movement-1);
    //             }
    //         }
    //     }
    // }
}
