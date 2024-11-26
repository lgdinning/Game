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
    public Rigidbody move;
    private Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        active = true;
        move = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {

            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");

            direction = new Vector3(horizontalInput, 0, verticalInput);
            direction.Normalize();

            // transform.Translate(Vector3.forward * speed*2 * Time.deltaTime * verticalInput);
            // transform.Translate(Vector3.right * speed*2 * Time.deltaTime * horizontalInput);

            if ((canInteract) && (Input.GetKeyDown(KeyCode.Space))) {
                stateManager.GetComponent<StationStateManager>().Switch(interactable.GetComponent<SpaceStationInteractable>().index);
            }
        }
    }

    void FixedUpdate() {
        if (active) {
            move.AddForce(direction * speed);
        }
        // if ((horizontalInput == 0 && verticalInput == 0)) {
        //     Debug.Log("Check");
        //     move.velocity = new Vector3(0,0,0);
        // }

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
