using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Gameplay
{

    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// How fast the player moves forward while not debuffed
        /// </summary>
        [Header("Stats")]
        [SerializeField] float moveSpeed = 15;
        /// <summary>
        /// How fast the player moves forward (affected by debuffs)
        /// </summary>
        float currentSpeed;
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
        bool mopLoaded;
        float mopLoading;

        [Header("Debuffs")]
        [SerializeField] DebuffInfo slowDebuff;
        [SerializeField] DebuffInfo speedDebuff;
        [SerializeField] DebuffInfo blindDebuff;
        

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
        int buckets;


        CharacterController controller;
        HUDDisplay hudDisplay;
        Menus.Scoring scoring;

        //references to data from PlatformSpawner
        PlatformSpawner platformManager;
        float platformSize;

        #region start/update
        private void Start()
        {
            //reference to character controller
            controller = GetComponent<CharacterController>();
            hudDisplay = Game.instance.hudDisplay;
            scoring = FindObjectOfType<Menus.Scoring>();

            //set position and lane position
            lanes = Game.instance.lanes;
            currentLane = Mathf.FloorToInt(lanes.Length / 2);
            transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
            currentSpeed = moveSpeed;

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
                DebuffTimers();
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
            movement.z = currentSpeed;

            //multiply by deltaTime and move
            Vector3 frameMovement = movement * Time.deltaTime;
            controller.Move(frameMovement);

            distance += frameMovement.z * distanceMultiplier;
            hudDisplay.UpdateDistanceText(distance);

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
            if (mopLoaded)
            {
                if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    mopLoaded = false;
                    mopLoading = 0;
                    MopAttack();
                }
            }
            else
            {
                mopLoading += Time.deltaTime;
                Game.instance.hudDisplay.SetMopChargeImage(mopLoading / mopReload);

                if (mopLoading > mopReload)
                {
                    mopLoaded = true;
                }
            }

        }

        void MopAttack()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, mopRadius);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle") || col.gameObject.layer == LayerMask.NameToLayer("Debuff"))
                {
                    Destroy(col.gameObject);
                }
            }
        }
        #endregion

        #region collisions
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bucket"))
            {
                Destroy(other.gameObject);
                PickUpBucket();
            }

            else if (other.gameObject.layer == LayerMask.NameToLayer("Debuff"))
            {
                ObjectDebuffType debuffType = other.GetComponent<Debuff>().GetDebuffType();
                Destroy(other.gameObject);

                switch (debuffType)
                {
                    case ObjectDebuffType.Slow:
                        SetDebuff(slowDebuff);
                        break;
                    case ObjectDebuffType.Blind:
                        SetDebuff(blindDebuff);
                        break;
                    case ObjectDebuffType.Speed:
                        SetDebuff(speedDebuff);
                        break;
                }
            }
        }
        #endregion

        #region collectibles

        void PickUpBucket()
        {
            buckets += 1;
            hudDisplay.UpdateBucketText(buckets);
        }
        #endregion

        #region debuffs

        /// <summary>
        /// Reduce the timers on each debuff every frame
        /// </summary>
        void DebuffTimers()
        {
            CalculateDebuffTimer(slowDebuff);
            CalculateDebuffTimer(speedDebuff);
            CalculateDebuffTimer(blindDebuff);
        }

        /// <summary>
        /// Reduce the debuff timer if it is active
        /// </summary>
        /// <param name="debuff">The debuff to reduce the timer of</param>
        void CalculateDebuffTimer(DebuffInfo debuff)
        {
            if (debuff.IsActive)
            {
                debuff.Timer -= Time.deltaTime;
                UpdateDebuffUITimer(debuff.DebuffUI, debuff.Timer);
                if (debuff.Timer <= 0)
                {
                    RemoveDebuff(debuff);
                }
            }
        }

        /// <summary>
        /// Add a marker on the screen to show the debuff
        /// </summary>
        /// <param name="_name">The text for the name of the debuff</param>
        /// <param name="_time">The text for the time remaining of the debuff</param>
        /// <returns>The debuff ui marker that can be edited</returns>
        DebuffUI AddDebuffUI(string _name, float _time)
        {
            DebuffUI debuffUI = Instantiate(Game.instance.debuffPrefab, Game.instance.debuffLocation).GetComponent<DebuffUI>();
            debuffUI.SetDebuffNameText(_name);
            debuffUI.SetDebuffTimerText(_time);
            return debuffUI;
        }

        /// <summary>
        /// Updates the time text on a debuff ui marker
        /// </summary>
        /// <param name="_debuffUI">Which marker to edit</param>
        /// <param name="_time">The new amount of time to set</param>
        void UpdateDebuffUITimer(DebuffUI _debuffUI, float _time)
        {
            if (_debuffUI != null)
                _debuffUI.SetDebuffTimerText(_time);
        }

        /// <summary>
        /// Deletes a debuff ui marker from the screen
        /// </summary>
        /// <param name="_debuffUI">The marker to remove</param>
        void RemoveDebuffUI(DebuffUI _debuffUI)
        {
            if (_debuffUI != null)
                Destroy(_debuffUI.gameObject);
        }


        /// <summary>
        /// Adds a debuff to the player and sets the time and ui. If it is already on the player, resets the timer
        /// </summary>
        /// <param name="_debuff">The debuff to set</param>
        void SetDebuff(DebuffInfo _debuff)
        {
            if (!_debuff.IsActive)
            {
                _debuff.IsActive = true;
                _debuff.Timer = _debuff.time;
                _debuff.DebuffUI = AddDebuffUI(_debuff.name, _debuff.time);

                ToggleDebuffEffect(_debuff, true);
                
            }
            else
            {
                _debuff.Timer = _debuff.time;
            }
        }

        /// <summary>
        /// Removes a debuff from the player
        /// </summary>
        /// <param name="_debuff">The debuff to remove</param>
        void RemoveDebuff(DebuffInfo _debuff)
        {
            _debuff.IsActive = false;
            RemoveDebuffUI(_debuff.DebuffUI);

            ToggleDebuffEffect(_debuff, false);

        }

        /// <summary>
        /// Toggles something with the player depending on the type of debuff
        /// </summary>
        /// <param name="_debuff">The debuff effects to toggle</param>
        /// <param name="_active">Whether the debuff is turning on or off</param>
        void ToggleDebuffEffect(DebuffInfo _debuff, bool _active)
        {
            switch (_debuff.type)
            {
                case DebuffType.SpeedChange:
                    ModifySpeed(_active, _debuff.value);
                    break;
                case DebuffType.Blind:
                    SetBlindPanel(_active);
                    break;
            }
        }

        /// <summary>
        /// Change the player speed by a value
        /// </summary>
        /// <param name="_add">whether to add or subtract the speed modifier</param>
        /// <param name="_speed">value of the speed modifier</param>
        void ModifySpeed(bool _add, float _speed)
        {
            if (_add)
            {
                currentSpeed += _speed;
            }
            else
            {
                currentSpeed -= _speed;
            }
        }

        /// <summary>
        /// Shows the panel to cover the screen when the player is blinded
        /// </summary>
        /// <param name="_visible">Whether the panel is showing or hiding</param>
        void SetBlindPanel(bool _visible) => Game.instance.hudDisplay.SetBlindPanelVisibility(_visible);
        

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
            Game.instance.hudDisplay.PlayerDeath();
            scoring.ScoreDisplay(distance, buckets);
        }
        #endregion


    }
}