using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBotAnimation : MonoBehaviour
{
    private Animator anim;
    private EnemyAI ai;
    bool idleBool, walkBool, attackBool;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
    }

    // Update is called once per frame

    private void Update()
    {
        idleBool = !ai.isWalking && !ai.isAttacking;
        walkBool = ai.isWalking && !ai.isAttacking;
        attackBool = !ai.isWalking && ai.isAttacking;

        //Idle
        if (idleBool)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
        }

        //Walking
        if (walkBool)
        {
            anim.SetBool("Walking", true);
            anim.SetBool("Attacking", false);
        }

        //Attacking
        if (attackBool)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", true);
        }
    }
}
