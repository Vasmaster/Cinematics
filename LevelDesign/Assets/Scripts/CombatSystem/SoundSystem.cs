using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{

    public class SoundSystem : MonoBehaviour
    {
        [FMODUnity.EventRef]
        private static string _playerGrunt = "event:/grunts/female_grunts";

        [FMODUnity.EventRef]
        private static string _playerSpells = "event:/spells/fire_ball_cast";

        [FMODUnity.EventRef]
        private static string _playerFootsteps = "event:/footsteps/footstep_materials_mix";

        private static float m_Wood;
        private static float m_Water;
        private static float m_Dirt;
        private static float m_Sand;
        private static float m_Grass;
        private static float m_Stone;
        private static float m_Snow;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void PlaySpellCast(Vector3 _playerPos)
        {
            FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(_playerSpells);
            e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_playerPos));

            e.start();
            e.release();
        }

        public static void PlayFootSteps(Vector3 _playerPos)
        {
            //Defaults
            m_Water = 0.0f;
            m_Dirt = 0.0f;
            m_Sand = 0.0f;
            m_Wood = 0.0f;
            m_Snow = 0.0f;
            m_Grass = 0.0f;
            m_Stone = 0.0f;

            RaycastHit hit;
            if (Physics.Raycast(_playerPos, Vector3.down, out hit, 1000.0f))
            {

                if (hit.collider.gameObject.layer == 9)
                {
                    m_Snow = 1.0f;
                    Debug.Log(hit.collider.gameObject.layer);
                }
                if (hit.collider.gameObject.layer == 14)
                {
                    m_Stone = 1.0f;

                }


                else//If the ray hits somethign other than the ground, we assume it hit a wooden prop (This is specific to the Viking Village scene) - and set the parameter values for wood.
                {
                    m_Water = 0.0f;
                    m_Dirt = 0.0f;
                    m_Sand = 0.0f;
                    m_Wood = 0.0f;
                }
            }

           

            if (_playerFootsteps != null)
            {
                FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(_playerFootsteps);
                e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_playerPos));

                SetParameter(e, "wood", m_Wood);
                SetParameter(e, "dirt", m_Dirt);
                //SetParameter(e, "sand", m_Sand);
                //SetParameter(e, "Water", m_Water);
                SetParameter(e, "snow", m_Snow);
                SetParameter(e, "grass", m_Grass);
                SetParameter(e, "stone", m_Stone);



                e.start();
                e.release();//Release each event instance immediately, there are fire and forget, one-shot instances. 
            }
        }

        public static void PlayerHit(Vector3 _playerPos)
        {
            FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(_playerGrunt);
            e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_playerPos));

            e.start();
            e.release();
        }

        static void SetParameter(FMOD.Studio.EventInstance e, string name, float value)
        {
            FMOD.Studio.ParameterInstance parameter;
            e.getParameter(name, out parameter);
            if (parameter == null)
            {
                return;
            }
            parameter.setValue(value);
        }
    }

}
