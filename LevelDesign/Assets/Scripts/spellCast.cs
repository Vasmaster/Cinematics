using UnityEngine;
using System.Collections;

public class spellCast : MonoBehaviour {


	private float _spellDamage;
	private float _lifeSpan = 2f;
	private float _timer = 0f;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
		killSwitch ();
	}


	public void SetDamage(float _damage) {

		_spellDamage = _damage;
	}

	public float ReturnDamage() {

		return _spellDamage;

	}

	void killSwitch() {

		if ((int)_timer == _lifeSpan) {

			Destroy (this.gameObject);


		} else {

			_timer += Time.deltaTime;
		}

	}

}
