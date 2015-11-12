using UnityEngine;
using System.Collections;

public class Player_MeleeHuman : MonoBehaviour {

    public Transform aggressor;
    public LayerMask counterMask;
    public float counterRange;
    public bool canCounter;
    public Animator animator;

	void Start () {
	
	}
	
	void Update () {
	    if (Input.GetButtonDown("Counter"))
        {
            CounterCheck();
        }
	}

    void CounterCheck ()
    {
        Debug.Log("CounterCheck");

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

        Debug.Log(aggressor.name);
        if (aggressor != null)
        {
            StartCoroutine (Counter());
        }
    }

    IEnumerator Counter ()
    {
        canCounter = false;
        Vector3 lookVec = aggressor.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;

        animator.SetInteger("AttackInt", 1);
        animator.SetBool("Countering", true);

        StartCoroutine (aggressor.GetComponent<EnemyAttack>().Countered());
        float time = 0.5f;

        yield return new WaitForSeconds(time);

        animator.SetInteger("AttackInt", 0);
        animator.SetBool("Countering", false);
        canCounter = true;
    }
}
