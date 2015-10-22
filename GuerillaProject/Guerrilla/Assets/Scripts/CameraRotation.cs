using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public bool invertY;
    public Vector2 rotSpeed;
    public Transform player;

	void Start () {
        player = transform.parent;
        transform.SetParent(null);
	}
	
	// Update is called once per frame
	void Update () {
        CamMove();
        CamRot();
	}

    void CamMove ()
    {
        transform.position = player.position;
    }

    void CamRot ()
    {
        Vector3 locEuler = transform.localEulerAngles;
        Vector2 inputs = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

        locEuler.y += inputs.x * rotSpeed.x * Time.deltaTime;

        if (!invertY)
            locEuler.x += inputs.y * rotSpeed.y * Time.deltaTime;
        else
            locEuler.x += -inputs.y * rotSpeed.y * Time.deltaTime;
        locEuler.z = 0;

        transform.localEulerAngles = locEuler;
    }
}
