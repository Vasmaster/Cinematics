using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.Data;
using System;

public class QuestItem : MonoBehaviour {

    private int ItemID;

    

	public QuestItem(string _itemName, int _questID, int _questAmount, List<GameObject> _itemList)
    {
        string conn = "URI=file:" + Application.dataPath + "/ItemDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        Debug.Log(_itemList.Count);

        if (_questAmount > 0)
        {
            bool _hasSet = false;

            for (int i = 0; i < _itemList.Count; i++)
            {

                Debug.Log("Adding ItemIDS" + _itemList.Count);

                IDbCommand[] dbcmd = new IDbCommand[_itemList.Count];
                string[] sqlQuery = new string[_itemList.Count];
                IDataReader[] idreader = new IDataReader[_itemList.Count];
                int[] _itemID = new int[_itemList.Count];

                dbcmd[i] = dbconn.CreateCommand();
                sqlQuery[i] = String.Format("INSERT INTO Items (ItemName, QuestID, IsActive) VALUES (\"{0}\", \"{1}\", \"{2}\")", _itemName, _questID, 1);
                dbcmd[i].CommandText = sqlQuery[i];
                dbcmd[i].ExecuteScalar();
                dbcmd[i].Dispose();

                dbcmd[i] = dbconn.CreateCommand();
                sqlQuery[i] = String.Format("SELECT last_insert_rowid()");
                dbcmd[i].CommandText = sqlQuery[i];
                idreader[i] = dbcmd[i].ExecuteReader();

                while (idreader[i].Read())
                {
                    _itemID[i] = idreader[i].GetInt32(0);
                    Debug.Log(_itemID[i]);


                    Debug.Log(_itemName);
                }


                if (_itemList[i] != null) { 
                    _itemList[i].GetComponent<QuestObject>().SetItemID(_itemID[i]);
                }

                dbcmd[i].Dispose();
                dbcmd[i] = null;
                
                     
                   
              




            }
            
        }
        else {
            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = String.Format("INSERT INTO Items (ItemName, QuestID, IsActive) VALUES (\"{0}\", \"{1}\", \"{2}\")", _itemName, _questID, 1);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbcmd.Dispose();
            dbcmd = null;

            IDbCommand lastID = dbconn.CreateCommand();
            string lastQuery = String.Format("SELECT last_insert_rowid()");
            lastID.CommandText = lastQuery;
            IDataReader idReader = lastID.ExecuteReader();

            while (idReader.Read())
            {
                ItemID = idReader.GetInt32(0);

            }
            if (GameObject.Find(_itemName).GetComponent<QuestObject>() == null)
            {
                GameObject.Find(_itemName).AddComponent<QuestObject>().SetItemID(ItemID);
            }
            else
            {
                GameObject.Find(_itemName).GetComponent<QuestObject>().SetItemID(ItemID);
            }
        }

        dbconn.Close();
        dbconn = null;
    }

   
}
