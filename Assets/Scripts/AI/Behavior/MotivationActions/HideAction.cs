using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;
using Environment.Hiding;

namespace AI.Actions
{
    public class HideAction : MonoBehaviour
    {
        [SerializeField]
        private float m_maximumHidingSpotDistance = 20.0f;

        private HidespotBehavior[] m_allHidingSpots;
        private List<GameObject> m_nearbyHidingSpots = new List<GameObject>();
        private List<GameObject> m_validHidingSpots = new List<GameObject>();

        private Transform m_playerTransform;
        private Vector3 m_chosenHidingPosition;

        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;

        private void Awake()
        {
            m_allHidingSpots = FindObjectsOfType(typeof(HidespotBehavior)) as HidespotBehavior[];
            m_playerTransform = FindObjectOfType<PlayerTagScript>().transform;
            m_navMeshAgent = transform.parent.transform.parent.GetComponent<NavMeshAgent>();
            m_animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (m_navMeshAgent.velocity.magnitude <= 0)
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerCrouch);
            }
            else
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            }
        }

        private void OnEnable()
        {
            FindNearbyHidingSpots();
        }

        private void FindNearbyHidingSpots()
        {
            Vector3 currentPosition = transform.parent.transform.parent.position;
            m_nearbyHidingSpots.Clear();

            foreach (HidespotBehavior potentialHidingSpot in m_allHidingSpots)
            {
                if (Vector3.Distance(currentPosition, potentialHidingSpot.gameObject.transform.position) <= m_maximumHidingSpotDistance)
                {
                    m_nearbyHidingSpots.Add(potentialHidingSpot.gameObject);
                }
            }
            FindValidHidingSpots();
        }

        private void FindValidHidingSpots()
        {
            if (m_nearbyHidingSpots.Count <= 0)
            {
                return;
            }
            m_validHidingSpots.Clear();

            foreach (GameObject nearbyHidingSpot in m_nearbyHidingSpots)
            {
                RaycastHit hidingSpotRayHit;
                if (Physics.Raycast(nearbyHidingSpot.transform.position, m_playerTransform.position, out hidingSpotRayHit, Mathf.Infinity))
                {
                    if (!hidingSpotRayHit.collider.gameObject.tag.Equals("Player"))
                    {
                        m_validHidingSpots.Add(hidingSpotRayHit.collider.gameObject);
                    }
                }
            }
            PickRandomValidSpot();
        }

        private void PickRandomValidSpot()
        {
            if (m_validHidingSpots.Count <= 0)
            {
                return;
            }

            int randomValidPositionIndex = Random.Range(0, m_validHidingSpots.Count - 1);
            m_chosenHidingPosition = m_validHidingSpots[randomValidPositionIndex].transform.position;

            m_navMeshAgent.SetDestination(m_chosenHidingPosition);
        }
    }
}
