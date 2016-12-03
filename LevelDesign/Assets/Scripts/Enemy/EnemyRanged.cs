using UnityEngine;
using System.Collections;

public enum AOEType {

	EarthQuake,
	HellBlast,
	None,

}

public class EnemyRanged : MonoBehaviour {

	private bool _attack;
	private bool _patrol;
	private bool _inRange;

	private GameObject _target;
	private GameObject _projectile;
	private GameInteraction _myInterface;

	private float _coolDown;
	private Vector3 _newPosition;

	[Header("Enemy Attributes")]
	public string _nameToDisplay;
	public float _enemyHealth;
	public float _enemyMana;

	[Header("Enemy Ranged Attack Attributes")]
	public GameObject _rangedSpell_1;
	public float _enemyAttackSpeed = 1f;
	public float _enemyAttackRate;
	public float _enemyMovingSpeed;


	public float _moveToEnemySpeed;
	public float _distanceBetweenEnemyPlayer;
	public float _minimumDistance;
	public float _rangedDamage;

	[Header("AoE if player is to close")]
	public AOEType _AOEType;
	public bool _repeatHellBlast;
	public float _AOEDistance;
	public float _AOECooldown;
	public float _AOEDamage;
	private EnemyAOE _enemyAOE;

    

    private Vector3 _wayPointTarget;
	private Vector3 _moveDirection;
	private Vector3 _velocity;
	private Vector3 _enemyPlayerDistance;

	private GameManager _gameManager;

	private int _currentWayPoint;
	[Header("Patrol Waypoints")]
	public Transform[] _wayPoints;


    [Header("User Feedback")]
    public ParticleSystem _gotHit;
    public ParticleSystem _death;

    private bool _playOnce;

    // Use this for initialization
    void Start () {
	
		_attack = false;
		_patrol = true;
		_coolDown = _enemyAttackRate;
		_myInterface = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameInteraction> ();
		_enemyAOE = GetComponentInChildren<EnemyAOE> ();
        _playOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (_attack) {
				RangedAttack (_target);
		}
		if (_patrol) {

			Patrol ();

		}


        if ((int)_enemyHealth <= 0)
        {

            enemyDeath();
        }



    }


	void OnTriggerEnter(Collider coll) {

		if (coll.tag == "PlayerRangedSpell") {
			
			float _dmgDealt = coll.GetComponent<playerSpell> ().ReturnDamage ();
			_enemyHealth -= _dmgDealt;

			_myInterface.SetEnemyHealth (_enemyHealth);
			Destroy (coll.gameObject);

            _gotHit.Play();

            
            Debug.Log(coll.gameObject.transform.GetChild(0).name);

        }

		if (coll.tag == "PlayerMelee") {

			Debug.Log ("melee hit");
			float _dmgDealt = coll.GetComponent<playerMelee> ().ReturnDamage ();
			if (_dmgDealt > 0) {
				
				_enemyHealth -= _dmgDealt;
				_myInterface.SetEnemyHealth (_enemyHealth);

                _gotHit.Play();
            }
		}


	}

	public void setTarget (Collider col) {

		_target = col.gameObject;

	}

	public void setAttack(bool _state) {

		_attack = _state;


	}

	public void setPatrol(bool _state) {

		_patrol = _state;

	}

	void OnTriggerExit(Collider coll) {
		



	}


	void RangedAttack(GameObject _player) {

	
		transform.LookAt (_player.transform.position);	
		Debug.DrawLine (transform.position, _player.transform.position, Color.red);

		GetComponent<Rigidbody> ().velocity = Vector3.zero;

		_enemyPlayerDistance = transform.position - _player.transform.position;


		if (Vector3.Distance (transform.position, _player.transform.position) > _distanceBetweenEnemyPlayer) {
			transform.position = Vector3.MoveTowards (transform.position, _player.transform.position, _moveToEnemySpeed * Time.deltaTime);

			if (Vector3.Distance (transform.position, _player.transform.position) == _distanceBetweenEnemyPlayer) {

				_inRange = true;

			}
		} 

		if (Vector3.Distance (transform.position, _player.transform.position) <= _AOEDistance) {
			
			transform.position = Vector3.MoveTowards (transform.position, transform.position, _moveToEnemySpeed * Time.deltaTime);

			_enemyAOE.SetInAOERange (true);
			_enemyAOE.DoAOE (_AOEType.ToString (), _AOEDamage, _AOECooldown, _repeatHellBlast);
			_inRange = false;

		}

		if(Vector3.Distance(transform.position, _player.transform.position) > _AOEDistance) {
			
			_enemyAOE.SetInAOERange (false);
			_inRange = true;
		}

		if (_AOEType.ToString () == "None") {
			_inRange = true;
		}

		/////////////////////////////////////////////////////
		/// SHOOTING
		///////////////////////////////////////////////////// 
		if (_inRange) {
			if (Mathf.RoundToInt (_coolDown) == _enemyAttackRate) {

				Vector3 _playerAimVector = _player.transform.position - transform.position;
				//Vector3 _playerAimVector = new Vector3 (_player.transform.position.x - transform.position.x, _player.transform.position.y - transform.position.y, _player.transform.position.z - transform.position.z);

				GameObject _projectile = Instantiate (_rangedSpell_1, transform.position, Quaternion.identity) as GameObject;
				_projectile.name = "enemySpellAttack";
				_projectile.GetComponent<spellCast> ().SetDamage (_rangedDamage);
				_projectile.GetComponent<Rigidbody> ().AddForce (_playerAimVector * _enemyAttackSpeed);
				_coolDown = 0;



			} else {

				_coolDown += Time.deltaTime;

			}
		}
	}




	void Patrol() {

		if (_patrol) {

			if (_currentWayPoint < _wayPoints.Length) {
				

				_wayPointTarget = _wayPoints [_currentWayPoint].position;
				_moveDirection = _wayPointTarget - transform.position;
			//	_velocity = GetComponent<Rigidbody> ().velocity;
			
				if (_moveDirection.magnitude < 1) {
					
					_currentWayPoint++;



				} else {

				

				}

			} else {

				_currentWayPoint = 0;


			}
			transform.LookAt (_wayPointTarget);
            transform.position = Vector3.MoveTowards(transform.position, _wayPointTarget, _enemyMovingSpeed);


        }
	}

	public Vector3 ReturnPosition() {

		return this.transform.position;

	}

	void enemyDeath() {
        if(!_playOnce) {
            _death.Play();

            _myInterface.EnemyDeath();
            _playOnce = true;
        }

        if (!_death.IsAlive())
        {
            Debug.Log("lkdjfdfdlf");
            Destroy(this.gameObject);
        }


    }

	public float ReturnAOEDamage() {

		return _AOEDamage;

	}



}
