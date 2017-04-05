using UnityEngine;
using System.Collections;

public class playerSpellCast : MonoBehaviour {
    
	private GameObject _projectile;

	// Use this for initialization
	void Start () {

	}

	void Update () {

	}

	public void PlayerSpellCast(GameObject _nwSelectedTarget, Vector3 _position, GameObject _prefab, float _damage, int _type, float _velocity) {
		
        if(_nwSelectedTarget != null) { 
        
		    Vector3 _playerAimVector = _nwSelectedTarget.transform.position - _position;
		    _projectile = Instantiate(_prefab, _position, Quaternion.identity) as GameObject;
            _projectile.transform.LookAt(_nwSelectedTarget.transform);    
			_projectile.GetComponent<Rigidbody> ().AddForce (_playerAimVector * _velocity);
			_projectile.AddComponent<playerSpell> ();
			_projectile.GetComponent<playerSpell> ().SetDamage (_damage);

            //transform.GetChild(1).transform.LookAt(_nwSelectedTarget.transform);
	    }
    }

}
