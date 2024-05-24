using System.Collections;
using System.Collections.Generic;
using UnityEngine;
Using TMPro;

public class UiController : MonoBehaviour
{
   [Header("UI References")]
   [SerializeField] private TMP_Text  moneyTxt; 
   [SerializeField] private TMP_Text  lifeTxt;

    public void UpdateLife(int life)
    {
        lifeTxt.text = life.ToString();
    }

    public void UpdateMoney(int money)
    {
        moneyTxt.text = money.ToString();
    }
}
