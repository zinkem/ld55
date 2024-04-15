using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollect : MonoBehaviour {
  public SpawnerSpec[] choices;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public SpawnerSpec PickWeapon() {
      int choice = Random.Range(0, choices.Length);
      return choices[choice];
  }
}
