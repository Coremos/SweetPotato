using UnityEngine;
using UnityEngine.Events;

public class AllyAnimCode : AnimCode
{
    public UnityAction DeathEvent;
    public ParticleSystem FootageSmokeRight;
    public ParticleSystem FootageSmokeLeft;

    public void Death()
    {
        DeathEvent?.Invoke();
    }

    public void FootageParticleR()
    {
        FootageSmokeRight.Stop();
        FootageSmokeRight.Play();
    }
    public void FootageParticleL()
    {
        FootageSmokeLeft.Stop();
        FootageSmokeLeft.Play();
    }
}