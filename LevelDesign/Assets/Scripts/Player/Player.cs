using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using UnityEngine.EventSystems;

namespace CombatSystem
{

    public class Player : MonoBehaviour
    {

        private Animator _playerAnimator;

        private Footsteps _footSteps;
        private PlayerSounds _playerSounds;

        public GameObject _clickMoveIcon;
        private GameObject _posClicked;
        private bool _clickMovePlaced = false;

        // Get the interface script for health
        private GameInteraction _myInterface;

        // get the PlayerPickup class
        private PlayerPickup _playerPickup;

        // move to a vector3 location
        private Vector3 _targetPosition;

        private Vector3 _oldPosition;
        private float _distanceTraveled;

        // if the player is moving
        private bool _isMoving;

        // if the player is attack
        private bool _isCharging;

        private bool _moveToNPC = false;

        private bool _earthQuakePush = false;

        private bool _inCombat = false;

        // distance between the player and the enemy
        private Vector3 _playerEnemyDistance;

        [Header("Player Attributes")]


        // public slot to adjust moving speed of the player
        public float _playerMovingSpeed = 10f;

        // health of the player
        public int _playerHealth;
        private int _maxHealth;
        private bool _regenHealth = false;

        public int _playerMana;
        private int _maxMana;
        private bool _regenMana = false;

        public float _immunityTime;
        private float _damageTimer = 0;
        private float _globalCDTimer;
        [Header("Default Ranged Attack Attributes")]
        // slot for the spell
        public GameObject _pRangedSpell;

        // minimum range to cast a spell
        public float _minRangedAttackRange;

        // how fast can the player attack in seconds
        public float _playerAttackRate;

        // how fast is the spell moving
        public float _playerSpellVelocity;

        // how much damage does the spell do
        public float _playerRangedDamage;

        public float _playerRangedManaCost;

        [Header("Melee Attack Attributes")]
        // melee range in units
        public float _meleeRange;

        public float _meleeDamage;

        [Header("Player Auto Attack?")]
        // do we want the player to auto attack?
        public bool _autoAttack = false;

        // the object we have selected
        private GameObject _selectedTarget;

        private bool _inRangeRangedAttack = false;
        private bool _inRangeMeleeAttack = false;
        private float _coolDown = 0f;

        [Header("Shadow Bolt Attributes")]

        // The attack rate of the shadowBolt
        public float _shadowBolt_AttackRate;

        // create a timer for the shadowbolt and put it to 0f
        private float _shadowBolt_coolDown = 0f;

        // the gameobject to instantiate
        public GameObject _shadowBolt;
        //how much damage does it do
        public int _shadowBoltDamage;
        // how much mana does it cost
        public int _shadowBoltManaCost;

        // how fast is it moving
        public float _shadowBoltVelocity;


        // disengage spell throws the player back
        [Header("Disengage Spell")]
        public float _disenageCooldown;

        private bool _isAttacking;

        private bool _takenDamage;

        [Header("GameObjects")]

        [SerializeField]
        private playerSpellCast _playerSpell;
        [SerializeField]
        private playerMelee _playerMelee;

        [SerializeField]
        private Transform _playerWaypoint;
        private Vector3 _earthQuakeTarget;

        private bool _playerHooked;
        private GameObject _hookPos;


        // are we disengaging
        private bool _isDisengage;

        // the target the player will move to
        private Vector3 _disengageTarget;

        // create a timer
        private float _disengageTimer;


        private bool _draggingUI = false;
        private bool _hoveringUI = false;

        // WE NEED TO USE A CHARACTER CONTROLLER FOR COLLISSIONS
        private CharacterController _playerMovement;

