using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public Animator animator;
    Transform player;
    float dis;
    public float attackRange;
    bool attacking;
    bool countered;

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
        if (dis <= attackRange && !attacking)
            StartCoroutine (AttackSel());
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

            yield return new WaitForSeconds(1.5f);

            countered = false;

            animator.SetInteger("AttackInt", 0);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            attacking = false;
        }
    }

    IEnumerator AttackSel()
    {
        attacking = true;
        gameObject.layer = LayerMask.NameToLayer("EnemyAttacking");
        animator.SetInteger("AttackInt", 1);
        float time = 1;

        //yield return new WaitForEndOfFrame();
        LookAt();
        animator.SetTrigger("AttackTrigger");

        yield return new WaitForSeconds(time + 0.25f);

        if (!countered)
        {
            animator.SetInteger("AttackInt", 0);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            attacking = false;
        }
    }

    void LookAt ()
    {
        Vector3 lookVec = player.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
    }
}
