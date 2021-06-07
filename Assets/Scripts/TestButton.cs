using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Cat cat1;
    public Cat cat2;

    public int NextEnergy;
    public Cat NextCat;
    public Player player;

    private Text buttonText;

    public void ChargeEnergyToCat() {
        Debug.Log("Test button Clicked ---------------------");
        if (BoardManager.Instance.IsPlayerActionEnabled) {
            //Debug.Log("Player charge action Start !!");
            player.playerActionController.ChargeEnergy(NextCat, NextEnergy);
            RandomNext();
        }
    }

    private void RandomNext() {
        NextEnergy = Random.Range(6, 20);
        int next = Random.Range(0, 2);
        if (next == 0) {
            NextCat = cat1;
        } else {
            NextCat = cat2;
        }
        UpdateButtonText();
    }

    private void UpdateButtonText() {
        buttonText.text = NextCat.name + "(" + NextEnergy + ")";
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        RandomNext();
    }

}
