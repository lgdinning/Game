using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public List<List<GameObject>> map;
    public Dictionary<GameObject,int> depthChart;
    public Material plains;
    public Material water;
    public Material wall;
    public Material enemy;
    // Start is called before the first frame update
    void Start()
    {
        depthChart = new Dictionary<GameObject, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplay(HashSet<GameObject> set, bool up, int add) {
        if (!up) {
            add = add * -1;
        } 
        foreach (GameObject tile in set) {
            
            if (depthChart.TryGetValue(tile, out int depth)) {

                if (depth + add <= 0) {
                    
                    depthChart.Remove(tile);
                    switch (tile.GetComponent<TileBehaviour>().status) {
                        case 1:
                            tile.GetComponent<MeshRenderer>().material = plains;
                            break;
                        case 2:
                            tile.GetComponent<MeshRenderer>().material = water;
                            break;
                        case 3:
                            tile.GetComponent<MeshRenderer>().material = wall;
                            break;
                    }
                } else {
                    depthChart[tile] += add;
                }
            } else {

                depthChart[tile] = add;
                tile.GetComponent<MeshRenderer>().material = enemy;
            }
        }
    } 
}
