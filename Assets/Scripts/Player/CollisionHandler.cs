using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AI.EmptyClass;

namespace Player
{
    public class CollisionHandler : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EnemyTagScript enemyTag = collision.collider.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }
}
