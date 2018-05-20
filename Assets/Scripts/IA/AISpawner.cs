﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    #region Fields
    [SerializeField]
    private AIZoneController zoneController;
    [SerializeField]
    private AISpawnController spawnController;
    [SerializeField]
    private Vector3 spawnerArea;
    [SerializeField]
    private ParticleSystem spawnVFX;

    [SerializeField]
    private List<SpawnInfo> activeSpawnInfos;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "ERROR: ZoneController not set for AISpawner in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(spawnController, "ERROR: SpawnController not set for AISpawner in gameObject '" + gameObject.name + "'");
    }

    private void Update()
    {
        List<SpawnInfo> spawnInfosToRemove = new List<SpawnInfo>();

        foreach (SpawnInfo spawnInfo in activeSpawnInfos)
        {
            spawnInfo.elapsedTime += Time.deltaTime;

            if (spawnInfo.elapsedTime >= spawnInfo.nextSpawnTime)
            {
                EnemyType enemyType = spawnInfo.enemiesToSpawn[spawnInfo.nextSpawnIndex];
                SpawnEnemy(spawnInfo, enemyType);
            }

            if (spawnInfo.nextSpawnIndex >= spawnInfo.enemiesToSpawn.Count)
            {
                spawnInfosToRemove.Add(spawnInfo);
            }
        }

        foreach (SpawnInfo spawnInfo in spawnInfosToRemove)
        {
            spawnInfo.elapsedTime = 0;
            spawnInfo.nextSpawnIndex = 0;
            spawnInfo.nextSpawnTime = 0;
            activeSpawnInfos.Remove(spawnInfo);
        }
        spawnInfosToRemove.Clear();
    }
    #endregion

    #region Public Methods
    // Called by AISpawnController
    public void Spawn(SpawnInfo spawnInfo)
    {
        if (!activeSpawnInfos.Contains(spawnInfo))
        {
            zoneController.monument.GetMonumentIndicator().RequestOpen();
            activeSpawnInfos.Add(spawnInfo);
            spawnInfo.elapsedTime = 0;
            spawnInfo.nextSpawnIndex = 0;
            spawnInfo.nextSpawnTime = (spawnInfo.nextSpawnIndex + 1) * spawnInfo.spawnDuration / spawnInfo.enemiesToSpawn.Count;
        }
    }

    public void ClearSpawnInfos()
    {
        activeSpawnInfos.Clear();
    }
    #endregion

    #region Private methods
    void SpawnEnemy (SpawnInfo spawnInfo, EnemyType enemyType)
    {
        ++spawnInfo.nextSpawnIndex;
        spawnInfo.nextSpawnTime = spawnInfo.nextSpawnIndex * spawnInfo.spawnDuration / spawnInfo.enemiesToSpawn.Count;

        Vector3 randomPosition = new Vector3(
            Random.Range(0.5f * -spawnerArea.x, 0.5f * spawnerArea.x),
            Random.Range(0.5f * -spawnerArea.y, 0.5f * spawnerArea.y),
            Random.Range(0.5f * -spawnerArea.z, 0.5f * spawnerArea.z)
        );
        randomPosition += transform.position;

        AIEnemy instantiatedEnemy = spawnController.GetEnemy(enemyType);
        instantiatedEnemy.transform.SetParent(spawnController.activeEnemies);
        instantiatedEnemy.transform.position = randomPosition;
        instantiatedEnemy.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
        instantiatedEnemy.SetZoneController(zoneController);

        instantiatedEnemy.Restart();

        /* For particle effects */
        ParticleSystem spawnVfx = ParticlesManager.instance.LaunchParticleSystem(
            spawnVFX,
            instantiatedEnemy.transform.position + instantiatedEnemy.GetComponent<Collider>().bounds.size.y * Vector3.up / 2.0f, 
            this.transform.rotation
        );
        ActivateGameObjectOnTime onTimeVFX = spawnVfx.GetComponent<ActivateGameObjectOnTime>();
        onTimeVFX.objectToActivate = instantiatedEnemy.gameObject;
        instantiatedEnemy.gameObject.SetActive(false);
    }
    #endregion
}
