using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fsmAgentAnimationAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance footsteps;
    private FMOD.Studio.EventInstance death;

    public void PlayFootstep()
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Kieron/fsmAgentFootsteps");
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }
    
    public void PlayDeath()
    {
        death = FMODUnity.RuntimeManager.CreateInstance("event:/Kieron/fsmAgentDeath");
        death.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        death.start();
        death.release();
    }
}
