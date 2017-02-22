using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.Data;
using System;

public class DialogueManager : MonoBehaviour {

    
    private Rect _questWindow;
    private Rect _questTitleRect;
    private Rect _questTextRect;
    private Rect _questAcceptRect;


    public GUISkin _skin;

    private bool _showQuestWindow = false;
    private bool _showButton = false;
    private bool _draggingWindow = false;

    private Vector2 _windowPosition = new Vector2(100, 100);
    private Vector2 _windowSize = new Vector2(400, 400);


    private string _questTitle;
    private string _questText;

    private int _npcID;
    private int _questID;
    private bool _questCompleted;

    private CombatSystem.Player _player;

    private bool _showZone = false;
    private string _zoneName;
    private string _zoneDesc;

    private bool _showMessage = false;
    private string _message;

    private List<int> _questRewards = new List<int>(); 

	// Use this for initialization
	void Start () {
        
        if(PlayerPrefs.GetFloat("QuestWindowPosX") > 0)
        {
            _windowPosition = new Vector2(PlayerPrefs.GetFloat("QuestWindowPosX"), PlayerPrefs.GetFloat("QuestWindowPosY"));
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {

        //_player = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
    }

    void OnGUI()
    {
        if (_showQuestWindow)
        {
            ShowQuestWindow();
        }

        if(_draggingWindow)
        {
            _windowPosition.x = Event.current.mousePosition.x;
            _windowPosition.y = Event.current.mousePosition.y;

            PlayerPrefs.SetFloat("QuestWindowPosX", _windowPosition.x);
            PlayerPrefs.SetFloat("QuestWindowPosY", _windowPosition.y);
        }
        if(_showZone)
        {
            ShowZone();
        }

        if(_showMessage)
        {
            Rect _zoneRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100);
            GUI.Label(_zoneRect, _message, _skin.GetStyle("Message"));

            InvokeRepeating("CancelShowMessage", 2, 100);
        }
    }

    public void SetDialogue(string _title, string _text, bool _buttonVis, int _nwNpcID, int _nwQuestID)
    {

        _questTitle = _title;
        _questText = _text;
        _showButton = _buttonVis;
        _npcID = _nwNpcID;
        _questID = _nwQuestID;
        _showQuestWindow = true;
        _questCompleted = Quest.QuestDatabase.CheckQuestCompleteByID(_questID);

        //Debug.Log("NPCID " + _npcID + " QuetID " + _questID);
    }

    void ShowQuestWindow()
    {
        _questWindow = new Rect(_windowPosition.x, _windowPosition.y, _windowSize.x, _windowSize.y);
        _questTitleRect = new Rect(_windowPosition.x + 75, _windowPosition.y + 50, _windowSize.x - 100, _windowSize.y - 300);
        _questTextRect = new Rect(_windowPosition.x + 90, _windowPosition.y + 80, _windowSize.x - 200, _windowSize.y - 50);
        _questAcceptRect = new Rect(_windowPosition.x + 100, _windowPosition.y + _windowSize.y - 50, _windowSize.x - 200, _windowSize.y - 350);

        GUI.Box(_questWindow, "", _skin.GetStyle("QuestWindow"));

        GUI.Label(_questTitleRect, _questTitle, _skin.GetStyle("QuestTitle"));
        GUI.Label(_questTextRect, _questText, _skin.GetStyle("QuestText"));

        if(_showButton)
        {
            if (!_questCompleted)
            {
                GUI.Box(_questAcceptRect, "", _skin.GetStyle("QuestAcceptButton"));
            }
            else
            {
                GUI.Box(_questAcceptRect, "", _skin.GetStyle("QuestFinishButton"));
            }
        }
        else
        {

        }

        if(_questWindow.Contains(Event.current.mousePosition))
        {
            _player.HoveringOverUI(true);

            if(Event.current.button == 0 && Event.current.type == EventType.mouseDrag && !_draggingWindow)
            {
                _draggingWindow = true;
                _player.SetDraggingUI(true);
            }

            if(Event.current.button == 0 && Event.current.type == EventType.mouseUp && _draggingWindow)
            {
                _draggingWindow = false;
                _player.SetDraggingUI(false);
            }
        }
        if (_questAcceptRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0 && Event.current.type == EventType.mouseDown)
            {
                if (!_questCompleted)
                {
                    Quest.QuestDatabase.AcceptQuest(_questID);
                }
                if(_questCompleted)
                {
                    //Quest.QuestDatabase.ReturnQuestRewards(_questID);
                    Quest.QuestDatabase.FinishQuest(_questID);
                    //Quest.QuestDatabase.ClearAll();
                    _player.SetTarget(null);
                }
                _showQuestWindow = false;
                _player.HoveringOverUI(false);
            }
        }

    }

    public void ExitDialogue(bool _buttonVis)
    {
       _showQuestWindow = false; 
    }

    public void SetEndQuest(int _qID, string _title, string _completeText)
    {
        /*
        _textTitle.gameObject.SetActive(true);
        _textBox.gameObject.SetActive(true);
        _textBG.gameObject.SetActive(true);
        _textTitle.text = _title;
        _textBox.text = _completeText;
        _finishQuest.gameObject.SetActive(true);
        */
    }

    void ShowZone()
    {
        Rect _zoneRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100);
        GUI.Label(_zoneRect, _zoneName, _skin.GetStyle("ShowZone"));

        InvokeRepeating("CancelShowZone", 4, 100);
    }

    public void SetShowZone(bool _set, string _name, string _desc)
    {
        _showZone = _set;
        _zoneName = _name;
        _zoneDesc = _desc;
    }

    void CancelShowZone()
    {
        _showZone = false;
        CancelInvoke();
    }

    void CancelShowMessage()
    {
        _showMessage = false;
        _message = "";
        CancelInvoke();
    }

    public void ShowMessage(string _msg, bool _set)
    {
        _showMessage = _set;
        _message = _msg;
        
    }

}
