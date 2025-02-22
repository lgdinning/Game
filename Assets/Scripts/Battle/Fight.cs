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
        int multiplier = a.spd;
        return 1f;
    }
    public void FightSim(CharacterAttack atk, EnemyAttack def, bool atkTurn) {
        //var rand = new Random();

        int atkHits = 1;
        int defHits = 1;
        int atkSpeed = atk.spd;
        int defSpeed = def.spd;
        int atkPower = atk.atk;
        int atkProtection = atk.arm;
        int defPower = atk.atk;
        int defProtection = atk.arm;
        
        if (atk.usesNRG) {
            atkPower = atk.nrg;
            defProtection = def.shld;
        }
        if (def.usesNRG) {
            defPower = def.nrg;
            atkProtection = atk.shld;
        }
        if (atkSpeed > defSpeed) {
            atkHits += (atkSpeed - defSpeed) / 5;
        } else {
            defHits += (atkSpeed - defSpeed) / 5;
        }
        if (atk.rng != def.rng) {
            defHits = 0;
        }
        while (atkHits + defHits > 0 && atk.hp > 0 && def.hp > 0) {
            if (atkTurn && (atkPower-defProtection > 0)) {
                def.hp -= atkPower - defProtection;
            } else if (!atkTurn && (defPower-atkProtection > 0)) {
                atk.hp -= defPower - atkProtection;
            }
            if (atkTurn) {
                atkHits -= 1;
            } else {
                defHits -= 1;
            }
            if ((atkTurn && defHits > 0) || (!atkTurn && atkHits > 0)) {
                atkTurn = !atkTurn;
            }
        }
    }
}
