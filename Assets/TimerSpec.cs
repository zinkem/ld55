using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimerSpec : ScriptableObject
{
  public int period;

  public class Timer {
    private int counter;
    private int period;
    public Timer(int spawnPeriod) {
      period = spawnPeriod;
      counter = 0;
    }

    public bool Check() {
      counter++;

      if(counter > period ) {
        counter = 0;
        return true;
      }

      return false;
    }

    public void Reset() {
      counter = 0;
    }

    public int GetPeriod() {
      return period;
    }
  }

  public Timer GetTimer() {
    return new Timer(period);
  }


}
