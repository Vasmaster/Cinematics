using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text _textTitle;
    public Text _textBox;
    public Image _textBG;
    public Button _acceptButton;
    public Button _finishQuest;

    private Quest _quest;
    private QuestManager _QM;

	// Use this for initialization
	void Start () {


        _QM = GameObject.FindObjectOfType<QuestManager>();



        _textTitle.gameObject.SetActive(false);
        _textBox.gameObject.SetActive(false);
        _textBG.gameObject.SetActive(false);
        _acceptButton.gameObject.SetActive(false);
        _finishQuest.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetDialogue(string _title, string _text, bool _buttonVis)
    {

        _textTitle.gameObject.SetActive(true);
        _textBox.gameObject.SetActive(true);
        _textBG.gameObject.SetActive(true);
        _acceptButton.gameObject.SetActive(_buttonVis);
        

        _textTitle.text = _title;
        _textBox.text = _text;
      
        
        
    }

    public void ExitDialogue(bool _buttonVis)
    {
        _textTitle.gameObject.SetActive(false);
        _textBox.gameObject.SetActive(false);
        _textBG.gameObject.SetActive(false);
        _acceptButton.gameObject.SetActive(_buttonVis);
       
    }

    public void SetEndQuest(int _qID, string _title, string _completeText)
    {
        _textTitle.gameObject.SetActive(true);
        _textBox.gameObject.SetActive(true);
        _textBG.gameObject.SetActive(true);
        _textTitle.text = _title;
        _textBox.text = _completeText;
        _finishQuest.gameObject.SetActive(true);

    }

    


}
