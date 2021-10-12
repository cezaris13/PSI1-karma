using System;
using System.Collections.Generic;

namespace Karma.Models
{
    public class CharityEvent : ICharityEvent, IComparable
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CharityEventState State { get; set; }

        public List<Volunteer> Volunteers { get; set; }

        public string ManagerId { get; set; }

        public CharityEvent(string name, string description, Guid id, string managerId, CharityEventState state = CharityEventState.Undefined)
        {
            Id = id;
            Name = name;
            Description = description;
            State = state;
            Volunteers = new List<Volunteer>();
            ManagerId = managerId;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            CharityEvent tempCharityEvent = obj as CharityEvent;
            if(tempCharityEvent == null)
                throw new ArgumentException("Object is not a Charity event");
            else
            {
                int compareResult= Name.CompareTo(tempCharityEvent.Name);
                if (compareResult!= 0)
                {
                    return compareResult;
                }
                else
                {
                    return Description.CompareTo(tempCharityEvent.Description);
                }
            }

        }
    }
}
