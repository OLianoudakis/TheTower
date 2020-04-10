using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace GameCamera
{
    public class FinalShotController : MonoBehaviour
    {
        [SerializeField]
        private Transform m_CCTVPosition;

        [SerializeField]
        private Transform m_lookAt;

        [SerializeField]
        private bool m_followPlayer = false;

        [SerializeField]
        private Constraints m_constraints = null;

        [SerializeField]
        private GameObject m_enemies;

        [SerializeField]
        private AudioClip m_firstAudioCue;
        [SerializeField]
        private AudioClip m_secondAudioCue;

        [SerializeField]
        private GameEndGroup m_gameCredits;

        private CameraPositionController m_mainCamera;
        private uint m_stateId;
        private static uint stateId = 0;

        private DialogueGroupController m_dialogueGroup;
        private Text m_dialogueText;
        private InputController m_playerInput;
        private AudioSource m_audioSource;

        public void FirstMessage()
        {
            //(GetComponent(typeof(BoxCollider)) as BoxCollider).enabled = false;
            m_dialogueText.text = "...";
            m_dialogueGroup.ShowDialogueWindow();
        }

        public void SecondMessage()
        {
            m_dialogueText.text = "Hi...";
            m_dialogueGroup.ShowDialogueWindow();
        }

        public void Credits()
        {
            m_audioSource.PlayOneShot(m_secondAudioCue);
            m_gameCredits.BeginCredits();
        }

        private void Awake()
        {
            m_dialogueGroup = FindObjectOfType(typeof(DialogueGroupController)) as DialogueGroupController;
            m_dialogueText = m_dialogueGroup.GetComponentInChildren(typeof(Text)) as Text;
            m_playerInput = FindObjectOfType(typeof(InputController)) as InputController;
            m_audioSource = GetComponent(typeof(AudioSource)) as AudioSource;
        }

        private void Start()
        {
            m_mainCamera = FindObjectOfType<CameraPositionController>();
            if (m_constraints == null)
            {
                m_constraints = new Constraints();
            }
            m_constraints.m_positionBoundaries = new PositionBoundaries();
            BoxCollider boxCollider = GetComponent(typeof(BoxCollider)) as BoxCollider;
            m_constraints.m_positionBoundaries.bounds = new Vector3(
                Mathf.Abs(transform.position.x + boxCollider.center.x - m_CCTVPosition.position.x),
                Mathf.Abs(transform.position.y + boxCollider.center.y - m_CCTVPosition.position.y),
                Mathf.Abs(transform.position.z + boxCollider.center.z - m_CCTVPosition.position.z)
                );
            m_constraints.m_positionBoundaries.triggerCenter = transform.position + boxCollider.center;
            m_stateId = GenerateStateId();
        }

        private static uint GenerateStateId()
        {
            return ++stateId;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_constraints.m_positionBoundaries.canMove = false;
                m_mainCamera.SetPosition(m_CCTVPosition.position, m_stateId, followPlayer: m_followPlayer, constraints: m_constraints, lookAt: m_lookAt);
                m_audioSource.PlayOneShot(m_firstAudioCue);
                m_playerInput.enabled = false;
                m_enemies.SetActive(false);
                (GetComponent(typeof(Animator)) as Animator).SetInteger("AnimState", 1);
            }
        }
    }

}
