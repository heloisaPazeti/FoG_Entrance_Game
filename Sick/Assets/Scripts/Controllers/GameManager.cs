using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    [SerializeField] private UiController uiController;

    [Header("Player References")]
    [SerializeField] private GameObject playerPrefab;
    private Transform spawnPoint;
    private Player player;

    [Header("Level")]
    private LevelController lvlController;
    [HideInInspector] public bool finishLevel; 


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

        lvlController = FindAnyObjectByType<LevelController>();
        uiController.UpdateLife(player.life);
        uiController.UpdateMoney(player.money);
        uiController.SetMinQuota(lvlController.totalMoney);
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

    private void OnLoadNewScene()
    {
        lvlController = FindAnyObjectByType<LevelController>();
    }

    #endregion

    #region "Player"

    public void HurtPlayer(int damage)
    {
        player.TakeDamage(damage);
        uiController.UpdateLife(player.life);
    }

    public void GrantReward(float amount)
    {
        player.GetReward(amount);
        uiController.UpdateMoney(player.money);

        if (player.money >= lvlController.totalMoney)
            WinLevel();
    }

    public void EnqueueShot(GameObject shot)
    {
        player.EnqueueShot(shot);
    }

    #endregion

    #region "Level Management"

    public void SetLevelController(LevelController lvlC)
    {
        lvlController = lvlC;
    }

    public void GameOver()
    {
        uiController.SetGameOver(true);
    }

    public void WinLevel()
    {
        finishLevel = true;
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(lvlController.nextLevel);
    }



    #endregion
}
