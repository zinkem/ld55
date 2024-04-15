using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningMinionBehavior : MinionBehavior {

  public int health;

  private int resetAttackPeriod = 25;
  private int resetAttackCounter = 0;

  private GameObject attackTarget;

  public SpawnerSpec spawner;
  public int period = 100;

	private TimerSpec.Timer timer;

  public int spawns;

  // Update is called once per frame
  void FixedUpdate() {
    sr.sortingOrder = (int)-transform.position.y;

    if( timer == null) {
      timer = new TimerSpec.Timer(period);
    }

    if(timer.Check()) {
      float interval = Mathf.PI * 2 / spawns;

			for( int i = 0; i < spawns; i++) {
        float interval_position = (interval * i);
				Vector2 spawnPosition = (Vector2) transform.localPosition +
					new Vector2(
						Mathf.Cos(interval_position) * transform.localScale.x,
						Mathf.Sin(interval_position) / 3
					) * 35;

				GameObject go = Instantiate(spawner.thingToSpawn, spawnPosition, transform.rotation);
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
