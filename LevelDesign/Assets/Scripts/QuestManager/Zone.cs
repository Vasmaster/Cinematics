using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour {

    public int _questID;
    public int _autoComplete;

    private QuestManager _QM;

    void Start()
    {
        _QM = GameObject.FindObjectOfType<QuestManager>();
    }
	
    public void SetQuest(int _qID, int _complete)
    {
        _questID = _qID;

    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
            
                _QM.FinishExploreQuest(_questID, _autoComplete);
                Debug.Log("FOUND IT");
            
        }

    }

}
 