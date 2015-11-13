using UnityEngine;
using System.Collections;

public class Player_MeleeHuman : MonoBehaviour {

    public Transform camTrans;
    public Transform aggressor;
    public float angleAllowence;
    public Transform target;
    public LayerMask enemyMask;
    public LayerMask counterMask;
    public float strikeRange;
    public float counterRange;
    public bool canAttack;
    public bool canCounter;
    public Animator animator;
    Vector3 relPos;
    Rigidbody rig;
    int attackInt;
    Player_MovementHuman movement;

	void Start () {
        rig = GetComponent<Rigidbody>();
        movement = GetComponent<Player_MovementHuman>();
        canCounter = true;
	}
	
	void Update () {
        if (Input.GetButtonDown("Strike"))
            StrikeCheck();
	    if (Input.GetButtonDown("Counter"))
            CounterCheck();
	}

    void StrikeCheck ()
    {
        Vector3 pos = transform.position;
        float dis = strikeRange + 0.25f;
        Collider[] colliders = Physics.OverlapSphere(pos, strikeRange, enemyMask);

        Vector3 inputDirSmoothed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (inputDirSmoothed.magnitude < 0.1f)
            inputDir = camTrans.forward;
        else
            inputDir = camTrans.TransformDirection(inputDir);

        inputDir.y = 0;
        inputDir = inputDir.normalized;

        target = null;
        foreach (Collider col in colliders)
        {
            Transform testTrans = col.transform;

            Vector3 testVec = testTrans.position - transform.position;
            float testAngle = Vector3.Angle(inputDir, testVec);

            if (testAngle <= angleAllowence)
            {
                float disTest = Vector3.Distance(testTrans.position, pos);
                if (disTest < dis)
                {
                    dis = disTest;
                    target = testTrans;
                }
            }
        }

        if (target != null)
            StartCoroutine (Strike());
    }

    void CounterCheck ()
    {
        Vector3 pos = transform.position;
        float dis = counterRange + 0.25f;
        Collider[] colliders = Physics.OverlapSphere(pos, counterRange, counterMask);

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
            if (canCounter)
                StartCoroutine (Counter());
        }
    }

    public IEnumerator Hit (int recievedInt)
    {
        animator.SetTrigger("HitTrigger");
        animator.SetInteger("AttackInt", recievedInt);

        canCounter = false;

        movement.canMove = false;
        yield return new WaitForSeconds(0.35f);
        movement.canMove = true;

        animator.SetInteger("AttackInt", 0);
        canCounter = true;
    }

    IEnumerator Strike()
    {
        canAttack = false;
        attackInt = Random.Range(1, 2 + 1);

        float time = 0;

        switch (attackInt)
        {
            case 1:
                time = 0.8f;
                break;
            case 2:
                time = 0.8f;
                break;
        }

        EnemyAttack enemyAttack = target.GetComponent<EnemyAttack>();
        enemyAttack.canAttack = false;

        animator.SetInteger("AttackInt", attackInt);
        animator.SetTrigger("AttackTrigger");
        StartCoroutine(MoveToStrike());

        yield return new WaitForSeconds(time * 0.65f);
        StartCoroutine(enemyAttack.Hit(attackInt));

        yield return new WaitForSeconds(time * 0.35f);

        animator.SetInteger("AttackInt", 0);

        canAttack = true;

        yield return new WaitForSeconds(2.5f);
        enemyAttack.canAttack = true;
    }

    IEnumerator Counter ()
    {
        EnemyAttack enemyAttack = aggressor.GetComponent<EnemyAttack>();
        canCounter = false;
        attackInt = enemyAttack.attackInt;

        float time = 0;

        switch (attackInt)
        {
            case 1:
                relPos = new Vector3 (0, 0, 1.25f);
                time = 0.74f;
                break;
            case 2:
                relPos = new Vector3(0, 0, 1f);
                time = 1.32f;
                break;
        }

        StartCoroutine(MoveToCounter());

        animator.SetInteger("AttackInt", attackInt);
        animator.SetTrigger("CounterTrigger");

        StartCoroutine (enemyAttack.Countered());

        movement.canMove = false;
        yield return new WaitForSeconds(time * 1.025f);
        movement.canMove = true;

        animator.SetInteger("AttackInt", 0);
        canCounter = true;
    }

    IEnumerator MoveToCounter ()
    {
        Vector3 lookVec = aggressor.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;

        yield return new WaitForEndOfFrame();

        Vector3 targPos = aggressor.TransformPoint(relPos);
        transform.position = targPos;

        rig.velocity = aggressor.GetComponent<Rigidbody>().velocity;
    }

    IEnumerator MoveToStrike ()
    {
        Vector3 lookVec = target.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;

        yield return new WaitForEndOfFrame();

        Vector3 toVec = transform.position - target.position;
        toVec = toVec.normalized;

        Vector3 targPos = target.position + (toVec * 1.25f);
        transform.position = targPos;

        rig.velocity = target.GetComponent<Rigidbody>().velocity;
    }
}
