using UnityEngine;
using System.Collections;

namespace CombatSystem
{

    public class EnemyTrigger : MonoBehaviour
    {



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnTriggerEnter(Collider coll)
        {

            if (coll.tag == "Player" || coll.name == "PlayerMelee")
            {

                // If the enemy engages the player, the player get set in Combat
                // we pass the parent gameobject to the player class so we can set the target in the interface to this enemy

                CombatSystem.GameInteraction.SetSelectedUI(this.transform.parent.gameObject);
                coll.GetComponent<CombatSystem.PlayerMovement>().PlayerInCombat(true);
                this.transform.parent.GetComponent<EnemyRanged>().setAttack(true);
                this.transform.parent.GetComponent<EnemyRanged>().setPatrol(false);
                this.transform.parent.GetComponent<EnemyRanged>().setTarget(coll);


            }
        }

        void OnTriggerExit(Collider coll)
        {

            if (coll.name == "Player")
            {

                coll.GetComponent<CombatSystem.Player>().PlayerInCombat(false);
                this.transform.parent.GetComponent<EnemyRanged>().setAttack(false);
                this.transform.parent.GetComponent<EnemyRanged>().setPatrol(true);

            }
        }

        void OnTriggerStay(Collider coll)
        {
            if (coll.name == "PlayerAOE_DMG")
            {
                CombatSystem.GameInteraction.SetSelectedUI(this.transform.parent.gameObject);
                Combat.InitiateCombat();
                this.transform.parent.GetComponent<EnemyRanged>().setAttack(true);
                this.transform.parent.GetComponent<EnemyRanged>().setPatrol(false);
                this.transform.parent.GetComponent<EnemyRanged>().setTarget(coll);
                this.transform.parent.GetComponent<EnemyRanged>()._enemyHealth -= coll.transform.GetComponent<SpellObject>().ReturnDamage();


            }
        }

    }

    
}