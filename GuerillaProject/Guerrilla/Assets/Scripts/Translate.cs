using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {

    public Vector3 moveVec;
    public float speed;
    Rigidbody rig;

	void Start ()
    {
        rig = GetComponent<Rigidbody>();
        rig.velocity = moveVec * speed;
    }
    /*
    void FixedUpdate ()
    {
        Vector3 pos = transform.position;
        rig.MovePosition(pos + (moveVec * speed * Time.fixedDeltaTime));
//        Debug.Log(rig.velocity);
    } */  
}
