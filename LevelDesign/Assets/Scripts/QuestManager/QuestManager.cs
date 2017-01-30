using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class QuestManager : MonoBehaviour {

    private DialogueManager _DM;
    private QuestLog _QL;
    private PlayerLevel _PL;

    private int _qID;
    private string _qTitle;
    private string _qText;
    private int _qType;
    private string _qItem;
    private int _qAmount;
    private string _qMob;
    private int _qActive;
    private int _qComplete;
    private string _qZone;
    private int _qZoneAutoComplete;
    private int _qCollected;
    private string _qCompleteText;
    private int _qGold;
    private int _qExp;
    private string _qItemReward;
    private int _qFollowup;

    private int _previousCompleted;

    private List<int> _questIndeces = new List<int>();
    private List<string> _questTitles = new List<string>();

    void Start()
    {
        _DM = GameObject.FindObjectOfType<DialogueManager>();
        _QL = GameObject.FindObjectOfType<QuestLog>();
        _PL = GameObject.FindObjectOfType<PlayerLevel>();
    }

    public void GetNpcQuestInfo(int _npcID)
    {

        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM Quests" +  " WHERE NPC_ID = '" + _npcID + "' AND  QuestEnabled = 1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _qID = reader.GetInt32(0);
            _qTitle = reader.GetString(1);
            _qText = reader.GetString(2);
            _qType = reader.GetInt32(3);
            _qItem = reader.GetString(4);
            _qAmount = reader.GetInt32(5);
            _qMob = reader.GetString(6);
            _qActive = reader.GetInt32(7);
            _qComplete = reader.GetInt32(8);
            _qZone = reader.GetString(9);
            _qZoneAutoComplete = reader.GetInt32(10);
            _qCollected = reader.GetInt32(12);
            _qCompleteText = reader.GetString(13);
            _qGold = reader.GetInt32(14);
            _qExp = reader.GetInt32(15);
            _qItemReward = reader.GetString(16);
            _qFollowup = reader.GetInt32(17);
           

         
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public void GetNpcCompletedInfo(int _npcID)
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestID, QuestTitle, QuestCompletedText, QuestGold, QuestExp, QuestRewardItem " + "FROM Quests" + " WHERE NPC_ID = '" + _npcID + "' AND  QuestComplete = 1 LIMIT 1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _qID = reader.GetInt32(0);
            _qTitle = reader.GetString(1);
            _qText = reader.GetString(2);
            _qCompleteText = reader.GetString(3);
            _qGold = reader.GetInt32(4);
            _qExp = reader.GetInt32(5);
            _qItemReward = reader.GetString(6);


        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void AcceptQuest()
    {
        Debug.Log(_qID);
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Quests " + "SET QuestActive = " + 1 +  " WHERE QuestID = '" + _qID + "'";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        _DM.ExitDialogue(false);

        _QL.AddQuestLog(_qID, _qTitle, _qAmount,0);

        GameObject.FindObjectOfType<NPC_Trigger>().SetPlayerReturns();

    }


    public void AddQuestItem(int _itemID, int _questID)
    {


            string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
            IDbConnection dbconn;
            Debug.Log("Adding QuestItem");
            IDbConnection _dbconn;
            _dbconn = (IDbConnection)new SqliteConnection(conn);
            _dbconn.Open(); //Open connection to the database.                       
            IDbCommand _dbcmd = _dbconn.CreateCommand();
            string _sqlQuery = "UPDATE Quests " + "SET QuestCollected = " + "QuestCollected + " + "'" + 1 + "' WHERE QuestID = '" + _questID + "'";
            Debug.Log(_sqlQuery);
            _dbcmd.CommandText = _sqlQuery;
            _dbcmd.ExecuteScalar();
            _dbcmd.Dispose();
            _dbcmd = null;
            _dbconn.Close();
            _dbconn = null;

     
       
    }

    public void CheckProgress(int _questID)
    {


        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestAmount, QuestCollected " + "FROM Quests" + " WHERE QuestID = '" + _questID + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        

        while (reader.Read())
        {
            _qAmount = reader.GetInt32(0);
            _qCollected = reader.GetInt32(1);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;

        if (_qCollected == _qAmount)
        {
            Debug.Log("Finished quest " + _questID);
            IDbCommand _dbcmd = dbconn.CreateCommand();
            string finishQuery = "UPDATE Quests SET QuestComplete = " + 1 + " WHERE QuestID = " + _questID;
            _dbcmd.CommandText = finishQuery;
            _dbcmd.ExecuteScalar();
            _dbcmd.Dispose();
            _dbcmd = null;
        }


       
        dbconn.Close();
        dbconn = null;
    }

    public void FinishQuest()
    {

        _PL.FinishQuest(_qGold, _qExp, _qItemReward);
        // set inactive in database
        //update quest log
        // update the player

        Debug.Log(_qID);
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Quests " + "SET QuestActive = " + 0 + ", QuestEnabled = 0 WHERE QuestID = '" + _qID + "'";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();

        string followQuery = "UPDATE Quests " + "SET QuestEnabled = " + 1 + " WHERE FollowupID = '" + _qID + "'";
        dbcmd.CommandText = followQuery;
        dbcmd.ExecuteScalar();


        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        _DM.ExitDialogue(false);

        Debug.Log(_qID);
    }

    public void FinishExploreQuest(int _questID, int _autoComplete)
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestGold, QuestExp, QuestZoneAutoComplete  " + "FROM Quests" + " WHERE QuestID = '" + _questID + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();


        while (reader.Read())
        {
            _qGold = reader.GetInt32(0);
            _qExp = reader.GetInt32(1);
            _qZoneAutoComplete = reader.GetInt32(2);
        }

        reader.Close();
        reader = null;

        string updateQuery = "UPDATE Quests SET QuestComplete = " + 1 + " WHERE QuestID = '" + _questID + "'";
        dbcmd.CommandText = updateQuery;
        dbcmd.ExecuteScalar();

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;


        if (_autoComplete == 1)
        {
            _PL.FinishQuest(_qGold, _qExp, _qItemReward);
        }
        
    }

    public void GetActiveQuests()
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM Quests" + " WHERE QuestActive = '" + 1 + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _qID = reader.GetInt32(0);
            _qTitle = reader.GetString(1);
            _qText = reader.GetString(2);
            _qType = reader.GetInt32(3);
            _qItem = reader.GetString(4);
            _qAmount = reader.GetInt32(5);
            _qMob = reader.GetString(6);
            _qActive = reader.GetInt32(7);
            _qComplete = reader.GetInt32(8);
            _qZone = reader.GetString(9);
            _qZoneAutoComplete = reader.GetInt32(10);
            _qCollected = reader.GetInt32(12);

            GameObject.FindObjectOfType<QuestLog>().GetComponent<QuestLog>().AddQuestLog(_qID, _qTitle, _qAmount, _qCollected);


        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

       

    }

    public void IsItemActive(int _itemID)
    {

    }

    public int GetFollowupInformation(int _questID)
    {
       

        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestComplete " + "FROM Quests" + " WHERE QuestID = '" + _questID + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _previousCompleted = reader.GetInt32(0);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return _previousCompleted;
    }

    public void GetAllQuests()
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestID, QuestTitle " + "FROM Quests";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("Adding " +  reader.GetInt32(0));
            _questIndeces.Add(reader.GetInt32(0));
            _questTitles.Add(reader.GetString(1));
            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public List<int> ReturnQuestIndeces()
    {
        return _questIndeces;
    }
    public List<string> ReturnQuestTitles()
    {
        return _questTitles;
    }

    // RETURN FUNCTION TO RETURN THE DATA AFTER WE HAVE CALLED THE GETQUESTINFO
    public int ReturnQuestID()
    {
        return _qID;
    }

    public int IsComplete()
    {
        return _qComplete;
    }

    public string ReturnQuestTitle()
    {
        return _qTitle;
    }

    public string ReturnQuestText()
    {
        return _qText;
    }

    public int ReturnQuestActive()
    {
        return _qActive;
    }
   
    public string ReturnQuestComplete()
    {
        return _qCompleteText;
    }

    public int ReturnFollowup()
    {
        return _qFollowup;
    }

    public void ClearQuests()
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM Quests" ;
        dbcmd.CommandText = sqlQuery;

        IDbCommand _deleteAll = dbconn.CreateCommand();
        string _deleteQuery = "UPDATE SQLITE_SEQUENCE SET SEQ = " + 0 + " WHERE NAME IS " + "'Quests'";
        _deleteAll.CommandText = _deleteQuery;

        _deleteAll.ExecuteScalar();
        IDataReader reader = dbcmd.ExecuteReader();


        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;

        _deleteAll.Dispose();
        _deleteAll = null;
        dbconn.Close();
        dbconn = null;

        Debug.Log("Cleared All Quests");
        GameObject.FindObjectOfType<QuestLog>().GetComponent<QuestLog>().ClearLog();
        
    }

    public void ClearCollected()
    {
        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Quests " + "SET QuestCollected = " + 0;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        Debug.Log("Cleared All Collected Items");
    }

    public void ClearActiveQuests()
    {

        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Quests " + "SET QuestActive = " + 0;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        Debug.Log("Cleared All Active Quests");

    }

    public void ClearAllQuestItems()
    {
        string conn = "URI=file:" + Application.dataPath + "/ItemDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM Items";
        dbcmd.CommandText = sqlQuery;

        IDbCommand _deleteAll = dbconn.CreateCommand();
        string _deleteQuery = "UPDATE SQLITE_SEQUENCE SET SEQ = " + 0 + " WHERE NAME IS " + "'Items'";
        _deleteAll.CommandText = _deleteQuery;

        _deleteAll.ExecuteScalar();
        IDataReader reader = dbcmd.ExecuteReader();


        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;

        _deleteAll.Dispose();
        _deleteAll = null;
        dbconn.Close();
        dbconn = null;

        Debug.Log("Cleared All QuestItems");
        GameObject.FindObjectOfType<QuestLog>().GetComponent<QuestLog>().ClearLog();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{

    private QuestManager _QM;

    public override void OnInspectorGUI()
    {
        _QM = (QuestManager)target;

        base.OnInspectorGUI();
        if(GUILayout.Button("Clear all Quest"))
        {

            _QM.ClearQuests();
         
            

        }
        if(GUILayout.Button("Clear all Collected"))
        {
            _QM.ClearCollected();
        }

        if(GUILayout.Button("Clear all Active Quests"))
        {
            _QM.ClearActiveQuests();
        }
        if (GUILayout.Button("Clear all Active QuestItems"))
        {
            _QM.ClearAllQuestItems();
        }
    }

}
#endif