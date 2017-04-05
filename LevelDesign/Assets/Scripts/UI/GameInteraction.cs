using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;


class mySorter : IComparer
{

    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }


}
namespace CombatSystem
{

    public class GameInteraction : MonoBehaviour
    {

        private Image _enemyHP;
        private Image _enemyHUD;
        private Text _enemyText;



        private static Image _castBar;
        private static Image _playerHP;
        private static Image _playerMana;
        private static Image _playerInCombat;

        private bool _inCombat = false;
        private static bool _spellHasBeenCast = false;

        public float _globalCooldown;
        private float _globalTimer;

        private bool _globalTrigger;

        private int _spellIndex;
        private float[] _spellCD;
        private static float[] _spellTimer;
        private bool[] _spellTrigger;
        private static bool[] _cooldownComplete;

        private GameObject[] _playerSpells;
        private static List<float> _allCooldowns;
        
        private static Texture2D[] _icons;
        private static List<Texture2D> _guiIcons = new List<Texture2D>();

        private static GUISkin _skin;
        private static GameObject _selectedActor;
        private static bool _showBars = false;

        private static bool _showIcons = false;

        private static Texture2D _cursorNormal;
        private static Texture2D _cursorNPC;
        private static Texture2D _cursorCombat;


        void OnEnable()
        {

            _enemyHP = GameObject.FindGameObjectWithTag("EnemyHP").GetComponent<Image>();
            _enemyHP.transform.parent.gameObject.SetActive(false);

            _playerHP = GameObject.FindGameObjectWithTag("PlayerHP").GetComponent<Image>();
            _playerMana = GameObject.FindGameObjectWithTag("PlayerMana").GetComponent<Image>();
            _playerInCombat = GameObject.FindGameObjectWithTag("PlayerInCombat").GetComponent<Image>();
            _playerSpells = GameObject.FindGameObjectsWithTag("SpellCooldown");
            _castBar = GameObject.FindGameObjectWithTag("CastBar").GetComponent<Image>();

            _skin = Resources.Load("Skins/Combat_HUD") as GUISkin;
            CombatSystem.CombatDatabase.GetAllSpells();
            _icons = Resources.LoadAll<Texture2D>("PlayerSpells/SpellIcons");

            _allCooldowns = CombatDatabase.ReturnAllSpellCooldowns();
            _spellTimer = new float[_allCooldowns.Count];
            _cooldownComplete = new bool[_allCooldowns.Count];

            _cursorNormal = Resources.Load("Icons/Cursor/Cursor_Normal") as Texture2D;
            _cursorCombat = Resources.Load("Icons/Cursor/Cursor_Combat") as Texture2D;
            _cursorNPC = Resources.Load("Icons/Cursor/Cursor_NPC") as Texture2D;

            

        }

