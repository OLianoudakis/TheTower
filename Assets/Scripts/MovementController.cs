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

    private POIBehavior poiBehavior;
    private bool goesToPOI;

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
                if (!hit.collider.gameObject.tag.Equals("POI"))
                    SetDestination(hit.point, false, null);
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

            if (goesToPOI)
            {
                goesToPOI = false;
                poiBehavior.ActivatePOIBehavior();
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

    public void SetDestination(Vector3 destination, bool isPOI, POIBehavior poi)
    {
        agent.destination = destination;
        if (poiBehavior!=null && poi == null)
        {
            poiBehavior.DeactivatePOIBehavior();
            goesToPOI = false;
            poiBehavior = null;
        }
        else if (poi != null)
        {
            poiBehavior = poi;
            goesToPOI = true;
        }
        if (currentSpawnMarker != null)
        {
            Destroy(currentSpawnMarker);
            currentSpawnMarker = null;
        }

        if (!isPOI)
            currentSpawnMarker = Instantiate(spawnMarker, destination + new Vector3(0.0f, 0.01f, 0.0f), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        else
            currentSpawnMarker = Instantiate(spawnMarker, poi.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
    }
}
