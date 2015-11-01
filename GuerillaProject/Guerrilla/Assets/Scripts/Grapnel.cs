using UnityEngine;
using System.Collections;

public class Grapnel : MonoBehaviour {

    public Animator animator;
    public Transform playerVis;
    public GameObject grapnelPref;
    public Transform grapnelTrans;
    bool thrownBool;
    bool swinging;
    public LayerMask grapnelMask;
    public Vector3 throwAddition;
    public float throwSpeed;
    public Transform camParent;
    Rigidbody rig;
    public float turnTime;
    bool grapnel;
    Vector3 toGrapnel;
    public float grapnelRange;

    //Vector3 rigVelCus;
    //Vector3 posA;
    //Vector3 posB;

    //Player_MovementHuman moveScript;

    void Start()
    {
        //moveScript = GetComponent<Player_MovementHuman>();
        rig = GetComponent<Rigidbody>();
        //posB = rig.position;

        //Debug.Log(rig.position);
        //Debug.Log(transform.position);
    }

    void Update()
    {
        if (Input.GetButtonDown("Grapnel"))
            GrapnelThrow();
        if (Input.GetButtonUp("Grapnel") && thrownBool)
            GrapnelDestroy();
        if (Input.GetButtonUp("Grapnel") && swinging)
            GrapnelDetach();

        if (thrownBool)
            GrapnelCast();
        //if (swinging)
        //    LookAtVel();

        LookAtFunc();
        AnimationFunc();
    }

    void AnimationFunc()
    {
        animator.SetBool("Swinging", swinging);
        animator.SetBool("GrapnelThrow", thrownBool);
    }

    /*
    void FixedUpdate ()
    {
        RigVelCusFunc();
        LookAtVel();
    }   */
    /*
    void RigVelCusFunc ()
    {
        posA = rig.position;
        Vector3 dif = posA - posB;
        Vector3 camVec = CamParent.forward;
        camVec.y = 0;
        dif += camVec * 0.01f;
        rigVelCus = dif / Time.fixedDeltaTime;
        posB = rig.position;
    }   */
    
    void LookAtFunc ()                                   //Keep this for a while
    {
        //Vector3 lookVec = rig.velocity;
        //bool mGrounded = moveScript.grounded;

        Vector3 lookVec = Vector3.zero;
        if (swinging)
            lookVec = rig.velocity + (transform.forward * 0.0001f) + (toGrapnel * 0.0001f);
        else if (thrownBool)
            lookVec = toGrapnel + (Vector3.up * 0.0001f);
        else
            lookVec = transform.forward;
        //if (mGrounded)
        //    lookVec -= rig.velocity;
        
        lookVec = lookVec.normalized;

        if (lookVec == Vector3.zero)
        {
            Debug.Log("Warning Now");

            Debug.Log("Swinging: " + swinging);
            Debug.Log("Thrown: " + thrownBool);
        }
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        Quaternion slerp = Quaternion.Slerp(playerVis.rotation, lookRot, Time.deltaTime / turnTime);
        playerVis.rotation = slerp;
    }   
    
    public void GroundHit ()    //  Handle what happens when the ground is touched
    {
    /*    if (swinging || thrownBool)
        {
            //swinging = false;
            //thrownBool = false;
            GrapnelDetach();
            GrapnelDestroy();
        }   */
    }

    public void GrapnelDestroy ()   //  Destroy grapnel when it's unconnected / not swinging
    {
        thrownBool = false;
        if (grapnelTrans != null)
            Destroy(grapnelTrans.gameObject);
    }

    void GrapnelThrow ()    //  Thow grapnel
    {
        Vector3 pos = transform.position + (transform.forward * 1.25f);
        GameObject grapnelObj = Instantiate(grapnelPref, pos, Quaternion.identity) as GameObject;
        grapnelTrans = grapnelObj.GetComponent<Transform>();

        Vector3 throwDirection = camParent.forward + throwAddition;
        grapnelTrans.GetComponent<Rigidbody>().velocity = (throwDirection.normalized * throwSpeed) + rig.velocity;

        thrownBool = true;

        SpringJoint sj = grapnelObj.GetComponent<SpringJoint>();
        sj.connectedBody = rig;
        sj.maxDistance = grapnelRange;
    }

    void GrapnelCast () //  Test space in rope line
    {
        RaycastHit hit;
        Vector3 pos = transform.position + (transform.forward * 1.25f);
        Vector3 gPos = grapnelTrans.position;

        toGrapnel = gPos - pos;

        if (Physics.Linecast (pos, gPos, out hit, grapnelMask))
        {
            GrapnelAttach(hit);
        }
    }

    public void GrapnelDetach() //  Destroys grapnel after successful use
    {
        swinging = false;
        if (GetComponent<SpringJoint>())
            Destroy(gameObject.GetComponent<SpringJoint>());
    }

    void GrapnelAttach(RaycastHit hit)  //  Creates spring joint
    {
        swinging = true;

        SpringJoint sj = gameObject.AddComponent<SpringJoint>();
        sj.autoConfigureConnectedAnchor = false;
        
        //          Mathf.Infinity bugs with high force / velocity
        sj.spring = 9999999 * GetComponent<Rigidbody>().mass;
        sj.damper = 9999999 * GetComponent<Rigidbody>().mass;
        sj.breakForce = 9999999 * GetComponent<Rigidbody>().mass;
        sj.enableCollision = true;

        Vector3 pos = transform.position + (transform.forward * 1.25f);
        float dis = Vector3.Distance(pos, hit.point);
        sj.maxDistance = dis + 1;

        Rigidbody rig = hit.transform.GetComponent<Rigidbody>();
        if (rig)
        {
            sj.connectedBody = rig;
            sj.connectedAnchor = hit.transform.InverseTransformPoint(hit.point);
        }
        else
        {
            sj.connectedAnchor = hit.point;
        }
        GrapnelDestroy();
    }
}
