using UnityEngine;
using System.Collections;

public class SpeedTest : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;

    float dis;
    public float timeA;
    public float timeB;
    public bool count;
    float fin;

	void Start () {
        dis = Vector3.Distance(pointA.position, pointB.position);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Physics.Raycast (pointA.position, Vector3.right, 5) && !count)
        {
            timeA = Time.time;
            count = true;
        }
        if (Physics.Raycast (pointB.position, Vector3.right, 5) && count)
        {
            timeB = Time.time - timeA;
            Debug.Log("Clock: " + timeB);

            fin = dis / timeB;
            Debug.Log("Fin: " + fin);
            count = false;
        }
	}
}
