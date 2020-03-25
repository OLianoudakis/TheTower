using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.EmptyClass;

namespace AI.KnowledgeBase
{
    public class ShareKnowledge : MonoBehaviour
    {
        [SerializeField]
        private float m_shareKnowledgeCooldown = 1.0f;

        private KnowledgeBase m_knowledgeBase;
        private BoxCollider m_collider;
        private int m_layerMask;
        private Dictionary<int, Collider> m_agentsInVicinity = new Dictionary<int, Collider>();
        private Dictionary<int, float> m_shareKnowledgeCooldowns = new Dictionary<int, float>();

        private void Start()
        {
            m_knowledgeBase = GetComponent(typeof(KnowledgeBase)) as KnowledgeBase;
            m_collider = GetComponent(typeof(BoxCollider)) as BoxCollider;

            // set filter to ignore enemy perception and knowledgebase layer for raycast
            int perceptionLayerIndex = LayerMask.NameToLayer("Perception");
            int kbsLayerIndex = LayerMask.NameToLayer("KnowledgeBaseSharing");
            m_layerMask = ~((1 << perceptionLayerIndex) | (1 << kbsLayerIndex));
        }

        private void CheckCollision(Collider other)
        {
            // check if no wall between them
            Vector3 direction = new Vector3(other.transform.position.x, m_collider.center.y, other.transform.position.z) - m_collider.center;
            RaycastHit hit;
            if (Physics.Raycast(m_collider.center, direction, out hit, Mathf.Infinity, m_layerMask)
                && hit.collider.Equals(other))
            {
                KnowledgeBase kbOther = other.GetComponentInChildren(typeof(KnowledgeBase)) as KnowledgeBase;
                if (m_knowledgeBase.playerTransform && !kbOther.playerTransform)
                {
                    kbOther.PlayerSpotted(m_knowledgeBase.playerTransform);
                }
                else if (m_knowledgeBase.playerHiding && !kbOther.playerTransform && !kbOther.playerHiding)
                {
                    kbOther.SetLastKnownPlayerPosition(m_knowledgeBase.GetLastKnownPlayerPosition());
                }
                if (m_knowledgeBase.playerSuspicion && !kbOther.playerSuspicion)
                {
                    kbOther.PlayerSuspicion(m_knowledgeBase.GetLastKnownPlayerPosition());
                }
                if (m_knowledgeBase.noiseHeard && !kbOther.noiseHeard)
                {
                    kbOther.SetNoisePosition(m_knowledgeBase.GetNoisePosition());
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                int instanceId = other.GetInstanceID();
                m_agentsInVicinity.Add(instanceId, other);
                m_shareKnowledgeCooldowns.Add(instanceId, 0.0f);
                CheckCollision(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            int instanceId = other.GetInstanceID();
            if (m_agentsInVicinity.ContainsKey(instanceId))
            {
                if (m_shareKnowledgeCooldowns[instanceId] >= m_shareKnowledgeCooldown)
                {
                    m_shareKnowledgeCooldowns[instanceId] = 0.0f;
                    CheckCollision(other);
                }
                else
                {
                    m_shareKnowledgeCooldowns[instanceId] += Time.deltaTime;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int instanceId = other.GetInstanceID();
            if (m_agentsInVicinity.ContainsKey(instanceId))
            {
                m_shareKnowledgeCooldowns.Remove(instanceId);
                m_shareKnowledgeCooldowns.Remove(instanceId);
            }
        }
    }
}
