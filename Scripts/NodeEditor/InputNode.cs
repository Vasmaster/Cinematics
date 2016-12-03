using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class InputNode : BaseInputNode {

    private InputType inputType;




    public enum InputType
    {
        Number,
        Randomization,
        GameObject,
    }

    private string randomFrom = "";
    private string randomTo = "";

    private string inputValue = "";
    private GameObject inputObject;

    public InputNode()
    {
        windowTitle = "Input Node";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        inputType = (InputType)EditorGUILayout.EnumPopup("Input Type: ", inputType);

        if(inputType == InputType.Number)
        {
            inputValue = EditorGUILayout.TextField("Value: ", inputValue);

        }
        else if (inputType == InputType.Randomization) {

            randomFrom = EditorGUILayout.TextField("From: ", randomFrom);
            randomTo = EditorGUILayout.TextField("To: ", randomTo);

            if(GUILayout.Button("Random"))
            {
                calculateRandom();
            }

        }

        else if (inputType == InputType.GameObject) {
            inputObject = (GameObject)EditorGUILayout.ObjectField("GameObject: ", inputObject, typeof(GameObject));
        }

        nodeResult = inputValue.ToString();
        
    }

    public override void DrawCurves()
    {
        base.DrawCurves();
    }

    void calculateRandom()
    {
        float rFrom = 0;
        float rTo = 0;

        float.TryParse(randomFrom, out rFrom);
        float.TryParse(randomTo, out rTo);

        int randFrom = (int)(rFrom * 10);
        int randTo = (int)(rTo * 10);

        int selected = UnityEngine.Random.Range(randFrom, randTo + 1);

        float selectedValue = selected / 10;

        inputValue = selectedValue.ToString(); 
    }

    public override string  getResult()
    {
        return inputValue.ToString();
    }

    public override void Tick(float deltaTime)
    {
        
    }

}
