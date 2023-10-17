using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimCode : MonoBehaviour
{
    public UnityAction PlantEndEvent;

    public ParticleSystem FootageSmokeRight;
    public ParticleSystem FootageSmokeLeft;

    public void PlantStart()
    {
    }

    public void Plantend()
    {
        PlantEndEvent?.Invoke();
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
