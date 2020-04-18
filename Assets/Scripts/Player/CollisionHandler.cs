using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AI.EmptyClass;
using GameUI;

namespace Player
{
    public class CollisionHandler : MonoBehaviour
    {
        private GameOverController m_gameOverController;

        private void Awake()
        {
            m_gameOverController = FindObjectOfType(typeof(GameOverController)) as GameOverController;
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                gameObject.SetActive(false);
                m_gameOverController.ShowGameOver();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EnemyTagScript enemyTag = collision.gameObject.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                gameObject.SetActive(false);
                m_gameOverController.ShowGameOver();
            }
        }
    }
}
