using Karma.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public string GetEvents(string address)
        {
            KarmaContext KarmaContext = new KarmaContext();
            List<CharityEvent> localEvents = new List<CharityEvent>();
            foreach (var karmaEvent in KarmaContext.Events.ToList())
            {
                var regex = new Regex($"(.*){address}(.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matches = regex.Matches(karmaEvent.Address);
                if (matches.Count > 0)
                {
                    localEvents.Add(karmaEvent);
                }
            }
            return JsonConvert.SerializeObject(localEvents);
        }
    }
}
