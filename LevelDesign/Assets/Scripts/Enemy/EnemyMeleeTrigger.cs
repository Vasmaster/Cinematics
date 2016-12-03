using UnityEngine;
using System.Collections;

public class EnemyMeleeTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {

		if(coll.tag == "Player" || coll.name == "PlayerMelee") {
			this.transform.parent.GetComponent<EnemyMelee> ().setAttack (true);
			this.transform.parent.GetComponent<EnemyMelee> ().setPatrol (false);
			this.transform.parent.GetComponent<EnemyMelee> ().setTarget (coll);


		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.name == "Player") {

			this.transform.parent.GetComponent<EnemyMelee> ().setAttack (false);
			this.transform.parent.GetComponent<EnemyMelee> ().setPatrol (true);
			this.transform.parent.GetComponent<EnemyMelee> ().setCharged (false);
			this.transform.parent.GetComponent<EnemyMelee> ().setThrowHook (false);
			this.transform.parent.GetComponent<EnemyMelee> ().setMeleeRange (false);


		}
	}
}
