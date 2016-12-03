﻿using UnityEngine;
using System.Collections;
using UnityEditor;



public class WalkNode : BaseInputNode {

    private BaseInputNode input1;
    private Rect input1Rect;

    private GameObject walkToNode;

    private HasSound _sound;

    private AudioClip _soundSource;

    public WalkNode()
    {

        windowTitle = "Walk to Node";
        hasInputs = true;
    }

 

    public override void DrawWindow()
    {
        base.DrawWindow();

        Event e = Event.current;

        GUILayout.Label("Walk to: ");

       
            walkToNode = (GameObject)EditorGUILayout.ObjectField(walkToNode, typeof(GameObject), true);

            if (walkToNode != null)
            {
                GameObject.Find("Node" + base.ReturnID()).GetComponent<NodeObject>().SetWayPoint(walkToNode);
            }

        _sound = (HasSound)EditorGUILayout.EnumPopup("Sound: ", _sound);

        if (_sound == HasSound.Yes)
        {
            _soundSource = (AudioClip)EditorGUILayout.ObjectField(_soundSource, typeof(AudioClip));

            if(_soundSource != null)
            {
                GameObject.Find("Node" + base.ReturnID()).GetComponent<NodeObject>().SetAudio(_soundSource);
            }

        }

        if (e.type == EventType.Repaint)
        {
            input1Rect = GUILayoutUtility.GetLastRect();
        }



    }

    public void SetAudio(AudioClip _sound)
    {
        _soundSource = _sound;
    }

    public void SetSound(string _hasSound)
    {
        if (_hasSound == "No")
        {
            _sound = HasSound.No;
        }
        if (_hasSound == "Yes")
        {
            _sound = HasSound.Yes;
        }
    }

    public override void SetInput(BaseInputNode input, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if (input1Rect.Contains(clickPos))
        {

            input1 = input;

        }

    }

    public void SetWayPoint(GameObject _waypoint)
    {
        walkToNode = _waypoint;
    }

    public string ReturnGameObject()
    {
        if (walkToNode != null)
        {
            return walkToNode.ToString();
        }
        else
        {
            return null;
        }
    }

    public override void Tick(float deltaTime)
    {
        if(walkToNode != null)
        nodeResult = walkToNode.ToString();

    }

    public override void DrawCurves()
    {
        if (input1)
        {
            Rect rect = windowRect;
            rect.x += input1Rect.x;
            rect.y += input1Rect.y;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(input1.windowRect, rect);
        }
    }

}
