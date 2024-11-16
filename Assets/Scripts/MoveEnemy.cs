using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    public List<List<GameObject>> map;
    public List<GameObject> validTiles;
    public Dictionary<int,int> traversalGraph;

    public Heap a;
    // Start is called before the first frame update
    void Start()
    {
        a = new Heap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Heap {
        public List<List<int>> h;

        public Heap() {
            h = new List<List<int>>();
        }

        //current is h[i]
        //parent is h[(i-1)/2]
        //left child is h[(i*2+1)]
        //right child is h[(i*2+2)]
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

        public void Insert(int heur, int id) {
            h.Add(new List<int>() {heur, id});
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

    public void Scout() {

    }

}
