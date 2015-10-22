using UnityEngine;
using System.Collections;

public class Grapnel : MonoBehaviour {

    public GameObject grapnelPref;
    public Transform grapnelTrans;
    bool thrownBool;
    bool swinging;
    public LayerMask grapnelMask;
    public Vector3 throwDirection;
    public float throwSpeed;

    void Start ()
    {

    }

    void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
            GrapnelThrow();
        if (Input.GetButtonUp("Fire1") && thrownBool)
            GrapnelDestroy();
        if (Input.GetButtonUp("Fire1") && swinging)
            GrapnelDetach();

        if (thrownBool)
            GrapnelCast();
    }

    void GrapnelDestroy ()
    {
        thrownBool = false;
        Destroy(grapnelTrans.gameObject);
    }

    void GrapnelThrow ()
    {
        //Debug.Log(throwDirection.normalized);

        Vector3 pos = transform.position + (transform.forward * 1.25f);
        GameObject grapnelObj = Instantiate(grapnelPref, pos, Quaternion.identity) as GameObject;
        grapnelTrans = grapnelObj.GetComponent<Transform>();

        grapnelTrans.GetComponent<Rigidbody>().velocity = throwDirection.normalized * throwSpeed;

        thrownBool = true;
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

    void GrapnelDetach()
    {
        Destroy(gameObject.GetComponent<SpringJoint>());
    }

    void GrapnelAttach(RaycastHit hit)
    {
        swinging = true;

        SpringJoint sj = gameObject.AddComponent<SpringJoint>();
        sj.autoConfigureConnectedAnchor = false;
        sj.spring = Mathf.Infinity;
        sj.damper = Mathf.Infinity;
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
