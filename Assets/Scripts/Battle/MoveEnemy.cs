using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    public List<List<GameObject>> map;
    public List<GameObject> validTiles;
    public Dictionary<int,int> traversalGraph;
    public int targetX;
    public int targetY;
    public int movementDistance;
    public Material available;
    public Heap a;
    // Start is called before the first frame update
    void Start()
    {
        a = new Heap();
        targetX = 21;
        targetY = 12;

        traversalGraph = new Dictionary<int, int>();
        validTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter() {
        Debug.Log("Start");
        QueueUpdate(gameObject.transform.parent.GetComponent<TileBehaviour>().x, gameObject.transform.parent.GetComponent<TileBehaviour>().y, movementDistance);
    }

    // void OnMouseExit() {
    //     foreach (GameObject valid in validTiles) {
    //         switch (valid.GetComponent<TileBehaviour>().status) {
    //             case 1:
    //                 valid.GetComponent<MeshRenderer>().material = plains;
    //                 break;
    //             case 2:
    //                 valid.GetComponent<MeshRenderer>().material = water;
    //                 break;
    //             case 3:
    //                 valid.GetComponent<MeshRenderer>().material = wall;
    //                 break;
    //         }
    //     }    
    // }

    public class Heap {
        public List<List<int>> h;

        public Heap() {
            h = new List<List<int>>();
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
        }
    
        public void Print() {
            string toprint = "";
            foreach (List<int> a in h) {
                toprint = (toprint + " | " + a[0] + "," + a[1]);
                //Debug.Log(a[0] + "," + a[1]);
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
            //Print();
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

    public int calcHeur(int i, int j, int move) {
        return (Mathf.Abs(j-targetY) + Mathf.Abs(i-targetX) - move + movementDistance);
    }



    public void QueueUpdate(int i, int j, int movement) {
        //validTiles = new List<GameObject>(); //Stores tiles that we can move to
        traversalGraph.Clear(); //Stores amount of movement it takes to get to each valid tile
        a.Clear();
        traversalGraph[map[i][j].GetInstanceID()] = movement; //Root takes 0 movement to get to
        a.Insert(calcHeur(i, j, movement),i,j,movement);
        //q.Enqueue(new List<int> {i, j, movement}); //Add root to queue, nodes are in form of (x value, y value, movement remaining)
        //int tries = 0; this line was used for testing when i was getting infinite runtime errors
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

            //map[x][y].GetComponent<MeshRenderer>().material = available;


            //Check if left node is off map or if it's a wall
            if ((x != 0) && (map[x-1][y].GetComponent<TileBehaviour>().status != 3)) { 
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
            if ((y != 0) && (map[x][y-1].GetComponent<TileBehaviour>().status != 3)) { 
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
            if ((x != (map.Count-1)) && (map[x+1][y].GetComponent<TileBehaviour>().status != 3)) { 
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
            if ((y != (map[0].Count-1)) && (map[x][y+1].GetComponent<TileBehaviour>().status != 3)) { 
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
        while (!(pathX == i && pathY == j) && (tries < 500)) {
            map[pathX][pathY].GetComponent<MeshRenderer>().material = available;
            int ph = pathX;
            pathX = map[pathX][pathY].GetComponent<Path>().prevX;
            pathY = map[ph][pathY].GetComponent<Path>().prevY;
        }
        map[pathX][pathY].GetComponent<MeshRenderer>().material = available;
    }
}
