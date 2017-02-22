using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.Data;
using System;

namespace CombatSystem
{

    public class PlayerMovement : MonoBehaviour
    {

        private Animator _playerAnimator;

        private Vector3 _targetPosition;
        private GameObject _targetObject;
        private Vector3 _oldPosition;

        private float _playerMoveSpeed = 5.0f;
        private float _distanceTraveled;
        private float _rangedAttackDistance = 5.0f;
        private float _meleeAttackDistance = 2.0f;

        private static bool _isMoving = false;
        private bool _moveToAttack = false;
        private bool _moveToNPC = false;
        private static bool _isKnockedBack = false;

        private GameObject _selectedActor;
        private GameObject _posClicked;
        private GameObject _clickMoveIcon;

        private static bool _hovingOverUI = false;
        private static bool _draggingUI = false;

        private static int _playerHealth = 100;
        private static int _playerMana = 1000;

        private CharacterController _charController;
        private static DialogueManager _DM;

        private static bool _castSpell = false;
        private static bool _mayCastSpell = false;
        private static bool _castSpellOnce = false;

        private static bool _takenDamage = false;
        private static float _immunity = 2.0f;
        private static float _damageTimer = 0.0f;

        private static bool _mayCastAOE = false;
        private static bool _SetAOE = false;
        private static GameObject _aoeTarget;

        private static float _timer = 0f;
        private static float _spellTimer;

        private static float _disengageDistance;
        private static bool _setDisengageTarget;
        private Vector3 _disengageTarget;




        void OnEnable()
        {
            _charController = GetComponent<CharacterController>();

            _playerAnimator = GetComponentInChildren<Animator>();

            CombatSystem.AnimationSystem.SetController(_playerAnimator);

            _DM = GameObject.FindObjectOfType<DialogueManager>().GetComponent<DialogueManager>();
            GameInteraction.DisplayCastBar(false);

            _clickMoveIcon = Resources.Load("Icons/ClickedToMoveTo") as GameObject;
            

        }

        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            #region SetLocations

            // If we are not dragging the UI - to prevent setting a movement target while dragging
            if (!_draggingUI)
            {
                // Same as DraggingUI - to prevent weird stuff
                if (!_hovingOverUI)
                {
                    if (Input.GetMouseButtonDown(0) && !_hovingOverUI && !_draggingUI)
                    {
                        SetTargetPosition();

                        // If the Player is not in combat we set the SetSelectedUI to nothing 
                        // this removes the Selected HUD
                        if (!CombatSystem.Combat.ReturnInCombat())
                        {
                            CombatSystem.GameInteraction.SetSelectedUI(null);
                        }

                        _moveToAttack = false;
                        AddLocationMarker();
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        _isMoving = false;
                        CheckActor();
                        CombatSystem.GameInteraction.SetSelectedUI(_selectedActor);
                     
                    }
                }
            }
            #endregion


            #region MovementBooleans

            if (_isMoving)
            {
                MoveToPosition(_targetPosition);
                if (!AnimationSystem.ReturnRunningAnim())
                {    
                    AnimationSystem.SetPlayerRunning();
                }
            }
                 
            if(!_isMoving && !_moveToAttack)
            {
                AnimationSystem.StopPlayerRunning();
            }
                  
            if (_moveToAttack)
            {
                _targetPosition = _targetObject.transform.position;
                MoveToEnemy(_targetPosition);

                if (!AnimationSystem.ReturnRunningAnim())
                {
                    AnimationSystem.StopCombatIdle();

                    AnimationSystem.SetPlayerRunning();
                }
            }
                        
            #endregion

            #region SpellCast
            if (_castSpell && !_moveToAttack)
            {
                GameInteraction.DisplayCastBar(true);
                if (_timer < _spellTimer)
                {
                    _mayCastSpell = false;
                    _timer += Time.deltaTime;

                    GameInteraction.FillCastBar(_timer / _spellTimer);
                    if (!_castSpellOnce)
                    {
                        AnimationSystem.StopCombatIdle();
                        AnimationSystem.SetSkipIdle(true);
                        AnimationSystem.SetRangedSpell(_spellTimer);

                        _castSpellOnce = true;

                    }

                }
                if (_timer >= _spellTimer)
                {
                    _mayCastSpell = true;
                    _timer = 0f;
                }

                // If the player may cast a spell
                if (_mayCastSpell && _castSpell)
                {

                    // Cast the actual Spell
                    Combat.CastSpell(this.transform.position);

                    SoundSystem.PlaySpellCast(this.transform.position);

                    // Call the GameInteraction Class - SpellHasBeenCast()
                    // This is to trigger the COOlDOWN
                    GameInteraction.SpellHasBeenCast();

                    // Set bool _castSpell to false
                    _castSpell = false;

                    // Turn off the DisplayCastBar HUD
                    GameInteraction.DisplayCastBar(false);
                }
            }

