namespace StudentManagementSystem
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public char Grade { get; set; }

        // Empty constructor required for object creation
        public Student() { }

        // Convenient constructor to build a student quickly
        public Student(int id, string name, string course, char grade)
        {
            StudentId = id;
            Name = name;
            Course = course;
            Grade = grade;
        }
    }
}