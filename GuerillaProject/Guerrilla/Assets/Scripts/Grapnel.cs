//
//  Get this to work well when grounded
//

using UnityEngine;
using System.Collections;

public class Grapnel : MonoBehaviour {

    public Transform playerVis;
    public GameObject grapnelPref;
    public Transform grapnelTrans;
    bool thrownBool;
    bool swinging;
    public LayerMask grapnelMask;
    public Vector3 throwAddition;
    public float throwSpeed;
    public Transform CamParent;
    Rigidbody rig;
    public float turnTime;

    void Start ()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update ()
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
        
    }

    void LookAtVel ()
    {
        Vector3 lookVec = rig.velocity;
        lookVec = lookVec.normalized;

        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        Quaternion slerp = Quaternion.Slerp(playerVis.rotation, lookRot, Time.deltaTime / turnTime);
        playerVis.rotation = slerp;
    }

    public void GroundHit ()
    {
        if (swinging || thrownBool)
        {
            //swinging = false;
            //thrownBool = false;
            GrapnelDetach();
            GrapnelDestroy();
        }
    }

    public void GrapnelDestroy ()
    {
        thrownBool = false;
        if (grapnelTrans != null)
            Destroy(grapnelTrans.gameObject);
    }

    void GrapnelThrow ()
    {
        //Debug.Log(throwDirection.normalized);

        if (!GetComponent<Player_MovementHuman>().grounded)
        {
            Vector3 pos = transform.position + (transform.forward * 1.25f);
            GameObject grapnelObj = Instantiate(grapnelPref, pos, Quaternion.identity) as GameObject;
            grapnelTrans = grapnelObj.GetComponent<Transform>();

            Vector3 throwDirection = CamParent.forward + throwAddition;
            grapnelTrans.GetComponent<Rigidbody>().velocity = (throwDirection.normalized * throwSpeed) + rig.velocity;

            thrownBool = true;
        }
    }

    void GrapnelCast ()
    {
        RaycastHit hit;
        Vector3 pos = transform.position + (transform.forward * 1.25f);
        Vector3 gPos = grapnelTrans.position;
        if (Physics.Linecast (pos, gPos, out hit, grapnelMask))
        {
            GrapnelAttach(hit);
        }
    }

    public void GrapnelDetach()
    {
        swinging = false;
        if (GetComponent<SpringJoint>())
            Destroy(gameObject.GetComponent<SpringJoint>());
    }

    void GrapnelAttach(RaycastHit hit)
    {
        swinging = true;

        SpringJoint sj = gameObject.AddComponent<SpringJoint>();
        sj.autoConfigureConnectedAnchor = false;
        sj.spring = Mathf.Infinity;
        sj.damper = Mathf.Infinity;
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

        //StartCoroutine (JointTest(sj));
    }

    IEnumerator JointTest (SpringJoint sj)
    {
        yield return new WaitForSeconds(1);
        sj.maxDistance *= 0.95f;
        yield return new WaitForSeconds(1);
        sj.maxDistance *= 0.95f;
    }
}
