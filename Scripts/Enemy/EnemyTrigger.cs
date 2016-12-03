using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider coll) {

		if(coll.name == "Player" || coll.name == "PlayerMelee") {
			this.transform.parent.GetComponent<EnemyRanged> ().setAttack (true);
			this.transform.parent.GetComponent<EnemyRanged> ().setPatrol (false);
			this.transform.parent.GetComponent<EnemyRanged> ().setTarget (coll);


		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.name == "Player") {

			this.transform.parent.GetComponent<EnemyRanged> ().setAttack (false);
			this.transform.parent.GetComponent<EnemyRanged> ().setPatrol (true);



		}
	}

}
