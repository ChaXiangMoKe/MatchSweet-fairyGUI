using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

public class  GameManager: MonoBehaviour {

    public UIStart main;
	// Use this for initialization
	void Start () {
        main = new UIStart();
        main.Show();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
