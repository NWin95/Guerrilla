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
        countered = true;
        animator.SetBool("Countered", countered);

        yield return new WaitForSeconds(1.5f);

        countered = false;
        animator.SetBool("Countered", countered);

        animator.SetInteger("AttackInt", 0);
        animator.SetBool("Attacking", false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        attacking = false;
    }

    IEnumerator AttackSel()
    {
        attacking = true;
        gameObject.layer = LayerMask.NameToLayer("EnemyAttacking");
        animator.SetInteger("AttackInt", 1);
        //animator.SetBool("Countered", true);
        float time = 1;

        yield return new WaitForEndOfFrame();
        animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(time + 0.25f);

        if (!countered)
        {
            animator.SetInteger("AttackInt", 0);
            animator.SetBool("Attacking", false);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            attacking = false;
        }
    }
}
