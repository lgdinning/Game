using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHover : MonoBehaviour
{
    public GameObject phaseManager;
    public PhaseManager pm;
    public GameObject fightManager;
    public Fight fm;
    public GameObject actionStatus;
    public ActionStatus state;
    public Canvas canvas;
    public TMP_Text projAllyHP;
    public TMP_Text projEnemyHP;
    public TMP_Text allyHitRate;
    public TMP_Text enemyHitRate;
    public List<int> combatReport;

    // Start is called before the first frame update
    void Start()
    {
        pm = phaseManager.GetComponent<PhaseManager>();
        fm = fightManager.GetComponent<Fight>();
        state = actionStatus.GetComponent<ActionStatus>();
    }
    
    void OnMouseOver() {
        if (state.state == 3 && (state.character.GetComponent<MoveCharacter>().attackableTiles.Contains(gameObject.transform.parent.gameObject)))
        {
            combatReport = fm.CombatReport(state.character.GetComponent<CharacterAttack>(), gameObject.GetComponent<EnemyAttack>(), true);
            canvas.gameObject.SetActive(true);
            projAllyHP.text = "Ally HP: " + combatReport[0].ToString();
            projEnemyHP.text = "Enemy HP: " + combatReport[1].ToString();
            allyHitRate.text = "Hit Rate: " + combatReport[2].ToString();
            enemyHitRate.text = "Hit Rate: " + combatReport[3].ToString();
        }
    }

    void OnMouseExit() {
        if (state.state > 1)
        {
            canvas.gameObject.SetActive(false);    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
