using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditing
{

    public class ObjectPlacement : MonoBehaviour
    {

        private static GameObject _objectToPlace;
        private static bool _hasObject = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_hasObject)
            {

            }
        }

        public static void PlaceItem(Object _tmp, int _id, string _name, ItemType _type, int _stats)
        {
            
            _objectToPlace = Instantiate(_tmp, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            if (GameObject.Find("ITEMS") != null)
            {
                _objectToPlace.transform.parent = GameObject.Find("ITEMS").transform;
            }

            else
            {
                GameObject _itemGO = new GameObject();
                _itemGO.name = "ITEMS";
            }
            
            _objectToPlace.AddComponent<ItemCollectable>();
            _objectToPlace.GetComponent<ItemCollectable>().SetValues(_id, _name, _type.ToString(), _stats);

            if(_objectToPlace != null)
            {
                _hasObject = true;
            }

            
        }

        public static GameObject ReturnPlacedObject()
        {
            return _objectToPlace;
        }

        
    }
    //learn how to name stuff 
}
