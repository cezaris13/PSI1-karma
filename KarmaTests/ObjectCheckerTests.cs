// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Services;
using NUnit.Framework;

namespace KarmaTests
{
    public class ObjectCheckerTests
    {
        [Test]
        public void IsAnyNullOrEmptyTest_TypeContainsNullsAndEmpty_ReturnsTrue()
        {
            var obj = new Volunteer("", "", Guid.NewGuid());
            var sut = new ObjectChecker();
            bool result = sut.IsAnyNullOrEmpty(obj);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsAnyNullOrEmptyTest_TypeDoesNotContainNullsAndEmpty_ReturnsFalse()
        {
            var obj = new Volunteer("name", "surname", Guid.NewGuid());
            var sut = new ObjectChecker();
            bool result = sut.IsAnyNullOrEmpty(obj);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsAnyNullOrEmptyTest_TypeWithoutStrings_ReturnsFalse()
        {
            var obj = new TypeWithoutStrings();
            var sut = new ObjectChecker();
            bool result = sut.IsAnyNullOrEmpty(obj);

            Assert.AreEqual(false, result);
        }

        private class TypeWithoutStrings
        {
            public int First { get; set; }
            public Guid Second { get; set; }
            public bool Third { get; set; }
        }
    }
}
