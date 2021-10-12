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
        private string[] equipmentList { get; set; }

        public string this[int index]
        {
            get
            {
                return equipmentList[index];
            }
        }
        public long Size
        {
            get { return equipmentList.Count(); }
        }
        public VolunteerAndEquipment(string name, string surname, IEnumerable<string> equipment)
        {
            VolunteerName = name;
            VolunteerSurname = surname;
            equipmentList = equipment.ToArray();
        }
    }
    public static class VolunteerAndEquipmentExtension
    {
        public static List<VolunteerAndEquipment> OrderByEquipmentCount(this List<VolunteerAndEquipment> list)
        {
            return list.OrderByDescending(pair => pair.Size).ToList();
        }
    }
}