            //////////////////////////////////////////////////////////////////////
            //                  CANCEL ONE TIME ANIMATIONS                      //
            //////////////////////////////////////////////////////////////////////

            if (!_castSpell)
            {
                if (AnimationSystem.RangedSpellFinished())
                {
                    AnimationSystem.StopRangedSpell();
                    AnimationSystem.SetSkipIdle(true);
                    AnimationSystem.SetCombatIdle();
                    _castSpellOnce = false;
                }
            }

            if (_castSpell && _moveToAttack)
            {
                _DM.ShowMessage("You can not cast while moving", true);
            }
            #endregion

            if (_isKnockedBack)
            {
                PlayerThrow();
                
            }

            if(_mayCastAOE)
            {
                SetAoeTarget(transform.position);
            }

            ImmunityTimer();
            CheckGround(transform.position);

            //  RegenerateHealth();
            //  RegenerateMana();

            CheckMouseOver();

        }

        #region Movement
        void SetTargetPosition()
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                _targetPosition = _hit.point;
                _isMoving = true;
            }
        }

        void MoveToPosition(Vector3 _targetPos)
        {
            if (_targetPos != null)
            {

                _oldPosition = transform.position;
                Vector3 _dir = _targetPos - transform.position;                                               // get the Vector we are going to move to
                _dir.y = 0f;                                                                                // we dont want to move up
                Quaternion _targetRot = Quaternion.LookRotation(_dir);                                      // get the rotation in which we should look at

                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.deltaTime * 4);  // rotate the player

                Vector3 _forward = transform.TransformDirection(Vector3.forward);                           // create a forward Vector3

                if (_dir.magnitude > 0.5f)
                {                                                                                            // if the magnitude of the vector is greater than
                    _charController.SimpleMove(_forward * _playerMoveSpeed);                                // move the actual player

                    //////////////////////////////////////////////////////////////////////
                    //                          footstep sounds                         //
                    //////////////////////////////////////////////////////////////////////

                    _distanceTraveled += (transform.position - _oldPosition).magnitude;

                    // Check if the distance traveled is greater than 2
                    // In this case 2f is the distance between each step! ( FOR RUNNING )

                    if (_distanceTraveled >= 2f)
                    {
                        // Call the PlayFootstepSound in the SoundSystem Class
                        SoundSystem.PlayFootSteps(this.transform.position);
                        _distanceTraveled = 0.0f;
                    }
               
                   
                    

                }
                else
                {
                    _isMoving = false;
                    
                }

                if (transform.position == _targetPos)                                                  // if we have reached our destination
                {
                    _isMoving = false;                                                              // stop moving
                }

            }
        }

        void MoveToEnemy(Vector3 _targetPos)
        {
            if (_targetPos != null)
            {

                _oldPosition = transform.position;
                Vector3 _dir = _targetPos - transform.position;                                               // get the Vector we are going to move to
                _dir.y = 0f;                                                                                // we dont want to move up
                Quaternion _targetRot = Quaternion.LookRotation(_dir);                                      // get the rotation in which we should look at

                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.deltaTime * 4);  // rotate the player

                Vector3 _forward = transform.TransformDirection(Vector3.forward);                           // create a forward Vector3

                if (Vector3.Distance(transform.position, _targetPos) > _rangedAttackDistance)
                {                                                                                            // if the magnitude of the vector is greater than
                    _charController.SimpleMove(_forward * _playerMoveSpeed);                                // move the actual player

                    //////////////////////////////////////////////////////////////////////
                    //                          footstep sounds                         //
                    //////////////////////////////////////////////////////////////////////

                    _distanceTraveled += (transform.position - _oldPosition).magnitude;

                    // Check if the distance traveled is greater than 2
                    // In this case 2f is the distance between each step! ( FOR RUNNING )

                    if (_distanceTraveled >= 2f)
                    {
                        // Call the PlayFootstepSound in the SoundSystem Class
                        SoundSystem.PlayFootSteps(this.transform.position);
                        _distanceTraveled = 0.0f;
                    }
                }
                else
                {
                    CombatSystem.Combat.InitiateCombat();
                    _moveToAttack = false;
                }

            }
        }

        #endregion

        void CheckActor()
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {

                if (_hit.collider.gameObject.tag == "NPC")
                {
                    _targetPosition = _hit.point;

                    _isMoving = true;
                }
                if (_hit.collider.gameObject.tag == "EnemyRanged" || _hit.collider.gameObject.tag == "EnemyMelee")
                {

                    _targetPosition = _hit.point;
                    _moveToAttack = true;

                    _targetObject = _hit.collider.gameObject;

                }
                _selectedActor = _hit.collider.gameObject;
            }
        }

        #region Set UI Booleans
        public static void HoveringOverUI(bool _set)
        {
            _hovingOverUI = _set;
        }

        public static void SetDraggingUI(bool _set)
        {
            _draggingUI = _set;
        }
        #endregion

        public void PlayerInCombat(bool _set)
        {
            if (_set)
            {
                CombatSystem.Combat.InitiateCombat();
                CombatSystem.GameInteraction.DisplayPlayerInCombat(true, _playerHealth);
            }

            else
            {

            }
        }

        #region RETURN HEALTH AND MANA
        public static int ReturnPlayerHealth()
        {
            return _playerHealth;
        }

        public static int ReturnPlayerMana()
        {
            return _playerMana;
        }
        #endregion

        #region COMBAT

        public static void SetCastSpell(bool _set)
        {
            _castSpell = _set;
        }

        public static void CastSpell(float _casttime, GameObject _target, float _manaCost)
        {
            if (_target != null)
            {
                if (_target.tag == "EnemyRanged" || _target.tag == "EnemyMelee")
                {
                    if (_playerMana - _manaCost > 0)
                    {
                        _spellTimer = _casttime;
                        _timer = 0f;
                        _castSpell = true;

                        _playerMana -= (int)_manaCost;

                        GameInteraction.SetPlayerMana(_playerMana);
                    }
                    else
                    {
                        _castSpell = false;
                        _DM.ShowMessage("Not enough mana", true);
                    }
                }
            }
        }

        public static void CastAOE(float _casttime, float _manaCost)
        {
            if (_SetAOE)
            {
                if (_playerMana - _manaCost > 0)
                {
                    _mayCastAOE = true;
                    _playerMana -= (int)_manaCost;
                    GameInteraction.SetPlayerMana(_playerMana);
                    _aoeTarget = Instantiate(Resources.Load("Icons/AOE") as GameObject);
                }
                else
                {
                    _mayCastAOE = false;
                    _DM.ShowMessage("Not enough mana", true);
                }
            }
        }

        public static bool ReturnCastSpell()
        {
            return _castSpell;
        }

        public static bool ReturnCastAOE()
        {
            return _mayCastAOE;
        }


        public static void SetAoeTarget(Vector3 _playerPos)
        {
            if (_SetAOE)
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit _hit;

                if (Physics.Raycast(_ray, out _hit))
                {
                    if (Vector3.Distance(_playerPos, _hit.point) < 10f)
                    {
                        _aoeTarget.GetComponent<SpriteRenderer>().color = Color.green;
                        _aoeTarget.transform.position = new Vector3(_hit.point.x, _hit.point.y + 1, _hit.point.z);

                        if (Input.GetMouseButtonDown(0))
                        {
                            _isMoving = false;
                            Combat.CastAOE(_hit.point);
                        }
                    }
                    else
                    {
                        _aoeTarget.transform.position = new Vector3(_hit.point.x, _hit.point.y + 1, _hit.point.z);
                        _aoeTarget.GetComponent<SpriteRenderer>().color = Color.red;
                        if (Input.GetMouseButtonDown(0))
                        {
                            _isMoving = false;
                            _DM.ShowMessage("Out of range", true);
                        }
                    }
                }
            }
        }

        public static void ToggleAoE()
        {
            _SetAOE = !_SetAOE;

            if (!_SetAOE)
            {
                Destroy(_aoeTarget.gameObject);
            }
        }

        #endregion

        public void SetPlayerIdle()
        {
            _playerAnimator.SetBool("isRunning", false);
            _playerAnimator.SetBool("isWalking", false);
            _playerAnimator.SetBool("isCombatIdle", false);
            _playerAnimator.SetBool("skipIdle", false);

            Debug.Log("Player: Player has been set to Idle");
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.GetComponent<SpellObject>() != null)
            {
                if (!coll.GetComponent<SpellObject>().ReturnFromPlayer())
                {
                    if (!_takenDamage)
                    {
                        _playerHealth -= (int)coll.GetComponent<SpellObject>().ReturnDamage();
                        GameInteraction.SetPlayerHealth(_playerHealth);
                        Destroy(coll.gameObject);
                        SoundSystem.PlayerHit(this.transform.position);

                        // Set _takeDamage to true so the player is immune for <seconds>
                        _takenDamage = true;

                    }
                    
                }

            }

            if(coll.tag == "AOE_EarthQuake")
            {
                _isMoving = false;
                _moveToAttack = false;
                _isKnockedBack = true;
                _setDisengageTarget = false;

                Debug.Log("EARTHQUAKE");
                //PlayerThrow();
            }
        }


        void OnCollisionEnter()
        {
            Debug.Log("COLLISION");
        }

        void AddLocationMarker()
        {
            if (_posClicked != null)
            {
                Destroy(_posClicked);
                _posClicked = (GameObject)Instantiate(_clickMoveIcon, new Vector3(_targetPosition.x, _targetPosition.y + 0.1f, _targetPosition.z), Quaternion.identity);

            }
            else
            {
                _posClicked = (GameObject)Instantiate(_clickMoveIcon, new Vector3(_targetPosition.x, _targetPosition.y + 0.1f, _targetPosition.z), Quaternion.identity);

            }


        }

        public static void PlayerKnockback(float _distance)
        {
            _disengageDistance = _distance;
            _isKnockedBack = true;
            _setDisengageTarget = false;               
        }

        void PlayerThrow()
        {
            if(!_setDisengageTarget)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).name == "DisengageTarget") {
                    _disengageTarget = transform.GetChild(i).gameObject.transform.position;
                    }   
                }
                _setDisengageTarget = true;
            }
            if (_setDisengageTarget)
            {
                _oldPosition = transform.position;
                Vector3 _dir = _disengageTarget - transform.position;                                               // get the Vector we are going to move to
                _dir.y = 0f;                                                                                // we dont want to move up
                
                Vector3 _back = transform.TransformDirection(Vector3.back);                           // create a forward Vector3
                
                if (Vector3.Distance(transform.position, _disengageTarget) > 1f)
                {                                                                                            // if the magnitude of the vector is greater than
                    _charController.SimpleMove(_back * 20);                                // move the actual player
                }

                if (Vector3.Distance(transform.position, _disengageTarget) < 1f)
                {
                    _isKnockedBack = false;
                    //Destroy(_disengageTarget);
                    _setDisengageTarget = false;
                    _isMoving = false;
                    _moveToAttack = false;
                    
                }
            }
            GameInteraction.SpellHasBeenCast();
        }

        #region UPDATE function FUNCTIONS
        static void CheckGround(Vector3 _playerPos)
        {
            RaycastHit hit;
            if (Physics.Raycast(_playerPos, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "DamageField")
                {
                    if(!_takenDamage)
                    {
                        _playerHealth -= 5;
                        GameInteraction.SetPlayerHealth(_playerHealth);
                        SoundSystem.PlayerHit(_playerPos);
                        _takenDamage = true;
                        Combat.InitiateCombat();
                    }   
                }
                else
                {
                    
                }
            }
        }

        static void ImmunityTimer()
        {
            if (_takenDamage)
            {
                if (_damageTimer < _immunity)
                {
                    _damageTimer += Time.deltaTime;
                    
                }
            }
            if (_damageTimer >= _immunity)
            {

                _takenDamage = false;
                _damageTimer = 0;

            }
        }
        #endregion

        // HEALTH AND MANA CALCULATIONS
        #region Health and Mana calculations
        void RegenerateHealth()
        {
            if(!Combat.ReturnInCombat())
            {
                InvokeRepeating("RegenerateAddHealth()", 3.0f, 3.0f);
            }
            if(Combat.ReturnInCombat())
            {
                CancelInvoke();
            }
        }

        void RegenerateMana()
        {
            if (!Combat.ReturnInCombat())
            {
                InvokeRepeating("RegenerateAddMana()", 3.0f, 3.0f);
            }
            if(Combat.ReturnInCombat())
            {
                CancelInvoke();
            }
        }

        public static void AddPlayerHealth(int _amount)
        {
            _playerHealth += _amount;
            GameInteraction.SetPlayerHealth(_playerHealth);
        }

        public static void AddPlayerMana(int _amount)
        {
            _playerMana += _amount;
            GameInteraction.SetPlayerMana(_playerMana);
        }


        void RegenerateAddHealth()
        {
            _playerHealth += 5;
        }

        void RegenerateAddMana()
        {
            _playerMana += 5;
        }
        #endregion



        void CheckMouseOver()
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.tag == "NPC")
                {
                    GameInteraction.SetNpcCursor();
   
                }
                if(_hit.collider.tag == "EnemyRanged" || _hit.collider.tag == "EnemyMelee")
                {
                    GameInteraction.SetCombatCursor();
                }
                else
                {
                    GameInteraction.SetNormalCursor();
                }
            }
        }
    }
}
