using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;

namespace LevelEditing
{
    public class LevelEditingTools : MonoBehaviour
    {

        // COLLECTABLES
        private List<int> _itemID = new List<int>();
        private List<string> _itemNames = new List<string>();
        private List<string> _itemObjects = new List<string>();
        private List<ItemType> _itemTypes = new List<ItemType>();
        private List<int> _itemStats = new List<int>();



        public void GetAllItems()
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

        public int ReturnItemCount()
        {
            return _itemID.Count;
        }

        public List<string> ReturnItemNames()
        {
            return _itemNames;
        }

        public List<string> ReturnItemObjects()
        {
            return _itemObjects;
        }

        public int ReturnItemID(int _id)
        {
            return _itemID[_id];
        }

        public string ReturnItem(int _id)
        {
            return _itemObjects[_id];
        }

        public string ReturnItemName(int _id)
        {
            return _itemNames[_id];
        }

        public ItemType ReturnItemType(int _id)
        {
            return _itemTypes[_id];
        }
        public int ReturnItemStats(int _id)
        {
            return _itemStats[_id];
        }
    }

    [CustomEditor(typeof(LevelEditingTools))]

    public class DrawEditingTools : Editor
    {
        private int _actionIndex;
        private int _objectIndex;
        private int _itemIndex;
        private int _villageBuildingIndex;
        private int _villagePropIndex;

        private UnityEngine.Object[] _villageBuildings;
        private string[] _villageBuildingNames;

        private int _itemHealthIndex;

        private string[] _actionToTake = new string[] { "", "Add Object", "Add Item", "Add Actor", "Edit Selected", "Add Trigger" };
        private string[] _villageHierarchy = new string[] { "Village", "Village Buildings", "Village Props" };

        private string[] _itemNames = new string[] { "", "Health Pickup", "Mana Pickup", "Weapon Pickup", "Item Pickup" };

        private Editor _gameObjectEditor;

        private LevelEditingTools _tools;
        private bool _loadItemsOnce = false;

        private UnityEngine.Object _selectedHealthItem;


        private bool _placingObject;
        private GameObject _objectToPlace;


        void OnSceneGUI()
        {
            _tools = (LevelEditingTools)target;

            _villageBuildings = Resources.LoadAll("VillageBuildings");
            _villageBuildingNames = new string[_villageBuildings.Length + 1];
            _villageBuildingNames[0] = "";

            if (!_loadItemsOnce)
            {
                _tools.GetAllItems();
                _loadItemsOnce = true;
            }

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(new Vector2(10, 10), new Vector2(200, 800)));
            GUI.Box(new Rect(0, 0, 210, 400), "Level Editor");
            GUILayout.Space(30);
            GUILayout.Label("What Action");
            _actionIndex = EditorGUILayout.Popup(_actionIndex, _actionToTake);

            switch (_actionIndex)
            {
                case 0:
                    GUILayout.EndArea();
                    break;

                case 1:
                    for (int i = 0; i < _villageBuildings.Length; i++)
                    {
                        _villageBuildingNames[i + 1] = _villageBuildings[i].name;

                    }
                    _objectIndex = EditorGUILayout.Popup(_objectIndex, _villageHierarchy);


                    switch (_objectIndex)
                    {
                        case 0:

                            break;

                        case 1:
                            _villageBuildingIndex = EditorGUILayout.Popup(_villageBuildingIndex, _villageBuildingNames);
                            break;

                        case 2:
                            // Village Props
                            break;
                    }

                    GUILayout.EndArea();
                    if (_villageBuildingIndex > 0)
                    {
                        GUILayout.BeginArea(new Rect(new Vector2(250, 100), new Vector2(300, 300)));
                        _gameObjectEditor = Editor.CreateEditor(_villageBuildings[_villageBuildingIndex]);
                        _gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(200, 200), EditorStyles.whiteLabel);
                        GUILayout.EndArea();
                    }

                    break;

                case 2:
                    _itemIndex = EditorGUILayout.Popup(_itemIndex, _itemNames);


                    if (_itemIndex > 0)
                    {
                        switch (_itemIndex)
                        {
                            case 0:
                                break;

                            case 1:
                                _itemHealthIndex = EditorGUILayout.Popup(_itemHealthIndex, _tools.ReturnItemNames().ToArray());
                                _selectedHealthItem = Resources.Load("Collectables/Potions/HealthPotions/" + _tools.ReturnItem(_itemHealthIndex));



                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;

                            case 5:
                                break;
                        }
                    }
                    GUILayout.EndArea();
                    if (_selectedHealthItem != null)
                    {
                        GUILayout.BeginArea(new Rect(new Vector2(250, 100), new Vector2(300, 300)));
                        if (GUILayout.Button("Add Object"))
                        {
                            LevelEditing.ObjectPlacement.PlaceItem(_selectedHealthItem, _tools.ReturnItemID(_itemHealthIndex), _tools.ReturnItemName(_itemHealthIndex), _tools.ReturnItemType(_itemHealthIndex), _tools.ReturnItemStats(_itemHealthIndex));
                            _placingObject = true;
                            Debug.Log("We are placing an item");
                        }
                        _gameObjectEditor = Editor.CreateEditor(_selectedHealthItem);
                        _gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(200, 200), EditorStyles.whiteLabel);
                        GUILayout.EndArea();
                    }

                    break;
            }

            if(_placingObject)
            {
                _objectToPlace = LevelEditing.ObjectPlacement.ReturnPlacedObject();

                Ray _ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit _hit;

                if(Physics.Raycast(_ray, out _hit))
                {
                    
                }
                Vector3 _test = new Vector3(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.x, _hit.transform.localPosition.y, HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.z);
                Debug.Log(_test);

                //_objectToPlace.transform.position = _hit.collider.transform.localPosition;

            }


            Handles.EndGUI();
        }
    }
}
