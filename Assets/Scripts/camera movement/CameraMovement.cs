using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed, sensitivity;

    private float xRotation = 0f, yRotation = 0f;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * -speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(new Vector3(-transform.forward.z, 0, transform.forward.x).normalized * speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(-transform.forward.z, 0, transform.forward.x).normalized * -speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.Space))
            transform.Translate(new Vector3(0, speed * Time.deltaTime), Space.World);
        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(new Vector3(0, -speed * Time.deltaTime), Space.World);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
}
