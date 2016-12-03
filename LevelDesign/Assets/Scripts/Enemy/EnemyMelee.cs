using UnityEngine;
using System.Collections;

public enum RangedAttack {

	Charge,
	Hook,
	Teleport,
	None,

}

public enum EnemyMovement
{


    Patrol,
    StandIdle,
    Spawn,

}


public class EnemyMelee : MonoBehaviour {

	[Header("Enemy Attributes")]
	public string _nameToDisplay;
	public float _enemyHealth;
	public float _enemyMana;
	public float _enemyMovingSpeed;

	[Header("Attack Attributes")]
	[SerializeField]
	private float _globalCoolDown;
	public float _meleeRange;
	public float _enemyChargeSpeed;
	public float _attackRate;
	public float _defaultDamage;
	public float _hellStrikeDamage;
	public float _hellStrikeCoolDown;
	private float _hellStrikeTimer;

	public bool _randomPowerAttacks;
	public int _chanceOfPowerAttack;

	private float _damageOutput;

	[Header("If the player is to far")]
	public RangedAttack _rangedAttack;
	public GameObject _enemyHook;
	[SerializeField]
	private Transform _hookParent;
	public float _hookSpeed;
	public float _chargeSpeed;
	public float _rangedCooldown;


	private GameObject _target;
	private bool _attack;
	private bool _patrol;
	private bool _inMeleeRange;
	private bool _isAttacking;

	private Vector3 _wayPointTarget;
	private Vector3 _moveDirection;
	private Vector3 _enemyPlayerDistance;

	private bool _teleported;
	private bool _hasCharged;
	private bool _throwHook;

	private float _timer;
	private float _attackTimer;
    [Header("Enemy Movement")]
    public EnemyMovement _enemyMovement;
	private int _currentWayPoint;
	[Header("Patrol Waypoints")]
	public Transform[] _wayPoints;


	private GameInteraction _myInterface;

	// Use this for initialization
	void Start () {
		_patrol = true;
		_hasCharged = false;
		_throwHook = false;
		_isAttacking = false;
		_timer = _rangedCooldown;
		_attackTimer = _attackRate;
		_hellStrikeTimer = _hellStrikeCoolDown - _globalCoolDown;
		_myInterface = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameInteraction> ();
	}
	
	// Update is called once per frame
	void Update () {
	
        switch(_enemyMovement)
        {

            case EnemyMovement.Patrol:
                Patrol();
                break;

            case EnemyMovement.Spawn:
                Spawn();
                break;

            case EnemyMovement.StandIdle:
                StandIdle();
                break;
        }


		

        if(_attack)
        {
            MoveToTarget(_target, _enemyChargeSpeed);
        }

        if(!_inMeleeRange)
        {

            switch(_rangedAttack)
            {

                case RangedAttack.Charge:
                    Charge(_target);
                    break;

                case RangedAttack.Hook:
                    Hook(_target);
                    break;

                case RangedAttack.Teleport:
                    Teleport(_target);
                    break;

                case RangedAttack.None:
                    break;

            }

        }
        if(_inMeleeRange)
        {

            MeleeAttack(_target);
            
        }

        if((int) _timer < _rangedCooldown)
        {
           RangedCoolDown();
        }
        if((int) _attackTimer < _attackRate)
        {
            _attackTimer += Time.deltaTime;
        }

	
	}

    
	void Patrol() {

		if (_patrol) {

			if (_currentWayPoint < _wayPoints.Length) {
				
				_wayPointTarget = _wayPoints [_currentWayPoint].position;
				_moveDirection = _wayPointTarget - transform.position;
				//_velocity = GetComponent<Rigidbody> ().velocity;
                
				if (_moveDirection.magnitude <= 1f) {

					_currentWayPoint++;

				} else {

					 

				}

			} else {

				_currentWayPoint = 0;


			}
			transform.LookAt (_wayPointTarget);
            transform.position = Vector3.MoveTowards(transform.position, _wayPointTarget, _enemyMovingSpeed);
			//GetComponent<Rigidbody> ().velocity = _velocity;
            

           	
		}

	}

    void Spawn()
    {
        // AWESOME PARTICLES
    }

    void StandIdle()
    {

        transform.position = transform.position;

    }

