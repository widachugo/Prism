using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AnimCall : MonoBehaviour
{
    private Boss1 boss1;

    public void Start()
    {
        boss1 = GetComponentInParent<Boss1>();
    }

    public void RockAttack()
    {
        boss1.RockAttack();
    }

    public void StopRockAttack()
    {
        boss1.StopRockAttack();
    }

    public void ChocWaveParticle()
    {
        boss1.ChocWaveParticle();
    }
}
