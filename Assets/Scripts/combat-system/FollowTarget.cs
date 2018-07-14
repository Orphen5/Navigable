﻿using UnityEngine;

public class FollowTarget : PooledParticleSystem
{
    #region Fields

    [Header("General")]
    public AttackType attackType;
    public LayerMask hitLayerMask;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private AudioClip explosionSfx;

    private Transform mainCamera;
    private Vector3 camForwardDir;
    private Vector3 camBackwardDir;
    private AIEnemy enemy;
    private Transform enemyTransform;
    private Vector3 hitOffset;

    [Header("Motion")]
    [SerializeField]
    private float goWaySpeed = 20f;
    [SerializeField]
    private float returnWaySpeed = 25f;
    [SerializeField]
    private float maxDistance = 12f;
    [SerializeField]
    private float goWayFinalWaitTime = 0.5f;

    [Header("Ground avoidance")]
    [SerializeField]
    private bool hugGround = false;
    [SerializeField]
    private float minGroundClearance = 1.0f;
    [SerializeField]
    private LayerMask walkableLayer;

    private Vector3 initPos;
    private float time;

    private enum AttackStates { GoWay, ReturnWay, Stay };
    private AttackStates attackState;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        SetOrbDirection();
        DestroyOrb();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & hitLayerMask) != 0)
        {
            AIEnemy enemyHit = other.GetComponent<AIEnemy>();
            if (enemyHit)
            {
                StatsManager.instance.RegisterWeakAttackHit();
                enemyHit.TakeDamage(damage, attackType);
                enemyHit.SetKnockback(this.transform.position);
                SoundManager.instance.PlaySfxClip(explosionSfx);
            }
        }
    }

    #endregion

    #region Public Methods

    public override void Restart()
    {
        time = 0f;
        initPos = transform.position;
        attackState = AttackStates.GoWay;
        camForwardDir = transform.InverseTransformDirection(mainCamera.forward);
        enemyTransform = null;
        enemy = null;
        hitOffset = Vector3.zero;
    }

    public void SetEnemyTransform(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
        if (enemyTransform)
            this.enemy = enemyTransform.GetComponent<AIEnemy>();
    }

    public void SetHitOffset(Vector3 hitOffset)
    {
        this.hitOffset = hitOffset;
    }

    #endregion

    #region Private Methods

    private void SetOrbDirection()
    {
        switch (attackState)
        {
            case AttackStates.GoWay:
                if (Vector3.Distance(initPos, transform.position) < maxDistance)
                {
                    transform.Translate(camForwardDir * goWaySpeed * Time.deltaTime);
                    CheckGroundClearance();
                }
                else
                {
                    attackState = AttackStates.Stay;
                }
                break;

            case AttackStates.Stay:
                time += Time.deltaTime;
                if (time >= goWayFinalWaitTime)
                {
                    attackState = AttackStates.ReturnWay;
                }
                break;

            case AttackStates.ReturnWay:
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.GetPlayer1().transform.position, returnWaySpeed * Time.deltaTime);
                CheckGroundClearance();
                break;
        }
    }

    private void DestroyOrb()
    {
        if (attackState == AttackStates.ReturnWay)
        {
            if (Vector3.Distance(GameManager.instance.GetPlayer1().transform.position, transform.position) < 1f)
            {
                ReturnToPool();
            }
        }
    }

    private void CheckGroundClearance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 20, walkableLayer))
        {
            bool tooCloseToGround = minGroundClearance - hit.distance > 0.01f;
            bool tooFarFromGround = minGroundClearance - hit.distance < -0.01f;
            if ( tooCloseToGround || hugGround && tooFarFromGround)
            {
                transform.position += Vector3.up * (minGroundClearance - hit.distance);
            }
        }
    }

    #endregion
}
