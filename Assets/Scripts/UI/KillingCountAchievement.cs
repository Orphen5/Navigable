﻿using UnityEngine;

public class KillingCountAchievement : Combo {

    #region Attributes
    [SerializeField]
    private AttackType attackType;

    private Player player;
	#endregion
	
	#region MonoBehaviour methods
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
        player = GameManager.instance.GetPlayer1();
    }

    // Update is called once per frame
    void Update()
    {
        ReviewConditions();
        if (attackType != AttackType.METEORITE && attackType != AttackType.WEAK)
        {
            ResetCount();
        }      
        else
        {
            if(attackType == AttackType.WEAK)
            {
                if(currentCount > 0 && !player.GetIsBoomerangOn())
                {
                    ResetCount();
                }
            }else if(attackType == AttackType.METEORITE)
            {
                if (currentCount > 0 && !player.GetIsMeteoritesOn())
                {
                    ResetCount();
                }
            }
        }
    }

    #region Public methods
    public override void GrantReward()
    {
        if (timesObtained == 0)
            StatsManager.instance.RegisterAchievement(this);

        timesObtained += 1;
        StatsManager.instance.IncreaseRoundPoints(reward);
        TransitionUI.instance.AskForTransition(comboName, comboIcon);
        ResetCount();
    }

    public AttackType GetAttackType()
    {
        return attackType;
    }

    public override void IncreaseCurrentCount(int addToCount)
    {
        currentCount += addToCount;
    }

    public override void ReviewConditions()
    {
        if (currentCount >= score)
        {
            GrantReward();
        }
    }
    #endregion

    #region Private methods

    #endregion
}
