using UnityEngine;
using System.Collections;

namespace Quest
{

    public class NPC_Trigger : MonoBehaviour
    {
        private DialogueManager _DM;
        private NPC _npc;

        void Start()
        {
            _DM = GameObject.FindObjectOfType<DialogueManager>();
            _npc = this.GetComponentInChildren<NPC>();
            
        }

        void OnTriggerEnter(Collider coll)
        {
            
            // IF THE PLAYER HAS SELECTED THE NPC
            if (_npc.ReturnIsSelected())
            {
                if (coll.name == "Player")
                {
                    _npc.PlayerInteraction(coll.gameObject, false);

                    if (!_npc.ReturnMetBefore())
                    {
                        // If the NPC is not a quest giver
                        if (!_npc.ReturnQuestGiver())
                        {
                            _DM.SetDialogue("", _npc.ReturnDialogue1(), false, -1, -1);
                        }

                        if (_npc.ReturnQuestGiver() && !Quest.QuestDatabase.GetActiveFromNPC(_npc.ReturnNpcID()))
                        {
                            // IF THE NPC HAS A QUEST
                            Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                            _DM.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestText(), true, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                        }
                        else
                        {
                            Debug.Log("QUEST IS ACTIVE");
                        }
                        _npc.HasMetPlayer(true);
                        PlayerPrefs.SetString("MetNPC_" + _npc.ReturnNpcName(), "True");
                    }
                    if (_npc.ReturnMetBefore())
                    {

                        //Debug.Log(Quest.QuestDatabase.CheckQuestCompleteNpc(_npc.ReturnNpcID()));

                        if (!_npc.ReturnQuestGiver())
                        {
                            if (!_npc.ReturnQuestGiver())
                            {
                                _DM.SetDialogue("", _npc.ReturnDialogue2(), false, -1, -1);
                            }
                        }

                        if (_npc.ReturnQuestGiver() && Quest.QuestDatabase.GetActiveFromNPC(_npc.ReturnNpcID()))
                        {
                            Debug.Log("WE HAVE QUEST");
                            if (!Quest.QuestDatabase.CheckQuestCompleteNpc(_npc.ReturnNpcID()))
                            {
                                Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                                _DM.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestText(), false, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                            }
                            if (Quest.QuestDatabase.CheckQuestCompleteNpc(_npc.ReturnNpcID()))
                            {

                                Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                                _DM.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestCompleteText(), true, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                            }
                        }
                    }
                }

           }
           
        }

        void OnTriggerExit(Collider coll)
        {
            _npc.PlayerInteraction(coll.gameObject, true);
            _DM.ExitDialogue(false);
        }


    }
}