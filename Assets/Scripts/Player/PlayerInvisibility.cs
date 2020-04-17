using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.EmptyClass;

namespace Player
{
    public class PlayerInvisibility : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_invisibilityCanvas;

        [SerializeField]
        private float m_fadeLerpSpeed = 1.0f;

        private List<GameObject> m_playerGameObjects;
        private float m_fadeLerpInterval = 0.0f;
        private int m_invisibleCount = 0;
        private int m_defaultLayer;
        private bool m_currentlyFading = false;
        private bool m_isInvisible;
        
        public bool isInvisible
        {
            get { return (m_invisibleCount > 0); }
        }

        public void SetInvisible()
        {
            if (++m_invisibleCount == 1)
            {
                m_isInvisible = true;
                m_currentlyFading = true;
                foreach (GameObject playerGameObject in m_playerGameObjects)
                {
                    playerGameObject.layer = LayerMask.NameToLayer("Shadows");
                }
            }
        }

        public void SetVisible()
        {
            if (--m_invisibleCount == 0)
            {
                m_isInvisible = false;
                m_currentlyFading = true;
                foreach (GameObject playerGameObject in m_playerGameObjects)
                {
                    playerGameObject.layer = m_defaultLayer;
                }
            }
        }

        private void Awake()
        {
            GameObject[] gobjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            m_playerGameObjects = new List<GameObject>();
            for (int i = 0; i < gobjects.Length; i++)
            {
                if (gobjects[i].layer == LayerMask.NameToLayer("Player"))
                {
                    m_playerGameObjects.Add(gobjects[i]);
                }
            }
        }

        private void Start()
        {
            m_isInvisible = false;
            m_defaultLayer = gameObject.layer;
        }

        private void Update()
        {
            if (m_currentlyFading)
            {
                if (m_isInvisible)
                {
                    m_invisibilityCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_invisibilityCanvas.alpha = 1.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
                else
                {
                    m_invisibilityCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_invisibilityCanvas.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
            }
        }
    }
}
