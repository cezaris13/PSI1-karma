using System.Collections.Generic;
using System.Linq;
using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.Services
{
    public class EventPriority : IEventPriority
    {
        private IDbContextFactory<KarmaContext> m_karmaContextFactory { get; set; }

        public EventPriority(IDbContextFactory<KarmaContext> karmaContextFactory)
        {
            m_karmaContextFactory = karmaContextFactory;
        }

        public IEnumerable<ICharityEvent> GetEventsNotOfThisVolunteer(string currentUserId)
        {
            KarmaContext m_karmaContext = m_karmaContextFactory.CreateDbContext();
            return m_karmaContext.Events.Include(p => p.Volunteers).Where(p => p.ManagerId == currentUserId && p.MaxVolunteers > p.Volunteers.Count);
        }

        public IEnumerable<ICharityEvent> GetHighPriorityEvents(string currentUserId)
        {
            var allEvents = GetEventsNotOfThisVolunteer(currentUserId);
            return allEvents.Aggregate(new List<ICharityEvent>(), (list, currentEvent) =>
            {
                if ((currentEvent.MaxVolunteers / 2) > currentEvent.Volunteers.Count)
                {
                    list.Add(currentEvent);
                }
                return list;
            });
        }

        public IEnumerable<ICharityEvent> GetRestOfEvents(string currentUserId)
        {
            List<ICharityEvent> list1 = GetEventsNotOfThisVolunteer(currentUserId).ToList();
            List<ICharityEvent> list2 = GetHighPriorityEvents(currentUserId).ToList();
            var result = list1.Where(p => !list2.Any(x => x.Id == p.Id && x.Name == p.Name)).ToList();
            return result;
        }
    }
}
