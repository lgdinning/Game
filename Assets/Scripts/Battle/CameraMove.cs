using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float spd = 10f;
    public float verticalInput;
    public float horizontalInput;
    public float scroll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        scroll = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(Vector3.up * verticalInput * Time.deltaTime * spd);
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * spd);
        if (scroll < 0f && gameObject.transform.position.y <20) {
            transform.Translate(Vector3.forward * Time.deltaTime * spd * scroll * 200);
        }
        if (scroll > 0f && gameObject.transform.position.y >10) {
            transform.Translate(Vector3.forward * Time.deltaTime * spd * scroll * 200);
        }
    }
}
