// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Karma.Models;
using MatBlazor;
using NUnit.Framework;

namespace KarmaTests.Integration
{
    class Crud
    {
        private Volunteer testVolunteer = new Volunteer("John", "Smith", Guid.Parse("16842fce-a185-40ab-a657-b3daa1e2af30"));
        private Volunteer testVolunteer2 = new Volunteer("John", "Smith2", Guid.Parse("16842fce-a185-40ab-a657-b3daa1e2af31"));

        [SetUp]
        public void SetUp()
        {
            DeleteVolunteers();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DeleteVolunteers();
        }

        [Test]
        public void ShowMessageTest2(
            [Values(null, "", "SingleWord", "Many different words.? !; and stuff")] string message,
            [Values(MatToastType.Info, MatToastType.Danger, MatToastType.Success)] MatToastType type)
        {
            var db = new KarmaContext();
            TestVolunteer(db, testVolunteer.Id, null);
            TestVolunteer(db, testVolunteer2.Id, null);
            db.Volunteers.Add(testVolunteer);
            db.SaveChanges();
            TestVolunteer(db, testVolunteer.Id, testVolunteer);
            db.Volunteers.Add(testVolunteer2);
            db.SaveChanges();
            TestVolunteer(db, testVolunteer.Id, testVolunteer);
            TestVolunteer(db, testVolunteer2.Id, testVolunteer2);
            db.Volunteers.Remove(testVolunteer);
            db.SaveChanges();
            TestVolunteer(db, testVolunteer.Id, null);
            TestVolunteer(db, testVolunteer2.Id, testVolunteer2);
        }

        private void DeleteVolunteers()
        {
            var db = new KarmaContext();
            db.Volunteers.Remove(testVolunteer);
            db.Volunteers.Remove(testVolunteer2);
            db.SaveChanges();
        }

        private void TestVolunteer(KarmaContext db, Guid Id, Volunteer actual)
        {
            var volunteer = db.Volunteers.FirstOrDefault(x => x.Id == Id);
            Assert.AreEqual(null, actual);
        }
    }
}
