// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Karma.Models;

namespace Karma.Pages
{
    public partial class SpecialEquipment
    {
        private KarmaContext m_karmaKontext { get; } = new KarmaContext();

        private IEnumerable<VolunteerAndEquipment> GetPairsOfVolunteersAndEquipment()
        {
            var volunteers = m_karmaKontext.Volunteers.ToList();
            var equipment = m_karmaKontext.SpecialEquipment.ToList();
            return volunteers
                .GroupJoin(
                    equipment,
                    owner => owner,
                    eq => eq.Owner,
                    (owner, collection) => new VolunteerAndEquipment(owner.Name, owner.Surname, collection.Select(x => x.Name)))
                .ToList()
                .OrderByEquipmentCount();
        }
    }
}
