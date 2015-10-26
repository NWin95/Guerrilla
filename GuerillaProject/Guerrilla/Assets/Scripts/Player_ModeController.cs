﻿using UnityEngine;
using System.Collections;

public class Player_ModeController : MonoBehaviour {

    public CameraRotation camScript;
    Rigidbody rig;
    public bool human;

    public GameObject humanVis;
    public GameObject birdVis;

	void Start () {
        rig = GetComponent<Rigidbody>();

        if (human)
            SetHuman();
        else
            SetBird();
	}
	
	void Update () {
        if (Input.GetButtonDown("ModeToggle"))
            ToggleMode();
	}

    void ToggleMode ()
    {
        if (human)
            SetBird();
        else
            SetHuman();
    }

    void SetHuman ()
    {
        human = true;
        camScript.human = true;

        humanVis.SetActive(true);
        birdVis.SetActive(false);

        GetComponent<Player_MovementHuman>().enabled = true;
        GetComponent<Grapnel>().enabled = true;

        GetComponent<Player_MovementBird>().enabled = false;

        rig.useGravity = true;
        rig.mass = 75;
    }

    void SetBird ()
    {
        human = false;
        camScript.human = false;

        humanVis.SetActive(false);
        birdVis.SetActive(true);

        GetComponent<Player_MovementBird>().enabled = true;
        GetComponent<Player_MovementHuman>().enabled = false;

        Grapnel grapnelScript = GetComponent<Grapnel>();
        grapnelScript.GrapnelDetach();
        grapnelScript.GrapnelDestroy();
        grapnelScript.enabled = false;

        rig.useGravity = false;
        rig.mass = 50;
    }
}
