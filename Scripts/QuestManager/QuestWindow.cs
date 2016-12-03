using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public enum QuestTypes
{
    None,
    Find,
    Explore,
    Kill,
}

public enum QuestAmount
{
    Single,
    Multiple,
}

public enum QuestReward
{
    Gold,
    Experience,
    Both,
    Item,
}

public class QuestWindow : EditorWindow {

    public QuestTypes _QT;
    public GameObject _questItem;
    public GameObject[] _questItems;
    public QuestAmount _QA;
    public QuestReward _QR;
    public QuestManager _QM;

    private Quest _quest;

    public bool _prerequisite;
    private int _questIndex;
    private List<int> _questIndeces = new List<int>();
    private List<string> _questTitles = new List<string>();
    private int _followupID = 0;

    public int _zoneIndex;
    public Toggle _zoneAutoComplete;
    public bool _zoneAutoCompleteBool;
    public GameObject[] _zones;
    public string[] _zoneNames;

    public int _questItemsAmount = 1;
    public List<GameObject> _createdItems = new List<GameObject>();

    public string _questTitle;
    [TextArea]
    public string _questText;

    [TextArea]
    public string _questCompleteText;

    public NPC _NPC;

    public int _goldReward;
    public int _expReward;
    public GameObject _itemReward;

    private bool _getOnce = false;

    private int _qEnabled;
    

