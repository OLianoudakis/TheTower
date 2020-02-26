using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private GameObject m_positionMarker;

    private NavMeshAgent m_agent;
    private POIBehavior m_poiBehavior;
    private bool m_isGoingToPOI;
    
    private void Start()
    {
        m_agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        if (m_positionMarker)
        {
            m_positionMarker.SetActive(false);
        }
    }

    private void SetDestination(Vector3 destination, POIBehavior poiBehavior)
    {
        m_agent.destination = destination;
        if (m_poiBehavior && !poiBehavior)
        {
            m_poiBehavior.DeactivatePOIBehavior();
            m_isGoingToPOI = false;
            m_poiBehavior = null;
        }
        else if (poiBehavior)
        {
            m_poiBehavior = poiBehavior;
            m_isGoingToPOI = true;
        }
        if (m_positionMarker)
        {
            if (!m_positionMarker.activeInHierarchy)
            {
                m_positionMarker.SetActive(true);
            }
            m_positionMarker.transform.position = poiBehavior
            ? poiBehavior.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f)
            : destination + new Vector3(0.0f, 0.01f, 0.0f);
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if (hit.collider.gameObject.tag.Equals("POI"))
                {
                    POIBehavior poiBehavior = hit.collider.gameObject.GetComponent(typeof(POIBehavior)) as POIBehavior;
                    SetDestination(hit.point, poiBehavior);
                }
                else
                {
                    SetDestination(hit.point, null);
                }
                return; // return here to get navmesh chance to update its destination
            }
        }

        float distance = m_agent.remainingDistance;
        if ((distance != Mathf.Infinity) 
            && (m_agent.pathStatus == NavMeshPathStatus.PathComplete) 
            && (m_agent.remainingDistance == 0.0f))
        {
            if (m_positionMarker && m_positionMarker.activeInHierarchy)
            {
                m_positionMarker.SetActive(false);
            }

            if (m_isGoingToPOI)
            {
                m_isGoingToPOI = false;
                m_poiBehavior.ActivatePOIBehavior();
            }
        }

        if (m_agent.velocity.magnitude <= 0.0f)
        {
            m_animator.SetInteger("AnimState", 0);
        }
        else
        {
            m_animator.SetInteger("AnimState", 1);
        }
    }

    void OnDrawGizmos()
    {
        if (m_agent)
        {
            if (m_agent.destination == null)
            {
                return;
            }
            var path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, m_agent.destination, NavMesh.AllAreas, path);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.up);
            Gizmos.DrawRay(m_agent.destination, Vector3.up);
            Gizmos.color = Color.green;
            var offset = 0.2f * Vector3.up;
            for (int i = 1; i < path.corners.Length; ++i)
            {
                Gizmos.DrawLine(path.corners[i - 1] + offset, path.corners[i] + offset);
            }
        }
    }
}
