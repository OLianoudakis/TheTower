using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

public class RoomExitTrigger : MonoBehaviour
{
    [SerializeField]
    private TutorialManager m_tutorialManager;
    private SceneController m_sceneController;

    private void Awake()
    {
        m_sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            m_sceneController.LoadNextScene();
        }

        if (other.gameObject.tag.Equals("Enemy"))
        {
            m_tutorialManager.StepCompleted();
        }
    }
}