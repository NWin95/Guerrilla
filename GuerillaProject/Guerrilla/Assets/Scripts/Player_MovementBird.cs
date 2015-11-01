using UnityEngine;
using System.Collections;

public class Player_MovementBird : MonoBehaviour {

    public Animator animator;
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
        AnimationFunc();
	}

    void FixedUpdate()
    {
        MovementFixed();
    }

    void AnimationFunc()
    {
        float angle = Vector3.Angle(transform.forward, Vector3.up);
        float vel = rig.velocity.magnitude;

        animator.SetFloat("Angle", angle);
        animator.SetFloat("Velocity", vel);
    }

    void LookDirection()
    {
        Quaternion targRot = camParent.rotation;
        Quaternion slerp = Quaternion.Slerp(transform.rotation, targRot, Time.deltaTime / turnTime);

        transform.rotation = slerp;
    }

    void MovementFixed ()
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputVec = Vector3.ClampMagnitude(inputVec, 1);
        //inputMag = inputVec.magnitude;
        inputVec = transform.TransformDirection(inputVec);

        inputVel = inputVec * maxSpeed;

        rig.velocity = inputVel;
    }
}
