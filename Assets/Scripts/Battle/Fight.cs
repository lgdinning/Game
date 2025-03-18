using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Hit chance = 75 + 2*skl
    //Dodge chance = 3*fin
    //Damage = str - def OR nrg - shl

    public float FightHeur(CharacterAttack a, CharacterAttack b) {
        //int total = 100;
        //int multiplier = a.spd;
        return 1f;
    }
    public int FightSim(CharacterAttack ally, EnemyAttack enemy, bool allyTurn, bool real) {
        //var rand = new Random();

        int allyHits = 1;
        int enemyHits = 1;
        int allySpeed = ally.spd;
        int enemySpeed = enemy.spd;
        int allyPower = ally.atk;
        int allyProtection = ally.arm;
        int enemyPower = ally.atk;
        int enemyProtection = ally.arm;
        
        if (ally.usesNRG) {
            allyPower = ally.nrg;
            enemyProtection = enemy.shld;
        }
        if (enemy.usesNRG) {
            enemyPower = enemy.nrg;
            allyProtection = ally.shld;
        }
        if (allySpeed > enemySpeed) {
            allyHits += (allySpeed - enemySpeed) / 5;
        } else {
            enemyHits += (enemySpeed - allySpeed) / 5;
        }
        if (ally.rng != enemy.rng) {
            enemyHits = 0;
        }
        int enemyHPLost = 0;
        int allyHPLost = 0;
        while (allyHits + enemyHits > 0 && ally.hp > allyHPLost && enemy.hp > enemyHPLost) {
            if (allyTurn && (allyPower-enemyProtection > 0)) {
                enemyHPLost += allyPower - enemyProtection;
            } else if (!allyTurn && (enemyPower-allyProtection > 0)) {
                allyHPLost += enemyPower - allyProtection;
            }
            if (allyTurn) {
                allyHits -= 1;
            } else {
                enemyHits -= 1;
            }
            if ((allyTurn && enemyHits > 0) || (!allyTurn && allyHits > 0)) {
                allyTurn = !allyTurn;
            }
        }
        int final = 100;
        if (real) {
            ally.hp -= allyHPLost;
            enemy.hp -= enemyHPLost;
        } else {
            final -= allyHPLost;
            final -= enemyHPLost / 2;
        }
        return final;
    }
}
