using GiveOrTake.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GiveOrTake.Database
{
    [Table(nameof(User))]
    public class User
    {
        public User()
        {
            Items = new HashSet<Item>();
            Transactions = new HashSet<Transaction>();
        }

        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual RootAccess RootAccess { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {

                if (MiddleName == string.Empty)
                    return $"{FirstName} {LastName}";
                return $"{FirstName} {MiddleName} {LastName}";
            }
            set
            {
                var parts = value.Split(' ').ToList();
                if (parts.Count == 0)
                    throw new Exception($"{nameof(Name)} cannot be empty.");
                else
                {
                    if (parts.Count == 1)
                        FirstName = parts[0].Capitalize();
                    if (parts.Count == 2)
                        MiddleName = parts[1].Capitalize();
                    if (parts.Count >= 3)
                        LastName = parts[2].Capitalize();
                }
            }
        }

    }
}
