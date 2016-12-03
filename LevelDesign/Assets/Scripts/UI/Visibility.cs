using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Visibility : MonoBehaviour {

    private Text _npcText;

	// Use this for initialization
	void Start () {

        _npcText = GameObject.FindGameObjectWithTag("NPC_Text").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        

        if(_npcText.text == "")
        {

            this.GetComponent<Image>().enabled = false;
        }
        if (_npcText.text != "")
        {

            this.GetComponent<Image>().enabled = true;
        }

    }
}
