using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private int m_tutorialStep = 0;
        [SerializeField]
        private InputController m_playerInput;
        [SerializeField]
        private GameObject m_enemy;
        [SerializeField]
        private Animator m_door1;
        [SerializeField]
        private Animator m_door2;
        [SerializeField]
        private CanvasGroup m_fadeBlackGroup;
        [SerializeField]
        private CanvasGroup m_tutorialInfoGroup;
        [SerializeField]
        private Text m_tutorialInfoText;

        [SerializeField]
        private GameObject m_tutorialMoveToEffect;
        [SerializeField]
        private float m_fadeLerpSpeed = 0.1f;
        private float m_fadeLerpInterval = 0.0f;
        private bool m_currentlyFading = true;
        private bool m_firstCoroutineFinished = false;
        private bool m_sceneLoadSafeguard = true;
        private string m_message;
        private Vector3 m_tutorialMoveToPosition = new Vector3(-2.20f, 0.5f, 0.4f);

        public void NextStep(string message)
        {
            m_tutorialStep++;
            m_message = message;
            FadeInTutorialBox();
        }

        public void StepCompleted()
        {
            m_tutorialStep++;
            FadeInTutorialBox();
        }

        private void Start()
        {
            m_playerInput.enabled = false;
            StartCoroutine(FirstFadeIn());
        }

        private void FadeInTutorialBox()
        {
            StopAllCoroutines();
            switch (m_tutorialStep)
            {
                case 0:
                    ClickToMoveStep();
                    break;
                case 1:
                    ClickToInteractStep();
                    break;
                case 2:
                    HideBehindTheBarrelStep();
                    break;
                case 3:
                    WaitForEnemiesStep();
                    break;
                case 4:
                    m_enemy.SetActive(false);
                    FirstRoomDoneStep();
                    break;
                case 5:
                    ShadowsStep();
                    break;
                case 6:
                    KeyStep();
                    break;
            }
            StartCoroutine(TutorialGroupFade(true));
        }

        private void FadeOutTutorialBox()
        {
            StopAllCoroutines();
            StartCoroutine(TutorialGroupFade(false));
        }

        private void ClickToMoveStep()
        {
            m_playerInput.enabled = true;
            m_tutorialMoveToEffect.transform.localPosition = m_tutorialMoveToPosition;
            m_message = "Click on the ground to move";
            m_tutorialInfoText.text = m_message;
        }

        private void ClickToInteractStep()
        {
            m_message = "Click on the open book to interact with it";
            m_tutorialInfoText.text = m_message;
        }

        private void HideBehindTheBarrelStep()
        {
            m_message = "Click on the couch icon next to the barrel";
            m_tutorialInfoText.text = m_message;
        }

        private void WaitForEnemiesStep()
        {
            m_door1.SetInteger("AnimationState", AnimationConstants.AnimDoorOpen);
            m_door2.SetInteger("AnimationState", AnimationConstants.AnimDoorOpen);
            m_enemy.SetActive(true);
            m_message = "Wait for the guards to leave";
            m_tutorialInfoText.text = m_message;
        }

        private void FirstRoomDoneStep()
        {
            m_enemy.SetActive(false);
            m_message = " ";
            m_tutorialInfoText.text = m_message;
        }

        private void ShadowsStep()
        {
            m_playerInput.enabled = true;
            m_message = "Use the shadows";
            m_tutorialInfoText.text = m_message;
        }

        private void KeyStep()
        {
            m_tutorialInfoText.text = m_message;
        }

        private IEnumerator FirstFadeIn()
        {
            while (m_currentlyFading)
            {
                m_fadeBlackGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_fadeBlackGroup.alpha = 0.0f;
                    m_fadeLerpInterval = 0.0f;
                    m_fadeLerpSpeed = 0.5f;
                    m_currentlyFading = false;
                    m_firstCoroutineFinished = true;
                    m_fadeBlackGroup.gameObject.SetActive(false);
                    FadeInTutorialBox();
                    yield return null;
                }
            }
        }

        private IEnumerator TutorialGroupFade(bool fadeIn)
        {
            m_currentlyFading = true;
            if (fadeIn)
            {
                while (m_currentlyFading)
                {
                    m_tutorialInfoGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_tutorialInfoGroup.alpha = 1.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                        FadeOutTutorialBox();
                        yield return null;
                    }
                }
            }
            else
            {
                while (m_currentlyFading)
                {
                    m_tutorialInfoGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_tutorialInfoGroup.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                        yield return null;
                    }
                }
            }
        }
    }

}
