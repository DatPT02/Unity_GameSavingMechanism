using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
   public int player_score;
   public float[] player_position;

   public List<CoinData> coins_data = new List<CoinData>();
   public List<MovableObjectData> movableObj_data = new List<MovableObjectData>();

   public SaveData(Player player, List<CoinData> coins, List<MovableObjectData> movableObj)
   {
        player_score = player.Score;

        player_position = new float[3];
        player_position[0] = player.transform.position.x;
        player_position[1] = player.transform.position.y;
        player_position[2] = player.transform.position.z;

        coins_data = coins;
        movableObj_data = movableObj;
   }
   
   public void LoadData(Player player, ref List<CoinData> coins, ref List<MovableObjectData> movableObj)
   {
        player.Score = player_score;
        
        Vector3 p_position;
        p_position.x = player_position[0];  
        p_position.y = player_position[1];  
        p_position.z = player_position[2];  
        player.transform.position = p_position;

        coins = coins_data;
        movableObj = movableObj_data;
   }
}
