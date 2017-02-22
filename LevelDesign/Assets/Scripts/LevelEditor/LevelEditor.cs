using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;


#if UNITY_EDITOR

namespace LevelEditing
{

    public class LevelEditor : EditorWindow
    {

        private UnityEngine.Object[] _villageBuildings;
        private Editor _gameObjectEditor;

        private UnityEngine.Object _selectedObject;
        private int _selectedIndex;
        private string _selectedParent;

        private bool _worldUnfolded = false;
        private bool[] _unfold;
        private bool[] _collectableUnfold;

        private string[] _hierarchy;
        private string[] _collectables;


        private Texture2D _villageBuildingTex;

        // COLLECTABLES
        private List<int> _itemID = new List<int>();
        private List<string> _itemNames = new List<string>();
        private List<string> _itemObjects = new List<string>();
        private List<ItemType> _itemTypes = new List<ItemType>();
        private List<int> _itemStats = new List<int>();


        [MenuItem("Level Design/Level Editor")]

        static void ShowEditor()
        {

            LevelEditor editor = EditorWindow.GetWindow<LevelEditor>();

        }

        void OnEnable()
        {
            _villageBuildings = Resources.LoadAll("VillageBuildings");
            GetAllItems();
            _hierarchy = new string[] { "World Building", "Village", "Village Buildings", "Village Props", "Vikings", "Viking Buildings", "Viking Props" };
            _collectables = new string[] { "Collectables", "Health Pickup", "Mana Pickup", "Weapon Pickup", "Armour Pickup" };
            _unfold = new bool[_hierarchy.Length];
            _collectableUnfold = new bool[_collectables.Length];
            _unfold[0] = false;
            _collectableUnfold[0] = false;

        }

        void OnGUI()
        {

            GUILayout.BeginArea(new Rect(new Vector2(0, 25), new Vector2(300, 1000)));
            _unfold[0] = EditorGUILayout.Foldout(_unfold[0], _hierarchy[0]);
            _collectableUnfold[0] = EditorGUILayout.Foldout(_collectableUnfold[0], _collectables[0]);

            if (_unfold[0])
            {
                for (int i = 1; i < _hierarchy.Length; i++)
                {
                    EditorGUI.indentLevel++;
                    _unfold[i] = EditorGUILayout.Foldout(_unfold[i], _hierarchy[i]);
                    if (_unfold[2])
                    {
                        if (_hierarchy[i] == "Village Buildings")
                        {

                            for (int j = 0; j < _villageBuildings.Length; j++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(60);
                                if (GUILayout.Button(_villageBuildings[j].name, "Label"))
                                {
                                    _selectedObject = _villageBuildings[j];
                                    _selectedParent = "World";
                                    

                                }
                                GUILayout.EndHorizontal();
                            }

                        }
                    }

                    if (_hierarchy[i] == "Village Props")
                    {
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                }


            }


            if (_collectableUnfold[0])
            {
                EditorGUI.indentLevel++;
                for (int i = 1; i < _collectables.Length; i++)
                {
                    _collectableUnfold[i] = EditorGUILayout.Foldout(_collectableUnfold[i], _collectables[i]);

                    if (_collectableUnfold[i])
                    {
                        for (int j = 0; j < _itemNames.Count; j++)
                        {
                            if (_itemTypes[j] == ItemType.Health && _collectables[i] == "Health Pickup")
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(60);
                                if (GUILayout.Button(_itemNames[j], "Label"))
                                {
                                    _selectedObject = Resources.Load("Collectables/Potions/" + _itemObjects[j]);
                                    _selectedIndex = j;
                                    _selectedParent = "Collectables";
                                    Debug.Log(_itemObjects[j]);

                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                   
                    
                }
            }

            GUILayout.EndArea();


            // PREVIEW AREA
            GUILayout.BeginArea(new Rect(new Vector2(250, 25), new Vector2(500, 500)));

            if (_selectedObject != null)
            {
                GUILayout.Label(_selectedObject.name);

                if (GUILayout.Button("Place Object"))
                {
                    if (_selectedParent == "World")
                    {
                     //   LevelEditing.ObjectPlacement.PlaceObject(_selectedObject, 0, "", ItemType.None,0);
                    }
                    if(_selectedParent == "Collectables")
                    {
                        //LevelEditing.ObjectPlacement.PlaceObject(_selectedObject, _itemID[_selectedIndex] ,_itemNames[_selectedIndex], _itemTypes[_selectedIndex], _itemStats[_selectedIndex]);
                    }

                }
                
                _gameObjectEditor = Editor.CreateEditor(_selectedObject);
                _gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);

            }

            GUILayout.EndArea();



        }

        void GetAllItems()
        {
            string conn = "URI=file:" + Application.dataPath + "/Databases/ItemDB.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT * " + "FROM Items";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {

                _itemID.Add(reader.GetInt32(0));
                _itemNames.Add(reader.GetString(1));
                

                if (reader.GetString(3) == "Weapon")
                {
                    _itemTypes.Add(ItemType.Weapon);
                }

                if (reader.GetString(3) == "Health")
                {
                    _itemTypes.Add(ItemType.Health);
                }

                if (reader.GetString(3) == "Mana")
                {
                    _itemTypes.Add(ItemType.Mana);
                }

                if (reader.GetString(3) == "QuestItem")
                {
                    _itemTypes.Add(ItemType.QuestItem);
                }
                if (reader.GetString(3) == "Armour")
                {
                    _itemTypes.Add(ItemType.Armour);
                }


                _itemStats.Add(reader.GetInt32(4));
                _itemObjects.Add(reader.GetString(6));
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        void OnSceneGUI ()
        {
            if(GUILayout.Button("test"))
            {

            }
        }

    }
}
#endif