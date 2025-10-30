using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStatWriter : MonoBehaviour
{
    AllyStats a;
    AllyStats b;
    AllyStats c;
    AllAllies allStats;
    // Start is called before the first frame update
    void Awake()
    {
        a = new AllyStats(6, false, 1, 12, 8, 8, 15, 0, 0, 5, 5, 3);
        b = new AllyStats(5, false, 1, 18, 12, 3, 3, 0, 0, 12, 3, 5);
        c = new AllyStats(6, true, 2, 10, 2, 11, 8, 0, 0, 3, 3, 2);
        allStats = new AllAllies();
        allStats.allAllies.Add(a);
        allStats.allAllies.Add(b);
        allStats.allAllies.Add(c);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string allAllyData = JsonUtility.ToJson(allStats);
            string filePath = Application.persistentDataPath + "/CharacterStats.json";
            Debug.Log(filePath);
            System.IO.File.WriteAllText(filePath, allAllyData);
            Debug.Log("Data Saved!");
        }
    }
}

[System.Serializable]
public class AllEnemies
{
    public AllEnemies()
    {
        allEnemies = new List<EnemyStats>();
    }
    public List<EnemyStats> allEnemies;
}

[System.Serializable]
public class EnemyStats
{
    public EnemyStats(string behaviour, int movementDistance, bool usesNRG, int rng, int hp, int atk, int nrg, int spd, int skl, int fin, int arm, int shld, int wgt) {
        this.behaviour = behaviour;
        this.movementDistance = movementDistance;
        this.usesNRG = usesNRG;
        this.rng = rng;
        this.hp = hp;
        this.atk = atk;
        this.nrg = nrg;
        this.spd = spd;
        this.skl = skl;
        this.fin = fin;
        this.arm = arm;
        this.shld = shld;
        this.wgt = wgt;

    }
    public string behaviour;
    public int movementDistance;
    public bool usesNRG;
    public int rng;
    public int hp; //Health a character has
    public int atk; //Physical damage a character deals
    public int nrg; //Energy damage a character deals
    public int spd; //Speed determines how many times you attack in battle
    public int skl; //Determines hit rate and slight crit
    public int fin; //Determines dodge rate and crit rate
    public int arm; //Resistance to physical damage
    public int shld; //Resistance to energy damage
    public int wgt;
}

[System.Serializable]
public class MapSave
{
    public MapSave()
    {
        map = new List<List<int>>();
    }
    List<List<int>> map;
}

[System.Serializable]
public class AllAllies
{
    public AllAllies()
    {
        allAllies = new List<AllyStats>();
    }
    public List<AllyStats> allAllies;
}

[System.Serializable]
public class AllyStats
{
    public AllyStats(int movementDistance, bool usesNRG, int rng, int hp, int atk, int nrg, int spd, int skl, int fin, int arm, int shld, int wgt) {
        this.movementDistance = movementDistance;
        this.usesNRG = usesNRG;
        this.rng = rng;
        this.hp = hp;
        this.atk = atk;
        this.nrg = nrg;
        this.spd = spd;
        this.skl = skl;
        this.fin = fin;
        this.arm = arm;
        this.shld = shld;
        this.wgt = wgt;

    }
    public int movementDistance;
    public bool usesNRG;
    public int rng;
    public int hp; //Health a character has
    public int atk; //Physical damage a character deals
    public int nrg; //Energy damage a character deals
    public int spd; //Speed determines how many times you attack in battle
    public int skl; //Determines hit rate and slight crit
    public int fin; //Determines dodge rate and crit rate
    public int arm; //Resistance to physical damage
    public int shld; //Resistance to energy damage
    public int wgt;
}