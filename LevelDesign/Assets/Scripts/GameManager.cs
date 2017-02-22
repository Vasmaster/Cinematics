using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private bool _editMode = false;



	void OnAwake() {


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetEditMode(bool _set)
    {
        _editMode = _set;
    }

    public bool ReturnEditMode()
    {
        return _editMode;
    }
}
