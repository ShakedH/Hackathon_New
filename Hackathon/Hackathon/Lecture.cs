using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Lecture
    {
        private string m_Faculty;

        public string Faculty
        {
            get { return m_Faculty; }
            set { m_Faculty = value; }
        }

        private string m_courseNumber;

        public string CourseNumber
        {
            get { return CourseNumber; }
            set { CourseNumber = value; }
        }

        private string m_lecturer;

        public string Lecturer
        {
            get { return m_lecturer; }
            set { m_lecturer = value; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            Lecture other = (Lecture)obj;
            return Lecturer.Equals(other.Lecturer) &&
                Faculty.Equals(other.Faculty) &&
                CourseNumber.Equals(other.CourseNumber);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
    }
}
