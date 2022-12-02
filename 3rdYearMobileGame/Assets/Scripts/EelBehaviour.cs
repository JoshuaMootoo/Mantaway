using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelBehaviour : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    BoxCollider aggroArea;

    [SerializeField]
    BoxCollider damageArea;

    GameObject player;

    int attackAnimID;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;

        // Find Animator
        animator = GetComponent<Animator>();

        // Set animator Hash
        attackAnimID = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        animator.SetTrigger(attackAnimID);
    }
}
