using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;
using Player.EmptyClass;
using Player.StateHandling;
using Player.StateHandling.Moving;
using AI.KnowledgeBase;
using AI.Perception;
using AI.EmptyClass;
using GameUI;

namespace Environment.Checkpoints
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_idleState;
        [SerializeField]
        private Moving m_moving;

        private GameObject m_player;
        private Animator m_playerAnimator;
        private InputController m_inputController;

        private GameOverController m_gameOverGroup;
        private SpawnPointTagScript spawnPointsTagScripts;
        private List<Transform> spawnPointsTransform = new List<Transform>();
        private List<Checkpoint> m_checkpoints = new List<Checkpoint>();
        private SceneController m_sceneController;
        private GetActiveState m_playerActiveState;
        private NavMeshAgent m_playerAgent;
        private int m_playerStatePriority = 0;

        private EnemiesGroupTag m_enemiesParentObject;
        private List<KnowledgeBase> m_enemyKnowledgeBase = new List<KnowledgeBase>();
        private List<MainSight> m_enemyMainSight = new List<MainSight>();
        private List<SideSight> m_enemySideSight = new List<SideSight>();

        private bool m_startingFromCheckpoint = false;

        public int CurrentCheckpoint
        {
            get { return m_currentCheckpoint; }
            set { m_currentCheckpoint = value; }
        }
        [SerializeField]
        private int m_currentCheckpoint = -1;

        public void UpdateCurrentCheckpoint(int checkpoint)
        {
            m_currentCheckpoint = checkpoint;
        }

        public void StartFromCheckpoint()
        {
            if (m_currentCheckpoint >= 0)
            {
                m_startingFromCheckpoint = true;
            }
            else
            {
                m_sceneController.RestartLevel();
            }
        }

        private void Awake()
        {
            m_player = (FindObjectOfType(typeof(PlayerTagScript)) as PlayerTagScript).gameObject;
            m_playerAnimator = (FindObjectOfType(typeof(PlayerMeshTagScript)) as PlayerMeshTagScript).GetComponent(typeof(Animator)) as Animator;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
            m_gameOverGroup = FindObjectOfType(typeof(GameOverController)) as GameOverController;
            m_playerActiveState = FindObjectOfType(typeof(GetActiveState)) as GetActiveState;
            m_playerAgent = m_player.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            //m_moving = m_player.GetComponentInChildren(typeof(Moving)) as Moving;

            m_enemyKnowledgeBase.Clear();
            m_enemiesParentObject = FindObjectOfType(typeof(EnemiesGroupTag)) as EnemiesGroupTag;
            int childCount = m_enemiesParentObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_enemyKnowledgeBase.Add(m_enemiesParentObject.transform.GetChild(i).GetComponentInChildren(typeof(KnowledgeBase)) as KnowledgeBase);
                m_enemyMainSight.Add(m_enemiesParentObject.transform.GetChild(i).GetComponentInChildren(typeof(MainSight)) as MainSight);
                m_enemySideSight.Add(m_enemiesParentObject.transform.GetChild(i).GetComponentInChildren(typeof(SideSight)) as SideSight);
            }

            bool thereAreNulls = true;
            while (thereAreNulls)
            {
                bool oneNullFound = false;
                for (int i = 0; i < m_enemyKnowledgeBase.Count; i++)
                {
                    if (!m_enemyKnowledgeBase[i])
                    {
                        m_enemyKnowledgeBase.RemoveAt(i);
                        oneNullFound = true;
                        break;
                    }
                    if (!m_enemyMainSight[i])
                    {
                        m_enemyMainSight.RemoveAt(i);
                        oneNullFound = true;
                        break;
                    }
                    if (!m_enemySideSight[i])
                    {
                        m_enemySideSight.RemoveAt(i);
                        oneNullFound = true;
                        break;
                    }
                }
                if (!oneNullFound)
                {
                    thereAreNulls = false;
                }
            }


            spawnPointsTransform.Clear();
            spawnPointsTagScripts = FindObjectOfType(typeof(SpawnPointTagScript)) as SpawnPointTagScript;
            childCount = spawnPointsTagScripts.gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                spawnPointsTransform.Add(spawnPointsTagScripts.gameObject.transform.GetChild(i));
            }

            childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_checkpoints.Add(transform.GetChild(i).GetComponent(typeof(Checkpoint)) as Checkpoint);
            }
        }

        private void LateUpdate()
        {
            if (m_startingFromCheckpoint)
            {
                m_player.transform.position = spawnPointsTransform[m_currentCheckpoint].position;
                m_player.SetActive(true);
                if (m_moving.clickedOnEnable)
                {
                    m_moving.clickedOnEnable = false;
                }
                m_inputController.isLeftMouseClick = false;
                m_playerAgent.isStopped = true;
                m_playerAgent.ResetPath();
                m_playerActiveState.GetActivePlayerState().GetComponent<TransitionHandler>().AddActiveTransition(m_playerStatePriority, m_idleState);
                m_moving.StopMoving();
                //m_moving.enabled = false;
                m_gameOverGroup.HideGameOver();
                m_playerAgent.SetDestination(spawnPointsTransform[m_currentCheckpoint].position);
                m_playerAnimator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
                m_checkpoints[m_currentCheckpoint].ActivateCheckpoint();
                foreach (KnowledgeBase enemyKnowledge in m_enemyKnowledgeBase)
                {
                    enemyKnowledge.ResetKnowledge();
                }
                foreach (MainSight enemyMainSight in m_enemyMainSight)
                {
                    enemyMainSight.ResetSight();
                }
                foreach (SideSight enemySideSight in m_enemySideSight)
                {
                    enemySideSight.ResetSight();
                }
                m_startingFromCheckpoint = false;
            }
        }

    }
}