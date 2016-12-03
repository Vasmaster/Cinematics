using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class GameObjectDistance : BaseInputNode {

    private GameObject object1;
    private GameObject object2;

    public GameObjectDistance()
    {

        windowTitle = "GameObject Distance";
        hasInputs = false;

    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        object1 = (GameObject)EditorGUILayout.ObjectField(object1, typeof(GameObject), true);
        object2 = (GameObject)EditorGUILayout.ObjectField(object2, typeof(GameObject), true);
    }
    public override void DrawCurves()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        float retVal = 0;
        if(object1 && object2)
        {
            retVal = Vector3.Distance(object1.transform.position, object2.transform.position);
        }

        nodeResult = retVal.ToString();
    }

}
