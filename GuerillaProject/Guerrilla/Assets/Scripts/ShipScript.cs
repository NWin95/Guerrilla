using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipScript : MonoBehaviour {

    public bool engineOn;
    public List<GameObject> engines = new List<GameObject>();
	
	void Start () {
        if (engineOn)
            EnginesOn();
        else
            EnginesOff();
	}
	
	void Update () {
	
	}

    void EnginesOn ()
    {
        foreach (GameObject engine in engines)
            engine.SetActive(true);
    }

    void EnginesOff ()
    {
        foreach (GameObject engine in engines)
            engine.SetActive(false);
    }
}
