using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class PlayerInventory : MonoBehaviour {

    // open db
    private int _itemID;
    private string _itemName;
    private string _itemText;
    private int _questID;
    private string _itemActive;
    private string _itemType;
    
    public void InventorySelectQuery(string _database, string _query)
    {
        string conn = "URI=file:" + Application.dataPath + "/" + _database +".db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = _query;
        //string sqlQuery = "SELECT * " + "FROM Quests" + " WHERE NPC_ID = '" + _npcID + "' AND  QuestEnabled = 1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _itemID = reader.GetInt32(0);
            _itemName = reader.GetString(1);
            _itemText = reader.GetString(2);
            _questID = reader.GetInt32(3);
            _itemActive = reader.GetString(4);
            _itemType = reader.GetString(5);
            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

}
