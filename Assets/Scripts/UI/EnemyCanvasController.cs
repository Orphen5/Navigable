﻿using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvasController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private RectTransform healthBarCanvas;
    [SerializeField]
    private float damageActiveTime;
    [SerializeField]
    private float targetActiveTime;
    [SerializeField]
    private float fadeOutBaseTime;
    private float fadeOutTime;
    [SerializeField]
    private Image healthContainer;
    [SerializeField]
    private Image healthFill;
    [SerializeField]
    private Image healthIcon;
    private float baseHealth;
    private float time;
    private bool fadeOut;
    private Color transparent;
    private AIEnemy enemyScript;

	#endregion
	
	#region MonoBehaviour Methods

    private void Awake()
    {
        fadeOut = false;
        fadeOutTime = fadeOutBaseTime;
        enemyScript = GetComponent<AIEnemy>();
        baseHealth = enemyScript.baseHealth;
        transparent = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        healthBarCanvas.LookAt(Camera.main.transform);

        if (healthBarCanvas.gameObject.activeSelf && !fadeOut)
        {
            DisableHealthBar();
        }

        if (fadeOut)
        {
            HealthBarFadeOut();
        }
    }

	#endregion
	
	#region Public Methods
    public void HideHealthBar()
    {
        fadeOut = false;
        fadeOutTime = fadeOutBaseTime;
        healthBarCanvas.gameObject.SetActive(false);
    }

	public void SetHealthBar()
    {
        healthFill.fillAmount = enemyScript.GetCurrentHealth() / baseHealth;
    }

    public void EnableHealthBar(bool takeDamage)
    {
        healthBarCanvas.gameObject.SetActive(true);
        healthContainer.color = Color.white;
        healthFill.color = Color.white;
        healthIcon.color = Color.white;

        if (takeDamage || (!takeDamage && time <= targetActiveTime))
        {
            time = takeDamage ? damageActiveTime : targetActiveTime;
        }
    }

    #endregion

    #region Private Methods

    private void DisableHealthBar()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            fadeOut = true;
        }
    }

    private void HealthBarFadeOut()
    {
        fadeOutTime -= Time.deltaTime;
        Color currentColor = Color.Lerp(Color.white, transparent, 1 - (fadeOutTime / fadeOutBaseTime));
        healthContainer.color = currentColor;
        healthFill.color = currentColor;
        healthIcon.color = currentColor;

        if (fadeOutTime <= 0)
        {
            fadeOut = false;
            fadeOutTime = fadeOutBaseTime;
            healthBarCanvas.gameObject.SetActive(false);
        }
    }

    #endregion
}