    [MenuItem("Quest Manager/Add Quest")]
     

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(QuestWindow));
        
    }

    void OnEnable()
    {
        GameObject.FindObjectOfType<QuestManager>().GetComponent<QuestManager>().GetAllQuests();
    }

    void OnGUI()
    {

        if(!_getOnce)
        {
           
            _getOnce = true;
        }

        GUILayout.Label("Add a new Quest!", EditorStyles.boldLabel);

        GUILayout.Label("Settings!");
        _QT = (QuestTypes)EditorGUILayout.EnumPopup("Type of Quest: ",_QT);
        

        if (_QT.ToString() == "Find")
        {
         
            _QA = (QuestAmount)EditorGUILayout.EnumPopup("Amount: ",_QA);
            if (_QA.ToString() == "Single")
            {

                _questItemsAmount = 1;

                GUILayout.Label("What to find?");
                _questItem = (GameObject)EditorGUILayout.ObjectField("Quest Item", _questItem, typeof(GameObject));
                
            }
            if (_QA.ToString() == "Multiple")
            {
                
                if (_questItemsAmount != null)
                {
                    _questItemsAmount = EditorGUILayout.IntField("Amount of objects: ", _questItemsAmount);
                  
                    _questItem = (GameObject) EditorGUILayout.ObjectField("QuestItem", _questItem, typeof(GameObject));
                   

                    if (_questItem != null)
                    {
                        if (_questItem.GetComponent<QuestObject>() == null)
                        {
                            _questItem.AddComponent<QuestObject>();
                            
                        }
                       
                        for (int i = 1; i < _questItemsAmount; i++)
                        {
                            
                             if(GameObject.Find(_questItem.name + i) == null) { 
                         
                                GUILayout.Label("Add this quest item to the scene");
                                if(GUILayout.Button("Create these Quest items"))
                                {
                                    
                                    GameObject _newGO = Instantiate(_questItem, _questItem.transform.position + new Vector3(2,0,2), Quaternion.identity) as GameObject;
                                    _newGO.name = _questItem.name + i;
                                    _createdItems.Add(_questItem);
                                    _createdItems.Add(_newGO);

                                    Debug.Log(_createdItems.Count);


                                }
                            }

                        }
                    }

                }
            }
        }

        if (_QT.ToString() == "Explore")
        {
            GUILayout.Label("Exploration Quest", EditorStyles.boldLabel);
                      
           
            _zones = GameObject.FindGameObjectsWithTag("Zone");

            _zoneNames = new string[_zones.Length];

            for (int i = 0; i < _zones.Length; i++)
            {
                _zoneNames[i] = _zones[i].name;

            }

            _zoneIndex = EditorGUILayout.Popup("Zone to Explore: ", _zoneIndex, _zoneNames);
            _zoneAutoCompleteBool =  EditorGUILayout.Toggle("Auto Complete the quest?: ",_zoneAutoComplete);
            Debug.Log(_zoneNames[_zoneIndex]);
            
        }

        if (_QT.ToString() == "Kill")
        {

        }

        if(_QT.ToString() != "None") {

        

            GUILayout.Label("Is this a follow up quest?", EditorStyles.boldLabel);
            _prerequisite = EditorGUILayout.Toggle("This is a follow up quest:", _prerequisite);

            if(_prerequisite)
            {

                
                _questIndeces = GameObject.FindObjectOfType<QuestManager>().GetComponent<QuestManager>().ReturnQuestIndeces();
                _questTitles = GameObject.FindObjectOfType<QuestManager>().GetComponent<QuestManager>().ReturnQuestTitles();

                string[] _combinedQuest = new string[_questIndeces.Count];
                string[] _combinedQuestSplit = new string[_questIndeces.Count];

                for (int i = 0; i < _questTitles.Count; i++)
                {
                    _combinedQuest[i] = "" + _questIndeces[i] + " - " + _questTitles[i] + "";
                    

                }

                    _questIndex = EditorGUILayout.Popup("Follow up of quest: ", _questIndex, _combinedQuest);
                     _followupID = _questIndeces[_questIndex];

                _qEnabled = 0;
            }
            else
            {
                _qEnabled = 1;
            }


            GUILayout.Label("Which NPC is giving the Quest", EditorStyles.boldLabel);
            _NPC = (NPC)EditorGUILayout.ObjectField("NPC", _NPC, typeof(NPC));

                if(_NPC != null)
                {
                    GUILayout.Label("What is the title for the quest", EditorStyles.boldLabel);
                    _questTitle = EditorGUILayout.TextField("Quest Title: ", _questTitle);

                    GUILayout.Label("What is the text for accepting the quest", EditorStyles.boldLabel);
                    _questText = EditorGUILayout.TextField("Quest Text", _questText, GUILayout.Height(150));


                    GUILayout.Label("What is the text for completing the quest", EditorStyles.boldLabel);
                    _questCompleteText = EditorGUILayout.TextField("Completing Text", _questCompleteText, GUILayout.Height(150));
                }
        }

        if(_questCompleteText != null && _QT.ToString() != "None" && _NPC != null)
        {
            _QR = (QuestReward)EditorGUILayout.EnumPopup("Reward: ", _QR);

            if(_QR.ToString() == "Gold")
            {
                _goldReward = EditorGUILayout.IntField("How much Gold: ", _goldReward);
            }
            if(_QR.ToString() == "Experience")
            {
                _expReward = EditorGUILayout.IntField("How much Exp: ", _expReward);
            }

            if(_QR.ToString() == "Both")
            {
                _goldReward = EditorGUILayout.IntField("How much Gold: ", _goldReward);
                _expReward = EditorGUILayout.IntField("How much Exp: ", _expReward);
            }
            if(_QR.ToString() == "Item")
            {
                _itemReward = (GameObject)EditorGUILayout.ObjectField("Reward Item", _itemReward, typeof(GameObject));
                _expReward = EditorGUILayout.IntField("How much Exp: ", _expReward);
            }

        }

        // ADDING THE ACTUAL QUEST ON BUTTON PRESS


        if(_NPC != null && _questCompleteText != ""  && _QR.ToString() != null) { 
            if(GUILayout.Button("Add Quest"))
            {
                if(_QT.ToString() == "Find") { 
                    if(_itemReward == null) {
                        _itemReward = new GameObject();
                        _itemReward.name = "Dummy";

                        _quest = new Quest(_questTitle, _questText, 1, _questItem, _NPC.ReturnNpcID(), _questItemsAmount, _questCompleteText, "",0, _goldReward, _expReward, _itemReward.name, _createdItems, _followupID, _qEnabled);
                        _questItem.name = _questItem.name;
                        DestroyImmediate(_itemReward);

                        _createdItems.Clear();
                    }
                    else
                    {
                        _quest = new Quest(_questTitle, _questText, 1, _questItem, _NPC.ReturnNpcID(), _questItemsAmount, _questCompleteText,"",0, _goldReward, _expReward, _itemReward.name, _createdItems, _followupID, _qEnabled);
                        _createdItems.Clear();
                    }
                    
                }

                if(_QT.ToString() == "Explore")
                {
                    _questItem = new GameObject();
                    _itemReward = new GameObject();

                        _questItem.name = "Dummy";
                        _itemReward.name = "Dummy";

                    _questItemsAmount = 0;
                    int _questZoneAutoComplete;
                    if(_zoneAutoComplete)
                    {
                        _questZoneAutoComplete = 1;

                    }
                    else
                    {
                        _questZoneAutoComplete = 0;
                    }
                        
                    Debug.Log(_questTitle + " - " + _questText + " - " + 2 + " - " + _questItem + " - " + _NPC.ReturnNpcID() + " - " + _questItemsAmount + " - " + _questCompleteText + " - " + _zoneNames[_zoneIndex] + " - " + _zoneAutoCompleteBool + " - " + _goldReward + " - " + _expReward + " - " + "");

                    _quest = new Quest(_questTitle, _questText, 2, _questItem, _NPC.ReturnNpcID(), _questItemsAmount, _questCompleteText, _zoneNames[_zoneIndex], _questZoneAutoComplete, _goldReward, _expReward, "Filler", _createdItems, _followupID, _qEnabled);

                    DestroyImmediate(_questItem);
                    DestroyImmediate(_itemReward);
                    Debug.Log("Deleted tmp gameobjects");

                }

            }
        }
    }

}