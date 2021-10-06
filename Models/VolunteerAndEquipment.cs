// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Karma.Models
{
    public struct VolunteerAndEquipment
    {
        public string VolunteerName { get; set; }
        public string VolunteerSurname { get; set; }
        public IEnumerable<string> EquipmentList { get; set; }
        public VolunteerAndEquipment(string name, string surname, IEnumerable<string> equipment)
        {
            VolunteerName = name;
            VolunteerSurname = surname;
            EquipmentList = equipment;
        }
    }
    public static class VolunteerAndEquipmentExtension
    {
        public static List<VolunteerAndEquipment> OrderByEquipmentCount(this List<VolunteerAndEquipment> list)
        {
            return list.OrderByDescending(pair => pair.EquipmentList.Count()).ToList();
        }
    }
}
