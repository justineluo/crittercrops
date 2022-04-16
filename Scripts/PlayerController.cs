using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    float moveSpeed;
    public float originalMoveSpeed = 5f;
    public float fastSpeed = 7f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    public float airControl = 10f;
    Vector3 input, moveDirection;
    public InventoryObject inventory;
    public AudioClip jumpSFX;
    public AudioClip walkSFX;
    public AudioClip pickupSFX;
    public AudioSource audioSource;

    bool isPlayingWalkAudio;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        moveSpeed = originalMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = fastSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = originalMoveSpeed;
        }

        if (_controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                // jump
                AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
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
        
        PlayWalkAudio();
    }

    void PlayWalkAudio() {
        //if player is moving, is on the ground, and the audio is not already playing, play walk sfx
        if ((Input.GetAxis("Horizontal") != 0.0 || Input.GetAxis("Vertical") != 0.0) 
            && transform.position.y < 1.3 && !isPlayingWalkAudio) {
            audioSource.PlayOneShot(walkSFX, 0.5f);
            isPlayingWalkAudio = true;
        } else if (!((Input.GetAxis("Horizontal") != 0.0 || Input.GetAxis("Vertical") != 0.0) 
            && transform.position.y < 1.3)) {
            isPlayingWalkAudio = false;
            audioSource.Stop();
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Item"))
        {
            var item = other.GetComponent<Item>();

            audioSource.PlayOneShot(pickupSFX);
            Debug.Log(item.item.type);
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

}
