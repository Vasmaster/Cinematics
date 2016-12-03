using UnityEngine;
using System.Collections;

public class playerMelee : MonoBehaviour {


	public Animator _meleeAnimator;
	private float _meleeDamage = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoMeleeAttack(float _damage) {
		_meleeDamage = _damage;
		Debug.Log ("Melee damage " + _meleeDamage);
		_meleeAnimator.Play ("Anim_meleeAttack");

	}

	public float ReturnDamage() {
		return _meleeDamage;
	}

}
