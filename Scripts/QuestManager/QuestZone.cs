using UnityEngine;
using System.Collections;

using Mono.Data.Sqlite;
using System.Data;
using System;

public class QuestZone {

	public QuestZone(int _questID, string _questZone, int _questAutoComplete)
    {
        GameObject _zone = GameObject.Find(_questZone);
        if(_zone.GetComponent<Zone>() == null)
        {
            _zone.AddComponent<Zone>().SetQuest(_questID, _questAutoComplete);
        }
        else
        {
            _zone.GetComponent<Zone>().SetQuest(_questID, _questAutoComplete);
        }
    }



}
