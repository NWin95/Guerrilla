using UnityEngine;
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
        GetComponent<Player_MeleeHuman>().enabled = true;
        GetComponent<Grapnel>().enabled = true;

        GetComponent<Player_MovementBird>().enabled = false;

        rig.useGravity = true;
        rig.mass = 75;

        camScript.camPosTarg = new Vector3(0, 2, -4.5f);
        camScript.camRotTarg = new Vector3(7.5f, 0, 0);
    }

    void SetBird ()
    {
        human = false;
        camScript.human = false;

        humanVis.SetActive(false);
        birdVis.SetActive(true);

        Player_MovementHuman humMove = GetComponent<Player_MovementHuman>();
        humMove.grounded = false;
        humMove.contacts = 0;

        GetComponent<Player_MovementBird>().enabled = true;
        GetComponent<Player_MovementHuman>().enabled = false;
        GetComponent<Player_MeleeHuman>().enabled = false;

        Grapnel grapnelScript = GetComponent<Grapnel>();
        grapnelScript.GrapnelDetach();
        grapnelScript.GrapnelDestroy();
        grapnelScript.enabled = false;

        rig.useGravity = false;
        rig.mass = 50;

        camScript.camPosTarg = new Vector3(0, 3, -7);
        camScript.camRotTarg = new Vector3(7.5f, 0, 0);
    }
}
