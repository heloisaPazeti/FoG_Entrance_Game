using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text  moneyTxt; 
    [SerializeField] private TMP_Text  lifeTxt;
    [SerializeField] private TMP_Text quotaTxt;
    [SerializeField] private GameObject gameOver;

    public void SetMinQuota(int quota)
    {
        quotaTxt.text = quota.ToString();
    }

    public void UpdateLife(int life)
    {
        lifeTxt.text = life.ToString();
    }

    public void UpdateMoney(float money)
    {
        moneyTxt.text = money.ToString();
    }

    public void SetGameOver(bool setActive)
    {
        gameOver.SetActive(setActive);
    }

    
}
