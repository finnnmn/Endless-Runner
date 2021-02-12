using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves forward
    /// </summary>
    [Header("Stats")]
    [SerializeField] float moveSpeed = 5;
    /// <summary>
    /// How fast the player moves between lanes
    /// </summary>
    [SerializeField] float laneSwapSpeed = 10;
    /// <summary>
    /// Force added to the player when they jump
    /// </summary>
    [SerializeField] float jumpForce = 30;
    /// <summary>
    /// Strength of gravity; 30 jumpforce to 90 gravity works okay
    /// </summary>
    [SerializeField] float gravity = 90;
    /// <summary>
    /// Amount to multiply by distance on z axis to get distance score
    /// </summary>
    [SerializeField] float distanceMultiplier = 0.1f;

    [Header("Mop Attack")]
    [SerializeField] float mopRadius = 5;
    [SerializeField] float mopReload = 5;
    bool mopLoaded = true;

    /// <summary>
    /// False when hitting an obstacle, determines whether the player has control
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// Movement vector added to the character controller each frame
    /// </summary>
    Vector3 movement;

    /// <summary>
    /// Reference to the main camera
    /// </summary>
    [Header("Camera")]
    [SerializeField] GameObject followCamera;
    /// <summary>
    /// When true, camera will follow the player on the x axis
    /// </summary>
    [SerializeField] bool cameraFollowPlayerLanes;
    /// <summary>
    /// When true, camera will follow the player on the y axis
    /// </summary>
    [SerializeField] bool cameraFollowPlayerJump;
    /// <summary>
    /// Stores the distance between camera and player, used for calculations
    /// </summary>
    Vector3 cameraOffset;

    /// <summary>
    /// array of the x positions for each lane
    /// </summary>
    float[] lanes;
    /// <summary>
    /// Lane the player is currently in
    /// </summary>
    int currentLane;
    /// <summary>
    /// Set to false while the player is moving between lanes to stop inputs until the lane is reached
    /// </summary>
    bool canMove = true;

    //Score values
    float distance;
    int collectibles;


    CharacterController controller;
    ScoreDisplay scoreDisplay;

    //references to data from PlatformSpawner
    PlatformSpawner platformManager;
    float platformSize;

    #region start/update
    private void Start()
    {
        //reference to character controller
        controller = GetComponent<CharacterController>();
        scoreDisplay = Game.instance.scoreDisplay;

        //set position and lane position
        lanes = Game.instance.lanes;
        currentLane = Mathf.FloorToInt(lanes.Length / 2);
        transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);

        //calculate camera offset
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
            MopInput();
        }
    }
    #endregion

    #region movement
    /// <summary>
    /// Moves the player and camera forward every frame and handles jump input
    /// </summary>
    void MovePlayer()
    {
        //allow jumping when on the ground, otherwise fall
        if (controller.isGrounded)
        {
            movement.y = 0;
            PlayerJump();
        }
        else
        {
            movement.y -= gravity * Time.deltaTime;
        }

        //set player forward movement
        movement.z = moveSpeed;

        //multiply by deltaTime and move
        Vector3 frameMovement = movement * Time.deltaTime;
        controller.Move(frameMovement);

        distance += frameMovement.z * distanceMultiplier;
        scoreDisplay.UpdateDistanceText(distance);

        //Get camera position
        Vector3 cameraPos = followCamera.transform.position;

        //set camera to player x position if toggle is on
        if (cameraFollowPlayerLanes)
            cameraPos.x = transform.position.x - cameraOffset.x;

        //set camera to player y position if toggle is on
        if (cameraFollowPlayerJump)
            cameraPos.y = transform.position.y - cameraOffset.y;

        //move camera
        followCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z + frameMovement.z);
       

    }
    #endregion

    #region jump
    /// <summary>
    /// Jump when space key is pressed
    /// </summary>
    void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movement.y = jumpForce;
        }
    }
    #endregion

    #region lane control

    /// <summary>
    /// Handles player input for moving left and right
    /// </summary>
    void LaneInput()
    {
        float hInput = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            hInput += 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            hInput -= 1;

        if (hInput > 0) 
            MoveRight();
        else if (hInput < 0)
            MoveLeft();
    }

    /// <summary>
    /// If not on leftmost lane, move player left
    /// </summary>
    void MoveLeft()
    {
        if (currentLane > 0 && canMove)
        {
            //decrease value of currentLane
            currentLane -= 1;
            //move player left
            StartCoroutine(MoveToLane(lanes[currentLane], -1));
        }
    }

    /// <summary>
    /// if not on rightmost lane, move player right
    /// </summary>
    void MoveRight()
    {
        if (currentLane < lanes.Length - 1 && canMove)
        {
            //increase value of currentLane
            currentLane += 1;
            //move player right
            StartCoroutine(MoveToLane(lanes[currentLane], 1));
        }
    }

    /// <summary>
    /// Moves player to the desired lane
    /// </summary>
    /// <param name="laneX"> The x position of the lane to move to</param>
    /// <param name="direction">1 for moving right, -1 for moving left</param>
    /// <returns></returns>
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
    /// <summary>
    /// Spawns a platform whenever the player reaches the correct position
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnPlatforms()
    {
        int platformNumber = 1;
        while (isAlive)
        {
            //if past the point to spawn a new platform
            if (transform.position.z > platformSize * platformNumber)
            {
                //spawn a random platform
                platformManager.SpawnRandomPlatform();
                //set position needed for next platform
                platformNumber++;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region mop

    void MopInput()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (mopLoaded)
            {
                StartCoroutine(MopCoroutine());
            }
        }
    }

    IEnumerator MopCoroutine()
    {
        mopLoaded = false;
        MopAttack();
        yield return new WaitForSeconds(mopReload);
        mopLoaded = true;
    }

    void MopAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, mopRadius);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Destroy(col.gameObject);
            }
        }
    }
    #endregion

    #region collectibles

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {
            Destroy(other.gameObject);
            PickUpCollectible();
        }
    }

    void PickUpCollectible()
    {
        collectibles += 1;
        scoreDisplay.UpdateCollectibleText(collectibles);
    }
    #endregion

    #region death

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //die when colliding with an obstacle
        if (hit.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Death();
        }
    }

   

    /// <summary>
    /// Stops player input and ends the game
    /// </summary>
    void Death()
    {
        isAlive = false;
        Game.instance.PlayerDeath();
    }
    #endregion

   





}
