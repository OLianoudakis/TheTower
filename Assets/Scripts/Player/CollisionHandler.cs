using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AI.EmptyClass;
using Player.Inventory;
using GameUI;

namespace Player
{
    public class CollisionHandler : MonoBehaviour
    {
        private GameOverController m_gameOverController;
        private PlayerInventoryController m_inventoryController;
        private GrabbedObjectController m_grabbedObjectController;

        private void Awake()
        {
            m_gameOverController = FindObjectOfType(typeof(GameOverController)) as GameOverController;
            m_inventoryController = GetComponent(typeof(PlayerInventoryController)) as PlayerInventoryController;
            m_grabbedObjectController = GetComponentInChildren(typeof(GrabbedObjectController)) as GrabbedObjectController;
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                m_inventoryController.EmptyInventory();
                gameObject.SetActive(false);
                m_gameOverController.ShowGameOver();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EnemyTagScript enemyTag = collision.gameObject.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                m_inventoryController.EmptyInventory();
                m_grabbedObjectController.ResetGrabbedObjectToOrigin();
                gameObject.SetActive(false);
                m_gameOverController.ShowGameOver();
            }
        }
    }
}
