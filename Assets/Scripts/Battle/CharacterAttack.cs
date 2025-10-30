using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterAttack : MonoBehaviour, StatSpread
{
    public bool usesNRG;
    public int movementDistance;

    public int id;
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
    public GameObject actionStatus;
    public Canvas canvas;
    public ActionStatus state;
    public TMP_Text hpLabel;
    public TMP_Text atkLabel;
    public TMP_Text nrgLabel;
    public TMP_Text spdLabel;
    public TMP_Text sklLabel;
    public TMP_Text finLabel;
    public TMP_Text armLabel;
    public TMP_Text shldLabel;
    public TMP_Text wgtLabel;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        // rng = 1;
        // hp = 10;
        // atk = 7;
        // nrg = 7;
        // spd = 7;
        // skl = 7;
        // fin = 7;
        // arm = 3;
        // shld = 3;
        // wgt = 5;
        state = actionStatus.GetComponent<ActionStatus>();
    }

    void OnMouseOver()
    {
        if (state.state == 1)
        {
=======
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
        state = actionStatus.GetComponent<ActionStatus>();
    }

    void OnMouseOver() {
        if (state.state == 1) {
>>>>>>> dc75531c671bcbc8f3069fe1d00e639d3747b37b
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
    }

<<<<<<< HEAD
    void OnMouseExit()
    {
        if (state.state == 1)
        {
=======
    void OnMouseExit() {
        if (state.state == 1) {
>>>>>>> dc75531c671bcbc8f3069fe1d00e639d3747b37b
            canvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
