using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using Player.EmptyClass;
using Environment.Hiding;

namespace AI.Behavior.Trees
{
    public class HidingSubtree : MonoBehaviour
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private HidespotBehavior[] m_allHidingSpots;
        private LayerMask m_acceptedLayer;
        private float m_maximumHidingSpotDistance;

        private List<GameObject> m_nearbyHidingSpots = new List<GameObject>();
        private List<GameObject> m_validHidingSpots = new List<GameObject>();
        private Transform m_playerTransform;
        private Vector3 m_chosenHidingPosition;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, HidespotBehavior[] allHidingSpots,
            LayerMask acceptedLayer, float maximumHidingSpotDistance)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_allHidingSpots = allHidingSpots;
            m_acceptedLayer = acceptedLayer;
            m_maximumHidingSpotDistance = maximumHidingSpotDistance;

            m_root =
                new Selector
                (
                    new Action(FindNearbyHidingSpots),
                    new BlackboardCondition("targetPosition", Operator.IS_SET, Stops.NONE,
                        new NavMoveTo(m_navMeshAgent, "targetPosition")
                    )
                );
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
                Ray ray = new Ray(nearbyHidingSpot.transform.position, m_playerTransform.position - nearbyHidingSpot.transform.position);
                RaycastHit hidingSpotRayHit;
                if (Physics.Raycast(ray, out hidingSpotRayHit, Mathf.Infinity, m_acceptedLayer))
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

            int randomValidPositionIndex = UnityEngine.Random.Range(0, m_validHidingSpots.Count - 1);
            m_chosenHidingPosition = m_validHidingSpots[randomValidPositionIndex].transform.position;
            Transform targetTransform = m_behaviorTreeRoot.Blackboard.Get("targetTransform") as Transform;
        }
    }
}
