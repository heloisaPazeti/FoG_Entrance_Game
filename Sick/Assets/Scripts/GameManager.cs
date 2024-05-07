using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player References")]
    [SerializeField] private GameObject playerPrefab;
    private Transform spawnPoint;
    private Player player;

    [Header("Level")]
    [HideInInspector] public float moneyNeeded;


    #region "Setup"

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        InstantiatePlayer();
        GetLevelSetup();
    }

    private void InstantiatePlayer()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        player = Instantiate(playerPrefab, spawnPoint).GetComponent<Player>();
    }

    private void GetLevelSetup()
    {
        //Gets Level Setup
    }

    #endregion

    #region "Player"

    public void HurtPlayer(float damage)
    {
        player.TakeDamage(damage);
    }

    public void GrantReward(float amount)
    {
        player.GetReward(amount);
    }

    public void EnqueueShot(GameObject shot)
    {
        player.EnqueueShot(shot);
    }

    #endregion


    #region "End Level"

    public void GameOver()
    {
        Debug.Log("Ou nous... Anyways.");
    }

    public void WinLevel()
    {
        Debug.Log("Ou yeah... Anyways.");
        // Opens Door to Next Level
    }


    #endregion
}
