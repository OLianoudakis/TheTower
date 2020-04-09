using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.EditorProperties;

namespace AI.Personality
{
    [Serializable]
    public struct MotivationValueEntry
    {
        //[ReadOnly]
        [Range(0.0f, 1.0f)]
        public MotivationDesires m_motivationDesire;
        public float m_value;

        public MotivationValueEntry(MotivationDesires motivationDesire, float value)
        {
            m_motivationDesire = motivationDesire;
            m_value = value;
        }
    }

    [Serializable]
    public struct PersonalityTraitEntry
    {
        //[ReadOnly]
        [Range(-1.0f, 1.0f)]
        public PersonalityTraits m_personalityTraitType;
        public float m_value;

        public PersonalityTraitEntry(PersonalityTraits personalityTraitType, float value)
        {
            m_personalityTraitType = personalityTraitType;
            m_value = value;
        }
    }

    [CreateAssetMenu(fileName = "New Personality Model", menuName = "Personality Model")]
    public class PersonalityModel : ScriptableObject
    {
        [SerializeField]
        public PersonalityTraitEntry[] m_personalityTraitsValues 
            = new PersonalityTraitEntry[5]
            {
                new PersonalityTraitEntry(PersonalityTraits.Openness, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Conscientiousness, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Extraversion, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Agreeableness, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Neuroticism, 0.0f)
            };

        [SerializeField]
        public MotivationValueEntry[] m_targetMotivations
            = new MotivationValueEntry[16]
            {
                new MotivationValueEntry(MotivationDesires.Acceptance, 0.0f),
                new MotivationValueEntry(MotivationDesires.Curiosity, 0.0f),
                new MotivationValueEntry(MotivationDesires.Eating, 0.0f),
                new MotivationValueEntry(MotivationDesires.Family, 0.0f),
                new MotivationValueEntry(MotivationDesires.Honor, 0.0f),
                new MotivationValueEntry(MotivationDesires.Idealism, 0.0f),
                new MotivationValueEntry(MotivationDesires.Independence, 0.0f),
                new MotivationValueEntry(MotivationDesires.Order, 0.0f),
                new MotivationValueEntry(MotivationDesires.PhysicalActivity, 0.0f),
                new MotivationValueEntry(MotivationDesires.Power, 0.0f),
                new MotivationValueEntry(MotivationDesires.Romance, 0.0f),
                new MotivationValueEntry(MotivationDesires.Saving, 0.0f),
                new MotivationValueEntry(MotivationDesires.SocialContact, 0.0f),
                new MotivationValueEntry(MotivationDesires.SocialStatus, 0.0f),
                new MotivationValueEntry(MotivationDesires.Tranquility, 0.0f),
                new MotivationValueEntry(MotivationDesires.Vengeance, 0.0f)
            };
    }
}
