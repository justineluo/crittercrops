using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    public float moveSpeed;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    public float airControl = 10f;
    Vector3 input, moveDirection;
    public InventoryObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;

        if (_controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                // jump
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
            else
            {
                // ground player
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // mid-air
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Item"))
        {
            var item = other.GetComponent<Item>();

            inventory.AddItem(item, 1);
            Destroy(other.gameObject);

        }

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

}
