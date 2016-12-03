using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public GameObject _mainCamera;
    public Transform _centerPoint;

    public float _distance, _height, _angle;

    private GameObject _player;

    private bool _rotated;

	// Use this for initialization
	void Start () {


        _rotated = false;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        _centerPoint.position = gameObject.transform.position ;

        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _centerPoint.position, 0.1f) + new Vector3(_angle * -1 / 50,_height, _distance * -1);
        //   _mainCamera.transform.eulerAngles = new Vector3(0, _angle / 100, 0);
        
    }

    
}
