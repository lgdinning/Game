using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    public List<List<GameObject>> map; //A grid representing the map with all the tiles in it and pieces attached to those tiles
    public HashSet<GameObject> validTiles; //This represents the list of tiles that the enemy can move to
    public HashSet<GameObject> attackableTiles; //This represents the tiles that this enemy can attack
    public HashSet<GameObject> targets; //List of allies that the enemy can attack
    public GameObject currentTarget;
    public Stack<List<int>> validPath; //A list of tiles that leads to the target destination
    public Dictionary<int,int> traversalGraph; //A list to note which tiles have been visited by the BFS algorithm and how far away they are
    public int targetX; //The x value in the grid that we want to go to
    public int targetY; //The y value in the grid that we want to go to
    public int movementDistance; //The distance that this unit can move
    public int attackRange; //The range that this unit can attack from
    public Material available; 
    public Material attackable;
    public Material plains;
    public Material water;
    public Material wall;
    public Heap a; //A heap used to pick priority tiles in the A* algorithm
    public Heap attackHeap; //A heap used to prioritize which unit to attack and which space to attack from
    public bool isMoving; //Checks if the current unit is in movement to lock anything else from happening until it is done
    public bool displaying; //Checks if the current unit is displaying its move range
    public GameObject enemyDisplay; //Grabs the script that highlights enemy ranges
    public GameObject actionManager; //Grabs the scripts that manages the action status and movement status
    public GameObject battleManager;
    public Fight fightManager;
    public ActionStatus state; //ActionStatus from actionManager
 
    // Start is called before the first frame update
    void Start()
    {
        a = new Heap();
        attackHeap = new Heap();
        traversalGraph = new Dictionary<int, int>();
        validTiles = new HashSet<GameObject>();
        attackableTiles = new HashSet<GameObject>();
        targets = new HashSet<GameObject>();
        validPath = new Stack<List<int>>();
        state = actionManager.GetComponent<ActionStatus>();
        fightManager = battleManager.GetComponent<Fight>();
    }

    // public void ToggleOn() { 
    //     BFS(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
    // }

    // public void ToggleOff() {

    //     foreach (GameObject v in attackableTiles) {
    //         switch (v.GetComponent<TileBehaviour>().status) {
    //             case 1:
    //                 v.GetComponent<MeshRenderer>().material = plains;
    //                 break;
    //             case 2:
    //                 v.GetComponent<MeshRenderer>().material = water;
    //                 break;
    //             case 3:
    //                 v.GetComponent<MeshRenderer>().material = wall;
    //                 break;
    //         }
    //     }
        
    // }

    void OnMouseDown() { //When in free moving state, no piece selected, this will let you toggle the range visibility
        if (state.state == 1 && !actionManager.GetComponent<ActionStatus>().playerMoving) {
            BFS(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
            if (!isMoving) {
                displaying = !displaying;
            }
        }

    }

    void OnMouseEnter() { //When in free moving state, no piece selected, this will toggle on the range visibility
        if (state.state == 1 && !actionManager.GetComponent<ActionStatus>().playerMoving) {
            BFS(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
            if (!displaying) {
                enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(attackableTiles, true, 1);
            }
        }
    }

    void OnMouseExit() { //When in free moving state, no piece selected, this will toggle off the range visibility

        if (state.state == 1 && !displaying) {
            enemyDisplay.GetComponent<DisplayManager>().UpdateDisplay(attackableTiles, false, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Heap {
        public List<List<int>> h;
        public Dictionary<List<int>,GameObject> listToTarget;

        public Heap() {
            h = new List<List<int>>();
            listToTarget = new Dictionary<List<int>,GameObject>();
        }

        //current is h[i]
        //parent is h[(i-1)/2]
        //left child is h[(i*2+1)]
        //right child is h[(i*2+2)]
        public int Len() {
            return h.Count;
        }

        public void Clear() {
            h = new List<List<int>>();
            listToTarget.Clear();
        }
    
        public void Print() {
            string toprint = "";
            foreach (List<int> a in h) {
                toprint = (toprint + " | " + a[0] + "," + a[1]);
            }
            Debug.Log(toprint);
        }

        public void Heapify(int i, bool upDown) {
            int l = i*2+1;
            int r = i*2+2;
            int largest = i;
            //Debug.Log(l +","+r+","+i);
            //Debug.Log(h.Count);
            if ((l < h.Count) && (h[l][0] < h[largest][0])) {
                largest = l;
            } 
            if ((r < h.Count) && (h[r][0] < h[largest][0])) {
                largest = r;
            } 
            //Debug.Log(largest + "," + i);
            if (largest != i) {
                List<int> ph = h[largest];
                h[largest] = h[i];
                h[i] = ph;
                if (upDown) {
                    if (i != 0) {
                        Heapify((i-1)/2, true);
                    }
                } else {
                    if (largest*2+1 < h.Count) {
                        Heapify((largest), false);
                    }
                }
            }
        }

        public void Insert(int heur, int x, int y, int m) {
            h.Add(new List<int>() {heur, x, y, m});
            Heapify(((h.Count-1)-1)/2, true);
        }

        public void Insert(int heur, int x, int y, GameObject target) {
            h.Add(new List<int>() {heur, x, y});
            Heapify(((h.Count-1)-1)/2, true);
            listToTarget.Add(new List<int>() {heur, x, y}, target);
        }

        public GameObject Get(List<int> key) {
            if (listToTarget.TryGetValue(key, out GameObject retrieved)) {
                return retrieved;
            } else {
                return null;
            }
        }

        public List<int> Pop() {
            List<int> ph = h[0];
            h[0] = h[h.Count-1];
            h[h.Count-1] = ph;
            h.RemoveAt(h.Count-1);
            Heapify(0, false);
            return ph;
        }
    }
    public int DictOrDefault(Dictionary<int,int> a, int key) {
        int remMove;
        if (a.TryGetValue(key, out remMove)) {
            return remMove;
        } else {
            return 999;
        }
    }

    public int BFSDictOrDefault(Dictionary<int,int> a, int key) {
        int remMove;
        if (a.TryGetValue(key, out remMove)) {
            return remMove;
        } else {
            return -5;
        }
    }

    public int calcHeur(int i, int j, int move) {
        return (Mathf.Abs(j-targetY) + Mathf.Abs(i-targetX) - move + movementDistance);
    }

    public int TargetHeur(int i, int j, int tx, int ty, int move) {
        return (Mathf.Abs(j-ty) + Mathf.Abs(i-tx));
    }

    public void UpdateAttackables() {
        BFS(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
    }

    public bool BFS(int i, int j, int movement) {
        bool toReturn = false;
        attackableTiles = new HashSet<GameObject>();
        validTiles = new HashSet<GameObject>(); //Stores tiles that we can move to
        targets.Clear();
        traversalGraph.Clear(); //Stores amount of movement it takes to get to each valid tile
        Queue<List<int>> q = new Queue<List<int>>(); //Order of traversal
        validTiles.Add(map[i][j]); //Add root node (where player piece is)
        traversalGraph[map[i][j].GetInstanceID()] = movement; //Root takes 0 movement to get to
        q.Enqueue(new List<int> {i, j, movement}); //Add root to queue, nodes are in form of (x value, y value, movement remaining)
        int tries = 0; //this line was used for testing when i was getting infinite runtime errors
        List<int> curr;
        while (q.Count > 0) { //While there are still things in queue
            tries += 1;
            curr = q.Dequeue(); //Read current node
            //Debug.Log(q.Count);
            //Convert for readability
            int x = curr[0]; 
            int y = curr[1];
            int m = curr[2];

            //Show current node as readable
            //map[x][y].GetComponent<MeshRenderer>().material = attackable;
            if (m > 0) { //If node not out of range
                //Check if left node is off map or if it's a wall
                if (x != 0) {
                    if (map[x-1][y].GetComponent<TileBehaviour>().HasAlly()) {
                        toReturn = true;
                        targets.Add(map[x-1][y].transform.GetChild(0).gameObject);
                    }
                    if ((map[x-1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x-1][y].GetComponent<TileBehaviour>().HasAlly())) { 
       
                    
                        if (m > BFSDictOrDefault(traversalGraph,map[x-1][y].GetInstanceID())) {  //Check if node has been visited or if it has already been checked in a shorter amount of movement
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
                    if (map[x][y-1].GetComponent<TileBehaviour>().HasAlly()) {
                        toReturn = true;
                        targets.Add(map[x][y-1].transform.GetChild(0).gameObject);
                    }
                    if ((map[x][y-1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y-1].GetComponent<TileBehaviour>().HasAlly())) {

                        if (m > BFSDictOrDefault(traversalGraph,map[x][y-1].GetInstanceID())) {
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
                    if (map[x+1][y].GetComponent<TileBehaviour>().HasAlly()) {
                        toReturn = true;
                        targets.Add(map[x+1][y].transform.GetChild(0).gameObject);
                    }
                    if ((map[x+1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x+1][y].GetComponent<TileBehaviour>().HasAlly())) {
 
                        if (m > BFSDictOrDefault(traversalGraph,map[x+1][y].GetInstanceID())) {
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
                    if (map[x][y+1].GetComponent<TileBehaviour>().HasAlly()) {
                        toReturn = true;
                        targets.Add(map[x][y+1].transform.GetChild(0).gameObject);
                    }
                    if ((map[x][y+1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y+1].GetComponent<TileBehaviour>().HasAlly())) {
                        
                        if (m > BFSDictOrDefault(traversalGraph,map[x][y+1].GetInstanceID())) {
 
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
                if (map[x][y].GetComponent<TileBehaviour>().HasAlly()) {
                    toReturn = true;
                    targets.Add(map[x][y].transform.GetChild(0).gameObject);
                    //map[x][y].GetComponent<MeshRenderer>().material = attackable;
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
        return toReturn;
    }

    public void DoTurn(int i, int j) {
        isMoving = true;
        BFS(i, j, movementDistance);
        if (targets.Count <= 0) {
            QueueUpdate(i, j);
        } else {
            List<GameObject> holder = new List<GameObject>();
            HashSet<GameObject> validTilesCopy = validTiles;
            HashSet<GameObject> validTilesCopy2 = new HashSet<GameObject>(validTiles);
            HashSet<List<GameObject>> ph = new HashSet<List<GameObject>>();
            foreach (GameObject phtile in targets) {
                holder.Add(phtile);
            }
            attackHeap.Clear();
            foreach (GameObject possibleTarget in holder) {
                BFS(possibleTarget.transform.parent.GetComponent<TileBehaviour>().x, possibleTarget.transform.parent.GetComponent<TileBehaviour>().y, 0);
                validTilesCopy.IntersectWith(attackableTiles);
                foreach (GameObject tileToAttack in validTilesCopy) {
                    ph.Add(new List<GameObject>(){tileToAttack,possibleTarget});
                    int thisX = gameObject.transform.parent.GetComponent<TileBehaviour>().x;
                    int thisY = gameObject.transform.parent.GetComponent<TileBehaviour>().y;
                    int otherX = tileToAttack.GetComponent<TileBehaviour>().x;
                    int otherY = tileToAttack.GetComponent<TileBehaviour>().y;
                    if (!tileToAttack.GetComponent<TileBehaviour>().HasEnemy() || (thisX == otherX && thisY == otherY)) {
                        TileBehaviour t = tileToAttack.GetComponent<TileBehaviour>();
                        attackHeap.Insert(fightManager.FightSim(possibleTarget.GetComponent<CharacterAttack>(), gameObject.GetComponent<EnemyAttack>(), false, false), t.x, t.y, possibleTarget);
                    } 
                }
                validTilesCopy = new HashSet<GameObject>(validTilesCopy2);
            }

            //Choose tile to move towards
            // attackHeap.Clear();
            // foreach (List<GameObject> phtile in ph) {
            //     //phtile.GetComponent<MeshRenderer>().material = attackable;
            //     if (!phtile[0].GetComponent<TileBehaviour>().HasEnemy()) {
            //         TileBehaviour t = phtile[0].GetComponent<TileBehaviour>();
            //         attackHeap.Insert(TargetHeur(i, j, t.x, t.y, movementDistance), t.x, t.y, 1);
            //     } 
            // }
            if (attackHeap.Len() > 0) {
                List<int> heapPop = attackHeap.Pop();
                targetX = heapPop[1];
                targetY = heapPop[2];
                currentTarget = attackHeap.Get(heapPop);
            }

            QueueUpdate(i,j);
            //QueueUpdate(ph[0].transform.parent.GetComponent<TileBehaviour>().x, ph[0].transform.parent.GetComponent<TileBehaviour>().y);
        }
    }

    public void QueueUpdate(int i, int j) {
        isMoving = true;
        //validTiles = new List<GameObject>(); //Stores tiles that we can move to
        traversalGraph.Clear(); //Stores amount of movement it takes to get to each valid tile
        a.Clear();
        traversalGraph[map[i][j].GetInstanceID()] = movementDistance; //Root takes 0 movement to get to
        a.Insert(calcHeur(i, j, movementDistance),i,j,movementDistance);
        //q.Enqueue(new List<int> {i, j, movement}); //Add root to queue, nodes are in form of (x value, y value, movement remaining)
        List<int> curr;
        int x = -1;
        int y = -1;
        int m = -1;
        while (a.Len() > 0 && !(x == targetX && y == targetY)) { //While there are still things in queue

            curr = a.Pop(); //Read current node
            //Convert for readability
            x = curr[1]; 
            y = curr[2];
            m = curr[3];

            //Check if left node is off map or if it's a wall
            if ((targetX == x-1 && targetY == y) || ((x != 0) && (map[x-1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x-1][y].GetComponent<TileBehaviour>().HasAlly()))) { 
                  //Check if node has been visited or if it has already been checked in a shorter amount of movement
                if (map[x-1][y].GetComponent<TileBehaviour>().status == 2) { //if water
                    if (calcHeur(x-1, y, m-2) < DictOrDefault(traversalGraph,map[x-1][y].GetInstanceID())) {                    
                        map[x-1][y].GetComponent<Path>().prevX = x;
                        map[x-1][y].GetComponent<Path>().prevY = y; //Add to visitable tiles
                        traversalGraph[map[x-1][y].GetInstanceID()] = calcHeur(x-1, y, m-2); //
                        a.Insert(calcHeur(x-1, y, m-2), x-1, y, m-2); 
                    }
                } else { //if plains
                    if (calcHeur(x-1, y, m-1) < DictOrDefault(traversalGraph,map[x-1][y].GetInstanceID())) {
                        map[x-1][y].GetComponent<Path>().prevX = x;
                        map[x-1][y].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x-1][y].GetInstanceID()] = calcHeur(x-1, y, m-1);
                        a.Insert(calcHeur(x-1, y, m-1), x-1, y, m-1);
                    }
                }       
            }
            //Check if bottom node is off map or if it's a wall
            if ((targetX == x && targetY == y-1) || ((y != 0) && (map[x][y-1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y-1].GetComponent<TileBehaviour>().HasAlly()))) { 
                  //Check if node has been visited or if it has already been checked in a shorter amount of movement
                if (map[x][y-1].GetComponent<TileBehaviour>().status == 2) { //if water
                    if (calcHeur(x, y-1, m-2) < DictOrDefault(traversalGraph,map[x][y-1].GetInstanceID())) {                    
                        map[x][y-1].GetComponent<Path>().prevX = x;
                        map[x][y-1].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x][y-1].GetInstanceID()] = calcHeur(x, y-1, m-2); //
                        a.Insert(calcHeur(x, y-1, m-2), x, y-1, m-2); 
                    }
                } else { //if plains
                    if (calcHeur(x, y-1, m-1) < DictOrDefault(traversalGraph,map[x][y-1].GetInstanceID())) {
                        map[x][y-1].GetComponent<Path>().prevX = x;
                        map[x][y-1].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x][y-1].GetInstanceID()] = calcHeur(x, y-1, m-1);
                        a.Insert(calcHeur(x, y-1, m-1), x, y-1, m-1);
                    }
                }       
            }
            //Check if right node is off map or if it's a wall
            if ((targetX == x+1 && targetY == y) || ((x != (map.Count-1)) && (map[x+1][y].GetComponent<TileBehaviour>().status != 3) && (!map[x+1][y].GetComponent<TileBehaviour>().HasAlly()))) { 
                  //Check if node has been visited or if it has already been checked in a shorter amount of movement
                if (map[x+1][y].GetComponent<TileBehaviour>().status == 2) { //if water
                    if (calcHeur(x+1, y, m-2) < DictOrDefault(traversalGraph,map[x+1][y].GetInstanceID())) {                    
                        map[x+1][y].GetComponent<Path>().prevX = x;
                        map[x+1][y].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x+1][y].GetInstanceID()] = calcHeur(x+1, y, m-2); //
                        a.Insert(calcHeur(x+1, y, m-2), x+1, y, m-2); 
                    }
                } else { //if plains
                    if (calcHeur(x+1, y, m-1) < DictOrDefault(traversalGraph,map[x+1][y].GetInstanceID())) {
                        map[x+1][y].GetComponent<Path>().prevX = x;
                        map[x+1][y].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x+1][y].GetInstanceID()] = calcHeur(x+1, y, m-1);
                        a.Insert(calcHeur(x+1, y, m-1), x+1, y, m-1);
                    }
                }       
            }
            //Check if right node is off map or if it's a wall
            if ((targetX == x && targetY == y+1) || ((y != (map[0].Count-1)) && (map[x][y+1].GetComponent<TileBehaviour>().status != 3) && (!map[x][y+1].GetComponent<TileBehaviour>().HasAlly()))) { 
                  //Check if node has been visited or if it has already been checked in a shorter amount of movement
                if (map[x][y+1].GetComponent<TileBehaviour>().status == 2) { //if water
                    if (calcHeur(x, y+1, m-2) < DictOrDefault(traversalGraph,map[x][y+1].GetInstanceID())) {                    
                        map[x][y+1].GetComponent<Path>().prevX = x;
                        map[x][y+1].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x][y+1].GetInstanceID()] = calcHeur(x, y+1, m-2); //
                        a.Insert(calcHeur(x, y+1, m-2), x, y+1, m-2); 
                    }
                } else { //if plains
                    if (calcHeur(x, y+1, m-1) < DictOrDefault(traversalGraph,map[x][y+1].GetInstanceID())) {
                        map[x][y+1].GetComponent<Path>().prevX = x;
                        map[x][y+1].GetComponent<Path>().prevY = y;
                        traversalGraph[map[x][y+1].GetInstanceID()] = calcHeur(x, y+1, m-1);
                        a.Insert(calcHeur(x, y+1, m-1), x, y+1, m-1);
                    }
                }       
            }

        }

        int pathX = targetX;
        int pathY = targetY;
        int tries = 0;
        validPath.Clear();
        while (!(pathX == i && pathY == j) && (tries < 500)) {
            //map[pathX][pathY].GetComponent<MeshRenderer>().material = available;
            validPath.Push(new List<int> {pathX, pathY});
            int ph = pathX;
            pathX = map[pathX][pathY].GetComponent<Path>().prevX;
            pathY = map[ph][pathY].GetComponent<Path>().prevY;
            //validPath.Add(new List<int> {pathX, pathY});
        }
        //map[pathX][pathY].GetComponent<MeshRenderer>().material = available;
        StartCoroutine(Move());
    }

    public bool IsMoving() {
        return isMoving;
    }

    IEnumerator Move() {
        int len = validPath.Count-1;
        int howFar = movementDistance;
        if (len - howFar < 1) {
            howFar = len;
        } 
        List<List<int>> wholePath = new List<List<int>>();
        for (int m = 0; m <= howFar; m++) {
            wholePath.Add(validPath.Pop());
        }
 
        while ((wholePath.Count > 0) && map[wholePath[wholePath.Count-1][0]][wholePath[wholePath.Count-1][1]].GetComponent<TileBehaviour>().HasEnemy()) {
            wholePath.RemoveAt(wholePath.Count-1);
        }
        
        foreach (List<int> x in wholePath) {
            gameObject.transform.SetParent(map[x[0]][x[1]].transform, false);
            yield return new WaitForSeconds(0.05f);
        }
        // while (howFar > 0 && map[validPath[len-howFar][0]][validPath[len-howFar][1]].GetComponent<TileBehaviour>().HasEnemy()) {
        //     Debug.Log(gameObject.GetInstanceID().ToString() + " " + howFar.ToString());
        //     howFar -= 1;
        // }
        // for (int m = 0; m < howFar; m++) {    
        //     if (len-m >= 1) {
        //         gameObject.transform.SetParent(map[validPath[len-m][0]][validPath[len-m][1]].transform, false);
        //         yield return new WaitForSeconds(0.05f);
        //     }
        // }
        fightManager.FightSim(currentTarget.GetComponent<CharacterAttack>(), gameObject.GetComponent<EnemyAttack>(), false, true);
        isMoving = false;
    }

}
