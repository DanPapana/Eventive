using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Dtos;
using Eventive.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;

namespace Eventive.ApplicationLogic.Services
{
    public class EventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IUserRepository userRepository;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository)
        {
            this.eventRepository = eventRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<EventOrganized> GetActiveEvents(Guid? participantId = null)
        {
            return eventRepository.GetActiveEvents(participantId);
        }

        public IEnumerable<EventOrganized> GetActiveEvents(string category, Guid? participantId = null)
        {
            if (string.IsNullOrEmpty(category))
            {
                return eventRepository.GetActiveEvents(participantId);
            }

            return eventRepository.GetActiveEvents((EventCategory) Enum.Parse(typeof(EventCategory), category), participantId);
        }

        public EventOrganized GetEventById(Guid eventId)
        {            
            var eventOrganized = eventRepository.GetEventById(eventId);

            if (eventOrganized is null)
            {
                throw new EntityNotFoundException(eventId);
            }

            return eventOrganized;
        }

        public EventFollowing FollowEvent(Guid eventId, Guid participantId)
        {
            var eventOrganized = eventRepository.GetEventById(eventId);
            var participant = userRepository.GetParticipantByGuid(participantId);

            EventFollowing following = EventFollowing.Create(eventOrganized, participant);
            eventRepository.AddInteraction(following);
            return following;
        }

        public EventApplication ApplyToEvent(Guid eventId, Guid participantId, string applicationText = null)
        {
            var eventOrganized = eventRepository.GetEventById(eventId);
            var participant = userRepository.GetParticipantByGuid(participantId);

            EventApplication application = EventApplication.Create(eventOrganized, participant, applicationText);
            eventRepository.AddInteraction(application);
            return application;
        }

        public EventApplication GetApplication(Guid eventId, Guid participantId)
        {
            return eventRepository.GetApplication(eventId, participantId);
        }

        public EventFollowing GetFollowing(Guid eventId, Guid participantId)
        {
            return eventRepository.GetFollowing(eventId, participantId);
        }

        public string GetApplicationText(Guid eventId, Guid participantId)
        {
            return eventRepository.GetUserApplicationText(eventId, participantId);
        }

        public IEnumerable<EventOrganized> GetAppliedEventsForUser(Guid participantId)
        {
            return eventRepository.GetAppliedEventsForUser(participantId);
        }

        public IEnumerable<EventOrganized> GetFollowedEventsForUser(Guid participantId)
        {
            return eventRepository.GetFollowedEventsForUser(participantId);
        }

        public IEnumerable<EventOrganized> GetPastEventsOfUser(Guid participantId)
        {
            return eventRepository.GetPastAppliedEvents(participantId);
        }

        public IEnumerable<Guid> GetAppliedEventsGuidForUser(Guid participantId)
        {
            return eventRepository.GetAppliedEventsGuidForUser(participantId);
        }

        public IEnumerable<Guid> GetFollowedEventsGuidForUser(Guid participantId)
        {
            return eventRepository.GetFollowedEventsGuidForUser(participantId);
        }

        public IEnumerable<Comment> GetEventComments(Guid eventId)
        {
            return eventRepository.GetEventComments(eventId);
        }

        public IEnumerable<EventClick> GetEventClicks(Guid eventId, Guid participantId)
        {
            return eventRepository.GetClicks(eventId, participantId);
        }

        public IEnumerable<EventRating> GetEventRatings(Guid eventId)
        {
            return eventRepository.GetEventRatings(eventId);
        }

        public IEnumerable<EventRating> GetUserRatings(Guid participantId)
        {
            return eventRepository.GetUserRatings(participantId);
        }


        public IEnumerable<EventApplication> GetEventApplications(Guid eventId)
        {
            return eventRepository.GetEventApplications(eventId);
        }
        public IEnumerable<EventFollowing> GetEventFollowings(Guid eventId)
        {
            return eventRepository.GetEventFollowings(eventId);
        }

        public IEnumerable<EventClick> GetEventClicks(Guid eventId)
        {
            return eventRepository.GetEventClicks(eventId);
        }

        public EventRating GetUserRating(Guid eventId, Guid participantId)
        {
            return eventRepository.GetUserRating(eventId, participantId);
        }

        private double GetInteractionDecay(IEnumerable<IEventInteraction> interactions)
        {
            double gravity = double.Parse(ConfigurationManager.AppSettings.Get("TrendingGravity"));
            double decay = 0;
            foreach (var interaction in interactions)
            {
                double days = (DateTime.UtcNow - interaction.Timestamp).TotalDays;
                decay += days * gravity;
            }
            return decay;
        }

        private double GetInteractionScore(IEnumerable<IEventInteraction> interactions, double interactionWeight)
        {
            var interactionDecay = GetInteractionDecay(interactions);
            var interactionCount = interactions.ToList().Count;

            return interactionCount * interactionWeight / (interactionDecay / (interactionCount + 1));
        }

        private double GetTotalTrendingScoreForAnEvent(Guid eventId)
        {

            double applicationWeight = double.Parse(ConfigurationManager.AppSettings.Get("TrendingApplicationWeight"));
            double followWeight = double.Parse(ConfigurationManager.AppSettings.Get("TrendingFollowWeight"));
            double commentWeight = double.Parse(ConfigurationManager.AppSettings.Get("TrendingCommentWeight"));
            double clickWeight = double.Parse(ConfigurationManager.AppSettings.Get("TrendingClickWeight"));

            var applicationScore = GetInteractionScore(GetEventApplications(eventId), applicationWeight);
            var followingScore = GetInteractionScore(GetEventFollowings(eventId), followWeight);
            var commentScore = GetInteractionScore(GetEventComments(eventId), commentWeight);
            var clickScore = GetInteractionScore(GetEventClicks(eventId), clickWeight);

            return applicationScore + followingScore + commentScore + clickScore;
        }

        private IEnumerable<EventOrganized> GetEventsOrderedByScore(IEnumerable<EventOrganized> eventsToScore)
        {
            Dictionary<EventOrganized, double> eventScore = new Dictionary<EventOrganized, double>();
            double maximumNumberOfTrendingEvents = double.Parse(ConfigurationManager.AppSettings.Get("NumberOfTrendingEventsShown"));

            foreach (EventOrganized eventOrganized in eventsToScore)
            {
                double trendingScore = GetTotalTrendingScoreForAnEvent(eventOrganized.Id);
                eventScore.Add(eventOrganized, trendingScore);
            }

            var orderedScores = eventScore
                .OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            List<EventOrganized> trendingEvents = new List<EventOrganized>();

            if (maximumNumberOfTrendingEvents > orderedScores.Count())
            {
                maximumNumberOfTrendingEvents = orderedScores.Count();
            }

            for (int i = 0; i < maximumNumberOfTrendingEvents; i++)
            {
                trendingEvents.Add(orderedScores.ElementAt(i).Key);
            }

            return trendingEvents.AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetTrendingEvents(string category, Guid? participantId = null)
        {
            IEnumerable<EventOrganized> events = GetActiveEvents(category, participantId);
            if (events.Count() > 0)
            {
                return GetEventsOrderedByScore(events);
            }

            return events;
        }

        /*
         * Calculate a score from 0.0 to 1.0
         * 1.0 meaning the distance from the user's current location is 0
         * 0.0 meaning the distance is > 1000 km 
         */
        public async Task<double> GetEventProximityScoreForUserAsync(string category, Guid participantId, string latitude, string longitude)
        {
            double defaultScore = 0.5;
            if (string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(longitude))
            {
                return defaultScore;
            }

            double userLatitude = Convert.ToDouble(latitude);
            double userLongitude = Convert.ToDouble(longitude);
            Dictionary<EventOrganized, double> distanceToEventInMeters = new Dictionary<EventOrganized, double>();

            IEnumerable<EventOrganized> events = GetActiveEvents(category, participantId);
            string destinations = string.Empty;
            foreach (var eventOrganized in events)
            {
                destinations += string.Format("{0},{1}|", eventOrganized.EventDetails.Latitude, eventOrganized.EventDetails.Longitude);
            }

            if (!string.IsNullOrEmpty(destinations))
            {
                string apiKey = ConfigurationManager.AppSettings.Get("GOOGLE_API_KEY");
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0},{1}&destinations={2}&key={3}",
                    userLatitude, userLongitude, destinations, apiKey);

                using var client = new HttpClient();
                var request = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

                if (request.IsSuccessStatusCode && request.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var responseObject = request.Content.ReadAsAsync<DistanceResponse>();
                }
            }

            return 2;
        }

        public Comment AddComment(Guid creatorId, Guid eventId, string message)
        {
            var participant = userRepository.GetParticipantByGuid(creatorId);
            var eventOrganized = eventRepository.GetEventById(eventId);

            if (participant is null || eventOrganized is null)
            {
                return null;
            }

            var comment = Comment.Create(participant, eventOrganized, message);
            eventRepository.AddInteraction(comment);
            return comment;
        }

        public EventRating AddRating(Guid participantId, Guid eventId, string score)
        {
            int ratingScore = int.Parse(score);
            var participant = userRepository.GetParticipantByGuid(participantId);
            var eventOrganized = eventRepository.GetEventById(eventId);

            if (participant is null || eventOrganized is null)
            {
                return null;
            }

            var rating = EventRating.Create(eventOrganized, participant, ratingScore);
            eventRepository.AddInteraction(rating);
            return rating;
        }

        public EventOrganized UpdateEvent(Guid eventId, string title,
                    EventCategory category,
                    string description,
                    string location,
                    double? latitude,
                    double? longitude,
                    string city,
                    DateTime deadline,
                    DateTime occurenceDate,
                    string image,
                    int maximumParticipants,
                    decimal fee,
                    bool applicationRequired)
        {

            var eventToUpdate = GetEventById(eventId);

            eventToUpdate.UpdateEvent(title, category, description, location, latitude, longitude, city, deadline, occurenceDate,
                                                image, maximumParticipants, fee, applicationRequired);
            eventRepository.Update(eventToUpdate);

            return eventToUpdate;
        }

        public bool RemoveInteraction(IEventInteraction eventInteraction)
        {
            return eventRepository.RemoveInteraction(eventInteraction);
        }

        public bool RemoveEvent(string eventId)
        {
            Guid.TryParse(eventId, out Guid eventGuid);
            return eventRepository.RemoveEvent(eventGuid);
        }

        public Comment GetCommentById(string commentId)
        {
            Guid.TryParse(commentId, out Guid commentGuid);
            return eventRepository.GetCommentById(commentGuid);
        }

        public EventClick RegisterClick(Guid eventId, Guid participantId)
        {
            var commenter = userRepository.GetParticipantByGuid(participantId);
            var eventToRegisterFor = eventRepository.GetEventById(eventId);

            if (commenter is null || eventToRegisterFor is null)
            {
                return null;
            }

            var newClick = EventClick.Create(eventToRegisterFor, commenter);
            eventRepository.AddInteraction(newClick);
            return newClick;
        }

        public string FormatParticipationFee(decimal participationFee)
        {
            string formattedFee = participationFee.ToString("#.##");

            if (formattedFee.Length > 0)
            {
                return $"${formattedFee}";
            }

            return "None";
        }

        public string FormatMaximumParticipants(int maximumParticipants)
        {
            if (maximumParticipants > 0)
            {
                return $"{maximumParticipants}";
            }

            return "None";
        }

        public string FormatEventDate(DateTime eventDate)
        {
            return $"{eventDate:dd/MM/yyyy}";
        }

        public string FormatEventTime(DateTime eventTime)
        {
            return $"{eventTime:H:mm}";
        }

        public string FormatUserName(Participant hostingUser)
        {
            return $"{hostingUser.FirstName} {hostingUser.LastName}";
        }
    }
}
