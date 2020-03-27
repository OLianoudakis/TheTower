using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.EmptyClass;

namespace Environment
{
    public class Sittable : MonoBehaviour
    {
        [SerializeField]
        private Transform m_sittablePosition;

        private List<Transform> m_enemyTransforms = new List<Transform>();

        public bool CanSit(Transform enemyTransform)
        {
            return m_enemyTransforms.Find(x => x == enemyTransform);
        }

        public Transform sittablePosition
        {
            get { return m_sittablePosition; }
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                m_enemyTransforms.Add(enemyTag.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                m_enemyTransforms.Remove(enemyTag.transform);
            }
        }
    }
}
