using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Inventory;
using Environment.InteractibleBehaviors;
using GameUI;

namespace Environment.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private int m_checkpoint;
        [SerializeField]
        private Item[] m_itemsRequired;
        [SerializeField]
        private GameObject[] m_closedDoors;
        [SerializeField]
        private GameObject[] m_visibleKeys;
        private CheckpointController m_checkpointController;
        private PlayerInventoryController m_playerInventoryController;
        private InfoGroupController m_infoGroup;

        public void ActivateCheckpoint()
        {
            if (m_itemsRequired.Length > 0)
            {
                foreach (Item itemRequired in m_itemsRequired)
                {
                    if (!m_playerInventoryController.SearchForItem(itemRequired.m_itemType))
                    {
                        m_playerInventoryController.AddItem(itemRequired, 1);
                    }
                }
            }

            if (m_closedDoors.Length > 0)
            {
                foreach (GameObject door in m_closedDoors)
                {
                    (door.GetComponent(typeof(Interactible)) as Interactible).RestartBehavior();
                    (door.GetComponent(typeof(POIBehavior)) as POIBehavior).enabled = true;
                    (door.GetComponent(typeof(POIBehavior)) as POIBehavior).ResetMessages();
                    (door.GetComponent(typeof(Animator)) as Animator).SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimDoorClose);
                }
            }

            if (m_visibleKeys.Length > 0)
            {
                foreach (GameObject key in m_visibleKeys)
                {
                    (key.GetComponent(typeof(Interactible)) as Interactible).RestartBehavior();
                    (key.GetComponent(typeof(POIBehavior)) as POIBehavior).enabled = true;
                    (key.GetComponent(typeof(POIBehavior)) as POIBehavior).ResetMessages();
                }
            }
        }

        private void CheckCheckpointCriteria()
        {
            if (m_itemsRequired.Length == 0)
            {
                CheckpointReached();
                return;
            }

            bool allItemsFound = true;
            for (int i = 0; i < m_itemsRequired.Length; i++)
            {
                allItemsFound = m_playerInventoryController.SearchForItem(m_itemsRequired[i].m_itemType);
            }

            if (allItemsFound)
            {
                CheckpointReached();
            }
        }

        private void CheckpointReached()
        {
            m_checkpointController.UpdateCurrentCheckpoint(m_checkpoint);
            m_infoGroup.SpawnInfoGroup("Checkpoint reached.");
            BoxCollider boxCollider = GetComponent(typeof(BoxCollider)) as BoxCollider;
            boxCollider.enabled = false;
        }

        private void Awake()
        {
            m_checkpointController = transform.parent.GetComponent(typeof(CheckpointController)) as CheckpointController;
            m_playerInventoryController = FindObjectOfType(typeof(PlayerInventoryController)) as PlayerInventoryController;
            m_infoGroup = FindObjectOfType(typeof(InfoGroupController)) as InfoGroupController;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                CheckCheckpointCriteria();
            }
        }
    }
}