        // Use this for initialization
        void Start()
        {

            SetNormalCursor();

             _globalTimer = _globalCooldown;
            _globalTrigger = false;

            IComparer myComparer = new mySorter();

            Array.Sort(_playerSpells, myComparer);

            _spellCD = new float[_playerSpells.Length];
            _spellTimer = new float[_playerSpells.Length];
            _spellTrigger = new bool[_playerSpells.Length];
            _playerInCombat.enabled = false;

            for (int i = 0; i < CombatSystem.CombatDatabase.ReturnSpellCount(); i++)
            {

                for (int j = 0; j < _icons.Length; j++)
                {
                    if (CombatSystem.CombatDatabase.ReturnSpellIcon(i).ToString() == _icons[j].ToString().Remove(_icons[j].ToString().Length - 23))
                    {
                        _guiIcons.Add(_icons[j]);
                    }
                }
            }


            for (int i = 0; i < _allCooldowns.Count; i++)
            {
                _spellTimer[i] = _allCooldowns[i];
                _cooldownComplete[i] = true;
            }



        }
        // Update is called once per frame
        void Update()
        {

            for (int i = 0; i < _allCooldowns.Count; i++)
            {
                if(_spellTimer[i] < _allCooldowns[i] && _spellHasBeenCast)
                {
                    _spellTimer[i] += Time.deltaTime;

                    


                    if(_spellTimer[i] >= _allCooldowns[i])
                    {
                        _cooldownComplete[i] = true;
                        _spellHasBeenCast = false;
                    }
                }
            }
           

            if(Input.GetKeyDown(KeyCode.Alpha1) && _cooldownComplete[0])
            {
                PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(0), _selectedActor, CombatDatabase.ReturnSpellManaCost(0));
                if (PlayerMovement.ReturnCastSpell())
                {
                    Combat.SetSpell(CombatDatabase.ReturnSpellID(0), CombatDatabase.ReturnSpellType(0), CombatDatabase.ReturnSpellValue(0), CombatDatabase.ReturnSpellManaCost(0), CombatDatabase.ReturnCastTime(0), CombatDatabase.ReturnSpellPrefab(0), _selectedActor);
                    _spellTimer[0] = 0.0f;
                    _cooldownComplete[0] = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _cooldownComplete[1])
            {
                PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(1), _selectedActor, CombatDatabase.ReturnSpellManaCost(1));
                if (PlayerMovement.ReturnCastSpell())
                {
                    Combat.SetSpell(CombatDatabase.ReturnSpellID(1), CombatDatabase.ReturnSpellType(1), CombatDatabase.ReturnSpellValue(1), CombatDatabase.ReturnSpellManaCost(1), CombatDatabase.ReturnCastTime(1), CombatDatabase.ReturnSpellPrefab(1), _selectedActor);
                    _spellTimer[1] = 0.0f;
                    _cooldownComplete[1] = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && _cooldownComplete[2])
            {
                PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(2), _selectedActor, CombatDatabase.ReturnSpellManaCost(2));
                if (PlayerMovement.ReturnCastSpell())
                {
                    Combat.SetSpell(CombatDatabase.ReturnSpellID(2), CombatDatabase.ReturnSpellType(2), CombatDatabase.ReturnSpellValue(2), CombatDatabase.ReturnSpellManaCost(2), CombatDatabase.ReturnCastTime(2), CombatDatabase.ReturnSpellPrefab(2), _selectedActor);
                    _spellTimer[2] = 0.0f;
                    _cooldownComplete[2] = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && _cooldownComplete[3])
            {
                
                if(CombatDatabase.ReturnAbility(3) != Abilities.None)
                {
                    if(CombatDatabase.ReturnAbility(3) == Abilities.Disengage)
                    {
                        PlayerMovement.PlayerKnockback(CombatDatabase.ReturnDisengageDistance(3));
                        _spellTimer[3] = 0.0f;
                        _cooldownComplete[3] = false;
                    }
                }
                else
                {
                    PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(3), _selectedActor, CombatDatabase.ReturnSpellManaCost(3));
                    if (PlayerMovement.ReturnCastSpell())
                    {
                        Combat.SetSpell(CombatDatabase.ReturnSpellID(3), CombatDatabase.ReturnSpellType(3), CombatDatabase.ReturnSpellValue(3), CombatDatabase.ReturnSpellManaCost(3), CombatDatabase.ReturnCastTime(3), CombatDatabase.ReturnSpellPrefab(3), _selectedActor);
                        _spellTimer[3] = 0.0f;
                        _cooldownComplete[3] = false;
                    }
                }
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) && _cooldownComplete[4])
            {

                if (CombatDatabase.ReturnAbility(4) != Abilities.None)
                {
                    if (CombatDatabase.ReturnAbility(4) == Abilities.Disengage)
                    {
                        PlayerMovement.PlayerKnockback(CombatDatabase.ReturnDisengageDistance(4));
                        _spellTimer[4] = 0.0f;
                        _cooldownComplete[4] = false;
                    }
                }
                if(CombatDatabase.ReturnSpellType(4) == SpellTypes.AOE)
                {
                    PlayerMovement.ToggleAoE();
                    PlayerMovement.CastAOE(CombatDatabase.ReturnCastTime(4), CombatDatabase.ReturnSpellManaCost(4));
                    if(PlayerMovement.ReturnCastAOE())
                    {
                        Combat.SetAOE(CombatDatabase.ReturnSpellValue(4), CombatDatabase.ReturnSpellPrefab(4));
                    }
                }

                else
                {
                    PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(4), _selectedActor, CombatDatabase.ReturnSpellManaCost(4));
                    if (PlayerMovement.ReturnCastSpell())
                    {
                        Combat.SetSpell(CombatDatabase.ReturnSpellID(4), CombatDatabase.ReturnSpellType(4), CombatDatabase.ReturnSpellValue(4), CombatDatabase.ReturnSpellManaCost(4), CombatDatabase.ReturnCastTime(4), CombatDatabase.ReturnSpellPrefab(4), _selectedActor);
                        _spellTimer[4] = 0.0f;
                        _cooldownComplete[4] = false;
                    }
                }

            }
        }


        void OnGUI()
        {
                DisplaySpellIcons();
            
            if (_selectedActor != null)
            {
                if (_showBars)
                {
                    Rect _rect = new Rect(Screen.width - Screen.width + 60, Screen.height - Screen.height + 50, 310, 128);
                    Rect _npcName = new Rect(_rect.x + 150, _rect.y + 30, 100, 20);
                    Rect _npcProfession = new Rect(_rect.x + 150, _rect.y + 80, 100, 20);
                    //GUI.Box(_rect, "");
                    if (_selectedActor.tag == "NPC")
                    {
                        GUI.Box(_rect, "", _skin.GetStyle("NPC_HUD"));
                        GUI.Label(_npcName, _selectedActor.GetComponentInChildren<NPC>().ReturnNpcName(), _skin.GetStyle("NPC_Name"));
                        GUI.Label(_npcProfession, _selectedActor.GetComponentInChildren<NPC>().ReturnProfession(), _skin.GetStyle("NPC_Name"));
                    }
                    if (_selectedActor.tag == "EnemyRanged" || _selectedActor.tag == "EnemyMelee")
                    {
                        _enemyHP.transform.parent.gameObject.SetActive(true);
                        _enemyHP.fillAmount = _selectedActor.GetComponent<EnemyRanged>()._enemyHealth / 100;
                        _enemyHP.transform.parent.GetComponentInChildren<Text>().text = _selectedActor.GetComponent<EnemyRanged>()._nameToDisplay;
                    }
                }
                if(!_showBars)
                {
                    _enemyHP.transform.parent.gameObject.SetActive(false);
                }
            }
            else
            {
                _enemyHP.transform.parent.gameObject.SetActive(false);
            }
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

                if (_globalTimer < _globalCooldown)
                {

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


            if (_spellTimer[_index] < _spellCD[_index])
            {
                _playerSpells[_index].GetComponent<Image>().fillAmount = _spellTimer[_index] / _spellCD[_index];
                //     Debug.Log(_playerSpells[_index].name);

            }
            if (Math.Round(_spellTimer[_index], 1) == _spellCD[_index])
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
    
        public void SetSelected(GameObject _selected)
        {

            if (_selected != null)
            {

                _enemyHUD = GameObject.FindGameObjectWithTag("EnemyHUD").GetComponent<Image>();
                _enemyHUD.enabled = true;
                _enemyHP = GameObject.FindGameObjectWithTag("EnemyHP").GetComponent<Image>();
                _enemyText = GameObject.FindGameObjectWithTag("EnemyName").GetComponent<Text>();

                if (_selected.tag == "EnemyRanged")
                {

                    _enemyHP.fillAmount = _selected.GetComponent<EnemyRanged>()._enemyHealth / 100;
                    _enemyText.text = _selected.GetComponent<EnemyRanged>()._nameToDisplay;
                }

                if (_selected.tag == "EnemyMelee")
                {

                    _enemyHP.fillAmount = _selected.GetComponent<EnemyMelee>()._enemyHealth / 100;
                    _enemyText.text = _selected.GetComponent<EnemyMelee>()._nameToDisplay;
                }

                if (_selected.tag == "NPC")
                {

                    _enemyText.text = _selected.GetComponentInChildren<NPC>().ReturnNpcName();
                    _enemyHP.fillAmount = _selected.GetComponentInChildren<NPC>().ReturnNpcHealth();

                }
            }

            if (_selected = null)
            {
                _enemyHUD = GameObject.FindGameObjectWithTag("EnemyHUD").GetComponent<Image>();
                _enemyHUD.enabled = false;
            }

        }
       
        public void SetEnemyHealth(float _health)
        {

            if (_enemyHP.fillAmount > 0)
            {
                _enemyHP.fillAmount = _health / 100;
            }

        }

        public static void SetPlayerHealth(float _health)
        {

            _playerHP.fillAmount = _health / 100;
            _playerInCombat.fillAmount = _health / 100;

        }

        public static void SetPlayerMana(float _mana)
        {

            _playerMana.fillAmount = _mana / 100;


        }

        public void EnemyDeath()
        {

            SetSelectedUI(null);

        }



        public static void DisplayPlayerInCombat(bool _combat, float _health)
        {
            _playerInCombat.enabled = _combat;
            _playerInCombat.fillAmount = _health / 100;
            
        } 

        public static void SetSelectedUI(GameObject _selected)
        {
            _selectedActor = _selected;

            if (_selected != null)
            {
                _showBars = true;
            }
            if(_selected == null)
            {
                _showBars = false;
            }

        }


        static void DisplaySpellIcons()
        {
            // The rectangles for the actuall spells
            Rect[] _spellRect = new Rect[_guiIcons.Count];
            

            // We create a MainRect to 'catch' the mouse and prevent moving the character when pressing a spell
            Rect _mainRect = new Rect(Screen.width / 2 - 242, Screen.height - 125, 500, 70);
            GUI.Box(_mainRect, "", _skin.GetStyle("SpellBox"));

            for (int i = 0; i < _guiIcons.Count; i++)
            {                
                // Create the spell buttons    
                _spellRect[i] = new Rect(Screen.width / 2 - 243 + (85 * i), Screen.height - 122, 64, 64);
                GUI.Box(_spellRect[i], _guiIcons[i], _skin.GetStyle("SpellBox"));
                
                //GUI.Box(_spellRect[i],)
                // If the mouse is in one of the rectangles
                if(_spellRect[i].Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0 && Event.current.type == EventType.mouseDown && !PlayerMovement.ReturnCastSpell())
                    {
                        if (CombatDatabase.ReturnAbility(i) != Abilities.None)
                        {
                            if (CombatDatabase.ReturnAbility(i) == Abilities.Disengage)
                            {
                                PlayerMovement.PlayerKnockback(CombatDatabase.ReturnDisengageDistance(i));
                                _spellTimer[i] = 0.0f;
                                _cooldownComplete[i] = false;
                            }
                        }
                        else
                        {
                            PlayerMovement.CastSpell(CombatDatabase.ReturnCastTime(i), _selectedActor, CombatDatabase.ReturnSpellManaCost(i));
                            if (PlayerMovement.ReturnCastSpell())
                            {
                                Combat.SetSpell(CombatDatabase.ReturnSpellID(i), CombatDatabase.ReturnSpellType(i), CombatDatabase.ReturnSpellValue(i), CombatDatabase.ReturnSpellManaCost(i), CombatDatabase.ReturnCastTime(i), CombatDatabase.ReturnSpellPrefab(i), _selectedActor);
                                _spellTimer[i] = 0.0f;
                                _cooldownComplete[i] = false;
                            }
                        }
                    }
                }   
            }

            // If the mouse is over the MainRect 
            if (_mainRect.Contains(Event.current.mousePosition))
            {

                CombatSystem.PlayerMovement.HoveringOverUI(true);
                CombatSystem.PlayerMovement.SetDraggingUI(true);
                
            }
            else
            {
                CombatSystem.PlayerMovement.HoveringOverUI(false);
                CombatSystem.PlayerMovement.SetDraggingUI(false);
            }

        }

        public static void DisplayCastBar(bool _set)
        {
            _castBar.transform.parent.gameObject.SetActive(_set);
        }
        public static void FillCastBar(float _value)
        {
            _castBar.fillAmount = _value;
        }

        public static void SpellHasBeenCast()
        {
            _spellHasBeenCast = true;
        }

        public static void SetNpcCursor()
        {
            Cursor.SetCursor(_cursorNPC, Vector2.zero, CursorMode.Auto);
            
        }

        public static void SetCombatCursor()
        {
            Cursor.SetCursor(_cursorCombat, Vector2.zero, CursorMode.Auto);
        }
        public static void SetNormalCursor()
        {
            Cursor.SetCursor(_cursorNormal, Vector2.zero, CursorMode.Auto);
            //Debug.Log(Cursor.visible);
        }

    }
}