﻿using UnityEngine;

public class ConeAttackBehaviour : PooledParticleSystem
{
    #region Fields
    public LayerMask layerMask;
    public int damage;
    public ConeAttackDetection enemiesDetector;
    public int enemiesToCombo;
    public int evilComboReward;
    public float hurtEnemiesDelay;
    public float timeToDistable;
    private int comboCount;
    private float timer;
    private float timeToReturnToPool;

    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        timer -= Time.deltaTime;
        timeToReturnToPool -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            HurtEnemies();
            timer = 1000.0f;
        }
        if(timeToReturnToPool <= 0.0f)
        {
            ReturnToPool();
        }
    }
    #endregion

    #region Public Methods
    public override void Restart()
    {
        enemiesDetector.attackTargets.Clear();
        timer = hurtEnemiesDelay;
        timeToReturnToPool = timeToDistable;
        comboCount = 0;
    }
    #endregion

    #region Private Methods
    private void HurtEnemies()
    {
        foreach (AIEnemy aiEnemy in enemiesDetector.attackTargets)
        {
            aiEnemy.MarkAsTarget(false);
            aiEnemy.TakeDamage(damage, AttackType.CONE);
            comboCount++;
        }

        CheckIfCombo();
        enemiesDetector.attackTargets.Clear();
    }

    private void CheckIfCombo()
    {
        if (comboCount >= enemiesToCombo)
        {
            GameManager.instance.GetPlayer1().AddEvilPoints(evilComboReward);
            UIManager.instance.ShowComboText(UIManager.ComboTypes.StrongCombo);
        }

        comboCount = 0;
    }
    #endregion
}
