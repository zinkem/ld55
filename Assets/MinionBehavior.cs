using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behaviors {
  SeekNearestEnemy,
  RandomMove,
  SeekPlayer
}

public class MinionBehavior : MonoBehaviour {

  public float speed;
  public GameObject[] death;

  protected Rigidbody2D rb;
	protected Animator animator;
	protected SpriteRenderer sr;

  protected GameObject player;
  private GameObject enemyTarget;

  public Behaviors behavior;

  // Start is called before the first frame update
  protected void Start() {

    rb = GetComponent<Rigidbody2D>();
		if(rb == null) {
			throw new Exception("GameObject " + name + " is using MinionBehavior with no RigidBody2D Attached.");
		}

		animator = GetComponent<Animator>();
		if(animator == null) {
			throw new Exception("GameObject " + name + " is using MinionBehavior with no Animator Attached.");
		}

    animator.SetBool("walking", true);

		sr = GetComponent<SpriteRenderer>();
		if(sr == null) {
			throw new Exception("GameObject " + name + " is using MinionBehavior with no SpriteRenderer Attached.");
		}

    player = GameObject.Find("Wizard");
  }

  // Update is called once per frame
  void FixedUpdate() {
    sr.sortingOrder = (int)-transform.position.y;

    switch(behavior) {
      case Behaviors.SeekNearestEnemy:
        SeekNearestEnemy();
      break;

      case Behaviors.RandomMove:
        RandomMove();
      break;

      case Behaviors.SeekPlayer:
        SeekPlayer();
      break;
    }


    if(player != null && (player.transform.position - transform.position).magnitude > 300) {
      Destroy(gameObject);
    }
  }

  public void OnCollisionEnter2D(Collision2D col) {
    switch(col.gameObject.tag) {
      case "Player":
      break;
      case "Enemy":
        LootAndDestroy(col.gameObject);
        LootCorpse();
        Destroy(gameObject);
      break;
    }
  }

  protected void Orient() {
    Vector2 scratch = transform.localScale;
    scratch.x = Mathf.Sign(rb.velocity.x);
    transform.localScale = scratch;
  }

  protected void RandomMove() {
    float x = UnityEngine.Random.Range(-1f, 1f);
    float y = UnityEngine.Random.Range(-1f, 1f);

    rb.AddForce(new Vector2(x, y) * speed);

    Orient();
  }

  protected void SeekNearestEnemy(){
    if(enemyTarget == null ) {
      int layerMask = LayerMask.GetMask("Enemies");
      //check nearby for Enemy tagged objects
      RaycastHit2D hits = Physics2D.BoxCast(transform.position, new Vector2(100, 100), 0.0f, Vector2.zero, 0.0f, layerMask);

      if(hits.transform != null) {
        enemyTarget = hits.transform.gameObject;
      } else {
        //if no Enemy found, move away from player
        RunFromPlayer();
      }
    } else {
      SeekObject(enemyTarget);
    }
  }

  protected void SeekObject(GameObject target) {
    Vector2 direction = (target.transform.position - transform.position).normalized;
    rb.AddForce(direction * speed);
    Orient();
  }

  protected void SeekPlayer() {
    if(player == null) return;

    Vector2 direction = (player.transform.position - transform.position).normalized;
    rb.AddForce(direction * speed);
    Orient();
  }

  protected void RunFromPlayer() {
    if(player == null) return;

    Vector2 direction = (-player.transform.position + transform.position).normalized;
    direction.y /= 2;
    rb.AddForce(direction * speed);
    Orient();
  }

  protected void LootAndDestroy(GameObject go) {
    MonsterBehavior mb = go.GetComponent<MonsterBehavior>();
    if(mb) mb.LootCorpse();
    Destroy(go.gameObject);
  }

  public void LootCorpse(){
    foreach(GameObject go in death) {
      Instantiate(go, transform.position, transform.rotation);
    }
  }
}
