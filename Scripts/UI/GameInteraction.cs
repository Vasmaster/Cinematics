using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

class mySorter : IComparer
{

    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }


}

public class GameInteraction : MonoBehaviour {

	private Image _enemyHP;
	private Image _enemyHUD;
	private Text _enemyText;

	private Image _playerHP;
	private Image _playerMana;

    public float _globalCooldown;
    private float _globalTimer;

    private bool _globalTrigger;

    private int _spellIndex;
    private float[] _spellCD;
    private float[] _spellTimer;
    private bool[] _spellTrigger;

    private GameObject[] _playerSpells;

    // Use this for initialization
    void Start () {

        /*
		_playerHP = GameObject.FindGameObjectWithTag ("PlayerHP").GetComponent<Image> ();
		_playerMana = GameObject.FindGameObjectWithTag ("PlayerMana").GetComponent<Image> ();
        _globalTimer = _globalCooldown;
        _globalTrigger = false;

        

        _playerSpells = GameObject.FindGameObjectsWithTag("SpellCooldown");

        IComparer myComparer = new mySorter();

        Array.Sort(_playerSpells, myComparer);

        _spellCD = new float[_playerSpells.Length];
        _spellTimer = new float[_playerSpells.Length];
        _spellTrigger = new bool[_playerSpells.Length];

        */

    }
	
	// Update is called once per frame
	void Update () {

        /*
		if (Input.GetKeyDown ("l")) {

			// open questlog

		}

        if(_globalTimer <= _globalCooldown)
        {

            _globalTimer += Time.deltaTime;

        }
        for (int i = 0; i < _playerSpells.Length; i++)
        {

            if (_spellCD != null)
            {
                if (_spellCD[i] > 0)
                {

                    if (_spellTimer[i] < _spellCD[i])
                    {
                        _spellTimer[i] += Time.deltaTime;
                    }

                }

                if (_spellTrigger[i])
                {

                    SpellCooldown(i);

                }
            }

        }
       

        if(_globalTrigger)
        {

            GlobalCooldown();
        }

      */
	
	}

    public void SetGlobalCooldown()
    {
        _globalTrigger = true;

        _globalTimer = 0;
    }

    void GlobalCooldown() 
    {
        
        GameObject[] _globalCD = GameObject.FindGameObjectsWithTag("GlobalCooldown");

        for (int i = 0; i < _globalCD.Length; i++)
        {

            if(_globalTimer < _globalCooldown) { 

                _globalCD[i].GetComponent<Image>().fillAmount = _globalTimer / _globalCooldown;
                
            }
            if (Math.Round(_globalTimer, 1) == _globalCooldown) 
            {
                
                _globalCD[i].GetComponent<Image>().fillAmount = 0;
                _globalTrigger = false;

            }
        }

        
    }

   void SpellCooldown(int _index)
    {

           
        if(_spellTimer[_index] < _spellCD[_index]) {
            _playerSpells[_index].GetComponent<Image>().fillAmount = _spellTimer[_index] / _spellCD[_index];
            Debug.Log(_playerSpells[_index].name);
       
        }
        if(Math.Round(_spellTimer[_index],1) == _spellCD[_index])
        {

            _playerSpells[_index].GetComponent<Image>().fillAmount = 0;
            _spellTrigger[_index] = false;
        }
    }

    public void SetSpellCooldown(int _spellNumber, float _cooldown)
    {

        
        
        _spellIndex = _spellNumber - 1;


        
        _spellCD[_spellIndex] = _cooldown;
        
        _spellTrigger[_spellIndex] = true;
        _spellTimer[_spellIndex] = 0;
       

    
            //_playerSpells[_spellFix].GetComponent<Image>().fillAmount;
    

    }

	public void SetSelected(GameObject _selected) {
		
        if(_selected != null) { 
		    _enemyHUD = GameObject.FindGameObjectWithTag ("EnemyHUD").GetComponent<Image> ();
		    _enemyHUD.enabled = true;
		    _enemyHP = GameObject.FindGameObjectWithTag ("EnemyHP").GetComponent<Image> ();
		    _enemyText = GameObject.FindGameObjectWithTag ("EnemyName").GetComponent<Text> ();

		    if (_selected.tag == "EnemyRanged") {

			    _enemyHP.fillAmount = _selected.GetComponent<EnemyRanged> ()._enemyHealth / 100;
			    _enemyText.text = _selected.GetComponent<EnemyRanged> ()._nameToDisplay;
		    }

		    if (_selected.tag == "EnemyMelee") {

			    _enemyHP.fillAmount = _selected.GetComponent<EnemyMelee> ()._enemyHealth / 100;
			    _enemyText.text = _selected.GetComponent<EnemyMelee> ()._nameToDisplay;
    		}

            if(_selected.tag == "NPC")
            {

                _enemyText.text = _selected.GetComponent<NPC>()._nameToDisplay;
                _enemyHP.fillAmount = _selected.GetComponent<NPC>()._health;

            }
        }



    }

    public void SetEnemyHealth(float _health) {

		_enemyHP.fillAmount = _health / 100;
	
	}

	public void SetPlayerHealth(float _health) {

		_playerHP.fillAmount = _health / 100;

	}

	public void SetPlayerMana(float _mana) {

		_playerMana.fillAmount = _mana / 100;

	}

    public void EnemyDeath()
    {

        _enemyHUD = GameObject.FindGameObjectWithTag("EnemyHUD").GetComponent<Image>();
        _enemyHUD.enabled = false;

        _enemyHP = GameObject.FindGameObjectWithTag("EnemyHP").GetComponent<Image>();
        _enemyText = GameObject.FindGameObjectWithTag("EnemyName").GetComponent<Text>();

        _enemyHP.fillAmount = 0;
        _enemyText.text = null;

    }

    public void TriggerQuest(string _text)
    {

    }

}
