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
        private Dictionary<EnemyTagScript, bool> m_agentsInVicinity = new Dictionary<EnemyTagScript, bool>();
        private Dictionary<EnemyTagScript, float> m_shareKnowledgeCooldowns = new Dictionary<EnemyTagScript, float>();

        public void Disable()
        {
            m_agentsInVicinity.Clear();
            m_shareKnowledgeCooldowns.Clear();
            m_collider.enabled = false;
        }

        public void Enable()
        {
            m_collider.enabled = true;
        }

        private void Awake()
        {
            m_knowledgeBase = GetComponent(typeof(KnowledgeBase)) as KnowledgeBase;
            m_collider = GetComponent(typeof(BoxCollider)) as BoxCollider;

            // set filter to ignore enemy perception and knowledgebase layer for raycast
            //int perceptionLayerIndex = LayerMask.NameToLayer("Perception");
            //int kbsLayerIndex = LayerMask.NameToLayer("KnowledgeBaseSharing");
            // set filter on enemy collider
            m_layerMask = LayerMask.GetMask("Enemy", "Walls", "Default", "Highlight");
        }

        private void Start()
        {
            Disable();
        }

        private void CheckCollision(Collider other)
        {
            // check if no wall between them
            Vector3 fromRay = new Vector3
                   (
                       m_collider.transform.position.x,
                       m_collider.transform.position.y + m_collider.center.y + 0.5f, // heuristic value, to not hit any furniture
                       m_collider.transform.position.z
                   ) + m_collider.transform.forward; // add forward to not hit self
            Vector3 direction =
                new Vector3(other.transform.position.x, other.transform.position.y + m_collider.center.y + 0.5f, other.transform.position.z)
                - fromRay;
            RaycastHit hit;
            if (Physics.Raycast(fromRay, direction, out hit, Mathf.Infinity, m_layerMask)
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
                m_agentsInVicinity.Add(enemyTag, true);
                m_shareKnowledgeCooldowns.Add(enemyTag, 0.0f);
                CheckCollision(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                if (m_agentsInVicinity.ContainsKey(enemyTag))
                {
                    m_shareKnowledgeCooldowns[enemyTag] += Time.deltaTime;
                    if (m_shareKnowledgeCooldowns[enemyTag] >= m_shareKnowledgeCooldown)
                    {
                        m_shareKnowledgeCooldowns[enemyTag] = 0.0f;
                        CheckCollision(other);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                if (m_agentsInVicinity.ContainsKey(enemyTag))
                {
                    m_agentsInVicinity.Remove(enemyTag);
                    m_shareKnowledgeCooldowns.Remove(enemyTag);
                }
            }
        }
    }
}
