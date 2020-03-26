using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality;

namespace AI.Behavior.EmotionalActions
{
    public class EmotionalActionsCatalogue : MonoBehaviour
    {
        private Catalogue[,] m_actionsCatalog = new Catalogue[,] { { Catalogue.PlayerSpottedJoy, Catalogue.PlayerDiscoveredJoy, Catalogue.PlayerSuspicionJoy, Catalogue.ObjectMovedJoy, Catalogue.NoiseHeardJoy, Catalogue.PlayerLostJoy, Catalogue.NoiseHeardBySomebodyElseJoy },
                                                               { Catalogue.PlayerSpottedDistress, Catalogue.PlayerDiscoveredDistress, Catalogue.PlayerSuspicionDistress, Catalogue.ObjectMovedDistress, Catalogue.NoiseHeardDistress, Catalogue.PlayerLostDistress, Catalogue.NoiseHeardBySomebodyElseDistress},
                                                               { Catalogue.PlayerSpottedResentment, Catalogue.PlayerDiscoveredResentment, Catalogue.PlayerSuspicionResentment, Catalogue.ObjectMovedResentment, Catalogue.NoiseHeardResentment, Catalogue.PlayerLostResentment, Catalogue.NoiseHeardBySomebodyElseResentment},
                                                               { Catalogue.PlayerSpottedPity, Catalogue.PlayerDiscoveredPity, Catalogue.PlayerSuspicionPity, Catalogue.ObjectMovedPity, Catalogue.NoiseHeardPity, Catalogue.PlayerLostPity, Catalogue.NoiseHeardBySomebodyElsePity},
                                                               { Catalogue.PlayerSpottedHope, Catalogue.PlayerDiscoveredHope, Catalogue.PlayerSuspicionHope, Catalogue.ObjectMovedHope, Catalogue.NoiseHeardHope, Catalogue.PlayerLostHope, Catalogue.NoiseHeardBySomebodyElseHope},
                                                               { Catalogue.PlayerSpottedFear, Catalogue.PlayerDiscoveredFear, Catalogue.PlayerSuspicionFear, Catalogue.ObjectMovedFear, Catalogue.NoiseHeardFear, Catalogue.PlayerLostFear, Catalogue.NoiseHeardBySomebodyElseFear},
                                                               { Catalogue.PlayerSpottedSatisfaction, Catalogue.PlayerDiscoveredSatisfaction, Catalogue.PlayerSuspicionSatisfaction, Catalogue.ObjectMovedSatisfaction, Catalogue.NoiseHeardSatisfaction, Catalogue.PlayerLostSatisfaction, Catalogue.NoiseHeardBySomebodyElseSatisfaction},
                                                               { Catalogue.PlayerSpottedRelief, Catalogue.PlayerDiscoveredRelief, Catalogue.PlayerSuspicionRelief, Catalogue.ObjectMovedRelief, Catalogue.NoiseHeardRelief, Catalogue.PlayerLostRelief, Catalogue.NoiseHeardBySomebodyElseRelief},
                                                               { Catalogue.PlayerSpottedDisappointment, Catalogue.PlayerDiscoveredDisappointment, Catalogue.PlayerSuspicionDisappointment, Catalogue.ObjectMovedDisappointment, Catalogue.NoiseHeardDisappointment, Catalogue.PlayerLostDisappointment, Catalogue.NoiseHeardBySomebodyElseDisappointment},
                                                               { Catalogue.PlayerSpottedPride, Catalogue.PlayerDiscoveredPride, Catalogue.PlayerSuspicionPride, Catalogue.ObjectMovedPride, Catalogue.NoiseHeardPride, Catalogue.PlayerLostPride, Catalogue.NoiseHeardBySomebodyElsePride},
                                                               { Catalogue.PlayerSpottedAdmiration, Catalogue.PlayerDiscoveredAdmiration, Catalogue.PlayerSuspicionAdmiration, Catalogue.ObjectMovedAdmiration, Catalogue.NoiseHeardAdmiration, Catalogue.PlayerLostAdmiration, Catalogue.NoiseHeardBySomebodyElseAdmiration},
                                                               { Catalogue.PlayerSpottedShame, Catalogue.PlayerDiscoveredShame, Catalogue.PlayerSuspicionShame, Catalogue.ObjectMovedShame, Catalogue.NoiseHeardShame, Catalogue.PlayerLostShame, Catalogue.NoiseHeardBySomebodyElseShame},
                                                               { Catalogue.PlayerSpottedReproach, Catalogue.PlayerDiscoveredReproach, Catalogue.PlayerSuspicionReproach, Catalogue.ObjectMovedReproach, Catalogue.NoiseHeardReproach, Catalogue.PlayerLostReproach, Catalogue.NoiseHeardBySomebodyElseReproach},
                                                               { Catalogue.PlayerSpottedLiking, Catalogue.PlayerDiscoveredLiking, Catalogue.PlayerSuspicionLiking, Catalogue.ObjectMovedLiking, Catalogue.NoiseHeardLiking, Catalogue.PlayerLostLiking, Catalogue.NoiseHeardBySomebodyElseLiking},
                                                               { Catalogue.PlayerSpottedDisliking, Catalogue.PlayerDiscoveredDisliking, Catalogue.PlayerSuspicionDisliking, Catalogue.ObjectMovedDisliking, Catalogue.NoiseHeardDisliking, Catalogue.PlayerLostDisliking, Catalogue.NoiseHeardBySomebodyElseDisliking},
                                                               { Catalogue.PlayerSpottedGratitude, Catalogue.PlayerDiscoveredGratitude, Catalogue.PlayerSuspicionGratitude, Catalogue.ObjectMovedGratitude, Catalogue.NoiseHeardGratitude, Catalogue.PlayerLostGratitude, Catalogue.NoiseHeardBySomebodyElseGratitude},
                                                               { Catalogue.PlayerSpottedAnger, Catalogue.PlayerDiscoveredAnger, Catalogue.PlayerSuspicionAnger, Catalogue.ObjectMovedAnger, Catalogue.NoiseHeardAnger, Catalogue.PlayerLostAnger, Catalogue.NoiseHeardBySomebodyElseAnger},
                                                               { Catalogue.PlayerSpottedGratification, Catalogue.PlayerDiscoveredGratification, Catalogue.PlayerSuspicionGratification, Catalogue.ObjectMovedGratification, Catalogue.NoiseHeardGratification, Catalogue.PlayerLostGratification, Catalogue.NoiseHeardBySomebodyElseGratification},
                                                               { Catalogue.PlayerSpottedRemorse, Catalogue.PlayerDiscoveredRemorse, Catalogue.PlayerSuspicionRemorse, Catalogue.ObjectMovedRemorse, Catalogue.NoiseHeardRemorse, Catalogue.PlayerLostRemorse, Catalogue.NoiseHeardBySomebodyElseRemorse},
                                                               { Catalogue.PlayerSpottedLove, Catalogue.PlayerDiscoveredLove, Catalogue.PlayerSuspicionLove, Catalogue.ObjectMovedLove, Catalogue.NoiseHeardLove, Catalogue.PlayerLostLove, Catalogue.NoiseHeardBySomebodyElseLove},
                                                               { Catalogue.PlayerSpottedHate, Catalogue.PlayerDiscoveredHate, Catalogue.PlayerSuspicionHate, Catalogue.ObjectMovedHate, Catalogue.NoiseHeardHate, Catalogue.PlayerLostHate, Catalogue.NoiseHeardBySomebodyElseHate} };

