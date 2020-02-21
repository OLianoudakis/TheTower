using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator; 
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }

        if (agent.velocity.magnitude <= 0.0f )
        {
            animator.SetInteger("AnimState", 0);
        }
        else
        {
            animator.SetInteger("AnimState", 1);
        }
    }
}
