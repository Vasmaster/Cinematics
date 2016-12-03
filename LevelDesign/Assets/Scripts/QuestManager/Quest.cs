using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.Data;
using System;

public class Quest {

    private int _questID;

    private QuestManager _QM = GameObject.FindObjectOfType<QuestManager>();
    private NPC _npc = GameObject.FindObjectOfType<NPC>();
    private QuestItem _QI;
    private QuestLog _QL = GameObject.FindObjectOfType<QuestLog>();
    private QuestZone _QZ;

    public Quest(string _title, string _text, int _questType, GameObject _toFetch, int _nwNpcID, int _amount, string _qCompleteText, string _qZone, int _qZoneAutoComplete, int _gold, int _exp, string _rewardItem, List<GameObject> _itemList, int _followupID, int _questEnabled)
    {

        

        if( _title != null && _text != null && _questType != null && _toFetch != null && _nwNpcID != null && _amount != null && _qCompleteText != null && _qZone != null && _qZoneAutoComplete != null && _gold != null && _exp != null && _rewardItem != null)
            {

        

            // Add the quest to the quest manager
            //  _QM.AddQuest(_title, _text, _toFetch, _nwNpcID, _amount);

            string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.

            IDbCommand dbcmd = dbconn.CreateCommand();
        
            string sqlQuery = String.Format("INSERT INTO Quests ( QuestTitle, QuestText, QuestType, QuestItem, QuestAmount, QuestMob, QuestActive, QuestComplete, QuestZone, QuestZoneAutoComplete, NPC_ID, QuestCompletedText, QuestGold, QuestExp, QuestRewardItem, FollowupID, QuestEnabled) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\", \"{12}\", \"{13}\", \"{14}\", \"{15}\", \"{16}\")",  _title, _text, _questType, _toFetch.name, _amount, "", 0,0, _qZone, _qZoneAutoComplete, _nwNpcID, _qCompleteText, _gold, _exp, _rewardItem, _followupID, _questEnabled);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbcmd.Dispose();
            dbcmd = null;

            IDbCommand lastID = dbconn.CreateCommand();
            string lastQuery = String.Format("SELECT last_insert_rowid()");
            lastID.CommandText = lastQuery;
            IDataReader idReader = lastID.ExecuteReader();

            while(idReader.Read())
            {
                _questID = idReader.GetInt32(0);
            
            }
            lastID.Dispose();
            lastID = null;

            dbconn.Close();
            dbconn = null;

            // If it is a Collect Quest
            if (_questType == 1) { 

                _QI = new QuestItem(_toFetch.name, _questID, _amount, _itemList);

            }

            // If it is a Explore quest
            if(_questType == 2)
            {
               _QZ = new QuestZone(_questID, _qZone, _qZoneAutoComplete);
            }

            Debug.Log("We have added the Quest: details : QuestTitle " + _title + " QuestText " + _text + " QuestType " + _questType + " Object to Collect " + _toFetch + " from NPC " + _nwNpcID + " Explore the zone " + _qZone + " Should it Auto Complete: " + _qZoneAutoComplete);


            if (_toFetch.name != "Dummy")
            {

                if (_amount > 1)
                {

                    for (int i = 0; i < _itemList.Count; i++)
                    {
                        if (_itemList[i] != null)
                        {
                            _itemList[i].GetComponent<QuestObject>().SetQuestID(_questID);
                        }
                    }
                }
                else
                {
                    GameObject.Find(_toFetch.name).GetComponent<QuestObject>().SetQuestID(_questID);
                }

            }
            
       
            
        }
        else
        {
            Debug.Log("Something is empty");

            if (_title != null)
            {
                Debug.Log("A");
                if (_text != null)
                {
                    Debug.Log("B");
                    if (_questType != null)
                    {
                        Debug.Log("C");
                        if (_toFetch != null)
                        {
                            Debug.Log("D");
                            if (_nwNpcID != null)
                            {
                                Debug.Log("E");
                                if (_amount != null)
                                {
                                    Debug.Log("F");
                                    if (_qCompleteText != null)
                                    {
                                        Debug.Log("G");
                                        if (_qZone != null)
                                        {
                                            Debug.Log("H");
                                            if (_qZoneAutoComplete != null)
                                            {
                                                Debug.Log("I");
                                                if (_gold != null)
                                                {
                                                    Debug.Log("J");
                                                    if (_exp != null)
                                                    {
                                                        Debug.Log("K");
                                                        if (_rewardItem != null)
                                                        {
                                                            Debug.Log("L");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    public int ReturnID()
    {
        return _questID;
    }

}
