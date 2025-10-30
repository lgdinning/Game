using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager displayManager;

    public List<List<GameObject>> map; //Grid of all tiles in map
    public List<List<GameObject>> updateMap;
    public Dictionary<GameObject,int> depthChart; //Dictionary carrying the tiles that are on the map and how many enemies can reach said tile
    public Material plains; //Green for plains
    public Material water; //Blue for water
    public Material wall; //Red for wall
    public Material enemy; //Pink for enemy range
    public Material clear;

    void Awake() {
        displayManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        depthChart = new Dictionary<GameObject, int>(); // Initialize
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //General Theory
    //We keep a dictionary (DepthChart) and update it when enabling or disabling enemy range indicator.
    //Add a point to a tile's depth chart when an enemy can move there and range is enabled. Remove one when enemy can move there that is disabled.
    //Mark red all tiles that can be moved to and are enabled.
    //set is the set of valid tiles that can be moved to
    //up determines if range is being enabled or disabled
    //add is 1
    public void UpdateDisplay(HashSet<GameObject> set, bool up, int add) { //Call this when an enemy range is enabled or disabled
        if (!up) { //If disabling, we set add to negative
            add = add * -1;
        } 
        foreach (GameObject tile in set) {
            int x = tile.GetComponent<TileBehaviour>().x;
            int y = tile.GetComponent<TileBehaviour>().y;
            
            if (depthChart.TryGetValue(tile, out int depth)) { //If it's in the dictionary already

                if (depth + add <= 0) { //If tile depth <= 0, it is no longer in enemy range and we change it back to its normal colour
                    
                    depthChart.Remove(tile);
                    updateMap[x][y].GetComponent<MeshRenderer>().material = clear;
                    // switch (tile.GetComponent<TileBehaviour>().status) {
                    //     case 1:
                    //         updateMap[x][y].GetComponent<MeshRenderer>().material = plains;
                    //         break;
                    //     case 2:
                    //         updateMap[x][y].GetComponent<MeshRenderer>().material = water;
                    //         break;
                    //     case 3:
                    //         updateMap[x][y].GetComponent<MeshRenderer>().material = wall;
                    //         break;
                    // }
                } else { //Otherwise add to depth
                    depthChart[tile] += add; 
                }
            } else { //If not in dictionary

                depthChart[tile] = add; //Add it in
                
                updateMap[x][y].GetComponent<MeshRenderer>().material = enemy; //Visually show that it is in range
            }
        }
    } 
}