    void MeleeAttack(GameObject _nwTarget)
    {
        if((int)_attackTimer == _attackRate) { 
            transform.LookAt(_nwTarget.transform.position);
            Animator _myAnim = GetComponentInChildren<Animator>();
            _myAnim.Play("Anim_MeleeAttack");
            _attackTimer = 0f;
            _damageOutput = _defaultDamage;
        }

    }

    void Charge(GameObject _nwTarget)
    {
        if(_nwTarget != null) { 
            Vector3 _directionVector = _nwTarget.transform.position - transform.position;
            Vector3 _nwPos = _directionVector.normalized;

            transform.position = Vector3.MoveTowards(transform.position, _nwTarget.transform.position + _nwPos, _chargeSpeed * Time.deltaTime);
        }
    }

    void Hook(GameObject _nwTarget)
    {
        if(!_throwHook && !_inMeleeRange && _nwTarget != null)
        {
            Debug.Log("Casting hook");
            Vector3 _hookTarget = _nwTarget.transform.position;
            if(Vector3.Distance(transform.position, _hookTarget) > float.Epsilon)
            {

                _enemyHook.transform.position = Vector3.MoveTowards(_enemyHook.transform.position, _hookTarget, _hookSpeed / 10);
                _attack = false;    
            }
            if(Vector3.Distance(_enemyHook.transform.position, _hookTarget) < 0.1f)
            {
                _throwHook = true;
                
                
            }

        }
        if(_throwHook)
        {

            Vector3 _hookTarget = _hookParent.transform.position;

            if(Vector3.Distance(_enemyHook.transform.position, _hookTarget) > float.Epsilon)
            {
                _enemyHook.transform.position = Vector3.MoveTowards(_enemyHook.transform.position, _hookTarget, _hookSpeed / 10);
                _nwTarget.GetComponent<Player>().HookPlayer(_enemyHook);
            }

            if(Vector3.Distance(_enemyHook.transform.position, _hookTarget) < 0.1f)
            {
                _attack = true;
                _nwTarget.GetComponent<Player>().UnHook(this.gameObject);
                _enemyHook.active = false;
            }

        }
    }

    void Teleport(GameObject _nwTarget)
    {
                        
        if(_nwTarget != null) { 

            if((int)_timer == _rangedCooldown) {

                Vector3 _directionVector = _nwTarget.transform.position - transform.position;
                Vector3 _nwPos = _directionVector.normalized * 2;

                transform.position += _nwPos;

                _timer = 0f;

            }
        }
    }

	void MoveToTarget(GameObject _nwTarget, float _chargeVelocity) {

		Debug.DrawLine (transform.position, _nwTarget.transform.position, Color.blue);
        	

		if (Vector3.Distance (transform.position, _nwTarget.transform.position) > _meleeRange) {
			
			transform.LookAt (_nwTarget.transform.position);
			transform.position = Vector3.MoveTowards (transform.position, _nwTarget.transform.position, _chargeVelocity * Time.deltaTime);
			_inMeleeRange = false;
		}

        if (Vector3.Distance(transform.position, _nwTarget.transform.position) < _meleeRange)
        {

            // if the ranged attack is 'Charge'
            if (_rangedAttack == RangedAttack.Charge) { 
            _hasCharged = true;
            }

            _inMeleeRange = true;
		}

    

	}

	public void setAttack(bool _setAttack) {

		_attack = _setAttack;

	}

	public void setPatrol(bool _setPatrol) {

		_patrol = _setPatrol;

	}

	public void setTarget(Collider coll) {

		_target = coll.gameObject;

	}

	public void setCharged(bool _setCharged) {
		_hasCharged = _setCharged;

	}

	public void setThrowHook(bool _setHook) {

		_throwHook = _setHook;

	}

	public void setMeleeRange(bool _setMelee) {

		_inMeleeRange = _setMelee;

	}

	void OnTriggerEnter(Collider coll) {

       
        if(coll.tag == "AggroRange")
        {
            return;
        }

        if (coll.tag == "PlayerMelee") {

			Debug.Log ("melee hit");
			float _dmgDealt = coll.GetComponent<playerMelee> ().ReturnDamage ();
			if (_dmgDealt > 0) {

				_enemyHealth -= _dmgDealt;
				_myInterface.SetEnemyHealth (_enemyHealth);
			}
		}



	}

	void RangedCoolDown() {

		_timer += Time.deltaTime;

	}

	void MeleeCoolDown() {
		_attackTimer += Time.deltaTime;
	}

	public float DefaultDamage() {

		return _damageOutput;

	}

}
