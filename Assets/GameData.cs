using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData {

  public static GameData current;
  public static GameData GetState() {
    if(current == null) {
      current = new GameData();
    }
    return current;
  }


  public int health;
  public int manaCrystals;
  public int fireSpeed;
  public float moveSpeed;
  public int spawns;

  public SpawnerSpec spawnSpec;
  public SpawnerSpec heavySpawnSpec;

  public GameData() {
    health = 100;
    manaCrystals = 0;
    fireSpeed = 25;
    moveSpeed = 500;
    spawns = 1;
  }

  public GameData CollectCrystals(int num) {
    manaCrystals += num;
    moveSpeed = 500 + Mathf.Sqrt(manaCrystals) * 10;

    moveSpeed = Mathf.Min(moveSpeed, 1000);

    int oldSpawns = spawns;
    spawns = (int)Mathf.Floor(Mathf.Sqrt(manaCrystals)/3) + 1;
    if (oldSpawns != spawns) {
      SceneManager.LoadScene("PowerupMenu", LoadSceneMode.Additive);
    }
    return this;
  }
}