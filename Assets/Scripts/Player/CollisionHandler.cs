using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AI.EmptyClass;

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
                m_gameOverController.ShowGameOver();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EnemyTagScript enemyTag = collision.collider.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag && enemyTag.gameOverAfterPlayerTouch)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }
}
