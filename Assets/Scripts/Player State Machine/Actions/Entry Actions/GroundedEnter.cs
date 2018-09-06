﻿using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedEnter")]
public class GroundedEnter : StateAction
{
    public override void Act(Player player)
    {
        player.elapsedRecoveryTime = 0.0f;
        player.elapsedDelayTime = 0.0f;
        player.SetCollidersActiveState(false);
        player.animator.SetTrigger("KnockOut");
        player.cameraState = Player.CameraState.CONEATTACK;
        UIManager.instance.SetUIElementsVisibility(false);
        player.mainCameraController.DeathPostProcessing();
    }
}
