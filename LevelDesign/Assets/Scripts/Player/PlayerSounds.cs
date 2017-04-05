using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    [FMODUnity.EventRef]
    public string playerGrunt;

    [FMODUnity.EventRef]
    public string SpellCast;

    public void PlayerSpellCast()
    {
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(SpellCast);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        e.start();
        e.release();
    }  

    public void PlayerGrunt()
    {
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(playerGrunt);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        e.start();
        e.release();
    }

}
