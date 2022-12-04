using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelBehaviour : MonoBehaviour
{
    Animator animator;

    int attackAnimID;
    bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {

        // Find Animator
        animator = GetComponent<Animator>();

        // Set animator Hash
        attackAnimID = Animator.StringToHash("Attack");
    }

    void OnTriggerEnter(Collider other)
    {
        // Attack on Area enter
        if (other.gameObject.tag == "Player")
        {
        if (!isAttacking)
        {
            animator.SetTrigger(attackAnimID);
            isAttacking = true;
        }
        }
    }

    void OnAttackEnd()
    {
        // Reset Attack after end

        isAttacking = false;
    }
}
