using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Hotkeys))]
public class HotKeyEditor : Editor {

    
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        
        if(GUILayout.Button("Save"))
        {
            Hotkeys _hotkey = (Hotkeys)target;
            _hotkey.ChangeKeys();
        }      
        
    }

    


}
