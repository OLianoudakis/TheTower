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

        private void SetPlayerIntoHiding()
        {
            m_playerAnimator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimCrouch);
            m_playerCollider.center = new Vector3 (0.0f, 0.38f, 0.0f);
            m_playerCollider.height = 1.76f;
            m_invisibleWalls.SetActive(true);
        }

        private void ExitHiding()
        {
            m_playerAnimator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
            m_playerCollider.center = new Vector3(0.0f, 1.0f, 0.0f);
            m_playerCollider.height = 3.0f;
            m_invisibleWalls.SetActive(false);
            m_interactible.DeactivateBehavior(false);
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_playerAnimator = FindObjectOfType<PlayerMeshTagScript>().GetComponent<Animator>();
            m_playerCollider = FindObjectOfType<PlayerTagScript>().GetComponent<CapsuleCollider>();

            foreach (Transform eachChild in transform)
            {
                if (eachChild.name == "InvisibleWalls")
                {
                    m_invisibleWalls = eachChild.gameObject;
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