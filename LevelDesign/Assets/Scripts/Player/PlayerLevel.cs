using UnityEngine;
using System.Collections;
using UnityEditor;

using Mono.Data.Sqlite;
using System.Data;
using System;

public class PlayerLevel : MonoBehaviour {

    [Header("Player Level System")]
    
    public int _currentLevel;
    public int _experienceNeeded;

    public float _experienceModifier;

    [SerializeField]
    private float _neededToLevel;

    public int _currentExperience;

	// Use this for initialization
	void Start () {

        

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void LevelUp()
    {

        _currentLevel++;
        Debug.Log("LEVEL UP");

    }

    public void PlayerProgress()
    {
        string conn = "URI=file:" + Application.dataPath + "/PlayerDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT PlayerExp " + "FROM Player" + " WHERE PlayerID = '" + 1 + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            _currentExperience = reader.GetInt32(0);
        }

        if(_currentExperience >= _neededToLevel)
        {
            LevelUp();
        }


        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void SetExpNeeded()
    {
        _neededToLevel = _experienceNeeded + _experienceNeeded * (_experienceModifier * _currentLevel);
    }

    public void FinishQuest(int _gold, int _exp, string _item)
    {
        //_currentExperience += _exp;
        

        string conn = "URI=file:" + Application.dataPath + "/PlayerDB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Player SET PlayerExp = PlayerExp + " + _exp  + ", PlayerGold = PlayerGold + " + _gold + " WHERE PlayerID = '" + 1 + "'";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();

        
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        PlayerProgress();
    }

}


[CustomEditor(typeof(PlayerLevel))]
public class PlayerLevelEditor : Editor
{
    private PlayerLevel _PL;

    public override void OnInspectorGUI()
    {

        _PL = (PlayerLevel)target;
        base.OnInspectorGUI();

        _PL.SetExpNeeded();
    }

}