        // Use this for initialization
        void Start()
        {

            // set the target position to our current
            _targetPosition = transform.position;
            _isMoving = false;                                                                                  // we are not moving by default
            _isCharging = false;                                                                                // we are not charging by default
            _coolDown = _playerAttackRate;                                                                      // set the default cooldown to the player attack rate so we can attack straight away
            _myInterface = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameInteraction>();   // get the interface class
           // _myInterface.DisplayPlayerInCombat(false, _playerHealth);

            //_playerPickup = GameObject.FindGameObjectWithTag ("PlayerPickup").GetComponent<PlayerPickup> ();    // get the pickup class

            _playerHooked = false;                                                                              // we are not hooked by default
            _isDisengage = false;                                                                               // we are not disengaging by default

            _globalCDTimer = 0;                                                                                 // global cooldown timer

            _disengageTimer = _disenageCooldown;                                                                // set the disengage timer to the disengagecooldown

            _playerMovement = GetComponent<CharacterController>();                                              // get the character controller component

            _playerAnimator = GetComponentInChildren<Animator>();

            _footSteps = GetComponentInChildren<Footsteps>();

            _playerSounds = GetComponent<PlayerSounds>();

            _maxHealth = _playerHealth;
            _maxMana = _playerMana;



        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!_draggingUI)
            {
                CheckGround();
                if (!_hoveringUI)
                {
                    // if the left mousebutton is pressed
                    if (Input.GetMouseButton(0))
                    {

                        //set the position to move to
                        SetTargetPosition();



                    }

                    // if right mousebutton is pressed
                    if (Input.GetMouseButton(1))
                    {

                        //We initialize the attacking
                        PlayerAttack();

                    }
                }
            }

            if (!_inCombat)
            {
                Regeneration();
            }

            if (_isMoving)
            {

                // if the bool _isMoving is true then move the player to the target position
                MovePlayer(_targetPosition);

            }

            // if the player is 'charging' towards the enemy - move to the enemy
            if (_isCharging)
            {

                MoveToAttack(_selectedTarget);

            }

            // if the earthquake push bool is true
            if (_earthQuakePush)
            {

                // push the player back
                PushBack();

            }

            if (_moveToNPC)
            {
                MoveToNPC(_selectedTarget);
            }

            // if we are attacking
            if (_isAttacking)
            {

                if (Mathf.RoundToInt(_coolDown) == _playerAttackRate)
                {
                    _playerSpell.PlayerSpellCast(_selectedTarget, transform.position, _pRangedSpell, _playerRangedDamage, 1, _playerSpellVelocity);
                    _playerAnimator.SetBool("isRunning", false);
                    _playerAnimator.SetBool("isRanged", true);
                    _playerAnimator.SetBool("isRanged", false);
                    _playerAnimator.SetBool("skipIdle", true);
                    _playerAnimator.SetBool("isCombatIdle", true);

                    _coolDown = 0f;
                    // Debug.Log("Attacking!");
                }
                if (!_autoAttack)
                {

                    _isAttacking = false;
                    // Debug.Log("Attacking!");

                    _playerAnimator.SetBool("isRunning", false);
                    _playerAnimator.SetBool("isRanged", true);
                    _playerAnimator.SetBool("isRanged", false);
                    _playerAnimator.SetBool("skipIdle", true);
                    _playerAnimator.SetBool("isCombatIdle", true);

                }

            }

            // If we press the '2' button OR _autoAttack is true
            if (Input.GetKeyDown("2") || _autoAttack)
            {

                // if we are in melee range
                if (_inRangeMeleeAttack && !_inRangeRangedAttack)
                {

                    if (!_isMoving && !_isCharging)
                    {
                        if (Mathf.RoundToInt(_coolDown) == _playerAttackRate)
                        {
                            _playerMelee.DoMeleeAttack(_meleeDamage);
                            _myInterface.SetGlobalCooldown();
                            _globalCDTimer = 0;
                            _myInterface.SetSpellCooldown(2, _playerAttackRate);
                        }
                    }

                }

            }

            // ONLY REGISTER AN ATTACK IF THE GLOBAL COOLDOWN HAS FINISHED

            if (Math.Round(_globalCDTimer, 1) == _myInterface._globalCooldown)
            {

                // If we press the '1' button OR _autoAttack is true
                if (Input.GetKeyDown("1"))
                //if (Input.GetKeyDown("1") || _autoAttack)
                {

                    // if we are in range for a ranged attack
                    if (_inRangeRangedAttack)
                    {

                        // if we are not moving OR not charging
                        if (!_isMoving && !_isCharging)
                        {

                            _playerSounds.PlayerSpellCast();
                            _isAttacking = true;
                            _myInterface.SetGlobalCooldown();
                            _globalCDTimer = 0;
                            _playerAnimator.SetBool("isRanged", true);
                            _myInterface.SetSpellCooldown(1, _playerAttackRate);

                        }
                    }





                }

                if (Input.GetKeyDown("3"))
                {

                    if (_inRangeRangedAttack)
                    {
                        if (!_isMoving && !_isCharging)
                        {
                            if (Mathf.RoundToInt(_shadowBolt_coolDown) == _shadowBolt_AttackRate)
                            {


                                _playerSounds.PlayerSpellCast();
                                _playerSpell.PlayerSpellCast(_selectedTarget, this.transform.position, _shadowBolt, _shadowBoltDamage, 3, _shadowBoltVelocity);
                                _shadowBolt_coolDown = 0f;
                                _playerMana -= _shadowBoltManaCost;
                                GameInteraction.SetPlayerMana(_playerMana);
                                _myInterface.SetGlobalCooldown();
                                _globalCDTimer = 0;

                                _myInterface.SetSpellCooldown(3, _shadowBolt_AttackRate);
                            }
                        }

                    }

                }

                if (Input.GetKeyDown("4"))
                {
                    if (Mathf.RoundToInt(_disengageTimer) == _disenageCooldown)
                    {
                        _isDisengage = true;
                        _disengageTarget = _playerWaypoint.position;

                        _disengageTimer = 0;

                        _myInterface.SetGlobalCooldown();
                        _globalCDTimer = 0;

                        _myInterface.SetSpellCooldown(4, _disenageCooldown);
                    }

                }
            }