        public string ChooseCatalogEntry(EmotionType emotionType, Events.EventType eventType)
        {
            int emotionTypeInt = (int)emotionType;
            int eventTypeInt = (int)eventType;
            Catalogue catalogEntrySelected = m_actionsCatalog[emotionTypeInt, eventTypeInt];

            switch (catalogEntrySelected)
            {
                case Catalogue.PlayerSpottedJoy:
                    return ("PlayerSpottedJoy" + PlayerSpottedJoy());
                case Catalogue.PlayerSpottedDistress:
                    return ("PlayerSpottedDistress" + PlayerSpottedDistress());
                case Catalogue.PlayerSpottedResentment:
                    return ("PlayerSpottedResentment" + PlayerSpottedResentment());
                case Catalogue.PlayerSpottedPity:
                    return ("PlayerSpottedPity" + PlayerSpottedPity());
                case Catalogue.PlayerSpottedHope:
                    return ("PlayerSpottedHope" + PlayerSpottedHope());
                case Catalogue.PlayerSpottedFear:
                    return ("PlayerSpottedFear" + PlayerSpottedFear());
                case Catalogue.PlayerSpottedSatisfaction:
                    return ("PlayerSpottedSatisfaction" + PlayerSpottedSatisfaction());
                case Catalogue.PlayerSpottedRelief:
                    return ("PlayerSpottedRelief" + PlayerSpottedRelief());
                case Catalogue.PlayerSpottedDisappointment:
                    return ("PlayerSpottedDisappointment" + PlayerSpottedDisappointment());
                case Catalogue.PlayerSpottedPride:
                    return ("PlayerSpottedPride" + PlayerSpottedPride());
                case Catalogue.PlayerSpottedAdmiration:
                    return ("PlayerSpottedAdmiration" + PlayerSpottedAdmiration());
                case Catalogue.PlayerSpottedShame:
                    return ("PlayerSpottedShame" + PlayerSpottedShame());
                case Catalogue.PlayerSpottedReproach:
                    return ("PlayerSpottedReproach" + PlayerSpottedReproach());
                case Catalogue.PlayerSpottedLiking:
                    return ("PlayerSpottedLiking" + PlayerSpottedLiking());
                case Catalogue.PlayerSpottedDisliking:
                    return ("PlayerSpottedDisliking" + PlayerSpottedDisliking());
                case Catalogue.PlayerSpottedGratitude:
                    return ("PlayerSpottedGratitude" + PlayerSpottedGratitude());
                case Catalogue.PlayerSpottedAnger:
                    return ("PlayerSpottedAnger" + PlayerSpottedAnger());
                case Catalogue.PlayerSpottedGratification:
                    return ("PlayerSpottedGratification" + PlayerSpottedGratification());
                case Catalogue.PlayerSpottedRemorse:
                    return ("PlayerSpottedRemorse" + PlayerSpottedRemorse());
                case Catalogue.PlayerSpottedLove:
                    return ("PlayerSpottedLove" + PlayerSpottedLove());
                case Catalogue.PlayerSpottedHate:
                    return ("PlayerSpottedHate" + PlayerSpottedHate());
                case Catalogue.PlayerDiscoveredJoy:
                    return ("PlayerDiscoveredJoy" + PlayerDiscoveredJoy());
                case Catalogue.PlayerDiscoveredDistress:
                    return ("PlayerDiscoveredDistress" + PlayerDiscoveredDistress());
                case Catalogue.PlayerDiscoveredResentment:
                    return ("PlayerDiscoveredResentment" + PlayerDiscoveredResentment());
                case Catalogue.PlayerDiscoveredPity:
                    return ("PlayerDiscoveredPity" + PlayerDiscoveredPity());
                case Catalogue.PlayerDiscoveredHope:
                    return ("PlayerDiscoveredHope" + PlayerDiscoveredHope());
                case Catalogue.PlayerDiscoveredFear:
                    return ("PlayerDiscoveredFear" + PlayerDiscoveredFear());
                case Catalogue.PlayerDiscoveredSatisfaction:
                    return ("PlayerDiscoveredSatisfaction" + PlayerDiscoveredSatisfaction());
                case Catalogue.PlayerDiscoveredRelief:
                    return ("PlayerDiscoveredRelief" + PlayerDiscoveredRelief());
                case Catalogue.PlayerDiscoveredDisappointment:
                    return ("PlayerDiscoveredDisappointment" + PlayerDiscoveredDisappointment());
                case Catalogue.PlayerDiscoveredPride:
                    return ("PlayerDiscoveredPride" + PlayerDiscoveredPride());
                case Catalogue.PlayerDiscoveredAdmiration:
                    return ("PlayerDiscoveredAdmiration" + PlayerDiscoveredAdmiration());
                case Catalogue.PlayerDiscoveredShame:
                    return ("PlayerDiscoveredShame" + PlayerDiscoveredShame());
                case Catalogue.PlayerDiscoveredReproach:
                    return ("PlayerDiscoveredReproach" + PlayerDiscoveredReproach());
                case Catalogue.PlayerDiscoveredLiking:
                    return ("PlayerDiscoveredLiking" + PlayerDiscoveredLiking());
                case Catalogue.PlayerDiscoveredDisliking:
                    return ("PlayerDiscoveredDisliking" + PlayerDiscoveredDisliking());
                case Catalogue.PlayerDiscoveredGratitude:
                    return ("PlayerDiscoveredGratitude" + PlayerDiscoveredGratitude());
                case Catalogue.PlayerDiscoveredAnger:
                    return ("PlayerDiscoveredAnger" + PlayerDiscoveredAnger());
                case Catalogue.PlayerDiscoveredGratification:
                    return ("PlayerDiscoveredGratification" + PlayerDiscoveredGratification());
                case Catalogue.PlayerDiscoveredRemorse:
                    return ("PlayerDiscoveredRemorse" + PlayerDiscoveredRemorse());
                case Catalogue.PlayerDiscoveredLove:
                    return ("PlayerDiscoveredLove" + PlayerDiscoveredLove());
                case Catalogue.PlayerDiscoveredHate:
                    return ("PlayerDiscoveredHate" + PlayerDiscoveredHate());
                case Catalogue.PlayerSuspicionJoy:
                    return ("PlayerSuspicionJoy" + PlayerSuspicionJoy());
                case Catalogue.PlayerSuspicionDistress:
                    return ("PlayerSuspicionDistress" + PlayerSuspicionDistress());
                case Catalogue.PlayerSuspicionResentment:
                    return ("PlayerSuspicionResentment" + PlayerSuspicionResentment());
                case Catalogue.PlayerSuspicionPity:
                    return ("PlayerSuspicionPity" + PlayerSuspicionPity());
                case Catalogue.PlayerSuspicionHope:
                    return ("PlayerSuspicionHope" + PlayerSuspicionHope());
                case Catalogue.PlayerSuspicionFear:
                    return ("PlayerSuspicionFear" + PlayerSuspicionFear());
                case Catalogue.PlayerSuspicionSatisfaction:
                    return ("PlayerSuspicionSatisfaction" + PlayerSuspicionSatisfaction());
                case Catalogue.PlayerSuspicionRelief:
                    return ("PlayerSuspicionRelief" + PlayerSuspicionRelief());
                case Catalogue.PlayerSuspicionDisappointment:
                    return ("PlayerSuspicionDisappointment" + PlayerSuspicionDisappointment());
                case Catalogue.PlayerSuspicionPride:
                    return ("PlayerSuspicionPride" + PlayerSuspicionPride());
                case Catalogue.PlayerSuspicionAdmiration:
                    return ("PlayerSuspicionAdmiration" + PlayerSuspicionAdmiration());
                case Catalogue.PlayerSuspicionShame:
                    return ("PlayerSuspicionShame" + PlayerSuspicionShame());
                case Catalogue.PlayerSuspicionReproach:
                    return ("PlayerSuspicionReproach" + PlayerSuspicionReproach());
                case Catalogue.PlayerSuspicionLiking:
                    return ("PlayerSuspicionLiking" + PlayerSuspicionLiking());
                case Catalogue.PlayerSuspicionDisliking:
                    return ("PlayerSuspicionDisliking" + PlayerSuspicionDisliking());
                case Catalogue.PlayerSuspicionGratitude:
                    return ("PlayerSuspicionGratitude" + PlayerSuspicionGratitude());
                case Catalogue.PlayerSuspicionAnger:
                    return ("PlayerSuspicionAnger" + PlayerSuspicionAnger());
                case Catalogue.PlayerSuspicionGratification:
                    return ("PlayerSuspicionGratification" + PlayerSuspicionGratification());
                case Catalogue.PlayerSuspicionRemorse:
                    return ("PlayerSuspicionRemorse" + PlayerSuspicionRemorse());
                case Catalogue.PlayerSuspicionLove:
                    return ("PlayerSuspicionLove" + PlayerSuspicionLove());
                case Catalogue.PlayerSuspicionHate:
                    return ("PlayerSuspicionHate" + PlayerSuspicionHate());
                case Catalogue.ObjectMovedJoy:
                    return ("ObjectMovedJoy" + ObjectMovedJoy());
                case Catalogue.ObjectMovedDistress:
                    return ("ObjectMovedDistress" + ObjectMovedDistress());
                case Catalogue.ObjectMovedResentment:
                    return ("ObjectMovedResentment" + ObjectMovedResentment());
                case Catalogue.ObjectMovedPity:
                    return ("ObjectMovedPity" + ObjectMovedPity());
                case Catalogue.ObjectMovedHope:
                    return ("ObjectMovedHope" + ObjectMovedHope());
                case Catalogue.ObjectMovedFear:
                    return ("ObjectMovedFear" + ObjectMovedFear());
                case Catalogue.ObjectMovedSatisfaction:
                    return ("ObjectMovedSatisfaction" + ObjectMovedSatisfaction());
                case Catalogue.ObjectMovedRelief:
                    return ("ObjectMovedRelief" + ObjectMovedRelief());
                case Catalogue.ObjectMovedDisappointment:
                    return ("ObjectMovedDisappointment" + ObjectMovedDisappointment());
                case Catalogue.ObjectMovedPride:
                    return ("ObjectMovedPride" + ObjectMovedPride());
                case Catalogue.ObjectMovedAdmiration:
                    return ("ObjectMovedAdmiration" + ObjectMovedAdmiration());
                case Catalogue.ObjectMovedShame:
                    return ("ObjectMovedShame" + ObjectMovedShame());
                case Catalogue.ObjectMovedReproach:
                    return ("ObjectMovedReproach" + ObjectMovedReproach());
                case Catalogue.ObjectMovedLiking:
                    return ("ObjectMovedLiking" + ObjectMovedLiking());
                case Catalogue.ObjectMovedDisliking:
                    return ("ObjectMovedDisliking" + ObjectMovedDisliking());
                case Catalogue.ObjectMovedGratitude:
                    return ("ObjectMovedGratitude" + ObjectMovedGratitude());
                case Catalogue.ObjectMovedAnger:
                    return ("ObjectMovedAnger" + ObjectMovedAnger());
                case Catalogue.ObjectMovedGratification:
                    return ("ObjectMovedGratification" + ObjectMovedGratification());
                case Catalogue.ObjectMovedRemorse:
                    return ("ObjectMovedRemorse" + ObjectMovedRemorse());
                case Catalogue.ObjectMovedLove:
                    return ("ObjectMovedLove" + ObjectMovedLove());
                case Catalogue.ObjectMovedHate:
                    return ("ObjectMovedHate" + ObjectMovedHate());
                case Catalogue.NoiseHeardJoy:
                    return ("NoiseHeardJoy" + NoiseHeardJoy());
                case Catalogue.NoiseHeardDistress:
                    return ("NoiseHeardDistress" + NoiseHeardDistress());
                case Catalogue.NoiseHeardResentment:
                    return ("NoiseHeardResentment" + NoiseHeardResentment());
                case Catalogue.NoiseHeardPity:
                    return ("NoiseHeardPity" + NoiseHeardPity());
                case Catalogue.NoiseHeardHope:
                    return ("NoiseHeardHope" + NoiseHeardHope());
                case Catalogue.NoiseHeardFear:
                    return ("NoiseHeardFear" + NoiseHeardFear());
                case Catalogue.NoiseHeardSatisfaction:
                    return ("NoiseHeardSatisfaction" + NoiseHeardSatisfaction());
                case Catalogue.NoiseHeardRelief:
                    return ("NoiseHeardRelief" + NoiseHeardRelief());
                case Catalogue.NoiseHeardDisappointment:
                    return ("NoiseHeardDisappointment" + NoiseHeardDisappointment());
                case Catalogue.NoiseHeardPride:
                    return ("NoiseHeardPride" + NoiseHeardPride());
                case Catalogue.NoiseHeardAdmiration:
                    return ("NoiseHeardAdmiration" + NoiseHeardAdmiration());
                case Catalogue.NoiseHeardShame:
                    return ("NoiseHeardShame" + NoiseHeardShame());
                case Catalogue.NoiseHeardReproach:
                    return ("NoiseHeardReproach" + NoiseHeardReproach());
                case Catalogue.NoiseHeardLiking:
                    return ("NoiseHeardLiking" + NoiseHeardLiking());
                case Catalogue.NoiseHeardDisliking:
                    return ("NoiseHeardDisliking" + NoiseHeardDisliking());
                case Catalogue.NoiseHeardGratitude:
                    return ("NoiseHeardGratitude" + NoiseHeardGratitude());
                case Catalogue.NoiseHeardAnger:
                    return ("NoiseHeardAnger" + NoiseHeardAnger());
                case Catalogue.NoiseHeardGratification:
                    return ("NoiseHeardGratification" + NoiseHeardGratification());
                case Catalogue.NoiseHeardRemorse:
                    return ("NoiseHeardRemorse" + NoiseHeardRemorse());
                case Catalogue.NoiseHeardLove:
                    return ("NoiseHeardLove" + NoiseHeardLove());
                case Catalogue.NoiseHeardHate:
                    return ("NoiseHeardHate" + NoiseHeardHate());
                case Catalogue.PlayerLostJoy:
                    return ("PlayerLostJoy" + PlayerLostJoy());
                case Catalogue.PlayerLostDistress:
                    return ("PlayerLostDistress" + PlayerLostDistress());
                case Catalogue.PlayerLostResentment:
                    return ("PlayerLostResentment" + PlayerLostResentment());
                case Catalogue.PlayerLostPity:
                    return ("PlayerLostPity" + PlayerLostPity());
                case Catalogue.PlayerLostHope:
                    return ("PlayerLostHope" + PlayerLostHope());
                case Catalogue.PlayerLostFear:
                    return ("PlayerLostFear" + PlayerLostFear());
                case Catalogue.PlayerLostSatisfaction:
                    return ("PlayerLostSatisfaction" + PlayerLostSatisfaction());
                case Catalogue.PlayerLostRelief:
                    return ("PlayerLostRelief" + PlayerLostRelief());
                case Catalogue.PlayerLostDisappointment:
                    return ("PlayerLostDisappointment" + PlayerLostDisappointment());
                case Catalogue.PlayerLostPride:
                    return ("PlayerLostPride" + PlayerLostPride());
                case Catalogue.PlayerLostAdmiration:
                    return ("PlayerLostAdmiration" + PlayerLostAdmiration());
                case Catalogue.PlayerLostShame:
                    return ("PlayerLostShame" + PlayerLostShame());
                case Catalogue.PlayerLostReproach:
                    return ("PlayerLostReproach" + PlayerLostReproach());
                case Catalogue.PlayerLostLiking:
                    return ("PlayerLostLiking" + PlayerLostLiking());
                case Catalogue.PlayerLostDisliking:
                    return ("PlayerLostDisliking" + PlayerLostDisliking());
                case Catalogue.PlayerLostGratitude:
                    return ("PlayerLostGratitude" + PlayerLostGratitude());
                case Catalogue.PlayerLostAnger:
                    return ("PlayerLostAnger" + PlayerLostAnger());
                case Catalogue.PlayerLostGratification:
                    return ("PlayerLostGratification" + PlayerLostGratification());
                case Catalogue.PlayerLostRemorse:
                    return ("PlayerLostRemorse" + PlayerLostRemorse());
                case Catalogue.PlayerLostLove:
                    return ("PlayerLostLove" + PlayerLostLove());
                case Catalogue.PlayerLostHate:
                    return ("PlayerLostHate" + PlayerLostHate());
                case Catalogue.NoiseHeardBySomebodyElseJoy:
                    return ("NoiseHeardBySomebodyElseJoy" + NoiseHeardBySomebodyElseJoy());
                case Catalogue.NoiseHeardBySomebodyElseDistress:
                    return ("NoiseHeardBySomebodyElseDistress" + NoiseHeardBySomebodyElseDistress());
                case Catalogue.NoiseHeardBySomebodyElseResentment:
                    return ("NoiseHeardBySomebodyElseResentment" + NoiseHeardBySomebodyElseResentment());
                case Catalogue.NoiseHeardBySomebodyElsePity:
                    return ("NoiseHeardBySomebodyElsePity" + NoiseHeardBySomebodyElsePity());
                case Catalogue.NoiseHeardBySomebodyElseHope:
                    return ("NoiseHeardBySomebodyElseHope" + NoiseHeardBySomebodyElseHope());
                case Catalogue.NoiseHeardBySomebodyElseFear:
                    return ("NoiseHeardBySomebodyElseFear" + NoiseHeardBySomebodyElseFear());
                case Catalogue.NoiseHeardBySomebodyElseSatisfaction:
                    return ("NoiseHeardBySomebodyElseSatisfaction" + NoiseHeardBySomebodyElseSatisfaction());
                case Catalogue.NoiseHeardBySomebodyElseRelief:
                    return ("NoiseHeardBySomebodyElseRelief" + NoiseHeardBySomebodyElseRelief());
                case Catalogue.NoiseHeardBySomebodyElseDisappointment:
                    return ("NoiseHeardBySomebodyElseDisappointment" + NoiseHeardBySomebodyElseDisappointment());
                case Catalogue.NoiseHeardBySomebodyElsePride:
                    return ("NoiseHeardBySomebodyElsePride" + NoiseHeardBySomebodyElsePride());
                case Catalogue.NoiseHeardBySomebodyElseAdmiration:
                    return ("NoiseHeardBySomebodyElseAdmiration" + NoiseHeardBySomebodyElseAdmiration());
                case Catalogue.NoiseHeardBySomebodyElseShame:
                    return ("NoiseHeardBySomebodyElseShame" + NoiseHeardBySomebodyElseShame());
                case Catalogue.NoiseHeardBySomebodyElseReproach:
                    return ("NoiseHeardBySomebodyElseReproach" + NoiseHeardBySomebodyElseReproach());
                case Catalogue.NoiseHeardBySomebodyElseLiking:
                    return ("NoiseHeardBySomebodyElseLiking" + NoiseHeardBySomebodyElseLiking());
                case Catalogue.NoiseHeardBySomebodyElseDisliking:
                    return ("NoiseHeardBySomebodyElseDisliking" + NoiseHeardBySomebodyElseDisliking());
                case Catalogue.NoiseHeardBySomebodyElseGratitude:
                    return ("NoiseHeardBySomebodyElseGratitude" + NoiseHeardBySomebodyElseGratitude());
                case Catalogue.NoiseHeardBySomebodyElseAnger:
                    return ("NoiseHeardBySomebodyElseAnger" + NoiseHeardBySomebodyElseAnger());
                case Catalogue.NoiseHeardBySomebodyElseGratification:
                    return ("NoiseHeardBySomebodyElseGratification" + NoiseHeardBySomebodyElseGratification());
                case Catalogue.NoiseHeardBySomebodyElseRemorse:
                    return ("NoiseHeardBySomebodyElseRemorse" + NoiseHeardBySomebodyElseRemorse());
                case Catalogue.NoiseHeardBySomebodyElseLove:
                    return ("NoiseHeardBySomebodyElseLove" + NoiseHeardBySomebodyElseLove());
                case Catalogue.NoiseHeardBySomebodyElseHate:
                    return ("NoiseHeardBySomebodyElseHate" + NoiseHeardBySomebodyElseHate());
                default:
                    return "Default Entered";
            }
        }
        #region Joy
        private string PlayerSpottedJoy()
        {
            string[] responses = new string[] { "Finally!",
                                                "Haha! He is here!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredJoy()
        {
            string[] responses = new string[] { "Oh! There you are!",
                                                "Oh hello again!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionJoy()
        {
            string[] responses = new string[] { "O ho ho! I think someone's here!",
                                                "Someone's been sneaking about! Excellent!" };
            int chosenResponse = Random.Range(0, responses.Length);
            return responses[chosenResponse];
        }

        private string ObjectMovedJoy()
        {
            string[] responses = new string[] { "Oh! This wasn't like that earlier!",
                                                "Oh oh! This has chaged! Yes it has!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardJoy()
        {
            string[] responses = new string[] { "I heard that haha!",
                                                "What was that? Is it him? Is it? Is it?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostJoy()
        {
            string[] responses = new string[] { "I lost you! Time to find you again!",
                                                "Yes, yes! Let's restart the hide 'n' seek!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseJoy()
        {
            string[] responses = new string[] { "I'm comiiiiiiiiiing!",
                                                "You saw him? Where! Where?!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Distress
        private string PlayerSpottedDistress()
        {
            string[] responses = new string[] { "Oh no! It's him!",
                                                "ah!",
                                                "Get away!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredDistress()
        {
            string[] responses = new string[] { "Ah! You're here again!",
                                                "Why are you here again?",
                                                "Not again!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionDistress()
        {
            string[] responses = new string[] { "Is someone lurking? Go away!",
                                                "Hello? Someone there...?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedDistress()
        {
            string[] responses = new string[] { "What was that?!",
                                                "oh, no no no no...." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardDistress()
        {
            string[] responses = new string[] { "Was that him? Was it? Oh no.",
                                                "ah! Did I see him? I don't know!",
                                                "That wasn't him was it? Was it?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostDistress()
        {
            string[] responses = new string[] { "Oh no! Where is he!? Where is he???",
                                                "Ah!!! Where did he go!?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseDistress()
        {
            string[] responses = new string[] { "He's there?! I don't wanna go!",
                                                "I can't... I won't!",
                                                "You! You deal with him!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Resentment
        private string PlayerSpottedResentment()
        {
            string[] responses = new string[] { "For real? You're here? What a pain",
                                                "Oh come on now..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredResentment()
        {
            string[] responses = new string[] { "Oh... back at it again?",
                                                "Oh well..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionResentment()
        {
            string[] responses = new string[] { "Maybe that was something... not checking it though",
                                                "Uh! I don't want to check this!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedResentment()
        {
            string[] responses = new string[] { "Uh! another thing different!",
                                                "What a pain...",
                                                "Maybe if I don't react, nothing will happen..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardResentment()
        {
            string[] responses = new string[] { "This was probably nothing...",
                                                "I'm too bored to check this"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostResentment()
        {
            string[] responses = new string[] { "Don't have to chase him now...",
                                                "This is probably even better..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseResentment()
        {
            string[] responses = new string[] { "I can't be bothered...",
                                                "Get him yourself..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Pity
        private string PlayerSpottedPity()
        {
            string[] responses = new string[] { "Oh! Don't you look dreadful.",
                                                "My dear, my dear you look so sad."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredPity()
        {
            string[] responses = new string[] { "I saw something sad.",
                                                "Isn't this a shame..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionPity()
        {
            string[] responses = new string[] { "I pity the poor soul that lurks in here" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedPity()
        {
            string[] responses = new string[] { "And what made you move little thing?",
                                                "Another thing out of order, goodness me.", };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardPity()
        {
            string[] responses = new string[] { "Carelessness. Poor you!",
                                                "Oh no. He made a mistake."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostPity()
        {
            string[] responses = new string[] { "He thinks he can hide.",
                                                "Look at him... He ragained hope." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElsePity()
        {
            string[] responses = new string[] { "I'll help you, you poor soul." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Hope
        private string PlayerSpottedHope()
        {
            string[] responses = new string[] { "Oh thank God you're here!",
                                                "Praise be you're here! You're here!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredHope()
        {
            string[] responses = new string[] { "Here you are again!",
                                                "Yes! He is here I knew it!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionHope()
        {
            string[] responses = new string[] { "I know you're here I just know it!",
                                                "I think... I hope!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedHope()
        {
            string[] responses = new string[] { "Oh somebody moved this. Thank the heavens!",
                                                "This means someone is here!",
                                                "This means maybe he's here! Please let it be so!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardHope()
        {
            string[] responses = new string[] { "A sound?",
                                                "Oh no. He made a mistake."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostHope()
        {
            string[] responses = new string[] { "I'll find you again...",
                                                "No one stays hidden forever." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseHope()
        {
            string[] responses = new string[] { "Help arrives!",
                                                "Help finds those in need!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Fear
        private string PlayerSpottedFear()
        {
            string[] responses = new string[] { "GET AWAY!! AHHHH!",
                                                "Please don't hurt me! Please...!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredFear()
        {
            string[] responses = new string[] { "No no no no!",
                                                "Oh no!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionFear()
        {
            string[] responses = new string[] { "Where are you???",
                                                "Please go away... please go away..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedFear()
        {
            string[] responses = new string[] { "Oh no! Who did that?!",
                                                "This... wasn't like that! Was it?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardFear()
        {
            string[] responses = new string[] { "What was that?",
                                                "It coulnd't be him right? Right!?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostFear()
        {
            string[] responses = new string[] { "Oh no I lost him! Oh nononono...",
                                                "Where did he go? WHERE DID HE GO!?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseFear()
        {
            string[] responses = new string[] { "Ah!",
                                                "No no no no no..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Satisfaction
        private string PlayerSpottedSatisfaction()
        {
            string[] responses = new string[] { "Yes! Yes! Yes!",
                                                "I will enjoy this mut!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredSatisfaction()
        {
            string[] responses = new string[] { "There!",
                                                "Found you!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionSatisfaction()
        {
            string[] responses = new string[] { "I will enjoy finding you...",
                                                "Where are you..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedSatisfaction()
        {
            string[] responses = new string[] { "Mut did this.",
                                                "I will enjoy finding you." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardSatisfaction()
        {
            string[] responses = new string[] { "I think I hear something",
                                                "Was that you mut?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostSatisfaction()
        {
            string[] responses = new string[] { "Little mut left... No matter!",
                                                "Where are you... little mut"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseSatisfaction()
        {
            string[] responses = new string[] { "Yes! You found him!",
                                                "Finally, some action!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Relief
        private string PlayerSpottedRelief()
        {
            string[] responses = new string[] { "Oh there he is! Oh thank god" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredRelief()
        {
            string[] responses = new string[] { "Oh I lost him! Phew...",
                                                "I lost him... this is good."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionRelief()
        {
            string[] responses = new string[] { "Oh I think I saw him. This is good",
                                                "Oh phew... He was there I think." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedRelief()
        {
            string[] responses = new string[] { "Oh it's just this thing.",
                                                "Ah it's only this. Ok." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardRelief()
        {
            string[] responses = new string[] { "Oh I think I hear something. Good",
                                                "Phew! My ears work fine!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostRelief()
        {
            string[] responses = new string[] { "Oh I lost him! Phew...",
                                                "I lost him... this is good."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseRelief()
        {
            string[] responses = new string[] { "Oh good.",
                                                "Oh good you found something."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Disappointment
        private string PlayerSpottedDisappointment()
        {
            string[] responses = new string[] { "Oh it's just you?",
                                                "Oh great... it's you"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredDisappointment()
        {
            string[] responses = new string[] { "As i thought...",
                                                "Another failure..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionDisappointment()
        {
            string[] responses = new string[] { "Come out... We both know you'll eventually fail.",
                                                "You're embarassing yourself." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedDisappointment()
        {
            string[] responses = new string[] { "Another thing moved...",
                                                "Isn't this dissapointing..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardDisappointment()
        {
            string[] responses = new string[] { "I thought you were better than this.",
                                                "Yet another mistake..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostDisappointment()
        {
            string[] responses = new string[] { "Remind me to care about you escaping...",
                                                "Ohh!! I can't believe he escaped!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseDisappointment()
        {
            string[] responses = new string[] { "He got found I see...",
                                                "Guess they were wrong about you..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Pride
        private string PlayerSpottedPride()
        {
            string[] responses = new string[] { "Well of course I found you",
                                                "You've already lost",
                                                "Like I wouldn't find you!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredPride()
        {
            string[] responses = new string[] { "Well. This is not surprising... is it?",
                                                "Well of course I found you again!",
                                                "Game over!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionPride()
        {
            string[] responses = new string[] { "No one remains hidden from me!",
                                                "We both know I'll find you",
                                                "Just come out already." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedPride()
        {
            string[] responses = new string[] { "Well well... This is new",
                                                "I of course noticed the change here!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardPride()
        {
            string[] responses = new string[] { "My peripheral sight is as sharp as always",
                                                "Aren't I the observant one! Who goes there?",
                                                "Nothing escapes my attention!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostPride()
        {
            string[] responses = new string[] { "Don't think you've escaped!",
                                                "I only let you escape this time.",
                                                "Losing you is part of the fun!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElsePride()
        {
            string[] responses = new string[] { "Step aside! I'll handle him",
                                                "Technically, you found him because of me",
                                                "Of course my assistance is necessary!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Admiration
        private string PlayerSpottedAdmiration()
        {
            string[] responses = new string[] { "Oh wow I did find you!",
                                                "Hey you're here! Amazing!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredAdmiration()
        {
            string[] responses = new string[] { "Wow I saw you with my peripheral!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionAdmiration()
        {
            string[] responses = new string[] { "You have remained hidden for such a long time!",
                                                "Wow! I really cannot find you!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedAdmiration()
        {
            string[] responses = new string[] { "Oh wow! This wasn't like that",
                                                "It's different now! Cool!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardAdmiration()
        {
            string[] responses = new string[] { "Oh hey! I heard something!",
                                                "Wow. That was something"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostAdmiration()
        {
            string[] responses = new string[] { "He escaped? Wow...",
                                                "Hey I lost him! Amazing! Haha!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseAdmiration()
        {
            string[] responses = new string[] { "I applaud at how observant you are!",
                                                "Wow you're good!",
                                                "You found him! Nice!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Shame
        private string PlayerSpottedShame()
        {
            string[] responses = new string[] { "...",
                                                "Um... stop... please?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredShame()
        {
            string[] responses = new string[] { "Hey so... I found you... again",
                                                "Maybe... give up? You know..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionShame()
        {
            string[] responses = new string[] { "Maybe I saw something... maybe...",
                                                "Um... hello?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedShame()
        {
            string[] responses = new string[] { "That wasn't... was it?",
                                                "Um..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardShame()
        {
            string[] responses = new string[] { "Who... who's there?",
                                                "Um... Who's there?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostShame()
        {
            string[] responses = new string[] { "Oh no..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseShame()
        {
            string[] responses = new string[] { "I applaud at how observant you are!",
                                                "Wow you're good!",
                                                "You found him! Nice!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Reproach
        private string PlayerSpottedReproach()
        {
            string[] responses = new string[] { "You got caught? Hahaha!",
                                                "Kneel loser!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredReproach()
        {
            string[] responses = new string[] { "You got found again!? Hahaha!",
                                                "What a loser!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionReproach()
        {
            string[] responses = new string[] { "Don't get your hopes up!",
                                                "I will find you!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedReproach()
        {
            string[] responses = new string[] { "Who was the idiot that made a mess here?",
                                                "Worthless!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardReproach()
        {
            string[] responses = new string[] { "That was sloppy!",
                                                "So noisy! So sloppy!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostReproach()
        {
            string[] responses = new string[] { "You actually think you got away?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseReproach()
        {
            string[] responses = new string[] { "Serves you right!",
                                                "The fool got caught"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Liking
        private string PlayerSpottedLiking()
        {
            string[] responses = new string[] { "Nice! There you are!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredLiking()
        {
            string[] responses = new string[] { "Hello again!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionLiking()
        {
            string[] responses = new string[] { "I kinda like this hide and seek",
                                                "I can do this all day."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedLiking()
        {
            string[] responses = new string[] { "This is actually better like that.",
                                                "I'm ok with this change." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardLiking()
        {
            string[] responses = new string[] { "Oh I heard something I did!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostLiking()
        {
            string[] responses = new string[] { "I'm ok with losing you." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseLiking()
        {
            string[] responses = new string[] { "Nice!",
                                                "You found something? Nice"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Disliking
        private string PlayerSpottedDisliking()
        {
            string[] responses = new string[] { "You I don't like seeing you here.",
                                                "Isn't this a displeasure..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredDisliking()
        {
            string[] responses = new string[] { "You could've appeared sooner",
                                                "Just surrender..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionDisliking()
        {
            string[] responses = new string[] { "Stop wasting my time...",
                                                "Just come out already..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedDisliking()
        {
            string[] responses = new string[] { "This is a mess...",
                                                "This really shouldn't be like that" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardDisliking()
        {
            string[] responses = new string[] { "Another sound I gotta investigate.",
                                                "I really dislike this..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostDisliking()
        {
            string[] responses = new string[] { "I am midly annoyed",
                                                "This is furstrating" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseDisliking()
        {
            string[] responses = new string[] { "There's an issue? Darn it...",
                                                "Oh crap..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Gratitude
        private string PlayerSpottedGratitude()
        {
            string[] responses = new string[] { "Right when I needed you most!",
                                                "Thank you for being here."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredGratitude()
        {
            string[] responses = new string[] { "You have re-appeared!",
                                                "I thought I lost you..." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionGratitude()
        {
            string[] responses = new string[] { "Oh thank god he is not showing up",
                                                "Would be nice if you stayed hidden."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedGratitude()
        {
            string[] responses = new string[] { "Thank you whoever moved that!",
                                                "This is better and I am thankful." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardGratitude()
        {
            string[] responses = new string[] { "Thank you for making this easy." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostGratitude()
        {
            string[] responses = new string[] { "Thank you for leaving!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseGratitude()
        {
            string[] responses = new string[] { "You got something? Thank god!",
                                                "Thank you for finding him"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Anger
        private string PlayerSpottedAnger()
        {
            string[] responses = new string[] { "Insignifigant!",
                                                "Get over here!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredAnger()
        {
            string[] responses = new string[] { "Useless!",
                                                "Begone!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionAnger()
        {
            string[] responses = new string[] { "Come out! COME OUT!",
                                                "Oh this is pissing me off!",
                                                "Just come out already!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedAnger()
        {
            string[] responses = new string[] { "For real? Who did that?",
                                                "Who moved that?",
                                                "Are you serious with this?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardAnger()
        {
            string[] responses = new string[] { "Stop the ruckus",
                                                "I can't stand the noise!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostAnger()
        {
            string[] responses = new string[] { "Get back here!!",
                                                "Don't you dare hide!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseAnger()
        {
            string[] responses = new string[] { "Hold him still! I'm coming",
                                                "Attack the intruder!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Gratification
        private string PlayerSpottedGratification()
        {
            string[] responses = new string[] { "Pleased to meet you!",
                                                "I will enjoy this"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredGratification()
        {
            string[] responses = new string[] { "We meet again!",
                                                "I will savor this moment of reunion." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionGratification()
        {
            string[] responses = new string[] { "I'm just glad you're here... somewhere." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedGratification()
        {
            string[] responses = new string[] { "About time somebody made that change.",
                                                "Well I guess I can use this as a clue.",
                                                "Take what I can I guess." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardGratification()
        {
            string[] responses = new string[] { "This makes this easier",
                                                "I know where that came from",
                                                "This is a start" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostGratification()
        {
            string[] responses = new string[] { "Welp! Back at it",
                                                "I can always find you again."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseGratification()
        {
            string[] responses = new string[] { "Good work!",
                                                "On my way!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Remorse
        private string PlayerSpottedRemorse()
        {
            string[] responses = new string[] { "Hey!",
                                                "I'm sorry but... you must stop"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredRemorse()
        {
            string[] responses = new string[] { "I'm sorry but you have to come with me.",
                                                "Just surrender and this will be over.",
                                                "Don't make me do something I don't want to do."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionRemorse()
        {
            string[] responses = new string[] { "I don't like this situation",
                                                "I'm sorry but you have to come out now."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedRemorse()
        {
            string[] responses = new string[] { "I should've paid more attention...",
                                                "I am sorry for the change Master" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardRemorse()
        {
            string[] responses = new string[] { "Here we go again...",
                                                "Hello?",
                                                "Anyone there?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostRemorse()
        {
            string[] responses = new string[] { "Master... I'm sorry.",
                                                "Another failure..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseRemorse()
        {
            string[] responses = new string[] { "You won't go through this alone!",
                                                "Hold on!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Love
        private string PlayerSpottedLove()
        {
            string[] responses = new string[] { "Oh... Hi there",
                                                "Well hello..."};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredLove()
        {
            string[] responses = new string[] { "I knew you'd be back!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionLove()
        {
            string[] responses = new string[] { "I enjoy this game when it is with you." };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedLove()
        {
            string[] responses = new string[] { "This is lovely!",
                                                "I am sure Master will love this!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardLove()
        {
            string[] responses = new string[] { "Is that you?",
                                                "Hello?" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostLove()
        {
            string[] responses = new string[] { "It's adorable that you've gone hiding!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseLove()
        {
            string[] responses = new string[] { "Hold on!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion
        #region Hate
        private string PlayerSpottedHate()
        {
            string[] responses = new string[] { "GET OUT OF HERE!",
                                                "NO VISITORS!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerDiscoveredHate()
        {
            string[] responses = new string[] { "AGAIN?!" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerSuspicionHate()
        {
            string[] responses = new string[] { "COME OUT NOW!",
                                                "I DON'T HAVE TIME FOR THIS" };
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string ObjectMovedHate()
        {
            string[] responses = new string[] { "WHO MOVED THAT?!",
                                                "YOU WILL PAY FOR THIS",
                                                "MASTER HATES CHANGES! AND SO DO I!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardHate()
        {
            string[] responses = new string[] { "AHHHH!",
                                                "STOP THE NOISE!",
                                                "SILENCE!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string PlayerLostHate()
        {
            string[] responses = new string[] { "GET BACK OUTSIDE!",
                                                "GET BACK HERE!",
                                                "ARE YOU SERIOUS?!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }

        private string NoiseHeardBySomebodyElseHate()
        {
            string[] responses = new string[] { "HOLD HIM STILL!",
                                                "DESTROY THE INVADER!"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
        #endregion

        private enum Catalogue
        {
            PlayerSpottedJoy = 0,
            PlayerSpottedDistress = 1,
            PlayerSpottedResentment = 2,
            PlayerSpottedPity = 3,
            PlayerSpottedHope = 4,
            PlayerSpottedFear = 5,
            PlayerSpottedSatisfaction = 6,
            PlayerSpottedRelief = 7,
            PlayerSpottedDisappointment = 8,
            PlayerSpottedPride = 9,
            PlayerSpottedAdmiration = 10,
            PlayerSpottedShame = 11,
            PlayerSpottedReproach = 12,
            PlayerSpottedLiking = 13,
            PlayerSpottedDisliking = 14,
            PlayerSpottedGratitude = 15,
            PlayerSpottedAnger = 16,
            PlayerSpottedGratification = 17,
            PlayerSpottedRemorse = 18,
            PlayerSpottedLove = 19,
            PlayerSpottedHate = 20,
            PlayerDiscoveredJoy = 21,
            PlayerDiscoveredDistress = 22,
            PlayerDiscoveredResentment = 23,
            PlayerDiscoveredPity = 24,
            PlayerDiscoveredHope = 25,
            PlayerDiscoveredFear = 26,
            PlayerDiscoveredSatisfaction = 27,
            PlayerDiscoveredRelief = 28,
            PlayerDiscoveredDisappointment = 29,
            PlayerDiscoveredPride = 30,
            PlayerDiscoveredAdmiration = 31,
            PlayerDiscoveredShame = 32,
            PlayerDiscoveredReproach = 33,
            PlayerDiscoveredLiking = 34,
            PlayerDiscoveredDisliking = 35,
            PlayerDiscoveredGratitude = 36,
            PlayerDiscoveredAnger = 37,
            PlayerDiscoveredGratification = 38,
            PlayerDiscoveredRemorse = 39,
            PlayerDiscoveredLove = 40,
            PlayerDiscoveredHate = 41,
            PlayerSuspicionJoy = 42,
            PlayerSuspicionDistress = 43,
            PlayerSuspicionResentment = 44,
            PlayerSuspicionPity = 45,
            PlayerSuspicionHope = 46,
            PlayerSuspicionFear = 47,
            PlayerSuspicionSatisfaction = 48,
            PlayerSuspicionRelief = 49,
            PlayerSuspicionDisappointment = 50,
            PlayerSuspicionPride = 51,
            PlayerSuspicionAdmiration = 52,
            PlayerSuspicionShame = 53,
            PlayerSuspicionReproach = 54,
            PlayerSuspicionLiking = 55,
            PlayerSuspicionDisliking = 56,
            PlayerSuspicionGratitude = 57,
            PlayerSuspicionAnger = 58,
            PlayerSuspicionGratification = 59,
            PlayerSuspicionRemorse = 60,
            PlayerSuspicionLove = 61,
            PlayerSuspicionHate = 62,
            ObjectMovedJoy = 63,
            ObjectMovedDistress = 64,
            ObjectMovedResentment = 65,
            ObjectMovedPity = 66,
            ObjectMovedHope = 67,
            ObjectMovedFear = 68,
            ObjectMovedSatisfaction = 69,
            ObjectMovedRelief = 70,
            ObjectMovedDisappointment = 71,
            ObjectMovedPride = 72,
            ObjectMovedAdmiration = 73,
            ObjectMovedShame = 74,
            ObjectMovedReproach = 75,
            ObjectMovedLiking = 76,
            ObjectMovedDisliking = 77,
            ObjectMovedGratitude = 78,
            ObjectMovedAnger = 79,
            ObjectMovedGratification = 80,
            ObjectMovedRemorse = 81,
            ObjectMovedLove = 82,
            ObjectMovedHate = 83,
            NoiseHeardJoy = 84,
            NoiseHeardDistress = 85,
            NoiseHeardResentment = 86,
            NoiseHeardPity = 87,
            NoiseHeardHope = 88,
            NoiseHeardFear = 89,
            NoiseHeardSatisfaction = 90,
            NoiseHeardRelief = 91,
            NoiseHeardDisappointment = 92,
            NoiseHeardPride = 93,
            NoiseHeardAdmiration = 94,
            NoiseHeardShame = 95,
            NoiseHeardReproach = 96,
            NoiseHeardLiking = 97,
            NoiseHeardDisliking = 98,
            NoiseHeardGratitude = 99,
            NoiseHeardAnger = 100,
            NoiseHeardGratification = 101,
            NoiseHeardRemorse = 102,
            NoiseHeardLove = 103,
            NoiseHeardHate = 104,
            PlayerLostJoy = 105,
            PlayerLostDistress = 106,
            PlayerLostResentment = 107,
            PlayerLostPity = 108,
            PlayerLostHope = 109,
            PlayerLostFear = 110,
            PlayerLostSatisfaction = 111,
            PlayerLostRelief = 112,
            PlayerLostDisappointment = 113,
            PlayerLostPride = 114,
            PlayerLostAdmiration = 115,
            PlayerLostShame = 116,
            PlayerLostReproach = 117,
            PlayerLostLiking = 118,
            PlayerLostDisliking = 119,
            PlayerLostGratitude = 120,
            PlayerLostAnger = 121,
            PlayerLostGratification = 122,
            PlayerLostRemorse = 123,
            PlayerLostLove = 124,
            PlayerLostHate = 125,
            NoiseHeardBySomebodyElseJoy = 126,
            NoiseHeardBySomebodyElseDistress = 127,
            NoiseHeardBySomebodyElseResentment = 128,
            NoiseHeardBySomebodyElsePity = 129,
            NoiseHeardBySomebodyElseHope = 130,
            NoiseHeardBySomebodyElseFear = 131,
            NoiseHeardBySomebodyElseSatisfaction = 132,
            NoiseHeardBySomebodyElseRelief = 133,
            NoiseHeardBySomebodyElseDisappointment = 134,
            NoiseHeardBySomebodyElsePride = 135,
            NoiseHeardBySomebodyElseAdmiration = 136,
            NoiseHeardBySomebodyElseShame = 137,
            NoiseHeardBySomebodyElseReproach = 138,
            NoiseHeardBySomebodyElseLiking = 139,
            NoiseHeardBySomebodyElseDisliking = 140,
            NoiseHeardBySomebodyElseGratitude = 141,
            NoiseHeardBySomebodyElseAnger = 142,
            NoiseHeardBySomebodyElseGratification = 143,
            NoiseHeardBySomebodyElseRemorse = 144,
            NoiseHeardBySomebodyElseLove = 145,
            NoiseHeardBySomebodyElseHate = 146
        }
    }
}