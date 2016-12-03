using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Hotkeys : MonoBehaviour {

    public int _defaultKey;
    public int _meleeKey;
    public int _shadowKey;
    public int _disengageKey;

    public Text _keyText_1;
    public Text _keyText_2;
    public Text _keyText_3;
    public Text _keyText_4;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeKeys()
    {

        _keyText_1.GetComponent<Text>();
        _keyText_2.GetComponent<Text>();
        _keyText_3.GetComponent<Text>();
        _keyText_4.GetComponent<Text>();

        _keyText_1.text =_defaultKey.ToString();
        _keyText_2.text = _meleeKey.ToString();
        _keyText_3.text = _shadowKey.ToString();
        _keyText_4.text = _disengageKey.ToString();
       
            
    }



}
