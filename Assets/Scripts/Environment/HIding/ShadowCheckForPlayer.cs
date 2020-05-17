using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Environment.Hiding
{
    public class ShadowCheckForPlayer : MonoBehaviour
    {
        MeshCollider m_meshCollider;

        private void Awake()
        {
            m_meshCollider = GetComponent(typeof(MeshCollider)) as MeshCollider;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            
            if (playerInvisibility)
            {
                float shadowCenterHeight = Mathf.Abs(m_meshCollider.bounds.max.y) + transform.position.y;
                float playerCenterHeight = ((CapsuleCollider)other).center.y + other.transform.position.y;
                if (shadowCenterHeight >= playerCenterHeight)
                {
                    playerInvisibility.SetInvisible();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerInvisibility)
            {
                //float shadowCenterHeight = m_meshCollider.bounds.center.y + transform.position.y;
                //float playerCenterHeight = ((CapsuleCollider)other).center.y + other.transform.position.y;
                //if (shadowCenterHeight >= playerCenterHeight)
                //{
                //    playerInvisibility.SetVisible();
                //}
                float shadowCenterHeight = Mathf.Abs(m_meshCollider.bounds.max.y) + transform.position.y;
                float playerCenterHeight = ((CapsuleCollider)other).center.y + other.transform.position.y;
                if (shadowCenterHeight >= playerCenterHeight)
                {
                    playerInvisibility.SetVisible();
                }
            }
        }
    }
}
