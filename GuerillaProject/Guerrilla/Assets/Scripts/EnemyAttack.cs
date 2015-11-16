using UnityEngine;
using System.Collections;

//Need to change from Coroutines to something that can interrupt itself


public class EnemyAttack : MonoBehaviour {

    public Animator animator;
    Transform player;
    float dis;
    public float attackRange;
    //public bool canAttack;
    bool countered;
    public int attackInt;

    public bool attackBool;
    public bool hitBool;

	void Start () {
        player = GameObject.Find("Player").transform;

        animator.SetBool("Grounded", true);
	}
	
	void Update () {
        Dis();
        Range();
	}

    void Range ()
    {
        if (dis <= attackRange /* && canAttack */)
        {
            StartCoroutine(AttackSel());
        }
    }

    void Dis ()
    {
        dis = Vector3.Distance(transform.position, player.position);
    }

    public IEnumerator Countered ()
    {
        if (!countered)
        {
            countered = true;
            animator.SetTrigger("CounteredTrigger");

            yield return new WaitForSeconds(0.75f);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            yield return new WaitForSeconds(0.75f);

            countered = false;

            animator.SetInteger("AttackInt", 0);

            yield return new WaitForSeconds(1f);

            //canAttack = true;

            if (attackBool)
                attackBool = false;
        }
    }

    public IEnumerator Hit(int recievedInt)
    {
        if (!hitBool)
        {
            hitBool = true;
            attackBool = true;

            animator.SetTrigger("HitTrigger");
            animator.SetInteger("AttackInt", recievedInt);

            //canCounter = false;
            //movement.canMove = false;
            yield return new WaitForSeconds(0.35f);
            //movement.canMove = true;

            animator.SetInteger("AttackInt", 0);
            //canCounter = true;

            yield return new WaitForSeconds(0.75f);

            attackBool = false;
            hitBool = false;
        }
    }

    IEnumerator AttackSel()
    {
        if (player.GetComponent<Player_ModeController>().human)
        {
            if (!attackBool && !hitBool)
            {
                attackBool = true;

                attackInt = Random.Range(1, 2 + 1);

                //canAttack = false;
                animator.SetInteger("AttackInt", attackInt);
                float time = 1;

                switch (attackInt)
                {
                    case 1:
                        time = 0.8f;
                        break;
                    case 2:
                        time = 0.8f;
                        break;
                }
                //yield return new WaitForEndOfFrame();
                LookAt();
                animator.SetTrigger("AttackTrigger");

                yield return new WaitForSeconds(time * 0.1f);
                gameObject.layer = LayerMask.NameToLayer("EnemyAttacking");

                yield return new WaitForSeconds(time * 0.55f);

                if (!countered)
                {
                    Player_MeleeHuman meleeHuman = player.GetComponent<Player_MeleeHuman>();
                    StartCoroutine(meleeHuman.Hit(attackInt));

                    yield return new WaitForSeconds(time * 0.35f);

                    animator.SetInteger("AttackInt", 0);
                    gameObject.layer = LayerMask.NameToLayer("Enemy");

                    yield return new WaitForSeconds(0.25f);
                    //canAttack = true;
                    attackBool = false;
                }
            }
        }
    }

    void LookAt ()
    {
        Vector3 lookVec = player.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
    }
}
