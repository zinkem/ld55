using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class WizardControlller : MonoBehaviour
{

	public SpawnerSpec spawnSpec;
	public SpawnerSpec heavySpawnSpec;

	private Rigidbody2D rb;
	private Animator animator;
	private PlayerInput input;
	private SpriteRenderer sr;

	private Vector2 moveInputValue;
	private bool summoning = false;
	private bool channelling = false;
	private bool onCooldown = false;
	private TimerSpec.Timer timer;

	private GameData gameState;

	// Start is called before the first frame update
	void Start() {

		rb = GetComponent<Rigidbody2D>();
		if(rb == null) {
			throw new Exception("GameObject " + name + " is using PrimaryControlHandler with no RigidBody2D Attached.");
		}

		animator = GetComponent<Animator>();
		if(animator == null) {
			throw new Exception("GameObject " + name + " is using PrimaryControlHandler with no Animator Attached.");
		}

		input = GetComponent<PlayerInput>();
		if(input == null) {
			throw new Exception("GameObject " + name + " is using PrimaryControlHandler with no PlayerInput Attached.");
		}

		sr = GetComponent<SpriteRenderer>();
		if(sr == null) {
			throw new Exception("GameObject " + name + " is using PrimaryControlHandler with no SpriteRenderer Attached.");
		}

		timer = new TimerSpec.Timer(40);

		SceneManager.LoadScene("HUD", LoadSceneMode.Additive);

		gameState = GameData.GetState();
		gameState.spawnSpec = spawnSpec;
		gameState.heavySpawnSpec = heavySpawnSpec;
	}


  public void OnMove(InputValue value) {
		moveInputValue = value.Get<Vector2>();
  }

	public void OnSummon(InputValue value) {
		if( !onCooldown ) {
			summoning = true;
		}
	}

	public void OnChannel(InputValue value){
		channelling = true;
	}

	// Update is called once per frame
	void FixedUpdate() {

		if( gameState.spawnSpec != spawnSpec) {
			spawnSpec = gameState.spawnSpec;
		}

		if( gameState.heavySpawnSpec != heavySpawnSpec) {
			heavySpawnSpec = gameState.heavySpawnSpec;
		}

		sr.sortingOrder = (int)-transform.position.y;

		if(moveInputValue.magnitude < 0.1f) {
			animator.SetBool("walking", false);
		} else {
			animator.SetBool("walking", true);

			// orient player
			Vector2 scratch = transform.localScale;
			scratch.x = Mathf.Sign(moveInputValue.x);
			transform.localScale = scratch;
		}

		// move based on input
		rb.AddForce(moveInputValue * gameState.moveSpeed * rb.mass);

		if(channelling) {
			Vector2 spawnPosition = (Vector2) transform.localPosition +
					new Vector2(transform.localScale.x, 0f) * 35;

			GameObject go = Instantiate(heavySpawnSpec.thingToSpawn, spawnPosition, transform.rotation);

			AttackingMinionBehavior amb = go.GetComponent<AttackingMinionBehavior>();
			if( amb != null)
				amb.health = (int) Mathf.Sqrt(gameState.manaCrystals);

			channelling = false;
		}

		if(summoning) {
    	float interval = Mathf.PI * 2 / gameState.spawns;

			for( int i = 0; i < gameState.spawns; i++) {

				float interval_position = (interval * i);

				Vector2 spawnPosition = (Vector2) transform.localPosition +
					new Vector2(
						Mathf.Cos(interval_position) * transform.localScale.x,
						Mathf.Sin(interval_position) / 3
					) * 35;

				GameObject go = Instantiate(spawnSpec.thingToSpawn, spawnPosition, transform.rotation);
			}

			summoning = false;
			onCooldown = true;
			timer.Reset();
		}

		if(timer.Check()) {
			onCooldown = false;
		}


		// attract collect objects to player
		int layerMask = LayerMask.GetMask("Collect");
    //check nearby for Enemy tagged objects
    RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(100, 100), 0.0f, Vector2.zero, 0.0f, layerMask);

		foreach( RaycastHit2D hit in hits ) {
      if(hit.rigidbody != null) {
        Vector2 direction = (-hit.transform.position + transform.position).normalized;
    		hit.rigidbody.AddForce(direction * gameState.moveSpeed);
      }
		}

	}

	void OnCollisionEnter2D(Collision2D col) {
		if( col.gameObject.tag == "Collect") {
			if( col.gameObject.GetComponent<AudioSource>() != null) {
				col.gameObject.GetComponent<AudioSource>().Play();
			}
			Destroy(col.gameObject);
			gameState.CollectCrystals(1);
		}

		if( col.gameObject.tag == "Enemy") {
			gameState.health--;
			Destroy(col.gameObject);

			if(gameState.health <= 0) {
				SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
				Destroy(gameObject);
			}

			GetComponent<AudioSource>().Play();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if( col.gameObject.tag == "Collect") {
			Destroy(col.gameObject);
			gameState.CollectCrystals(1);
		}
	}
}

