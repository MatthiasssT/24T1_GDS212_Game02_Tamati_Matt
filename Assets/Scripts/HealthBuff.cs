using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps")]
public class HealthBuff : PowerUpSystem
{
    public float boostAmount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Health>().maxHealth += (int)boostAmount;
    }
}
