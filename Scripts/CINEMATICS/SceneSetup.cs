using UnityEngine;
using System.Collections;
using UnityEditor;

public class SceneSetup : EditorWindow {

    public GameObject _gameManager;
    public GameObject _lux;
    public GameObject _cinematicCharacter;

    [MenuItem("Cinematics Manager/Scene Setup")]

    static void ShowEditor()
    {

        SceneSetup _setup = EditorWindow.GetWindow<SceneSetup>();



    }

    void Update()
    {
        Repaint();
    }

    void OnGUI()
    {

        if(GUILayout.Button("Set up scene"))
        {

            GameObject _addGM = Instantiate(_gameManager, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            GameObject _addLux = Instantiate(_lux, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            GameObject _addCinematicChar = Instantiate(_cinematicCharacter, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            GameObject _nodeParent = new GameObject();
            _nodeParent.tag = "NodeParent";
            _nodeParent.name = "NDDES";

            GameObject _wayPointParent = new GameObject();

            _wayPointParent.name = "WAYPOINTS";


        }

        if (GUILayout.Button("Add Waypoint"))
        {

            GameObject _waypoint = new GameObject();
            _waypoint.name = "CinematicWaypoint";


        }



    }

}
