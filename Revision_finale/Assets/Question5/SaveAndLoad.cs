using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour {
    PlayerAction playerAction;
    

    private void Start() {
        playerAction = FindObjectOfType<PlayerAction>();
        
    }
   
	
	
    public void SaveGame() {
        playerAction.SaveGame();
    }

    public void LoadGame() {
        playerAction.LoadGame();
    }
    
}
[Serializable]
class Player
{
    public int QtePetrole;
    public int QteOr;
    public float QteArgent;

}
