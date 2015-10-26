using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public bool invertY;
    public Vector3 rotSpeedH;
    public Vector3 rotSpeedB;
    public Transform player;

    public bool human;

	void Start () {
        player = transform.parent;
        transform.SetParent(null);
	}
	
	// Update is called once per frame
	void Update () {
        //CamMove();
        CamRot();
	}

    void LateUpdate ()
    {
        CamMove();
        //CamRot();
    }

    /*
    void FixedUpdate ()
    {
        //CamMove();
        //CamRot();
    }   */

    void CamMove ()
    {
        transform.position = player.position;
    }

    void CamRot ()
    {
        Vector3 locEuler = transform.localEulerAngles;
        Vector3 inputs;
        if (human)
        {
            //  Yaw  Pitch   Roll
            inputs = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);

            locEuler.y += inputs.x * rotSpeedH.x * Time.deltaTime;

            if (!invertY)
                locEuler.x += inputs.y * rotSpeedH.y * Time.deltaTime;
            else
                locEuler.x += -inputs.y * rotSpeedH.y * Time.deltaTime;
            locEuler.z = 0;

            transform.localEulerAngles = locEuler;
        }
        else
        {
            //  Pitch   Yaw     Roll
            inputs = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), Input.GetAxis("Roll"));

            Vector3 result;
            result.y = inputs.y * rotSpeedB.y;
            result.z = inputs.z * rotSpeedB.z;

            if (!invertY)
            {
                result.x = inputs.x * rotSpeedB.x;
            }
            else
            {
                result.x = inputs.x * rotSpeedB.x;
            }

            transform.Rotate(result);
            //transform.Rotate(inputs);
        }
    }
}
