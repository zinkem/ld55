using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowMinionBehavior : MinionBehavior {

  // minion collision attributes but no update
  // Start is called before the first frame update
  new protected void Start() {

  }

  // Update is called once per frame
  void FixedUpdate() {

  }

  new public void OnCollisionEnter2D(Collision2D col) {
    switch(col.gameObject.tag) {
      case "Player":
      break;
      case "Enemy":
        LootAndDestroy(col.gameObject);
      break;
    }

  }
}
