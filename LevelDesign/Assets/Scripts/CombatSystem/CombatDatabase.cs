﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;


namespace CombatSystem
{

    public class CombatDatabase : MonoBehaviour
    {

        private static List<int> _spellID = new List<int>();
        private static List<string> _spellNames = new List<string>();
        private static List<string> _spellDescriptions = new List<string>();
        private static List<SpellTypes> _spellTypes = new List<SpellTypes>();
        private static List<float> _spellValues = new List<float>();
        private static List<float> _spellCastTimes = new List<float>();
        private static List<string> _spellPrefabs = new List<string>();
        private static List<string> _spellIcons = new List<string>();
        private static List<float> _chargeRange = new List<float>();
        private static List<float> _disDistance = new List<float>();
        private static List<float> _blinkRange = new List<float>();
        private static List<float> _spellMana = new List<float>();
        private static List<Abilities> _ability = new List<Abilities>();
        private static List<float> _spellCooldown = new List<float>();


        public static void AddSpell(string _name, string _desc, SpellTypes _type, float _value, float _manaCost, float _casttime, string _prefab, string _icon, float _chargeRange, float _disDistance, float _blinkRange, Abilities _ability, float _cooldown)
        {

            string conn = "URI=file:" + Application.dataPath + "/Databases/PlayerSpellsDB.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.

            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = String.Format("INSERT INTO PlayerSpells (SpellName, SpellDesc, SpellType, SpellValue, SpellCasttime, SpellPrefab, SpellIcon, ChargeRange, DisEngageDistance, BlinkRange, SpellManaCost, Ability, SpellCooldown) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\", \"{12}\")", _name, _desc, _type.ToString(), _value, _casttime, _prefab, _icon, _chargeRange, _disDistance, _blinkRange, _manaCost, _ability.ToString(), _cooldown);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();

            dbcmd.Dispose();
            dbcmd = null;
                       
            dbconn.Close();
            dbconn = null;

        }

        public static void SaveSpell(int _id, string _name, string _desc, SpellTypes _type, float _value, float _manaCost, float _casttime, string _prefab, string _icon, float _chargeRange, float _disDistance, float _blinkRange, Abilities _ability, float _cooldown)
        {
            string conn = "URI=file:" + Application.dataPath + "/Databases/PlayerSpellsDB.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.

            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = String.Format("UPDATE PlayerSpells SET SpellName = '" + _name + "', SpellDesc = '" + _desc + "', SpellType = '" + _type.ToString() + "', SpellValue = '" + _value + "', SpellCasttime = '" + _casttime + "', SpellPrefab = '" + _prefab + "', SpellIcon = '" + _icon + "', ChargeRange = '" + _chargeRange + "', DisEngageDistance = '" + _disDistance + "', BlinkRange = '" + _blinkRange + "', SpellManaCost = '" + _manaCost + "', Ability = '" + _ability.ToString() + "', SpellCooldown = '" + _cooldown + "' WHERE SpellID = '" + _id + "'");
            dbcmd.CommandText = sqlQuery;
            
            dbcmd.ExecuteScalar();

            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        public static void GetAllSpells()
        {
            string conn = "URI=file:" + Application.dataPath + "/Databases/PlayerSpellsDB.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT * FROM PlayerSpells";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                _spellID.Add(reader.GetInt32(0));
                _spellNames.Add(reader.GetString(1));
                _spellDescriptions.Add(reader.GetString(2));
                if(reader.GetString(3) == "Buff")
                {
                    _spellTypes.Add(SpellTypes.Buff);
                }
                if(reader.GetString(3) == "Damage")
                {
                    _spellTypes.Add(SpellTypes.Damage);
                }
                if (reader.GetString(3) == "AOE")
                {
                    _spellTypes.Add(SpellTypes.AOE);
                }
                if (reader.GetString(3) == "Healing")
                {
                    _spellTypes.Add(SpellTypes.Healing);
                }
                if(reader.GetString(3) == "Ability")
                {
                    _spellTypes.Add(SpellTypes.Ability);
                }
                
                _spellValues.Add(reader.GetFloat(4));
                _spellCastTimes.Add(reader.GetFloat(5));
                _spellPrefabs.Add(reader.GetString(6));
                _spellIcons.Add(reader.GetString(7));
                _chargeRange.Add(reader.GetFloat(8));
                _disDistance.Add(reader.GetFloat(9));
                _blinkRange.Add(reader.GetFloat(10));
                _spellMana.Add(reader.GetInt32(11));

                if(reader.GetString(12) == "Disengage")
                {
                    _ability.Add(Abilities.Disengage);
                }
                if(reader.GetString(12) == "Charge")
                {
                    _ability.Add(Abilities.Charge);
                }
                if(reader.GetString(12) == "Blink")
                {
                    _ability.Add(Abilities.Blink);
                }
                else
                {
                    _ability.Add(Abilities.None);
                }
                _spellCooldown.Add(reader.GetFloat(13));

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        public static List<int> ReturnAllSpellID()
        {
            return _spellID;
        }

        public static List<string> ReturnAllSpellNames()
        {
            return _spellNames;
        }

        public static int ReturnSpellCount()
        {
            return _spellID.Count;
        }

        public static string ReturnSpellIcon(int _id)
        {
            return _spellIcons[_id];
        }
                

        public static float ReturnSpellValue(int _id)
        {
            return _spellValues[_id];
        }

        public static int ReturnSpellID(int _id)
        {
            return _spellID[_id];
        }

        public static float ReturnCastTime(int _id)
        {
            return _spellCastTimes[_id];
        }

        public static SpellTypes ReturnSpellType(int _id)
        {
            return _spellTypes[_id];
        }

        public static string ReturnSpellPrefab(int _id)
        {
            return _spellPrefabs[_id];
        }

        public static float ReturnSpellManaCost(int _id)
        {
            return _spellMana[_id];
        }

        public static string ReturnSpellName(int _id)
        {
            return _spellNames[_id];
        }

        public static string ReturnSpellDesc(int _id)
        {
            return _spellDescriptions[_id];
        }

        public static float ReturnChargeRange(int _id)
        {
            return _chargeRange[_id];
        }

        public static float ReturnDisengageDistance(int _id)
        {
            return _disDistance[_id];
        }

        public static float ReturnBlinkRange(int _id)
        {
            return _blinkRange[_id];
        }

        public static Abilities ReturnAbility(int _id)
        {
            return _ability[_id];
        }

        public static float ReturnSpellCooldown(int _id)
        {
            return _spellCooldown[_id];
        }

        public static List<float> ReturnAllSpellCooldowns()
        {
            return _spellCooldown;
        }

    }
}
