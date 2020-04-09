using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomExitTrigger : MonoBehaviour
{
    [SerializeField]
    private TutorialManager m_tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.gameObject.tag.Equals("Enemy"))
        {
            m_tutorialManager.StepCompleted();
        }
    }
}
