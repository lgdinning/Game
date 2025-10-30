using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public static Fight fight;

    public GameObject phaseManager;
    public PhaseManager phase;
    public GameObject fightManager;
    public List<GameObject> enemiesToDelete;
    public List<GameObject> enemyList;

    void Awake() {
        fight = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        phase = phaseManager.GetComponent<PhaseManager>();
        enemyList = phase.enemyPieces;
        //enemiesToDelete = phaseManager.GetComponent<PhaseManager>().enemiesToDelete;
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
<<<<<<< HEAD

    public List<int> CombatReport(CharacterAttack ally, EnemyAttack enemy, bool allyTurn)
    {
        int allyHits = 1;
        int enemyHits = 1;
        int allySpeed = ally.spd;
        int enemySpeed = enemy.spd;
        int allyPower = ally.atk;
        int allyProtection = ally.arm;
        int enemyPower = enemy.atk;
        int enemyProtection = enemy.arm;
        int allyHitRate = 90 + ally.skl - enemy.fin - (int)(0.5 * enemy.spd);
        int enemyHitRate = 90 + enemy.skl - ally.fin - (int)(0.5 * ally.spd);

        if (ally.usesNRG)
        {
            allyPower = ally.nrg;
            enemyProtection = enemy.shld;
        }
        if (enemy.usesNRG)
        {
            enemyPower = enemy.nrg;
            allyProtection = ally.shld;
        }

        if (allySpeed > enemySpeed)
        {
            allyHits += (allySpeed - enemySpeed) / 4;
        }
        else
        {
            enemyHits += (enemySpeed - allySpeed) / 4;
        }

        if (ally.rng != enemy.rng)
        {
            if (allyTurn)
            {
                enemyHits = 0;
            }
            else
            {
                allyHits = 0;
            }
        }

        int enemyHPLost = 0;
        int allyHPLost = 0;
        while (allyHits + enemyHits > 0 && ally.hp > allyHPLost && enemy.hp > enemyHPLost)
        {
            
            if (allyTurn && (allyPower - enemyProtection > 0))
            {
                enemyHPLost += allyPower - enemyProtection;
            }
            else if (!allyTurn && (enemyPower - allyProtection > 0))
            {
                allyHPLost += enemyPower - allyProtection;
            }

            if (allyTurn)
            {
                allyHits -= 1;
            }
            else
            {
                enemyHits -= 1;
            }
            if ((allyTurn && enemyHits > 0) || (!allyTurn && allyHits > 0))
            {
                allyTurn = !allyTurn;
            }
        }

        return new List<int>() { ally.hp - allyHPLost, enemy.hp - enemyHPLost, allyHitRate, enemyHitRate };
    
    }

    public int FightSim(CharacterAttack ally, EnemyAttack enemy, bool allyTurn, bool real)
    {
=======
    public int FightSim(CharacterAttack ally, EnemyAttack enemy, bool allyTurn, bool real) {
>>>>>>> dc75531c671bcbc8f3069fe1d00e639d3747b37b
        //var rand = new Random();

        int allyHits = 1;
        int enemyHits = 1;
        int allySpeed = ally.spd;
        int enemySpeed = enemy.spd;
        int allyPower = ally.atk;
        int allyProtection = ally.arm;
<<<<<<< HEAD
        int enemyPower = enemy.atk;
        int enemyProtection = enemy.arm;
        int allyHitRate = 90 + ally.skl - enemy.fin - (int)(0.5 * enemy.spd);
        int enemyHitRate = 90 + enemy.skl - ally.fin - (int)(0.5 * ally.spd);

        if (ally.usesNRG)
        {
            allyPower = ally.nrg;
            enemyProtection = enemy.shld;
        }
        if (enemy.usesNRG)
        {
            enemyPower = enemy.nrg;
            allyProtection = ally.shld;
        }

        if (allySpeed > enemySpeed)
        {
            allyHits += (allySpeed - enemySpeed) / 4;
        }
        else
        {
            enemyHits += (enemySpeed - allySpeed) / 4;
        }
        if (ally.rng != enemy.rng)
        {
            if (allyTurn)
            {
                enemyHits = 0;
            }
            else
            {
                allyHits = 0;
            }
        }
        int enemyHPLost = 0;
        int allyHPLost = 0;
        while (allyHits + enemyHits > 0 && ally.hp > allyHPLost && enemy.hp > enemyHPLost)
        {
            int currRand = Random.Range(0, 101);

            if (!real || (allyTurn && currRand <= allyHitRate) || (!allyTurn && currRand <= enemyHitRate))
            {
                if (allyTurn && (allyPower - enemyProtection > 0))
                {
                    enemyHPLost += allyPower - enemyProtection;
                }
                else if (!allyTurn && (enemyPower - allyProtection > 0))
                {
                    allyHPLost += enemyPower - allyProtection;
                }

                int weightDiff = ally.wgt - enemy.wgt;

                TileBehaviour allyTile = ally.gameObject.transform.parent.GetComponent<TileBehaviour>();
                int allyX = allyTile.x;
                int allyY = allyTile.y;

                TileBehaviour enemyTile = enemy.gameObject.transform.parent.GetComponent<TileBehaviour>();
                int enemyX = enemyTile.x;
                int enemyY = enemyTile.y;

                if (allyTurn && ally.wgt > enemy.wgt)
                {
                    int x = enemyX - allyX;
                    int y = enemyY - allyY;
                    if (x != 0)
                    {
                        x /= Mathf.Abs(x);
                    }
                    if (y != 0)
                    {
                        y /= Mathf.Abs(y);
                    }

                    List<int> direction = new List<int>() { x, y };

                    List<GameObject> pushPath = enemy.gameObject.GetComponent<MoveEnemy>().GetPushPath(2 * weightDiff, direction);
                    if (real)
                    {
                        enemy.GetComponent<MoveEnemy>().Pushed(pushPath);
                        allyHits = 0;
                        enemyHits = 0;
                    }
                    else
                    {
                        allyHits = 0;
                    }
                    enemyHPLost += ((2 * weightDiff) - pushPath.Count) * (int)(0.25f * allyPower);
                }
                else if (!allyTurn && enemy.wgt > ally.wgt)
                {
                    int x = allyX - enemyX;
                    int y = allyY - enemyY;
                    if (x != 0)
                    {
                        x /= Mathf.Abs(x);
                    }
                    if (y != 0)
                    {
                        y /= Mathf.Abs(y);
                    }

                    List<int> direction = new List<int>() { x, y };


                    List<GameObject> pushPath = ally.gameObject.GetComponent<MoveCharacter>().GetPushPath(-2 * weightDiff, direction);
                    if (real)
                    {
                        ally.GetComponent<MoveCharacter>().Pushed(pushPath);
                        enemyHits = 0;
                        allyHits = 0;
                    }
                    else
                    {
                        enemyHits = 0;
                    }
                    allyHPLost += ((-2 * weightDiff) - pushPath.Count) * (int)(0.25f * enemyPower);
                }
                Debug.Log("Hit!");
            }
            else
            {
                Debug.Log("Miss!");
            }

            if (allyTurn)
                {
                    allyHits -= 1;
                }
                else
                {
                    enemyHits -= 1;
                }
            if ((allyTurn && enemyHits > 0) || (!allyTurn && allyHits > 0))
            {
                allyTurn = !allyTurn;
            }
        }
        int final = 100;
        if (real)
        {
            ally.hp -= allyHPLost;
            enemy.hp -= enemyHPLost;
        }
        else
        {
            final -= allyHPLost;
            final -= enemyHPLost / 4;
            if (ally.hp - allyHPLost < 0)
            {
                final -= 100;
            }
        }
        if (real)
        {
            if (enemy.hp <= 0)
            { //If this enemy is dead
                phase.enemyPieces.Remove(enemy.gameObject);
                phase.cam.transform.parent = null;
                Destroy(enemy.gameObject);
            }
            if (ally.hp <= 0)
            {
                phase.playerPieces.Remove(ally.gameObject);
                Destroy(ally.gameObject);
                phase.allyTakedown = true;
            }
=======
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
>>>>>>> dc75531c671bcbc8f3069fe1d00e639d3747b37b
        }
        return final;
    }
}
