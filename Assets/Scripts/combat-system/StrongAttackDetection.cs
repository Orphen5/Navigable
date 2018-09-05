﻿using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;
    private float startingSize;
    private Projector projector;
    private new SphereCollider collider;
    private Player player;
    [SerializeField]
    private Color decalOriginalColor;
    [SerializeField]
    private Color decalFinalColor;
    private Color colorDiffernce;
    private Material decalMat;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        projector = GetComponent<Projector>();
        UnityEngine.Assertions.Assert.IsNotNull(projector, "ERROR: A Projector Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);
        projector.material = new Material(projector.material);
        decalMat = projector.material;

        decalMat.SetColor("_TintColor", decalOriginalColor);
        colorDiffernce = decalFinalColor - decalOriginalColor;

        collider = GetComponent<SphereCollider>();
        UnityEngine.Assertions.Assert.IsNotNull(collider, "ERROR: A Collider Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);
        Deactivate();
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(true);
                player.currentStrongAttackTargets.Add(aIEnemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(false);
                player.currentStrongAttackTargets.Remove(aIEnemy);
            }
        }
    }
    #endregion

    #region Public Methods
    public void SetAttackSize(float radius)
    {
        startingSize = radius;
        projector.orthographicSize = startingSize;
        collider.radius = startingSize;
    }

    public void Activate(bool useProjector = true)
    {
        collider.enabled = true;
        if (useProjector)
            projector.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
        projector.enabled = false;
    }

    public void IncreaseSize(float increase)
    {
        collider.radius = startingSize + increase;
        projector.orthographicSize = startingSize + increase;
    }

    public void ResetSize()
    {
        projector.orthographicSize = startingSize;
        collider.radius = startingSize;
        decalMat.SetColor("_TintColor", decalOriginalColor);
    }

    public void ChangeDecalColor(float time)
    {
        decalMat.SetColor("_TintColor", decalOriginalColor + colorDiffernce * time);
    }
    #endregion
}
