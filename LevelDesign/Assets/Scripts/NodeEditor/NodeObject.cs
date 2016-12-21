using UnityEngine;
using System.Collections;

public class NodeObject : MonoBehaviour
{

    [SerializeField]
    private int _charID;

    [SerializeField]
    private int _nodeID;
    [SerializeField]
    private int _inputID;
    [SerializeField]
    private int _outputID;

    [SerializeField]
    private string _anim;

    [SerializeField]
    private float _idleWait;

    [SerializeField]
    private float posX;

    [SerializeField]
    private float posY;

    private string windowTitle;
    [SerializeField]
    private GameObject _wayPoint;

    [SerializeField]
    private bool _completed;

    [SerializeField]
    private string _customAnimType;

    [SerializeField]
    private Animation _customAnim;

    private bool _isActive;

    [SerializeField]
    private AudioClip _audio;

    [SerializeField]
    private bool _autoPlay;

    public void setNodeID(int _id)
    {
        
        if (_id < 0)
        {
            Debug.Log("Setting node id");
            _nodeID += _id;
        }
        else {
            _nodeID = _id;
        }
    }

    public void setInputID(int _id)
    {
        _inputID = _id;
    }

    public void setOutputID(int _id)
    {
        if (_id < 0 && _outputID > 0)
        {
            Debug.Log("Setting output ID");
            _outputID += _id;
        }
        else {
            _outputID = _id;
        }
    }

    public void setAnimation(string _animation)
    {
        _anim = _animation;
    }

    public void setIdleWait(float _time)
    {
        _idleWait = _time;
    }

    public void setPosition(float x, float y)
    {
        posX = x;
        posY = y;
    }

    public float ReturnPosX()
    {
        return posX;
    }

    public float ReturnPosY()
    {
        return posY;
    }

    public void setTitle(string _title)
    {
        windowTitle = _title;
    }

    public string ReturnTitle()
    {
        return windowTitle;
    }

    public void SetWayPoint(GameObject _obj)
    {

        _wayPoint = _obj;
    }

    public GameObject ReturnWayPoint()
    {
        return _wayPoint;
    }

    public int ReturnOutputID()
    {
        return _outputID;
    }

    public bool ReturnComplete()
    {
        return _completed;
    }

    public void SetComplete()
    {
        _completed = true;
    }

    public string ReturnAnim()
    {
        return _anim;
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void SetInActive()
    {
        _isActive = false;
    }

    public bool ReturnActive()
    {
        return _isActive;
    }



    public float ReturnIdleWait()
    {
        return _idleWait;
    }

    public void SetCustomAnim(Animation _cAnim)
    {
        _customAnim = _cAnim;
    }

    public Animation ReturnCustomAnim()
    {
        return _customAnim;
    }

    public void SetCustomAnimType(string _cAnimType)
    {
        _customAnimType = _cAnimType;
    }

    public string ReturnCustomAnimType()
    {
        return _customAnimType;
    }

    public void SetAudio(AudioClip _sound)
    {
        _audio = _sound;
    }

    public AudioClip ReturnAudio()
    {
        return _audio;
    }

    public void SetCharID(int _id)
    {

        _charID = _id;

    }

    public int ReturnCharID()
    {
        return _charID;
    }

    public void SetName(int _id)
    {
        this.name = "Node" + (_nodeID - 1);
        Debug.Log(this.name);
    }

    public int ReturnNodeID()
    {
        return _nodeID;
    }

    public void AutoPlay(bool _play)
    {
        _autoPlay = _play;
    }

    public bool ReturnAutoPlay()
    {
        return _autoPlay;
    }

}
