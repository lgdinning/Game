using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        state = actionStatus.GetComponent<ActionStatus>();
        phase = phaseManager.GetComponent<PhaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if (state.state == 3 && state.character.GetComponent<MoveCharacter>().attackableTiles.Contains(gameObject.transform.parent.gameObject)) {
            phase.enemyPieces.Remove(gameObject);
            Destroy(gameObject);
            state.character.GetComponent<MoveCharacter>().WashTiles();
            state.Toggle();
            state.Shift();
        }
    }
}
