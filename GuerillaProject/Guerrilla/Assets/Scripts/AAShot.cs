using UnityEngine;
using System.Collections;

public class AAShot : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
    {
        coll.BroadcastMessage("AAShot", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
