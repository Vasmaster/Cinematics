using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    
    private Transform _centerPoint;
    private float _zoom = 0;

	// Use this for initialization
	void Start () {


    
        _centerPoint = GameObject.Find("Camera_Target").transform;
       
    }
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(_centerPoint);

    }

    void LateUpdate()
    {
        
        if (Input.GetKey("a"))
        {
            transform.RotateAround(_centerPoint.position, Vector3.up, 100 * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.RotateAround(_centerPoint.position, Vector3.down, 100 * Time.deltaTime);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {

            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z + 1.0f), Time.deltaTime * 2);
            //transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z + 0.2f);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z - 1.0f), Time.deltaTime * 2);
        }

        

    }
    
}
