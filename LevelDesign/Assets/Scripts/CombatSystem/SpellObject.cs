using UnityEngine;
using System.Collections;

public class SpellObject : MonoBehaviour
{

    private float _spellDamage;
    private float _timer;
    private bool _fromPlayer;

    private float _lifeSpan = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     
       KillSwitch();
    }


    public void SetDamage(float _damage)
    {
        _spellDamage = _damage;
    }

    public float ReturnDamage()
    {
        return _spellDamage;
    }

    public void SetFromPlayer(bool _set)
    {
        _fromPlayer = _set;
    }

    public bool ReturnFromPlayer()
    {
        return _fromPlayer;
    }

    void KillSwitch()
    {

        if (_timer == _lifeSpan)
        {
            Destroy(this.gameObject);
        }
        else {
            _timer += Time.deltaTime;
        }
    }
}
