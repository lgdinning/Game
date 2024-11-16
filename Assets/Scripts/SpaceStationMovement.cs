using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationMovement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float speed;
    public bool active;
    public GameObject stateManager;
    public bool canInteract;
    public GameObject interactable;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {

            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            transform.Translate(Vector3.forward * speed*2 * Time.deltaTime * verticalInput);
            transform.Translate(Vector3.right * speed*2 * Time.deltaTime * horizontalInput);

            if ((canInteract) && (Input.GetKeyDown(KeyCode.Space))) {
                stateManager.GetComponent<StationStateManager>().Switch(interactable.GetComponent<SpaceStationInteractable>().index);
            }
        }
    }

    public void Switch() {
        active = !active;
    }

    void OnTriggerEnter(Collider other) {
        interactable = other.gameObject;
        canInteract = true;
        Debug.Log("contact!");
    }

    void OnTriggerExit(Collider other) {
        canInteract = false;
    }
}
