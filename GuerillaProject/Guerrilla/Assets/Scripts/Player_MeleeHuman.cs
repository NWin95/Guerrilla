using UnityEngine;
using System.Collections;

public class Player_MeleeHuman : MonoBehaviour {

    public Transform aggressor;
    public LayerMask counterMask;
    public float counterRange;
    public bool canCounter;
    public Animator animator;
    Vector3 relPos;
    Rigidbody rig;

	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	void Update () {
	    if (Input.GetButtonDown("Counter"))
        {
            CounterCheck();
        }
	}

    void CounterCheck ()
    {
        Vector3 pos = transform.position;
        float dis = counterRange + 0.25f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, counterRange, counterMask);

        aggressor = null;
        foreach (Collider col in colliders)
        {
            Transform testTrans = col.transform;
            float disTest = Vector3.Distance(testTrans.position, pos);
            if (disTest < dis)
            {
                dis = disTest;
                aggressor = testTrans;
            }
        }

        if (aggressor != null)
        {
            StartCoroutine (Counter());
        }
    }

    IEnumerator Counter ()
    {
        canCounter = false;

        StartCoroutine(MoveTo());

        animator.SetInteger("AttackInt", 1);
        animator.SetTrigger("CounterTrigger");

        StartCoroutine (aggressor.GetComponent<EnemyAttack>().Countered());
        float time = 0.5f;

        yield return new WaitForSeconds(time);

        animator.SetInteger("AttackInt", 0);
        canCounter = true;
    }

    IEnumerator MoveTo ()
    {
        Vector3 lookVec = aggressor.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;

        yield return new WaitForEndOfFrame();

        relPos = new Vector3( 0, 0, 1.25f);
        Vector3 targPos = aggressor.TransformPoint(relPos);
        transform.position = targPos;

        rig.velocity = aggressor.GetComponent<Rigidbody>().velocity;
    }
}
