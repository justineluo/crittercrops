using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseViewController : MonoBehaviour
{
    Transform playerControl;
    public float mouseSensitivity = 90f;
    float yaw = 0;
    float pitch = 0;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerControl = transform.parent.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += moveX;
        playerControl.Rotate(Vector3.up * moveX);

        pitch -= moveY;
        pitch = Mathf.Clamp(pitch, -75, 75);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
