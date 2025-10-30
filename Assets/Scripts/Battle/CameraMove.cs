using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float spd = 10f; //Standard camera speed

    //Variables that will hold the directions that the camera will be moving
    public float verticalInput; 
    public float horizontalInput;
    public float scroll;
    public GameObject phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > 20) {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 20, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.y < 10) {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 10, gameObject.transform.position.z);
        }
        if (phaseManager.GetComponent<PhaseManager>().playerPhase)
        {
            verticalInput = Input.GetAxis("Vertical"); //Check if W or S is pressed
            horizontalInput = Input.GetAxis("Horizontal"); //Check if A or D is pressed
            scroll = Input.GetAxis("Mouse ScrollWheel"); //Check is mouse is scrolling

            transform.Translate(Vector3.up * verticalInput * Time.deltaTime * spd); //Move vertically accordingly
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * spd); //Move horizontally accordingly
            if (scroll < 0f && gameObject.transform.position.y < 20)
            { //If camera not at back
                transform.Translate(Vector3.forward * Time.deltaTime * spd * scroll * 200); //Move backward accordingly
            }
            if (scroll > 0f && gameObject.transform.position.y > 10)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * spd * scroll * 200); //Move forward accordingly
            }
        }
    }
}
