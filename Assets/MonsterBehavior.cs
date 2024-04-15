using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MinionBehavior {



  // Start is called before the first frame update
  protected new void Start() {
    base.Start();
  }

  // Update is called once per frame
  void FixedUpdate() {
    sr.sortingOrder = (int)-transform.position.y;
    SeekPlayer();

  }

  new void OnCollisionEnter2D(Collision2D col) {

    switch(col.gameObject.tag) {
      case "Player":
        Destroy(gameObject);
      break;
    }

  }
}
