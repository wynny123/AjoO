using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AjoO.Model
{
    public class Account
    {
        public DateTime Date { get; set; }
        public int Id {get; set;}
        public int Contribution { get; set; }
        public int Loan { get; set; }
        public int Balance { get; set; }


        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}