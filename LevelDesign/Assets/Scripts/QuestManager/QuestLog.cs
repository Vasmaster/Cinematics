using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour {

    public Text[] _questTitleText;
    public Text[] _questAmountText;
    public int[] _questID;

    public string[] _questTitle;
    public string[] _questAmount;

    private int[] _questCollected;

    private int _qCounter = 0;
    private int _idCounter = 0;


    private QuestManager _QM;

    void Start()
    {
        _QM = GameObject.FindObjectOfType<QuestManager>();
        _QM.GetActiveQuests();
        
    }

    public void AddQuestLog(int _qID, string _qTitle, int _qAmount, int _collected)
    {

        _questID[_idCounter] = _qID;
        _questCollected = new int[_questID.Length];
        _questCollected[_idCounter] = _collected;

        Debug.Log("Adding " + _qTitle + " to slot " + _idCounter);

        _questTitle[_idCounter] = _qTitle;
        _questAmount[_idCounter] = _qAmount.ToString();

        DisplayQuestLog();

        _qCounter--;
        _idCounter++;

    }

    public void UpdateQuestLog(int _qID)
    {
        if (_questCollected.Length > 0)
        {
            for (int i = 0; i < _questCollected.Length; i++)
            {
                if (_questID[i] == _qID)
                {
                    _questCollected[i] += 1;

                    Debug.Log(_questCollected[i]);
                    DisplayQuestLog();
                }
            }
        }
    }


    public void DisplayQuestLog()
    {
        for (int i = 0; i < _questAmountText.Length; i++)
        {
            if(_questTitle[i] != "" ) { 
                _questTitleText[i].text = _questTitle[i];
                _questAmountText[i].text = "( " + _questCollected[i] + " / " + _questAmount[i] + " )";
            }
        }
    }

    public void ClearLog()
    {
        for (int i = 0; i < _questTitleText.Length; i++)
        {
            _questID[i] = 0;
            _questTitleText[i].text = "";
            _questAmountText[i].text = "";
            Debug.Log("Cleared ALL");
        }
    }
   

}
