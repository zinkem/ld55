using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnerSpec : ScriptableObject
{
  public GameObject thingToSpawn;
  public int value = 0;
  public int num_spawns = 1;
  public int health = 1;

  public string spawnname;
  public string description;
}
