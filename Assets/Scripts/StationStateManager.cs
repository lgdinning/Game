using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationStateManager : MonoBehaviour
{
    public GameObject move;
    public GameObject dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch(int i) {
        move.GetComponent<SpaceStationMovement>().Switch();
        dialog.GetComponent<DialogueManager>().Switch(i);
    }

    public void DialogueOff() {
        move.GetComponent<SpaceStationMovement>().Switch();
        dialog.GetComponent<DialogueManager>().SwitchOff();
    }
}
