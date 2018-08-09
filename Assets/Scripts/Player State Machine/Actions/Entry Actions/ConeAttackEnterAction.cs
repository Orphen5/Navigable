﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackEnterAction")]
public class ConeAttackEnterAction : StateAction
{
    public bool startsCooldown = true;
    public string animationTrigger;

    public override void Act(Player player)
    {
        player.AddEvilPoints(-player.coneAttackEvilCost);
        player.comeBackFromConeAttack = false;

        if (startsCooldown)
            player.coneAttackCooldown.timeSinceLastAction = 0.0f;

        player.mainCameraController.timeSinceLastAction = 0.0f;
        player.mainCameraController.fastAction = true;
        player.animator.SetTrigger(animationTrigger);
    }
}
