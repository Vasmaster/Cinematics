using UnityEngine;
using System.Collections;

public class NPC_Trigger : MonoBehaviour {

    private QuestManager _QM;
    private DialogueManager _DM;

    public bool _playerReturns;

    void Start()
    {

        _QM = GameObject.FindObjectOfType<QuestManager>();
        _DM = GameObject.FindObjectOfType<DialogueManager>();
    }

	void OnTriggerEnter(Collider coll)
    {
        if (transform.parent.GetComponent<NPC>()._AutoCommunicate)
        {
            if (coll.name == "Player")
            {
               
                transform.parent.GetComponent<NPC>().PlayerInteraction(coll.gameObject, false);

                // THIS IS THE PART WHERE WE COMMUNICATE WITH THE DATABASE
                // ALWAYS FIRST GET THE QUEST DATA FROM THE DATABASE BEFORE CALLING ANY RETURN FUNCTION
                _QM.GetNpcQuestInfo(transform.parent.GetComponent<NPC>().ReturnNpcID());
                   

                // IF THE QUEST IS NOT ACTIVE GIVE THE PLAYER THE OPTION TO ACCEPT THE QUEST
                if(_QM.ReturnQuestActive() == 0) { 

                    Debug.Log(_QM.ReturnQuestTitle());

                }
            }
        }

        // IF THE PLAYER HAS SELECTED THE NPC
        if(transform.parent.GetComponent<NPC>().ReturnIsSelected())
        {
            if (coll.name == "Player")
            {
               
                    transform.parent.GetComponent<NPC>().PlayerInteraction(coll.gameObject, false);

                    // THIS IS THE PART WHERE WE COMMUNICATE WITH THE DATABASE
                    // ALWAYS FIRST GET THE QUEST DATA FROM THE DATABASE BEFORE CALLING ANY RETURN FUNCTION
                    _QM.GetNpcQuestInfo(transform.parent.GetComponent<NPC>().ReturnNpcID());


                    // IF THE QUEST IS NOT ACTIVE GIVE THE PLAYER THE OPTION TO ACCEPT THE QUEST
                    if (_QM.ReturnQuestActive() == 0)
                    {
                        
                            _DM.SetDialogue(_QM.ReturnQuestTitle(), _QM.ReturnQuestText(), true);
                        

                        Debug.Log(_QM.ReturnQuestTitle());
                    }
                    else
                    {
                        if (_QM.IsComplete() == 1)
                        {
                            _DM.SetEndQuest(_QM.ReturnQuestID(), _QM.ReturnQuestTitle(), _QM.ReturnQuestComplete());
                        }
                        else
                        {
                            _DM.SetDialogue(_QM.ReturnQuestTitle(), _QM.ReturnQuestText(), true);
                        }
                    }


               

            }
        }
        

    }

    void OnTriggerExit(Collider coll)
    {

        _DM.ExitDialogue(false);
    }
    /*
    void OnTriggerStay(Collider coll)
    {
        if (transform.parent.GetComponent<NPC>()._AutoCommunicate)
        {
            if (coll.name == "Player")
            {
                transform.parent.GetComponentInChildren<NPC_Dialog>().Converse();
                transform.parent.GetComponentInChildren<NPC_Dialog>().HaveMet();
                transform.parent.GetComponent<NPC>().PlayerInteraction(coll.gameObject, false);

                _QM.GetQuestData(transform.parent.GetComponent<NPC>()._npcID);
            }
        }

        // IF THE PLAYER HAS SELECTED THE NPC
        if (transform.parent.GetComponent<NPC>().ReturnIsSelected())
        {
            if (coll.name == "Player")
            {
                transform.parent.GetComponentInChildren<NPC_Dialog>().Converse();
                transform.parent.GetComponentInChildren<NPC_Dialog>().HaveMet();
                transform.parent.GetComponent<NPC>().PlayerInteraction(coll.gameObject, false);
                _QM.GetQuestData(transform.parent.GetComponent<NPC>()._npcID);
            }
        }


    }
    */

    public void SetPlayerReturns()
    {
        _playerReturns = true;
    }

}
