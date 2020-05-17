using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior.MotivationActions.Actions
{
    public class AlertOthersAction : MonoBehaviour
    {
        [SerializeField]
        private float m_voiceRadius = 10.0f;

        private SphereCollider m_voiceCollider;

        private void Awake()
        {
            m_voiceCollider = GetComponentInChildren<SphereCollider>();
            m_voiceCollider.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            StopCoroutine(AlertOthers());
            StartCoroutine(AlertOthers());
        }

        private IEnumerator AlertOthers()
        {
            //TODO add alert event to the child object
            m_voiceCollider.gameObject.SetActive(true);
            m_voiceCollider.radius = m_voiceRadius;
            yield return new WaitForSeconds(1.0f);
            m_voiceCollider.gameObject.SetActive(false);
        }
    }

}