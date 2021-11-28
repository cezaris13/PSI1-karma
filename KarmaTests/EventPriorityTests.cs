// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Karma.Models;
using Karma.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace KarmaTests
{
    public class EventPriorityTests
    {
        [Test]
        public void GetEventsNotOfThisVolunteer()
        {
            var options = new DbContextOptionsBuilder<KarmaContext>()
                   .UseInMemoryDatabase(databaseName: "KarmaEvents1")
                   .Options;

            using (var context = new KarmaContext(options))
            {
                context.Events.Add(new CharityEvent("Event1", "desc1", Guid.NewGuid(), "manager1", "address1"));
                context.Events.Add(new CharityEvent("Event2", "desc2", Guid.NewGuid(), "manager1", "address1"));
                context.Events.Add(new CharityEvent("Event3", "desc3", Guid.NewGuid(), "manager2", "address1"));
                context.SaveChanges();
            }

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new EventPriority(dbContextFactory.Object);
                var result = sut.GetEventsNotOfThisVolunteer("manager1");

                Assert.AreEqual(2, result.Count());
                CollectionAssert.AreEquivalent(result.Select(p => p.Name).ToList(), new List<string>() { "Event1", "Event2" });

                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void GetHighPriorityEvents()
        {
            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents2")
                .Options;

            using (var context = new KarmaContext(options))
            {
                context.Events.Add(new CharityEvent("Event1", "desc1", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 4,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                        new Volunteer("Name2", "Surname2", Guid.NewGuid()),
                        new Volunteer("NAme3", "Surname3", Guid.NewGuid())
                    }
                });
                context.Events.Add(new CharityEvent("Event2", "desc2", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 3,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                        new Volunteer("NAme3", "Surname3", Guid.NewGuid())
                    }

                });
                context.Events.Add(new CharityEvent("Event3", "desc3", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 4,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                    }
                });
                context.SaveChanges();
            }

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new EventPriority(dbContextFactory.Object);
                var result = sut.GetHighPriorityEvents("manager1");

                Assert.AreEqual(1, result.Count());
                CollectionAssert.AreEquivalent(result.Select(p => p.Name).ToList(), new List<string>() { "Event3" });

                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void GetRestOfEvents()
        {
            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents3")
                .Options;

            using (var context = new KarmaContext(options))
            {
                context.Events.Add(new CharityEvent("Event1", "desc1", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 4,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                        new Volunteer("Name2", "Surname2", Guid.NewGuid()),
                        new Volunteer("NAme3", "Surname3", Guid.NewGuid())
                    }
                });
                context.Events.Add(new CharityEvent("Event2", "desc2", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 3,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                        new Volunteer("NAme3", "Surname3", Guid.NewGuid())
                    }

                });
                context.Events.Add(new CharityEvent("Event3", "desc3", Guid.NewGuid(), "manager1", "address1")
                {
                    MaxVolunteers = 4,
                    Volunteers = new List<Volunteer>()
                    {
                        new Volunteer("Name1", "Surname1", Guid.NewGuid()),
                    }
                });
                context.SaveChanges();
            }

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new EventPriority(dbContextFactory.Object);
                var result = sut.GetRestOfEvents("manager1");

                Assert.AreEqual(2, result.Count());
                CollectionAssert.AreEquivalent(result.Select(p => p.Name).ToList(), new List<string>() { "Event1", "Event2" });

                dbContextFactory.VerifyAll();
            }
        }
    }
}
