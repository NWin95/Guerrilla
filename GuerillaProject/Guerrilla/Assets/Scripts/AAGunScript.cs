using UnityEngine;
using System.Collections;

public class AAGunScript : MonoBehaviour {

    public GameObject shot;

    public float shotSpeed;
    public float range;
    bool inRange;

    public Transform player;
    Rigidbody playerRig;
    public Transform parent;
    public Transform shaft;
    public Transform gun;

    public bool aimBool;

    public float shotTime;
    float shotCount;

    float travelTime;

	void Start () {
        if (parent != null)
            transform.SetParent(parent);
        playerRig = player.GetComponent<Rigidbody>();
        shotCount = shotTime;
	}
	
	void Update () {
        Aim();
        Gun();
	}

    void Gun ()
    {
        if (inRange)
        {
            shotCount -= Time.deltaTime;
            if (shotCount <= 0)
                Shoot();
        }
    }

    void Shoot ()
    {
        shotCount = shotTime;

        //Rigidbody shotSpawned = Rigidbody.Instantiate(shot, gun.position, Quaternion.identity) as Rigidbody;
        GameObject shotSpawned = Instantiate(shot, gun.position, Quaternion.identity) as GameObject;
        Vector3 pos = player.position + (playerRig.velocity * (travelTime * 0.75f));
        Vector3 dir = (pos - gun.position).normalized;

        shotSpawned.GetComponent<Rigidbody>().velocity = dir * shotSpeed;
    }

    void Aim ()
    {
        float rangeTest = Vector3.Distance(gun.position, player.position);
        if (rangeTest < range)
            inRange = true;
        else
            inRange = false;

        if (inRange)
        {
            //Debug.Log(inRange);
            if (aimBool)
            {
                Vector3 targVel = playerRig.velocity;
                float dis = Vector3.Distance(gun.position, player.position + targVel);
                travelTime = dis / shotSpeed;
                Vector3 targPos = player.position + (playerRig.velocity * (travelTime * 0.75f));

                Vector3 sVec = shaft.position - targPos;
                sVec.y = 0;
                Quaternion sRot = Quaternion.LookRotation(sVec);

                Vector3 gVec = gun.position - targPos;
                //gVec.x = 0;
                //gVec.z = 0;
                Quaternion gRot = Quaternion.LookRotation(gVec);

                shaft.rotation = sRot;
                gun.rotation = gRot;
            }
        }
    }
}
