using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour {

    Rigidbody rig;
    public float maxSpeed;
    public float jumpHeight;
    public bool grounded;
    public Transform groundPoint;
    Vector3 inputVel;
    public float turnTime;

    public Transform camParent;
    bool gRayCheck;
    bool gContactCheck;
    int contacts;

    float inputMag;

    float test;

	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        GroundingFunc();
        if (Input.GetButtonDown("Jump") && grounded)
            Jump();
        LookDirection();
	}

    void FixedUpdate ()
    {
        MovementFxied();
    }

    void LookDirection ()
    {
        if (inputMag > 0.1f)
        {
            Vector3 lookVec = camParent.forward;
            lookVec.y = 0;
            lookVec = lookVec.normalized;

            Vector3 lookPos = transform.position + lookVec;
            Quaternion lookRot = Quaternion.LookRotation(lookVec);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime / turnTime);
            transform.rotation = slerp;
        }
    }

    void GroundingFunc ()
    {
        Vector3 pos = groundPoint.position + (Vector3.up * 0.5f);
        if (Physics.Raycast(pos, -Vector3.up, 0.75f))
            gRayCheck = true;
        else
            gRayCheck = false;

        if (contacts > 0)
            gContactCheck = true;
        else
            gContactCheck = false;

        if (gRayCheck || gContactCheck)
            grounded = true;
        else
            grounded = false;

        //Debug.Log(gContactCheck);
    }

    void Jump ()
    {
        Vector3 vel = rig.velocity;

        vel += inputVel;

        float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;
        vel.y = jumpVel;
        rig.velocity = vel;

        //Debug.Log(rig.velocity);
    }

    void MovementFxied ()
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputMag = inputVec.magnitude;
        inputVec = transform.TransformDirection(inputVec);

        if (grounded)
        {
            inputVel = inputVec * maxSpeed;

            Vector3 pos = transform.position;
            rig.MovePosition(pos + (inputVel * Time.fixedDeltaTime));
        }
    }

    void OnCollisionStay (Collision collision)
    {
        contacts = collision.contacts.Length;
    }

    void OnCollisionExit ()
    {
        contacts = 0;
    }
}
