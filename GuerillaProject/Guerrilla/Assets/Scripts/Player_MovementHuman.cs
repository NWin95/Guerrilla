using UnityEngine;
using System.Collections;

public class Player_MovementHuman : MonoBehaviour {

    public Animator animator;
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
    bool groundedHold;
    public int contacts;

    float inputMag;

    Vector3 nonKinVel;
    Vector3 rigVelCus;
    Vector3 posA;
    Vector3 posB;

	void Start () {
        rig = GetComponent<Rigidbody>();
        posB = rig.position;
	}
	
	// Update is called once per frame
	void Update () {
        GroundingFunc();
        if (Input.GetButtonDown("Jump") && grounded)
            Jump();
        LookDirection();
        AnimationFunc();
	}

    void FixedUpdate ()
    {
        MovementFxied();
        RigVelCusFunc();
    }

    
    void AnimationFunc ()
    {
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("Speed_Input", nonKinVel.magnitude);
    }

    void RigVelCusFunc ()       //      Gets velocity including added (rigVelCus) and gets only the added (nonKinVel)
    {
        posA = rig.position;
        Vector3 dif = posA - posB;
        Vector3 camVec = camParent.forward;
        camVec.y = 0;
        dif += camVec * 0.0001f;
        rigVelCus = dif / Time.fixedDeltaTime;
        posB = rig.position;

        nonKinVel = rigVelCus - rig.velocity;
        //Debug.Log(nonKinVel.magnitude);
    }
    

    void LookDirection ()
    {
        if (inputMag > 0.1f)
        {
            Vector3 lookVec = camParent.forward;
            lookVec.y = 0;
            lookVec = lookVec.normalized;

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

        if (grounded && !groundedHold)                  //Hit the ground
            GetComponent<Grapnel>().GroundHit();
        if (!grounded && groundedHold)                  //Leave the ground
            LeaveGround();

        groundedHold = grounded;
        //Debug.Log(gContactCheck);
    }

    void LeaveGround ()
    {
        Vector3 vel = rig.velocity;
        vel += inputVel;
        rig.velocity = vel;
    }

    void Jump ()
    {
        Vector3 vel = rig.velocity;

        //vel += inputVel;

        float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;
        vel.y = jumpVel;
        rig.velocity = vel;

        //Debug.Log(rig.velocity);
    }

    void MovementFxied ()
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputVec = Vector3.ClampMagnitude(inputVec, 1);
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
        foreach (ContactPoint cPoint in collision.contacts)
        {
            if (Vector3.Angle(Vector3.up, cPoint.normal) < 75)
            {
                contacts++;
            }
        }
    }

    void OnCollisionExit ()
    {
        contacts = 0;
    }
}
