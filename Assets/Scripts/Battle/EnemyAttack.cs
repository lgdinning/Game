using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAttack : MonoBehaviour
{

    public int id;
    public int groupId;
    public int rng;
    public bool usesNRG;
    public int movementDistance;
    public string behaviour;
    public int hp; //Health a character has
    public int atk; //Physical damage a character deals
    public int nrg; //Energy damage a character deals
    public int spd; //Speed determines how many times you attack in battle
    public int skl; //Determines hit rate and slight crit
    public int fin; //Determines dodge rate and crit rate
    public int arm; //Resistance to physical damage
    public int shld; //Resistance to energy damage
    public int wgt; //Determines how far unit hits another one
    public GameObject phaseManager;
    public PhaseManager phase;
    public GameObject actionStatus;
    public ActionStatus state;
    public GameObject battleManager;
    public Fight fightManager;
    public Canvas canvas;
    public TMP_Text hpLabel;
    public TMP_Text atkLabel;
    public TMP_Text nrgLabel;
    public TMP_Text spdLabel;
    public TMP_Text sklLabel;
    public TMP_Text finLabel;
    public TMP_Text armLabel;
    public TMP_Text shldLabel;
    public TMP_Text wgtLabel;

    // void Awake() {

    // }

    // Start is called before the first frame update
    void Start()
    {
        usesNRG = false;
        groupId = 1;
        rng = 1;
        hp = 10;
        atk = 7;
        nrg = 7;
        spd = 7;
        skl = 7;
        fin = 5;
        arm = 3;
        shld = 3;
        wgt = 3;
        state = actionStatus.GetComponent<ActionStatus>();
        phase = phaseManager.GetComponent<PhaseManager>();
        fightManager = battleManager.GetComponent<Fight>();
        behaviour = "Defending";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver() {
        canvas.gameObject.SetActive(true);
        hpLabel.text = "HP: " + hp.ToString();
        atkLabel.text = "Atk: " + atk.ToString();
        nrgLabel.text = "Nrg: " + nrg.ToString();
        spdLabel.text = "Spd: " + spd.ToString();
        sklLabel.text = "Skl: " + skl.ToString();
        finLabel.text = "Fin: " + fin.ToString();
        armLabel.text = "Arm: " + arm.ToString();
        shldLabel.text = "Shld: " + shld.ToString();
        wgtLabel.text = "Wgt: " + wgt.ToString();
    }

    void OnMouseExit() {
        canvas.gameObject.SetActive(false);
    }

    void OnMouseDown() { //Used when ally would be attacking this unit.

        //State 3 means that ally piece has been moved and is selecting an action.
        //Also, this makes sure that this unit is attackable by the selected character.
        if (state.state == 3 && state.character.GetComponent<MoveCharacter>().attackableTiles.Contains(gameObject.transform.parent.gameObject)) {

            fightManager = battleManager.GetComponent<Fight>();

            //Make a call to the BattleManager to sort out damage calculations
            fightManager.FightSim(state.character.GetComponent<CharacterAttack>(), gameObject.GetComponent<EnemyAttack>(), true, true);

            // if (hp <= 0) { //If this enemy is dead
            //     phase.enemyPieces.Remove(gameObject); //Remove defeated enemy from viable pieces
            //     Destroy(gameObject); //Destroy enemy
            // }

            // if (state.character.GetComponent<CharacterAttack>().hp <= 0) { //If the ally who attacked the enemy is dead 
            //     phase.playerPieces.Remove(state.character); //Remove defeated ally from viable pieces
            //     state.character.GetComponent<MoveCharacter>().WashTiles(); //Clear the attackable and movable tiles
            //     Destroy(state.character); //Destroy character
            // }
            state.character.GetComponent<MoveCharacter>().WashTiles(); //After combat, wash the attackable and movable tiles
            state.Toggle(); //Change the state of the character piece to update that it has moved.
            state.Shift(); //Change the state back to the standard free gameplay. No piece selected
        }
    }
}
