using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerAction : MonoBehaviour {
    private float cash;
    private int oilNumber = 0;
    private int goldNumber = 0;
    PriceManager priceManager;


    private void Start() {
        priceManager = FindObjectOfType<PriceManager>();
        cash = 10000;
    }

    public void BuyGold() {
        float currentGoldPrice = priceManager.GetGoldPrice();
        try {
            Withdraw(currentGoldPrice);
            goldNumber++;

        } catch {
            print("Fond insuffisant");
        }
    }

    public void SellGold() {
        if (goldNumber > 0) {
            cash += priceManager.GetGoldPrice();
            goldNumber--;
        }
    }

    public void BuyOil() {
        float currentOilPrice = priceManager.GetOilPrice();
        try {
            Withdraw(currentOilPrice);
            oilNumber++;

        } catch {
            print("Fond insuffisant");
        }
    }

    public void SellOil() {
        if (oilNumber > 0) {
            cash += priceManager.GetOilPrice();
            oilNumber--;
        }
    }

    public int GetOilNumber() {
        return oilNumber;
    }

    public int GetGoldNumber() {
        return goldNumber;
    }

    public float GetAvailableCash() {
        return cash;
    }

    private void Withdraw(float amount) {
        if (amount > cash) {
            throw new System.ArgumentException("Manque de fond");
        } else {
            cash = cash - amount;
        }
    }
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "gameInfo.dat", FileMode.Create);
        Player playerDataToSave = new Player();
        playerDataToSave.QteArgent = cash;
        playerDataToSave.QtePetrole = oilNumber;
        playerDataToSave.QteOr = goldNumber;
        bf.Serialize(file, playerDataToSave);
        file.Close();
    }
    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.persistentDataPath + "gameInfo.dat"))
        {
            throw new Exception("Game file don't exist");
        }
        FileStream file = File.Open(Application.persistentDataPath + "gameInfo.dat", FileMode.Open);
        Player playeDataToLoad = (Player)bf.Deserialize(file);
        file.Close();
        cash = playeDataToLoad.QteArgent;
        oilNumber = playeDataToLoad.QtePetrole;
        goldNumber = playeDataToLoad.QteOr;
    }


}
