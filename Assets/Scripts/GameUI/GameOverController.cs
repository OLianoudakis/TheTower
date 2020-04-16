using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace GameUI
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField]
        private float m_fadeLerpSpeed = 0.5f;

        private CanvasGroup m_gameOverGroup;
        private InputController m_playerInput;
        private SceneController m_sceneController;

        private float m_fadeLerpInterval = 0.0f;

        private void Awake()
        {
            m_gameOverGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
            m_playerInput = FindObjectOfType(typeof(InputController)) as InputController;
            m_sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
        }

        public void ShowGameOver()
        {
            m_playerInput.enabled = false;
            m_gameOverGroup.interactable = true;
            m_gameOverGroup.blocksRaycasts = true;
            StartCoroutine(GameOverUIFade());
            Time.timeScale = 0.0f;
        }

        public void RestartLevel()
        {
            Time.timeScale = 1.0f;
            m_playerInput.enabled = true;
            m_gameOverGroup.interactable = false;
            m_gameOverGroup.blocksRaycasts = false;
            //m_gameOverGroup.alpha = 0.0f;
            m_sceneController.RestartLevel();
        }

        public void ExitGame()
        {
            m_sceneController.ExitGame();
        }

        private IEnumerator GameOverUIFade()
        {
            bool currentlyFading = true;
            while (currentlyFading)
            {
                m_gameOverGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_gameOverGroup.alpha = 1.0f;
                    m_fadeLerpInterval = 0.0f;
                    currentlyFading = false;
                    yield return null;
                }
            }
        }
    }

}
