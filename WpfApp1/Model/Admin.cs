using project_machshevon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectRoniel.Model
{
    public class Admin : User
    {
        private DateTime dateOfHire;
        private decimal salary;

        public DateTime DateOfHire
        {
            get { return dateOfHire; }
            set
            {
                if (dateOfHire != value)
                {
                    dateOfHire = value;
                    OnPropertyChanged("DateOfHire");
                }
            }
        }

        public decimal Salary
        {
            get { return salary; }
            set
            {
                if (salary != value)
                {
                    salary = value;
                    OnPropertyChanged("Salary");
                }
            }
        }
    }
}
