using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material wall;
    public Material plains;
    public Material water;
    public GameObject tile;
    public GameObject piece;
    public int length;
    public int width;
    public List<List<GameObject>> grid;
    public System.Random rand;
    public GameObject phaseManager;
    private int randNum;
    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        grid = new List<List<GameObject>>();
        //Spawn(test1);
        for (int i = 0; i < length; i++) {
            grid.Add(new List<GameObject>());
            for (int j = 0; j < width; j++) {
                grid[i].Add(Instantiate(tile, new Vector3(i-3.5f, 0, j-3.5f), tile.transform.rotation));
                randNum = rand.Next(1,11);
                if (randNum < 8) {
                    if (randNum < 2) {
                        GameObject thisPiece = Instantiate(piece, new Vector3(i-3.5f, 0.2f, j-3.5f), piece.transform.rotation);
                        thisPiece.transform.SetParent(grid[i][j].transform);
                        thisPiece.GetComponent<MoveCharacter>().map = grid;
                    }
                    grid[i][j].GetComponent<MeshRenderer>().material = plains;
                    grid[i][j].GetComponent<TileBehaviour>().status = 1;
                } else if (randNum < 10) {
                    grid[i][j].GetComponent<MeshRenderer>().material = water;
                    grid[i][j].GetComponent<TileBehaviour>().status = 2;
                } else {
                    grid[i][j].GetComponent<MeshRenderer>().material = wall;
                    grid[i][j].GetComponent<TileBehaviour>().status = 3;
                }
                grid[i][j].GetComponent<TileBehaviour>().x = i;
                grid[i][j].GetComponent<TileBehaviour>().y = j;
            }
        }
    }

    public List<List<int>> test1 = new List<List<int>> {new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,2,1,2,1,1,1,1,1,1},new List<int>{1,1,11,1,1,1,1,1,1,1},new List<int>{1,3,3,3,1,1,1,1,1,1},new List<int>{12,1,1,1,1,1,1,1,1,1}, new List<int>{1,1,1,1,1,1,1,1,1,1}, new List<int>{1,1,11,1,1,1,1,1,1,1}, new List<int>{1,12,1,1,1,2,1,11,1,1}, new List<int>{1,1,11,11,1,11,1,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1},new List<int>{1,3,1,3,1,3,2,1,1,1}};

    public void Spawn(List<List<int>> reference) {
        grid = new List<List<GameObject>>();
        GameObject thisPiece;
        for (int x = 0; x < reference.Count; x++) {
            grid.Add(new List<GameObject>());
            //Debug.Log(grid.Count);
            for (int y = 0; y < reference[x].Count; y++) {
                //Debug.Log(grid[x].Count);
                grid[x].Add(Instantiate(tile, new Vector3(x-3.5f, 0, y-3.5f), tile.transform.rotation));
                
                switch (reference[x][y]) {
                    case 1:
                        grid[x][y].GetComponent<MeshRenderer>().material = plains;
                        grid[x][y].GetComponent<TileBehaviour>().status = 1;
                        break;
                    case 2:
                        grid[x][y].GetComponent<MeshRenderer>().material = water;
                        grid[x][y].GetComponent<TileBehaviour>().status = 2;
                        break;
                    case 3:
                        grid[x][y].GetComponent<MeshRenderer>().material = wall;
                        grid[x][y].GetComponent<TileBehaviour>().status = 3;
                        break;
                    case 11:
                        thisPiece = Instantiate(piece, new Vector3(x-3.5f, 0.2f, y-3.5f), piece.transform.rotation);
                        thisPiece.transform.SetParent(grid[x][y].transform);
                        thisPiece.GetComponent<MoveCharacter>().map = grid;
                        phaseManager.GetComponent<PhaseManager>().playerPieces.Add(thisPiece);
                        grid[x][y].GetComponent<MeshRenderer>().material = plains;
                        grid[x][y].GetComponent<TileBehaviour>().status = 1;
                        break;
                    case 12:
                        thisPiece = Instantiate(piece, new Vector3(x-3.5f, 0.2f, y-3.5f), piece.transform.rotation);
                        thisPiece.transform.SetParent(grid[x][y].transform);
                        thisPiece.GetComponent<MoveCharacter>().map = grid;
                        phaseManager.GetComponent<PhaseManager>().playerPieces.Add(thisPiece);
                        grid[x][y].GetComponent<MeshRenderer>().material = water;
                        grid[x][y].GetComponent<TileBehaviour>().status = 2;
                        break;
                    default:
                        break;
                }
                grid[x][y].GetComponent<TileBehaviour>().x = x;
                grid[x][y].GetComponent<TileBehaviour>().y = y;
            }
        }
        // for (int x = 0; x < reference.Count; x++) {

        //     Debug.Log(grid[x].Count);
        
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
