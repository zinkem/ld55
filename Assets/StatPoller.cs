using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPoller : MonoBehaviour {

  TMP_Text tmp;
  GameData stats;

  public enum Kind {
    health,
    crystals
  };

  public Kind type;
  // Start is called before the first frame update
  void Start()
  {
    tmp = GetComponent<TMP_Text>();
    stats = GameData.GetState();
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    switch(type) {
      case Kind.health:
        tmp.text = ""+stats.health;
      break;
      case Kind.crystals:
        tmp.text = ""+stats.manaCrystals;
      break;
    }
  }
}
