using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

public class  GameManager: MonoBehaviour {

    public UIStart start;
	// Use this for initialization
	void Start () {
        start = new UIStart();
        start.Show();
	}

}
