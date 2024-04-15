using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
  bool exit = false;

  public SpawnerSpec[] choices;
  public SpawnerSpec[] heavyChoices;

  int selected = 0;

  GameObject[] selections = new GameObject[3];
  GameObject[] buttons = new GameObject[3];

  private int choice1;
  private int choice2;

  // Start is called before the first frame update
  void Start()
  {
    Time.timeScale = 0.0f;

    //setup
    choice1 = Random.Range(0, choices.Length);
    choice2 = Random.Range(0, heavyChoices.Length);

    selections[0] = GameObject.Find("Choice 1");
    SpriteRenderer sr = choices[choice1].thingToSpawn.GetComponent<SpriteRenderer>();
    selections[0].GetComponent<Image>().sprite = sr.sprite;

    selections[1] = GameObject.Find("Choice 2");
    sr = heavyChoices[choice2].thingToSpawn.GetComponent<SpriteRenderer>();
    selections[1].GetComponent<Image>().sprite = sr.sprite;

    buttons[0] = GameObject.Find("Button 1");
    buttons[1] = GameObject.Find("Button 2");
    buttons[2] = GameObject.Find("Button 3");

    buttons[selected].transform.localScale = buttons[selected].transform.localScale * -1;


    GameObject go = GameObject.Find("Title 1");
    go.GetComponent<TMP_Text>().text = choices[choice1].spawnname;

    go = GameObject.Find("Description 1");
    go.GetComponent<TMP_Text>().text = choices[choice1].description;

    go = GameObject.Find("Title 2");
    go.GetComponent<TMP_Text>().text = heavyChoices[choice2].spawnname;

    go = GameObject.Find("Description 2");
    go.GetComponent<TMP_Text>().text = heavyChoices[choice2].description;

  }

  // Update is called once per frame
  void Update()
  {
    if(exit){
      switch(selected) {
        case 0:
          GameData.GetState().spawnSpec = choices[choice1];
        break;
        case 1:
          GameData.GetState().heavySpawnSpec = heavyChoices[choice2];
        break;
        case 2:
          int curCrystals = GameData.GetState().manaCrystals;
          GameData.GetState().CollectCrystals((int) Mathf.Sqrt(curCrystals) * 4);
        break;
      }
      Time.timeScale = 1.0f;
      SceneManager.UnloadSceneAsync("PowerupMenu");
    }
  }

  public void OnSubmit(InputValue value) {
    exit = true;
  }

  private float lastInputTime = 0.0f;
  public void OnMove(InputValue value) {

    float now = Time.realtimeSinceStartup;
    if( now - lastInputTime < 0.1f ) {
      return;
    }

    Vector2 input = value.Get<Vector2>();

    if( input.x > 0.5f) {
      ChangeSelection(1);
    }

    if( input.x < -0.5f) {
      ChangeSelection(2);
    }

    lastInputTime = now;
  }

  void ChangeSelection(int n){
    buttons[selected].transform.localScale = buttons[selected].transform.localScale * -1;
    selected = (selected + n ) % buttons.Length;
    buttons[selected].transform.localScale = buttons[selected].transform.localScale * -1;
  }
}
