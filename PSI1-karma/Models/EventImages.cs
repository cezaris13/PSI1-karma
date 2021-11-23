// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Karma.Models
{
    public class EventImages
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public string ImageUrl { get; set; }

        public EventImages(Guid id, Guid eventId, string imageUrl)
        {
            Id = id;
            EventId = eventId;
            ImageUrl = imageUrl;
        }
    }
}
