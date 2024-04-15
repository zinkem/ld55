using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
  public SpawnerSpec[] spawnSpec;
  public TimerSpec timerSpec;
  private TimerSpec.Timer trigger;

  private int modifier = 2;

  private int heat;

  private int spawnWaves = 0;
  private int maxWaveSize = 50;


  private int waveBufferPeriod = 25;
  private int waveBufferCounter = 0;

  // Start is called before the first frame update
  void Start() {
    trigger = timerSpec.GetTimer();
    heat = 0;
  }

    // Update is called once per frame
  void FixedUpdate() {

    bool spawnThisFrame = false;
    if(spawnWaves > 0 ) {
      waveBufferCounter++;
      if(waveBufferCounter > waveBufferPeriod) {
        spawnThisFrame = true;
        spawnWaves--;
        waveBufferCounter = 0;
      }
    } else if(trigger.Check()) {
      spawnThisFrame = true;
      modifier = (int) Mathf.Ceil(modifier * 1.1f);
      heat++;
      spawnWaves = modifier/maxWaveSize;
    }

    if(spawnThisFrame) {
      int effectiveModifier = modifier;
      if(modifier > maxWaveSize) {
        int num_groups = modifier/maxWaveSize;
        effectiveModifier = modifier/num_groups;
      }

      int level = heat/5;

      int lowRand = Mathf.Min(Mathf.Max(level -1, 0), spawnSpec.Length-1);
      int highRand = Mathf.Min(level+1, spawnSpec.Length);

      for( int i = 0; i < effectiveModifier; i++) {

        int chooseSpawn = Random.Range(lowRand, highRand);

        float chooseXpos = Random.Range(150f, 240f) * ((Random.Range(0, 2) * 2) - 1);
        float chooseYpos = Random.Range(-135f, 135f);

        SpawnerSpec s = spawnSpec[chooseSpawn];
        Vector2 position = new Vector2(chooseXpos, chooseYpos);
        GameObject go = Instantiate(s.thingToSpawn, position, transform.rotation);

        i += s.value;
      }
    }
  }
}
