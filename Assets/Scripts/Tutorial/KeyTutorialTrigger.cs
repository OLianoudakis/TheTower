using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class KeyTutorialTrigger : MonoBehaviour
    {
        [SerializeField]
        private TutorialManager m_tutorialManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_tutorialManager.NextStep("Find the key");
                gameObject.SetActive(false);
            }
        }
    }
}