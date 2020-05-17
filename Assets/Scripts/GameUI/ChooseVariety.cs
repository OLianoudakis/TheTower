using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSounds;

namespace GameUI
{
    public class ChooseVariety : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_trackToLevelStart;
        [SerializeField]
        private Text m_explanationText;
        [SerializeField]
        private Image m_selectionUnderline;

        private string m_levelSelection = "FourthScene";
        private SceneController m_sceneController;
        private MusicTagScript m_musicObject;

        private Vector3 m_defautButtonLocation = new Vector3(50.0f, 385.0f, 0.0f);
        private Vector3 m_pedanticButtonLocation = new Vector3(540.0f, 345.0f, 0.0f);
        private Vector3 m_lazyButtonLocation = new Vector3(304.0f, 125.0f, 0.0f);
        private Vector3 m_honorableButtonLocation = new Vector3(-440.0f, 345.0f, 0.0f);
        private Vector3 m_fearfulButtonLocation = new Vector3(-195.0f, 125.0f, 0.0f);

        public void UpdateSelection(string selection)
        {
            m_levelSelection = selection;
            switch (selection)
            {
                case "FourthScene":
                    m_selectionUnderline.rectTransform.localPosition = m_defautButtonLocation;
                    m_explanationText.text = "A healthy variation of all available guard personalities.";
                    break;
                case "FourthSceneFearfulVariation":
                    m_selectionUnderline.rectTransform.localPosition = m_fearfulButtonLocation;
                    m_explanationText.text = "All guards are afraid of you and will run away and alert others when they see you.";
                    break;
                case "FourthSceneHonorableVariation":
                    m_selectionUnderline.rectTransform.localPosition = m_honorableButtonLocation;
                    m_explanationText.text = "All guards live by the code of honor and will chase you no matter what.";
                    break;
                case "FourthScenePedanticVariation":
                    m_selectionUnderline.rectTransform.localPosition = m_pedanticButtonLocation;
                    m_explanationText.text = "All guards are obsessed with items being in order and will prioritize keeping up appeances of the tower rather that chasing you";
                    break;
                case "FourthSceneLazyVariation":
                    m_selectionUnderline.rectTransform.localPosition = m_lazyButtonLocation;
                    m_explanationText.text = "All guards are lazy sloths and would rather sit on a stool than chasing you. When spotted they will most likely inform others to your location and sit on their stool";
                    break;
            }
        }

        public void StartSelectedVariation()
        {
            (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).Stop();
            (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).clip = m_trackToLevelStart;
            (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).Play();
            m_sceneController.LoadSpecificScene(m_levelSelection);
        }

        private void Awake()
        {
            m_sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
            m_musicObject = FindObjectOfType(typeof(MusicTagScript)) as MusicTagScript;
        }
    }
}