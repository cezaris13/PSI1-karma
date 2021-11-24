using System;
using System.Collections.Generic;
using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.Services
{
    public interface IEventPriority
    {
        IEnumerable<ICharityEvent> GetEventsNotOfThisVolunteer(string currentUserId);

        IEnumerable<ICharityEvent> GetHighPriorityEvents(string currentUserId);

        IEnumerable<ICharityEvent> GetRestOfEvents(string currentUserId);
    }
}
