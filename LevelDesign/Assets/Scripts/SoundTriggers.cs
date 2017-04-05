using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggers : MonoBehaviour {

    [FMODUnity.EventRef]
    public string m_EventPath;

    public void EnemyDeath()
    {
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_EventPath);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        e.start();
        e.release();
    }

    public void PlayerSpellCast()
    {
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_EventPath);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        e.start();
        e.release();
    }
}
