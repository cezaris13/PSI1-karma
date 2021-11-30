// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Karma.Models;
using Karma.Services;
using MatBlazor;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace KarmaTests
{
    public class DBServiceProviderTests
    {
        [Test]
        public void AddToDB_AddsVolunteer_ReturnsZero()
        {
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);
            notificationTransmitter
                .Setup(p => p.ShowMessage(It.IsAny<string>(), It.IsAny<MatToastType>()))
                .Verifiable();

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents1")
                .Options;

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var volunteer = new Volunteer("name", "surname", Guid.NewGuid());
                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, null);
                var result = sut.AddToDB(volunteer);

                Assert.AreEqual(0, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void AddToDB_NoSuchProperty_ReturnsMinusOne()
        {
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents2")
                .Options;

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, null);
                var result = sut.AddToDB(new WeatherForecastData());

                Assert.AreEqual(-1, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void RemoveFromDb_RemoveValidDBSetEntity_ReturnsZero()
        {
            var id = Guid.NewGuid();
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);
            notificationTransmitter
                .Setup(p => p.ShowMessage(It.IsAny<string>(), It.IsAny<MatToastType>()))
                .Verifiable();

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents4")
                .Options;
            using (var context = new KarmaContext(options))
            {
                context.Events.Add(new CharityEvent("Event1", "desc1", id, "manager1", "address1"));
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

                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, null);
                var result = sut.RemoveFromDB<CharityEvent>(id);

                Assert.AreEqual(0, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
            }
            using (var context = new KarmaContext(options))
            {
                Assert.AreEqual(2, context.Events.Select(p => p).Count());
            }
        }

        [Test]
        public void RemoveFromDb_NoSuchDBSet_ReturnsMinusOne()
        {
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents5")
                .Options;

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, null);
                var result = sut.RemoveFromDB<WeatherForecastData>(Guid.NewGuid());

                Assert.AreEqual(-1, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void UpdateDB_UpdatesVolunteer_ReturnsZero()
        {
            var id = Guid.NewGuid();
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);
            notificationTransmitter
                .Setup(p => p.ShowMessage(It.IsAny<string>(), It.IsAny<MatToastType>()))
                .Verifiable();

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents6")
                .Options;

            var objectChecker = new Mock<IObjectChecker>(MockBehavior.Strict);
            objectChecker
                .Setup(p => p.IsAnyNullOrEmpty(It.IsAny<object>()))
                .Returns(false);

            using (var context = new KarmaContext(options))
            {
                context.Volunteers.Add(new Volunteer("Name1", "Surname1", id));
                context.Volunteers.Add(new Volunteer("Name2", "Surname2", Guid.NewGuid()));
                context.Volunteers.Add(new Volunteer("Name3", "Surname3", Guid.NewGuid()));
                context.SaveChanges();
            }

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var volunteer = new Volunteer("name4", "surname4", id);
                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, objectChecker.Object);
                var result = sut.UpdateEntityInDB(volunteer);

                Assert.AreEqual(0, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
                objectChecker.VerifyAll();
            }
            using (var context = new KarmaContext(options))
            {
                Assert.AreEqual(3, context.Volunteers.Select(p => p).Count());
                Assert.AreEqual("name4", context.Volunteers.Where(p => p.Id == id).FirstOrDefault().Name);
                Assert.AreEqual("surname4", context.Volunteers.Where(p => p.Id == id).FirstOrDefault().Surname);
            }
        }

        [Test]
        public void UpdateDb_NotValidDBSet_ReturnsMinusOne()
        {
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents7")
                .Options;

            var objectChecker = new Mock<IObjectChecker>(MockBehavior.Strict);
            objectChecker
                .Setup(p => p.IsAnyNullOrEmpty(It.IsAny<object>()))
                .Returns(false);

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, objectChecker.Object);
                var result = sut.UpdateEntityInDB(new WeatherForecastData());

                Assert.AreEqual(-1, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
            }
        }

        [Test]
        public void UpdateDB_EmptyFields_ReturnsMinusTwo()
        {
            var notificationTransmitter = new Mock<INotificationTransmitter>(MockBehavior.Strict);
            notificationTransmitter
                .Setup(p => p.ShowMessage(It.IsAny<string>(), It.IsAny<MatToastType>()))
                .Verifiable();
            var options = new DbContextOptionsBuilder<KarmaContext>()
                .UseInMemoryDatabase(databaseName: "KarmaEvents8")
                .Options;

            var objectChecker = new Mock<IObjectChecker>(MockBehavior.Strict);
            objectChecker
                .Setup(p => p.IsAnyNullOrEmpty(It.IsAny<object>()))
                .Returns(true);

            using (var context = new KarmaContext(options))
            {
                var dbContextFactory = new Mock<IDbContextFactory<KarmaContext>>(MockBehavior.Strict);
                dbContextFactory
                    .Setup(p => p.CreateDbContext())
                    .Returns(context);

                var volunteer = new Volunteer("name", "surname", Guid.NewGuid());
                var sut = new DBServiceProvider(notificationTransmitter.Object, dbContextFactory.Object, objectChecker.Object);
                var result = sut.UpdateEntityInDB(volunteer);

                Assert.AreEqual(-2, result);
                notificationTransmitter.VerifyAll();
                dbContextFactory.VerifyAll();
                objectChecker.VerifyAll();
            }
        }
    }
}
