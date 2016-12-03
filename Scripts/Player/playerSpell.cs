using UnityEngine;
using System.Collections;

public class playerSpell : MonoBehaviour {

	private float _spellDamage;
	private float _timer;


	private float _lifeSpan = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		KillSwitch ();
	}


	public void SetDamage(float _damage) {
		_spellDamage = _damage;
	}

	public float ReturnDamage() {
		return _spellDamage;
	}

	void KillSwitch() {

		if (_timer == _lifeSpan) {
			Destroy (this.gameObject);
		} else {
			_timer += Time.deltaTime;
		}
	}
}
