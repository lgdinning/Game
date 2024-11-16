using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public bool active;
    public bool inDialogue;
    public TMP_Text textBox;
    public GameObject dialogueInterface;
    public GameObject stateManager;
    public GameObject dialogueDatabase;
    public bool isWriting;

    // Start is called before the first frame update
    void Start()
    {
        dialogueInterface.SetActive(!inDialogue);
        inDialogue = false;
    }

    // Update is called once per frame
    void Update() {
        if (active && Input.GetKeyDown(KeyCode.Space)) {
            if (!inDialogue) {
                stateManager.GetComponent<StationStateManager>().DialogueOff();
            } else if (!isWriting) {
                textBox.text = "";
                StartCoroutine(MessageWriter());
            } else {
                isWriting = false;
            }
        }
    }

    public void Switch(int i) {
        inDialogue = !inDialogue;
        dialogueInterface.SetActive(inDialogue);
        dialogueDatabase.GetComponent<DialogueDatabase>().Set(i);
        textBox.text = "";
        isWriting = true;
        StartCoroutine(MessageWriter());
        //active = true;
    }

    public void SwitchOff() {
        active = false;
        inDialogue = false;
        dialogueInterface.SetActive(false);
    }

    IEnumerator MessageWriter() {
        isWriting = true;
        foreach (char i in dialogueDatabase.GetComponent<DialogueDatabase>().currText) {
            textBox.text += i;
            if (isWriting) {
                yield return new WaitForSeconds(0.02f);
                active = true;
            }
        }
        isWriting = false;
        if (!dialogueDatabase.GetComponent<DialogueDatabase>().Next()) {
            inDialogue = false;
        }
    }
}