            if (_isDisengage)
            {

                DisEngage();
            }

            // Always perform the attack cooldown
            attackCooldown();
            ImmunityTimer();

            // if the health is 0, then kill the player
            if ((int)_playerHealth == 0)
            {

                PlayerDeath();
            }

            if (_playerHooked)
            {

                transform.position = _hookPos.transform.position;

            }

            if (_globalCDTimer < _myInterface._globalCooldown)
            {

                _globalCDTimer += Time.deltaTime;

            }

            if (_disengageTimer < _disenageCooldown)
            {
                _disengageTimer += Time.deltaTime;
            }

        }


        // a global cooldown for taking damage
        void ImmunityTimer()
        {

            if ((int)_damageTimer < _immunityTime)
            {
                _damageTimer += Time.deltaTime;

            }
            if ((int)_damageTimer == _immunityTime)
            {

                _takenDamage = false;
                _damageTimer = 0;

            }

        }

        // set the position to move to
        void SetTargetPosition()
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {


                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float _point = 0f;

                RaycastHit _hit;

                if (Physics.Raycast(_ray, out _hit))
                {

                    _targetPosition = _hit.point;

                }

                _isMoving = true;
                _isAttacking = false;
                _isCharging = false;

                AddLocationMarker();
            }

        }

        // The moving of the player
        void MovePlayer(Vector3 _target)
        {

            if (_target != null)
            {


                // since we are moving the player, the enemy should no longer 'charge'
                _isCharging = false;

                _oldPosition = transform.position;
                Vector3 _dir = _target - transform.position;                                               // get the Vector we are going to move to
                _dir.y = 0f;                                                                                // we dont want to move up
                Quaternion _targetRot = Quaternion.LookRotation(_dir);                                      // get the rotation in which we should look at

                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.deltaTime * 4);  // rotate the player

                Vector3 _forward = transform.TransformDirection(Vector3.forward);                           // create a forward Vector3

                if (_dir.magnitude > 0.5f)
                {                                                                // if the magnitude of the vector is greater than
                    _playerMovement.SimpleMove(_forward * _playerMovingSpeed);                                // move the actual player

                    //////////////////////////////////////////////////////////////////////
                    //                          footstep sounds                         //
                    //////////////////////////////////////////////////////////////////////

                    _distanceTraveled += (transform.position - _oldPosition).magnitude;

                    // Check if the distance traveled is greater than 2
                    // In this case 2f is the distance between each step! ( FOR RUNNING )

                    if (_distanceTraveled >= 2f)
                    {
                        // Call the PlayFootstepSound in the Footstep Class
                        _footSteps.PlayFootstepSound();
                        _distanceTraveled = 0.0f;
                    }

                    // Animation bool
                    if (!_playerAnimator.GetBool("isRunning"))
                    {
                        _playerAnimator.SetBool("isRunning", true);
                    }

                }
                else
                {
                    _isMoving = false;
                    _playerAnimator.SetBool("isRunning", false);
                }

                if (transform.position == _target)                                                  // if we have reached our destination
                {
                    _isMoving = false;                                                              // stop moving
                    _playerAnimator.SetBool("isRunning", false);
                }

            }



        }


        // Default ontrigger
        void OnTriggerEnter(Collider coll)
        {

            if (coll.tag == "AggroRange")
            {
                // nothing
            }

            // If we are hit by an enemyspellattack
            if (coll.name == "enemySpellAttack")
            {

                // get the amount of damage done by the spell from the spell it self
                float _dmgDealt = coll.gameObject.GetComponent<spellCast>().ReturnDamage();
                // update the player health
                PlayerTakeDamage((int)_dmgDealt);

                // set the interface health
                GameInteraction.SetPlayerHealth(_playerHealth);
                // Destroy the spell gameobject
                Destroy(coll.gameObject);


            }


            if (coll.name == "EnemyMeleeWeapon_TRIGGER")
            {


                float _dmgDealt = coll.transform.parent.parent.parent.GetComponent<EnemyMelee>().DefaultDamage();
                // update the player health
                PlayerTakeDamage((int)_dmgDealt);
                // set the interface health
                GameInteraction.SetPlayerHealth(_playerHealth);

            }

            if (coll.tag == "PlayerPickup")
            {

                //get the type of pickup from the PlayerPickup class
                switch (_playerPickup.GetPickup())
                {

                    case "healthPickup":

                        _playerHealth += _playerPickup.GetHealth();

                        GameInteraction.SetPlayerHealth(_playerHealth);

                        _playerPickup.DestroyPickup();

                        break;

                    case "manaPickup":

                        _playerPickup.GetMana();

                        _playerPickup.DestroyPickup();

                        break;

                }

            }

            if (coll.tag == "AOE_HellBlast" && !_takenDamage)
            {
                _takenDamage = true;

                PlayerTakeDamage((int)_selectedTarget.GetComponent<EnemyRanged>().ReturnAOEDamage());
                GameInteraction.SetPlayerHealth(_playerHealth);


            }

            if (coll.tag == "AOE_EarthQuake" && !_takenDamage)
            {

                _earthQuakePush = true;
                _earthQuakeTarget = _playerWaypoint.transform.position;

                if (_selectedTarget == null)
                {
                    _selectedTarget = coll.transform.parent.parent.gameObject;
                }

                PlayerTakeDamage((int)_selectedTarget.GetComponent<EnemyRanged>().ReturnAOEDamage());
                GameInteraction.SetPlayerHealth(_playerHealth);

            }

            if (coll.tag == "EnemyMeleeHook")
            {

                _isMoving = false;
                _playerHooked = true;
                _hookPos = coll.gameObject;
                transform.LookAt(coll.transform);

            }

            if (coll.tag == "EnemyMelee")
            {
                _isMoving = false;
            }
        }


        void DisEngage()
        {

            if (Vector3.Distance(transform.position, _disengageTarget) > float.Epsilon)
            {

                transform.position = Vector3.MoveTowards(transform.position, _disengageTarget, Time.deltaTime * 25);

            }
            if (Vector3.Distance(transform.position, _disengageTarget) < 1f)
            {
                _isDisengage = false;
                _isMoving = false;
            }

            MoveToAttack(_selectedTarget);

        }


        void PushBack()
        {

            _isCharging = false;
            _isMoving = true;
            _inRangeMeleeAttack = false;
            _inRangeRangedAttack = true;

            transform.position = Vector3.Lerp(transform.position, _earthQuakeTarget, 2 * Time.deltaTime);
            _takenDamage = true;

            if ((int)Vector3.Distance(transform.position, _earthQuakeTarget) == 3)
            {

                _earthQuakePush = false;
                _isMoving = false;
                _isCharging = true;
            }

        }

        void PlayerAttack()
        {


            //Plane _plane = new Plane (Vector3.up, transform.position);
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {

                if (_hit.collider.gameObject.tag == "EnemyRanged" || _hit.collider.gameObject.tag == "EnemyMelee")
                {

                    //_targetPosition = _hit.collider.GetComponent<EnemyRanged> ().ReturnPosition();
                    _selectedTarget = _hit.collider.gameObject;
                    _isCharging = true;
                    _isMoving = false;
                    _myInterface.SetSelected(_selectedTarget);
                    //Debug.Log(_selectedTarget);

                }
                if (_hit.collider.gameObject.tag == "NPC")
                {
                    _hit.collider.GetComponentInChildren<NPC>().IsSelected();
                    _selectedTarget = _hit.collider.gameObject;
                    _myInterface.SetSelected(_selectedTarget);
                    _isMoving = false;
                    _isCharging = false;
                    _moveToNPC = true;


                }

            }

        }

        void MoveToNPC(GameObject _selectedNPC)
        {
            Vector3 _targetPos;

            if (_selectedNPC != null)
            {
                _targetPos = _selectedNPC.transform.position;
            }
            else
            {
                _targetPos = transform.position;
            }


            _oldPosition = transform.position;

            Vector3 _dir = _targetPos - transform.position;
            _dir.y = 0;
            Quaternion _targetRot = Quaternion.LookRotation(_dir);

            if (Vector3.Distance(transform.position, _targetPos) > _meleeRange)
            {

                // if the player is not in range for a ranged attack, move to enemy
                //transform.position = Vector3.MoveTowards (transform.position, _targetPos, _playerMovingSpeed * Time.deltaTime);

                transform.rotation = _targetRot;

                Vector3 _forward = transform.TransformDirection(Vector3.forward);

                if (_dir.magnitude > 0.5f)
                {
                    _playerMovement.SimpleMove(_forward * _playerMovingSpeed);


                    // Animation bool
                    if (!_playerAnimator.GetBool("isRunning"))
                    {
                        _playerAnimator.SetBool("isRunning", true);
                    }


                    //////////////////////////////////////////////////////////////////////
                    //                          footstep sounds                         //
                    //////////////////////////////////////////////////////////////////////

                    _distanceTraveled += (transform.position - _oldPosition).magnitude;

                    // Check if the distance traveled is greater than 2
                    // In this case 2f is the distance between each step! ( FOR RUNNING )

                    if (_distanceTraveled >= 2f)
                    {
                        // Call the PlayFootstepSound in the Footstep Class
                        Debug.Log("Firing footsteps");
                        _footSteps.PlayFootstepSound();
                        _distanceTraveled = 0.0f;
                    }

                }




            }

            if (Mathf.RoundToInt(Vector3.Distance(transform.position, _targetPos)) <= _meleeRange)
            {

                // if the player is in melee range - stop moving the player
                _isCharging = false;
                _inRangeMeleeAttack = true;
                _inRangeRangedAttack = false;
                _isAttacking = false;
                _moveToNPC = false;


                // Animation bool
                if (_playerAnimator.GetBool("isRunning"))
                {
                    _playerAnimator.SetBool("isRunning", false);
                }

            }
        }


        void MoveToAttack(GameObject _nwSelectedTarget)
        {


            Vector3 _targetPos;

            if (_nwSelectedTarget != null)
            {
                _targetPos = _nwSelectedTarget.transform.position;
            }
            else
            {
                _targetPos = transform.position;
            }


            _oldPosition = transform.position;

            Vector3 _dir = _targetPos - transform.position;
            _dir.y = 0;
            Quaternion _targetRot = Quaternion.LookRotation(_dir);

            if (Vector3.Distance(transform.position, _targetPos) > _minRangedAttackRange)
            {

                // if the player is not in range for a ranged attack, move to enemy
                //transform.position = Vector3.MoveTowards (transform.position, _targetPos, _playerMovingSpeed * Time.deltaTime);

                transform.rotation = _targetRot;

                Vector3 _forward = transform.TransformDirection(Vector3.forward);

                if (_dir.magnitude > 0.5f)
                {
                    _playerMovement.SimpleMove(_forward * _playerMovingSpeed);


                    // Animation bool
                    if (!_playerAnimator.GetBool("isRunning"))
                    {
                        _playerAnimator.SetBool("isRunning", true);
                    }


                    //////////////////////////////////////////////////////////////////////
                    //                          footstep sounds                         //
                    //////////////////////////////////////////////////////////////////////

                    _distanceTraveled += (transform.position - _oldPosition).magnitude;

                    // Check if the distance traveled is greater than 2
                    // In this case 2f is the distance between each step! ( FOR RUNNING )

                    if (_distanceTraveled >= 2f)
                    {
                        // Call the PlayFootstepSound in the Footstep Class
                        Debug.Log("Firing footsteps");
                        _footSteps.PlayFootstepSound();
                        _distanceTraveled = 0.0f;
                    }

                }


                if (Mathf.RoundToInt(Vector3.Distance(transform.position, _targetPos)) == _minRangedAttackRange)
                {

                    // if the player is in range for a ranged attack set the bool _inRangedRangedAttack to true and turn off melee
                    _inRangeRangedAttack = true;
                    _inRangeMeleeAttack = false;
                    _isCharging = false;
                    _isAttacking = true;

                    // Animation bool
                    if (_playerAnimator.GetBool("isRunning"))
                    {
                        _playerAnimator.SetBool("isRunning", false);
                    }

                }

            }

            if (Mathf.RoundToInt(Vector3.Distance(transform.position, _targetPos)) <= _meleeRange)
            {

                // if the player is in melee range - stop moving the player
                _isCharging = false;
                _inRangeMeleeAttack = true;
                _inRangeRangedAttack = false;
                _isAttacking = true;

            }
        }



        void attackCooldown()
        {

            if (_coolDown < _playerAttackRate)
            {
                _coolDown += Time.deltaTime;
            }

            if (_shadowBolt_coolDown < _shadowBolt_AttackRate)
            {

                _shadowBolt_coolDown += Time.deltaTime;
            }

        }

        void PlayerDeath()
        {

            Debug.Log("YOU DEAD BRO");

        }

        public void HookPlayer(GameObject _target)
        {

            transform.position = _target.transform.position;

        }

        public void UnHook(GameObject _target)
        {
            _playerHooked = false;
            Debug.Log("Unhooked");
            MoveToAttack(_target);
            _selectedTarget = _target;
            _myInterface.SetSelected(_target);


        }


        public void SetPlayerIdle()
        {
            _playerAnimator.SetBool("isRunning", false);
            _playerAnimator.SetBool("isWalking", false);
            _playerAnimator.SetBool("isCombatIdle", false);
            _playerAnimator.SetBool("skipIdle", false);

            Debug.Log("Player: Player has been set to Idle");
        }


        void AddLocationMarker()
        {
            if (_posClicked != null)
            {
                Destroy(_posClicked);
                _posClicked = (GameObject)Instantiate(_clickMoveIcon, new Vector3(_targetPosition.x, _targetPosition.y + 0.1f, _targetPosition.z), Quaternion.identity);
                Debug.Log("Added a Marker");

            }
            else
            {
                _posClicked = (GameObject)Instantiate(_clickMoveIcon, new Vector3(_targetPosition.x, _targetPosition.y + 0.1f, _targetPosition.z), Quaternion.identity);
                Debug.Log("Added a Marker");
            }


        }

        public void CombatEngage(GameObject _target)
        {
            _playerAnimator.SetBool("isCombatIdle", true);
            _myInterface.SetSelected(_target);

        }

        public void PlayerInCombat(bool _combat)
        {
            _inCombat = _combat;
          //  _myInterface.PlayerInCombat(_combat, _playerHealth);
        }

        void CheckGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "DamageField")
                {
                    if (!_takenDamage)
                    {
                        PlayerTakeDamage(5);
                        GameInteraction.SetPlayerHealth(_playerHealth);
                        _takenDamage = true;
                        PlayerInCombat(true);
                    }
                }
                else
                {
                    PlayerInCombat(false);
                }
            }
        }

        void PlayerTakeDamage(int _damageAmount)
        {
            _playerHealth -= _damageAmount;
            _playerSounds.PlayerGrunt();
        }

        public void AddPlayerHealth(int _healingAmount)
        {
            _playerHealth += _healingAmount;
            // Player sound
            GameInteraction.SetPlayerHealth(_playerHealth);
        }

        public void AddPlayerMana(int _manaAmount)
        {

        }

        public void SetDraggingUI(bool _dragging)
        {
            _draggingUI = _dragging;
        }

        public void HoveringOverUI(bool _hovering)
        {
            _hoveringUI = _hovering;
        }

        void Regeneration()
        {
            if (_playerHealth < _maxHealth && !_regenHealth)
            {
                InvokeRepeating("RegenerateHealth", 2.5f, 2.5f);
                _regenHealth = true;
            }
            if (_playerHealth >= _maxHealth)
            {
                CancelInvoke("RegenerateHealth");
            }

            if (_playerMana < _maxMana && !_regenMana)
            {
                InvokeRepeating("RegenerateMana", 2.0f, 2.0f);
                _regenMana = true;
            }
            if (_playerMana >= _maxMana)
            {
                CancelInvoke("RegenerateMana");
            }
        }

        void RegenerateHealth()
        {
            _playerHealth += (_maxHealth / 100);
            GameInteraction.SetPlayerHealth(_playerHealth);
            _regenHealth = false;
        }

        void RegenerateMana()
        {
            _playerMana += _maxMana / 100;
            GameInteraction.SetPlayerMana(_playerMana);
            _regenMana = false;
        }

        public void SetTarget(GameObject _target)
        {
            _selectedTarget = _target;
            _myInterface.SetSelected(_target);
        }

    }
}