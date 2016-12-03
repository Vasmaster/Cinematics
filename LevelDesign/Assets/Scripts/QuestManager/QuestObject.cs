using UnityEngine;
using System.Collections;

public class QuestObject : MonoBehaviour {

    [SerializeField]
    private int _questID;
    private QuestManager _QM;
    private QuestLog _QL;

    public int _itemID;

    void Start()
    {
        _QM = GameObject.FindObjectOfType<QuestManager>();
        _QL = GameObject.FindObjectOfType<QuestLog>();
       // _QM.GetActiveQuests();

    }

    void OnTriggerEnter(Collider coll)
    {

        if(coll.tag == "Player")
        {
            if (_QM.ReturnQuestID() == _questID)
            {
                _QM.AddQuestItem(_itemID, _questID);
                Destroy(this.gameObject);
                _QL.UpdateQuestLog(_questID);
                _QM.CheckProgress(_questID);
            }
            else
            {
            }

        }


    }

    public void SetQuestID(int _id)
    {
        _questID = _id;
    }
   
    public void SetItemID(int _ID)
    {
        _itemID = _ID;
    }

}
