﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quest
{

    public class Zone : MonoBehaviour
    {

        [SerializeField]
        private string _zoneName;

        [SerializeField]
        private string _zoneDescription;

        [SerializeField]
        private int _questID;

        private DialogueManager _DM;

        void OnEnable()
        {
            _DM = GameObject.FindObjectOfType<DialogueManager>();
        }

        public void SetNames(string _name, string _desc)
        {
            _zoneName = _name;
            _zoneDescription = _desc;
        }

        public string ReturnName()
        {
            return _zoneName;
        }

        public string ReturnDescription()
        {
            return _zoneDescription;
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.tag == "Player")
            {
                _DM.SetShowZone(true, _zoneName, _zoneDescription);

                if (_questID > 0)
                {
                    if (Quest.QuestDatabase.ReturnZoneAutoComplete(_questID))
                    {
                        // END QUEST
                        Quest.QuestDatabase.SetQuestComplete(_questID);
                    }
                    else
                    {
                        Quest.QuestDatabase.SetQuestComplete(_questID);
                    }
                }

            }
        }

        public void SetQuestID(int _id)
        {
            _questID = _id;
        }
    }
}