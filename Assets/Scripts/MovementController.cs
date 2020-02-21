using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject spawnMarker;

    private GameObject currentSpawnMarker;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
                if (currentSpawnMarker != null)
                {
                    Destroy(currentSpawnMarker);
                    currentSpawnMarker = null;
                }
                currentSpawnMarker = Instantiate(spawnMarker, hit.point + new Vector3(0.0f,0.01f,0.0f), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
            }
        }

        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            if (currentSpawnMarker != null)
            {
                Destroy(currentSpawnMarker);
                currentSpawnMarker = null;
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
