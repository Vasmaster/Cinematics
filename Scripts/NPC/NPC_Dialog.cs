using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NPC_Dialog : MonoBehaviour {

    [Header("Text to display")]
    [TextArea(3, 10)]
    public string[] _NPCText;


    [Header("Quest Text")]
    [TextArea(3, 10)]
    public string[] _npcQuestText;
    private bool _haveMet;

    private DialogueManager _DM;

    // Use this for initialization
    void Start () {

        _haveMet = false;
        _DM = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Converse()
    {

       
        
        if (!_haveMet)
        {
            
            
                           
             
            
        }

        if(_haveMet)
        {
            if(_NPCText.Length > 0) {
                _DM.SetDialogue("", _NPCText[1], false);
            }
            else
            {
                _DM.SetDialogue("",_NPCText[0], false);
            }

            // check if quest complete

        }

    }

    public void StopConversation()
    {
        
            _DM.ExitDialogue(false);
    }

    public void HaveMet()
    {
        _haveMet = true;
    }

    public string ReturnQuestEnd()
    {
        return _npcQuestText[1];
    }

}
