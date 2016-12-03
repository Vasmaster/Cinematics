using UnityEngine;
using System.Collections;

public class TriggerEvents : MonoBehaviour {

    public GameObject _myTrap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {

        if(coll.name == "Player")
        {

            Rigidbody[] test = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < test.Length; i++)
            {
                test[i].isKinematic = false;
            }
            

        }

    }

}
