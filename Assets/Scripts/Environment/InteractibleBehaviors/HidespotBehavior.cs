using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Inventory;
using Player;
using Player.EmptyClass;

namespace Environment.Hiding
{
    public class HidespotBehavior : MonoBehaviour
    {
        private Animator m_playerAnimator;
        private CapsuleCollider m_playerCollider;
        private GameObject m_invisibleWalls;
        private Interactible m_interactible;
        private InputController m_inputController;
        private int m_playerLayer;

        private void SetPlayerIntoHiding()
        {
            m_playerAnimator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimCrouch);
            m_playerCollider.center = new Vector3 (0.0f, 0.38f, 0.0f);
            m_playerCollider.height = 1.76f;
            m_invisibleWalls.SetActive(true);
            m_playerCollider.gameObject.layer = LayerMask.NameToLayer("Hiding");
        }

        private void ExitHiding()
        {
            m_playerAnimator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
            m_playerCollider.center = new Vector3(0.0f, 1.0f, 0.0f);
            m_playerCollider.height = 3.0f;
            m_invisibleWalls.SetActive(false);
            m_interactible.DeactivateBehavior(false);
            m_playerCollider.gameObject.layer = m_playerLayer;
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            PlayerMeshTagScript playerMeshTag = FindObjectOfType(typeof(PlayerMeshTagScript)) as PlayerMeshTagScript;
            if (playerMeshTag)
            {
                m_playerAnimator = playerMeshTag.GetComponent(typeof(Animator)) as Animator;
            }
            PlayerTagScript playerTag = FindObjectOfType(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                m_playerCollider = playerTag.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
                m_playerLayer = m_playerCollider.gameObject.layer;
            }

            foreach (Transform child in transform)
            {
                if (child.name == "InvisibleWalls")
                {
                    m_invisibleWalls = child.gameObject;
                    break;
                }
            }
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                SetPlayerIntoHiding();

                if (m_inputController.isLeftMouseClick)
                {
                    ExitHiding();
                }
            }
        }
    }
}