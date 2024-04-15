using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingMinionBehavior : MinionBehavior {

  public int health;

  private int resetAttackPeriod = 25;
  private int resetAttackCounter = 0;

  private GameObject attackTarget;

  // Update is called once per frame
  void FixedUpdate() {
    sr.sortingOrder = (int)-transform.position.y;
    Vector2 originPositionLow = (Vector2) transform.position + new Vector2(0, -8);
    Vector2 originPositionHigh = (Vector2) transform.position + new Vector2(0, 4);

    RaycastHit2D hits = RaycastFrom(originPositionLow);

    if( hits.transform == null ) {
      hits = RaycastFrom(originPositionHigh);
    }

    if(hits.transform != null && attackTarget == null) {
      animator.SetBool("attacking", true);
      attackTarget = hits.transform.gameObject;
      Rigidbody2D atrb = attackTarget.GetComponent<Rigidbody2D>();
      atrb.velocity = Vector2.zero;
      atrb.AddForce(Vector2.right * transform.localScale.x * 1000);
    } else {
      if( animator.GetBool("attacking")) {
        resetAttackCounter++;

        if(resetAttackCounter > resetAttackPeriod) {
          animator.SetBool("attacking", false);
          resetAttackCounter = 0;
          if( attackTarget != null)  LootAndDestroy(attackTarget);
          attackTarget = null;
        }

      }
    }

    RunFromPlayer();

    if(player != null && (player.transform.position - transform.position).magnitude > 300) {
      Destroy(gameObject);
    }
  }

  private RaycastHit2D RaycastFrom(Vector2 origin) {
    //check nearby for Enemy tagged objects
    int layerMask = LayerMask.GetMask("Enemies");
    return Physics2D.Raycast(origin, Vector2.right * transform.localScale.x, 24.0f, layerMask);
  }

  new void OnCollisionEnter2D(Collision2D col) {
    switch(col.gameObject.tag) {
      case "Player":
      break;
      case "Enemy":
        health--;
        if(health <= 0){
          LootAndDestroy(gameObject);
        }
      break;
    }
  }

  new protected void Orient() {
    Vector2 direction = -player.transform.position + transform.position;
    Vector2 scratch = transform.localScale;
    scratch.x = Mathf.Sign(direction.x);
    transform.localScale = scratch;
  }
}
