using UnityEngine;
using System.Collections;

public class AAGunScript : MonoBehaviour {

    public Rigidbody shot;

    //public float shotSpeed;

    public Transform player;
    //Rigidbody playerRig;
    public Transform parent;
    public Transform shaft;
    public Transform gun;

    public bool aimBool;

	void Start () {
        if (parent != null)
            transform.SetParent(parent);
        //playerRig = player.GetComponent<Rigidbody>();
	}
	
	void Update () {
        Aim();
	}

    void Aim ()
    {
        if (aimBool)
        {
            Vector3 sVec = shaft.position - player.position;
            sVec.y = 0;
            Quaternion sRot = Quaternion.LookRotation(sVec);

            Vector3 gVec = gun.position - player.position;
            //gVec.x = 0;
            //gVec.z = 0;
            Quaternion gRot = Quaternion.LookRotation(gVec);

            shaft.rotation = sRot;
            gun.rotation = gRot;
        }
    }
}
