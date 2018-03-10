﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Public Data

    public static GameManager instance;
    public enum GameStates { OnStartMenu, InGame, OnGameEnd };
    public GameStates gameState;

    [SerializeField]
    private Player player;

	#endregion
	
	#region Private Serialized Fields
	
	#endregion
	
	#region Private Non-Serialized Fields
	
	#endregion
	
	#region Properties

    #endregion
	
	#region MonoBehaviour Methods
	
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

	#endregion
	
	#region Public Methods
	
    public Player GetPlayerOne()
    {
        return player;
    }

    public void OnGameWon()
    {
        gameState = GameStates.OnStartMenu;
    }

    public void OnGameLost()
    {
        gameState = GameStates.OnStartMenu;
    }

	#endregion
	
	#region Private Methods
	
	#endregion
}