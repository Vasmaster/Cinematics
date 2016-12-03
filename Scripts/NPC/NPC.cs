using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;


[System.Serializable]
public class NPC : MonoBehaviour
{

    private GameObject _player;

    [SerializeField]
    public int _npcID;

    [Header("General Information")]
    public string _nameToDisplay;
    public float _health;

    [Header("Conversation")]
    public bool _AutoCommunicate;
    private bool _communicate;

    [Header("NPC Behaviour")]
    public bool _patrol;
    public float _patrolSpeed;

    private int _currentWayPoint;
    [Header("Patrol Waypoints")]
    public Transform[] _wayPoints;

    private Vector3 _wayPointTarget;
    private Vector3 _moveDirection;





   

    // Use this for initialization
    void Start()
    {

        
        _communicate = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (_player != null)
        {

            transform.LookAt(_player.transform.position);

        }

        if (_patrol)
        {
            Patrol();
        }
                
    }

    

    public void SetConversation(GameObject _playerTarget, bool _moving)
    {

        _player = _playerTarget;
        _patrol = _moving;

        if (!_moving)
        {
            GetComponentInChildren<NPC_Dialog>().Converse();
        }
        if (_moving)
        {

            GetComponentInChildren<NPC_Dialog>().StopConversation();
        }
    }

    void Patrol()
    {

        if (_patrol)
        {

            if (_wayPoints.Length > 0)
            {

                if (_currentWayPoint < _wayPoints.Length)
                {


                    _wayPointTarget = _wayPoints[_currentWayPoint].position;
                    _moveDirection = _wayPointTarget - transform.position;

                    if (_moveDirection.magnitude < 1)
                    {

                        _currentWayPoint++;


                    }
                    else {
                    }

                }
                else {

                    _currentWayPoint = 0;


                }
                transform.LookAt(_wayPointTarget);
                transform.position = Vector3.MoveTowards(transform.position, _wayPointTarget, _patrolSpeed / 10);
            }

        }
    }

    public void IsSelected()
    {
        _communicate = true;

    }

    public bool ReturnIsSelected()
    {
        return _communicate;
    }

    public void PlayerInteraction(GameObject _playerTarget, bool _shouldMove)
    {
        _player = _playerTarget;
        _patrol = _shouldMove;

    }

  
    
  

    public int ReturnNpcID()
    {
        return _npcID;
    }


}

