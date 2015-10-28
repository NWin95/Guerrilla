using UnityEngine;
using System.Collections;

public class Player_MovementBird : MonoBehaviour {

    public Transform camParent;
    Rigidbody rig;
    //float inputMag;
    public float maxSpeed;
    Vector3 inputVel;
    public float turnTime;

	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	
	void Update () {
        LookDirection();
	}

    void FixedUpdate()
    {
        MovementFixed();
        //LookDirection();
    }

    void LookDirection()
    {
        //Vector3 lookVec = camParent.forward;
        //lookVec = lookVec.normalized;

        //Quaternion lookRot = Quaternion.LookRotation(lookVec);
        //Quaternion slerp = Quaternion.Slerp(transform.rotation, lookRot, Time.fixedDeltaTime / turnTime);     //Probably
        //Quaternion slerp = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime / turnTime);
        Quaternion targRot = camParent.rotation;
        Quaternion slerp = Quaternion.Slerp(transform.rotation, targRot, Time.deltaTime / turnTime);
        //Quaternion slerp = Quaternion.Slerp(transform.rotation, targRot, Time.fixedDeltaTime / turnTime);

        transform.rotation = slerp;
    }

    void MovementFixed ()
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //inputMag = inputVec.magnitude;
        inputVec = transform.TransformDirection(inputVec);

        inputVel = inputVec * maxSpeed;

        rig.velocity = inputVel;

        //Vector3 pos = transform.position;
        //rig.MovePosition(pos + (inputVel * Time.fixedDeltaTime));
    }
}
