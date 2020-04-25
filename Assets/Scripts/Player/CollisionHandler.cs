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
        private EnemiesGroupTag m_enemiesParentObject;
        private List<EnemyTagScript> m_enemies = new List<EnemyTagScript>();

        private void Awake()
        {
            m_gameOverController = FindObjectOfType(typeof(GameOverController)) as GameOverController;
            m_inventoryController = GetComponent(typeof(PlayerInventoryController)) as PlayerInventoryController;
            m_grabbedObjectController = GetComponentInChildren(typeof(GrabbedObjectController)) as GrabbedObjectController;

            m_enemiesParentObject = FindObjectOfType(typeof(EnemiesGroupTag)) as EnemiesGroupTag;
           
            if (m_enemiesParentObject)
            {
                m_enemies.Clear();
                int childCount = m_enemiesParentObject.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    m_enemies.Add(m_enemiesParentObject.transform.GetChild(i).GetComponentInChildren(typeof(EnemyTagScript)) as EnemyTagScript);
                }

                bool thereAreNulls = true;
                while (thereAreNulls)
                {
                    bool oneNullFound = false;
                    for (int i = 0; i < m_enemies.Count; i++)
                    {
                        if (!m_enemies[i])
                        {
                            m_enemies.RemoveAt(i);
                            oneNullFound = true;
                            break;
                        }
                    }
                    if (!oneNullFound)
                    {
                        thereAreNulls = false;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                foreach(EnemyTagScript enemy in m_enemies)
                {
                    enemy.Deactivate();
                }
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
                foreach (EnemyTagScript enemy in m_enemies)
                {
                    enemy.Deactivate();
                }
                m_inventoryController.EmptyInventory();
                m_grabbedObjectController.ResetGrabbedObjectToOrigin();
                gameObject.SetActive(false);
                m_gameOverController.ShowGameOver();
            }
        }
    }
}
