using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTriggerTrigger : MonoBehaviour
{
    [SerializeField]
    private SphereCollider m_exitCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            m_exitCollider.enabled = true;
        }
    }
}
