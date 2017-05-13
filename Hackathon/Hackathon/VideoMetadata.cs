using System;

namespace Hackathon
{
    [Serializable]
    public class VideoMetadata
    {
        public string Faculty { get; private set; }
        public string Department { get; private set; }
        public string Course { get; private set; }
        public string Lecturer { get; private set; }

        public VideoMetadata(string faculty, string department, string course, string lecturer)
        {
            this.Faculty = faculty;
            this.Department = department;
            this.Course = course;
            this.Lecturer = lecturer;
        }
    }
}