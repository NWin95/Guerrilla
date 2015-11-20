using UnityEngine;
using System.Collections;

public class AAShot : MonoBehaviour {

    bool activated;
    public float activationTime;

    void Start ()
    {
        GetComponent<ParticleSystem>().startLifetime = 0.1f;
        StartCoroutine(Activation());
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator Activation()
    {
        yield return new WaitForSeconds(activationTime);
        activated = true;
        GetComponent<ParticleSystem>().startLifetime = 0.4f;
    }

	void OnTriggerEnter(Collider coll)
    {
        if (activated)
        {
            coll.BroadcastMessage("AAShot", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
