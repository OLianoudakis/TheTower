using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality
{
    public enum PersonalityTraits
    {
        Neuroticism = 0,
        Extraversion = 1,
        Conscientiousness = 2,
        Agreeableness = 3,
        Openness = 4
    }

    public enum MotivationDesires
    {
        Acceptance = 0,
        Curiosity = 1,
        Eating = 2,
        Family = 3,
        Honor = 4,
        Idealism = 5,
        Independence = 6,
        Order = 7,
        PhysicalActivity = 8,
        Power = 9,
        Romance = 10,
        Saving = 11,
        SocialContact = 12,
        SocialStatus = 13,
        Tranquility = 14,
        Vengeance = 15
    }

    public enum EmotionType
    {
        Joy = 0,
        Distress = 1,
        Resentment = 2,
        Pity = 3,
        Hope = 4,
        Fear = 5,
        Satisfaction = 6,
        Relief = 7,
        Disappointment = 8,
        Pride = 9,
        Admiration = 10,
        Shame = 11,
        Reproach = 12,
        Liking = 13,
        Disliking = 14,
        Gratitude = 15,
        Anger = 16,
        Gratification = 17,
        Remorse = 18,
        Love = 19,
        Hate = 20
    }

    // Mood as a combination of Pleasure, Arousal and Dominance.
    // The list contains the possible combinations of those properties and respective mood type.
    public enum MoodType
    {
        Pleasure = 0, // P
        Arousal = 1, // A
        Dominance = 2 // D
    }
}
