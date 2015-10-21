using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {

    public Vector3 moveVec;
    public float speed;

	void Start ()
    {
        GetComponent<Rigidbody>().velocity = moveVec * speed;
    }
}
