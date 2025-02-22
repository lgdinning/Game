using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
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
    public int wgt; //Determines how far unit hits another one

    // Start is called before the first frame update
    void Start()
    {
        rng = 1;
        hp = 10;
        atk = 7;
        nrg = 7;
        spd = 7;
        skl = 7;
        fin = 7;
        arm = 3;
        shld = 3;
        wgt = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
