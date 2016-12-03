using UnityEngine;
using System.Collections;

using Mono.Data.Sqlite;
using System.Data;
using System;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string conn = "URI=file:" + Application.dataPath + "/QuestDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT QuestID, QuestTitle " + "FROM Quests";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while(reader.Read()) { 
            int _qID = reader.GetInt32(0);
            string _qTitle = reader.GetString(1);
            

            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }



}
