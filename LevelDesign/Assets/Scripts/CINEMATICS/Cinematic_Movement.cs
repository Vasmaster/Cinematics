using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class Cinematic_Movement : MonoBehaviour
{

    private GameObject[] _allNodes;

    private bool _isWalking = false;
    private bool _isIdle = false;
    private bool _isRunning = false;
    private bool _isCombatIdle = false;
    private bool _setOnce = false;
    private bool _isRanged = false;
    private bool _isMelee = false;

    private GameObject _wayPoint;

    private int _activeNode;

    private Animator _animator;

    private float _counter;

    private CharacterController _playerMovement;

    private AudioSource _soundManager;
    private bool _isPlaying;

    public int CharacterID;

    public void Start()
    {

        _animator = GetComponentInChildren<Animator>();

        _playerMovement = GetComponent<CharacterController>();

        _soundManager = GetComponent<AudioSource>();

        _allNodes = GameObject.FindGameObjectsWithTag("Node").OrderBy(go => go.name).ToArray();
        for (int i = 0; i < _allNodes.Length; i++)
        {
            if(!_allNodes[i].GetComponent<NodeObject>().ReturnComplete())
            {
                if(_allNodes[i].GetComponent<NodeObject>().ReturnAnim() == "Start")
                {
                    Debug.Log("STARTING THE SEQUENCE");
                    _allNodes[i].GetComponent<NodeObject>().SetComplete();
                    transform.position = _allNodes[i].GetComponent<NodeObject>().ReturnWayPoint().transform.position;
                    _activeNode = _allNodes[i].GetComponent<NodeObject>().ReturnOutputID();
                    _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();

                    

                }
            }
            else
            {

            }
        }       

    }

    public void Update()
    {
        if (_allNodes.Length > 0)
        {
            if (_allNodes[_activeNode] != null)
            {
                if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnActive())
                {
                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Walk")
                    {
                        _wayPoint = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnWayPoint();

                        _isWalking = true;
                    }

                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Run")
                    {
                        _wayPoint = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnWayPoint();

                        _isRunning = true;
                    }

                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Idle")
                    {
                        _isIdle = true;
                    }
                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "CombatIdle")
                    {
                        _isCombatIdle = true;
                    }

                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Start")
                    {
                        _isIdle = true;
                    }
                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Ranged")
                    {
                        _isRanged = true;
                    }
                    if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() == "Melee")
                    {
                        _isMelee = true;
                    }
                }
            }
            if (_isWalking)
            {
                _animator.SetBool("isWalking", true);
                Walk();
            }

            if (_isRunning)
            {
                _animator.SetBool("isRunning", true);
                Run();
            }
            if (_isIdle)
            {
                Debug.Log("Idle");
                Idle();
            }

            if(_isCombatIdle)
            {
                _animator.SetBool("isCombatIdle", true);
                Debug.Log("Combat Idle");
                CombatIdle();
            }

            if (_isRanged)
            {
                _animator.SetBool("isRanged", true);
                Ranged();
                
            }

            if (_isMelee)
            {
                _animator.SetBool("isMelee", true);

                Melee();
            }
        }
    }


    public void AnimStart(GameObject _wayPoint)
    {
        this.transform.position = _wayPoint.transform.position;
        Debug.Log("Start start");
    }

    public void SetWayPoint(GameObject _nwWayPoint)
    {
        _wayPoint = _nwWayPoint;
       // Debug.Log(_wayPoint);
    }

    void Walk()
    {

        if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio() != null)
        {
            if (!_isPlaying)
            {
                Debug.Log(_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio());
                _soundManager.clip = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio();

                _soundManager.Play();
                if (!_soundManager.isPlaying)
                {
                    _isPlaying = false;
                }
                else
                {
                    _isPlaying = true;
                }

            }
        }

        Vector3 _dir = _wayPoint.transform.position - transform.position;                                    // get the Vector we are going to move to
        _dir.y = 0f;                                                                    // we dont want to move up
        Quaternion _targetRot = Quaternion.LookRotation(_dir);                          // get the rotation in which we should look at
        transform.rotation = _targetRot;                                                // rotate the player

        Vector3 _forward = transform.TransformDirection(Vector3.forward);               // create a forward Vector3

        if (_dir.magnitude > 0.1f)
        {                                                    // if the magnitude of the vector is greater than
            _playerMovement.SimpleMove(_forward * 2.65f);        // move the actual player

        }
        else
        {
            _allNodes[_activeNode].GetComponent<NodeObject>().SetComplete();
            _allNodes[_activeNode].GetComponent<NodeObject>().SetInActive();
            _activeNode = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnOutputID();
            if (_allNodes[_activeNode] != null)
            {
                _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();
            }

            if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() != "Idle")
            {

                _animator.SetBool("skipIdle", true);

            }



            _isWalking = false;
            _animator.SetBool("isWalking", false);
        }

            

    }

    public void Idle()
    {

       
        
        
        if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnIdleWait() > 0)
        {

            if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio() != null)
            {
                if (!_isPlaying)
                {
                    Debug.Log(_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio());
                    _soundManager.clip = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio();

                    _soundManager.Play();
                    if(!_soundManager.isPlaying)
                    {
                        _isPlaying = false;
                    }
                    else
                    {
                        _isPlaying = true;
                    }

                }
            }

            if (!_setOnce)
            {
                _counter = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnIdleWait();
                _setOnce = true;
            }
            _counter -= Time.deltaTime;

           

            if (_counter < 0)
            {
                _allNodes[_activeNode].GetComponent<NodeObject>().SetComplete();
                _allNodes[_activeNode].GetComponent<NodeObject>().SetInActive();
                _activeNode = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnOutputID();
                _isIdle = false;
                if (_allNodes[_activeNode] != null)
                {
                    _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();
                }
            }
        }
        else
        {
            _isWalking = false;
            _isRunning = false;
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("skipIdle", false);
        }
        
    }

    public void Run()
    {


        if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio() != null)
        {
            if (!_isPlaying)
            {
                Debug.Log(_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio());
                _soundManager.clip = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio();

                _soundManager.Play();
                if (!_soundManager.isPlaying)
                {
                    _isPlaying = false;
                }
                else
                {
                    _isPlaying = true;
                }

            }
        }

        Vector3 _dir = _wayPoint.transform.position - transform.position;                                    // get the Vector we are going to move to
        _dir.y = 0f;                                                                    // we dont want to move up
        Quaternion _targetRot = Quaternion.LookRotation(_dir);                          // get the rotation in which we should look at
        transform.rotation = _targetRot;                                                // rotate the player

        Vector3 _forward = transform.TransformDirection(Vector3.forward);               // create a forward Vector3

        if (_dir.magnitude > 0.1f)
        {                                                    // if the magnitude of the vector is greater than
            _playerMovement.SimpleMove(_forward * 3f);        // move the actual player

        }
        else
        {
            _allNodes[_activeNode].GetComponent<NodeObject>().SetComplete();
            _allNodes[_activeNode].GetComponent<NodeObject>().SetInActive();
            _activeNode = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnOutputID();
            if (_allNodes[_activeNode] != null)
            {
                _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();
            }

            if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() != "Idle")
            {

                _animator.SetBool("skipIdle", true);

            }



            _isRunning = false;
            _animator.SetBool("isRunning", false);
        }


    }

    public void CombatIdle()
    {
        if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnIdleWait() > 0)
        {

            if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio() != null)
            {
                if (!_isPlaying)
                {
                    Debug.Log(_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio());
                    _soundManager.clip = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio();

                    _soundManager.Play();
                    if (!_soundManager.isPlaying)
                    {
                        _isPlaying = false;
                    }
                    else
                    {
                        _isPlaying = true;
                    }

                }
            }


            if (!_setOnce)
            {
                _counter = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnIdleWait();
                _setOnce = true;
            }
            _counter -= Time.deltaTime;



            if (_counter < 0)
            {
                _allNodes[_activeNode].GetComponent<NodeObject>().SetComplete();
                _allNodes[_activeNode].GetComponent<NodeObject>().SetInActive();
                _activeNode = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnOutputID();

                _isCombatIdle = false;
                _animator.SetBool("isCombatIdle", false);

                if (_allNodes[_activeNode] != null)
                {
                    _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();
                }
                else if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() != "Idle")
                {

                    _animator.SetBool("skipIdle", true);

                }
               
            }
        }
        else
        {
            _isWalking = false;
            _isRunning = false;
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("skipIdle", false);
        }
    }

    void Ranged()
    {

        if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio() != null)
        {
            if (!_isPlaying)
            {
                Debug.Log(_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio());
                _soundManager.clip = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnAudio();

                _soundManager.Play();
                if (!_soundManager.isPlaying)
                {
                    _isPlaying = false;
                }
                else
                {
                    _isPlaying = true;
                }

            }
        }


            _animator.SetBool("isRanged", true);
        
           

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Ranged"))
            {
            Debug.Log(" Ranged is playing")
;            }
            else {

                Debug.Log("Range has ended");

                _isRanged = false;
                _animator.SetBool("isRanged", false);

               _allNodes[_activeNode].GetComponent<NodeObject>().SetComplete();
                _allNodes[_activeNode].GetComponent<NodeObject>().SetInActive();

                _activeNode = _allNodes[_activeNode].GetComponent<NodeObject>().ReturnOutputID();
                if (_allNodes[_activeNode] != null)
                {
                    _allNodes[_activeNode].GetComponent<NodeObject>().SetActive();
                }

                if (_allNodes[_activeNode].GetComponent<NodeObject>().ReturnAnim() != "Idle")
                {

                    _animator.SetBool("skipIdle", true);

                }
            }



    }

    void Melee()
    {

    }
 
}