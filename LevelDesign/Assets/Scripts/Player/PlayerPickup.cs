using UnityEngine;
using System.Collections;
using UnityEditor;

public enum States{

	healthPickup,
	manaPickup,
	itemPickup,

}


public class PlayerPickup : MonoBehaviour {
	
	public States _pickupType;

	public GameObject _pickupMesh;

	public int _healthAmount;
	public int _manaAmount;
	public GameObject _itemPickup;

	void Start() {

		if (_pickupMesh != null) {
			GetComponent<MeshFilter> ().mesh = _pickupMesh.GetComponent<MeshFilter> ().sharedMesh;
		}

	}

	public string GetPickup() {

		return _pickupType.ToString ();

	}

	public int GetHealth() {

		return _healthAmount;

	}

	public int GetMana() {

		return _manaAmount;

	}

	public void DestroyPickup() {

		Destroy (this.gameObject);

	}

}

