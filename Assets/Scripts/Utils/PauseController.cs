using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Player.EmptyClass;
using GameSounds;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_pauseMenu;
    [SerializeField]
    private Slider m_musicVolumeSlider;


    [SerializeField]
    private float m_fadeLerpSpeed = 2.0f;

    [SerializeField]
    private bool m_isPaused = false;

    private AudioSource m_musicObject;
    private InputController m_playerInput;
    private SceneController m_sceneController;
    private float m_fadeLerpInterval = 0.0f;

    private void Awake()
    {
        m_musicObject = (FindObjectOfType(typeof(MusicTagScript)) as MusicTagScript).GetComponent(typeof(AudioSource)) as AudioSource;
        m_sceneController = (FindObjectOfType(typeof(SceneController)) as SceneController);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void LoadNextScene()
    {
        m_sceneController.LoadNextScene();
    }

    public void ExitGame()
    {
        m_sceneController.ExitGame();
    }

    public void ChangeMusicVolume()
    {
        m_musicObject.volume = m_musicVolumeSlider.value;
    }

    public void ResumeGame()
    {
        StartCoroutine(PauseUIFade(false));
        m_playerInput.enabled = true;
        m_isPaused = false;
        Time.timeScale = 1.0f;
    }

    private void PauseGame()
    {
        if (!m_playerInput)
        {
            m_playerInput = (FindObjectOfType(typeof(PlayerTagScript)) as PlayerTagScript).GetComponent(typeof(InputController)) as InputController;
        }
        StartCoroutine(PauseUIFade(true));
        m_playerInput.enabled = false;
        m_isPaused = true;
        Time.timeScale = 0.0f;
    }

    private IEnumerator PauseUIFade(bool fadeIn)
    {
        bool currentlyFading = true;
        if (fadeIn)
        {
            m_pauseMenu.interactable = true;
            m_pauseMenu.blocksRaycasts = true;
            while (currentlyFading)
            {
                m_pauseMenu.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_pauseMenu.alpha = 1.0f;
                    m_fadeLerpInterval = 0.0f;
                    currentlyFading = false;
                    yield return null;
                }
            }
        }
        else
        {
            while (currentlyFading)
            {
                m_pauseMenu.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_pauseMenu.alpha = 0.0f;
                    m_fadeLerpInterval = 0.0f;
                    m_pauseMenu.interactable = false;
                    m_pauseMenu.blocksRaycasts = false;
                    currentlyFading = false;
                    yield return null;
                }
            }
        }
    }
}
