using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    [Header("Stats")]
    //how fast the player moves forward
    [SerializeField] float moveSpeed = 5;
    //how fast the player moves sideways between lanes
    [SerializeField] float laneSwapSpeed = 10;
    //jump parameters
    [SerializeField] float jumpForce = 30;
    [SerializeField] float gravity = 90;

    bool isAlive = true;

    Vector3 movement;

    [Header("Camera")]
    [SerializeField] GameObject followCamera;
    //toggles whether camera moves on x and y axes
    [SerializeField] bool cameraFollowPlayerLanes;
    [SerializeField] bool cameraFollowPlayerJump;
    Vector3 cameraOffset;

    //lane positions
    float[] lanes;
    int currentLane;
    //false while the player is still moving between lanes
    bool canMove = true;
    //true while player is in the middle of a jump
    bool inJump;

    //reference to character controller
    CharacterController controller;

    //For spawning platforms ahead of the player
    PlatformSpawner platformManager;
    float platformSize;

    private void Start()
    {
        //reference to character controller
        controller = GetComponent<CharacterController>();

        //set position and lane position
        lanes = Game.instance.lanes;
        currentLane = Mathf.FloorToInt(lanes.Length / 2);
        transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);

        cameraOffset = transform.position - followCamera.transform.position;

        //start spawning platforms
        platformManager = Game.instance.platformManager;
        platformSize = platformManager.platformSize;
        StartCoroutine(SpawnPlatforms());
    }

    private void Update()
    {
        if (isAlive)
        {
            MovePlayer();
            LaneInput();
        }
    }

    #region movement
    void MovePlayer()
    {
        if (controller.isGrounded)
        {
            movement.y = 0;
            inJump = false;
            PlayerJump();
        }
        else
        {
            movement.y -= gravity * Time.deltaTime;
        }

        movement.z = moveSpeed;

        Vector3 frameMovement = movement * Time.deltaTime;
        controller.Move(frameMovement);

        Vector3 cameraPos = followCamera.transform.position;

        if (cameraFollowPlayerLanes)
            cameraPos.x = transform.position.x - cameraOffset.x;

        if (cameraFollowPlayerJump)
            cameraPos.y = transform.position.y - cameraOffset.y;


        followCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z + frameMovement.z);
       

    }
    #endregion

    #region jump
    void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            movement.y = jumpForce;
            inJump = true;
        }
    }
    #endregion

    #region lane control

    void LaneInput()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        if (hInput > 0) 
            MoveRight();
        else if (hInput < 0)
            MoveLeft();
    }

    void MoveLeft()
    {
        if (currentLane > 0 && canMove)
        {
            currentLane -= 1;
            StartCoroutine(MoveToLane(lanes[currentLane], -1));
        }
    }

    void MoveRight()
    {
        if (currentLane < lanes.Length - 1 && canMove)
        {
            currentLane += 1;
            StartCoroutine(MoveToLane(lanes[currentLane], 1));
        }
    }

    IEnumerator MoveToLane(float laneX, float direction)
    {
        //stop player from moving until at position
        canMove = false;
        //if moving right
        if (direction > 0)
        {
            //move right until at wanted position
            while (transform.position.x < laneX)
            {
                movement.x = laneSwapSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        //moving left
        {
            //move left until at wanted position
            while (transform.position.x > laneX)
            {

                movement.x = -laneSwapSpeed;
                yield return new WaitForEndOfFrame();
            }

        }
       
        //player can move again
        canMove = true;
        movement.x = 0;
    }

    #endregion

    #region platform spawning
    IEnumerator SpawnPlatforms()
    {
        int platformNumber = 1;
        while (isAlive)
        {
            if (transform.position.z > platformSize * platformNumber)
            {
                platformManager.SpawnRandomPlatform();
                platformNumber++;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region death

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Death();
        }
    }

    void Death()
    {
        isAlive = false;
        Game.instance.PlayerDeath();
    }
    #endregion





}
