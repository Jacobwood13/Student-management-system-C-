using System;

namespace StudentManagementSystem
{
    public class UserInterface
    {
        private readonly DatabaseManager _db;

        public UserInterface(DatabaseManager db)
        {
            _db = db;
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("   STUDENT MANAGEMENT SYSTEM");
                Console.WriteLine("=================================");
                Console.WriteLine("1. View All Students");
                Console.WriteLine("2. Add Student");
                Console.WriteLine("3. Search Student");
                Console.WriteLine("4. Update Student");
                Console.WriteLine("5. Delete Student");
                Console.WriteLine("6. Exit");
                Console.WriteLine("=================================");
                Console.Write("Choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": ShowViewStudentsPage(); break;
                    case "2": ShowAddStudentPage(); break;
                    case "3": ShowSearchStudentPage(); break;
                    case "4": ShowUpdateStudentPage(); break;
                    case "5": ShowDeleteStudentPage(); break;
                    case "6": running = false; break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void ShowViewStudentsPage()
        {
            Console.Clear();
            var students = _db.GetAllStudents();

            if (students.Count == 0)
            {
                Console.WriteLine("No students found.");
            }
            else
            {
                foreach (var s in students)
                {
                    Console.WriteLine($"ID: {s.StudentId} | Name: {s.Name} | Course: {s.Course} | Grade: {s.Grade}");
                }
            }
            PressEnterToContinue();
        }

        private void ShowAddStudentPage()
        {
            Console.Clear();
            if (!TryGetId(out int id)) return;

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Course: ");
            string course = Console.ReadLine() ?? "";
            Console.Write("Grade: ");
            char grade = (Console.ReadLine() ?? "F")[0];

            var student = new Student(id, name, course, grade);
            if (_db.InsertStudent(student))
                Console.WriteLine("Student added successfully.");

            PressEnterToContinue();
        }

        private void ShowSearchStudentPage()
        {
            Console.Clear();
            if (!TryGetId(out int id)) return;

            var student = _db.GetStudentById(id);
            if (student != null)
            {
                Console.WriteLine($"ID: {student.StudentId}\nName: {student.Name}\nCourse: {student.Course}\nGrade: {student.Grade}");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
            PressEnterToContinue();
        }

        private void ShowUpdateStudentPage()
        {
            Console.Clear();
            if (!TryGetId(out int id)) return;

            // Fetch current details to display dynamically
            var student = _db.GetStudentById(id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                PressEnterToContinue();
                return;
            }

            Console.Write($"New Name (current: {student.Name}): ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name)) name = student.Name;

            Console.Write($"New Course (current: {student.Course}): ");
            string course = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(course)) course = student.Course;

            Console.Write($"New Grade (current: {student.Grade}): ");
            string gradeInput = Console.ReadLine() ?? "";
            char grade = string.IsNullOrWhiteSpace(gradeInput) ? student.Grade : gradeInput[0];

            // Update local object data values
            student.Name = name;
            student.Course = course;
            student.Grade = grade;

            if (_db.UpdateStudent(student))
                Console.WriteLine("Student updated successfully.");
            else
                Console.WriteLine("Failed to update student.");

            PressEnterToContinue();
        }

        private void ShowDeleteStudentPage()
        {
            Console.Clear();
            if (!TryGetId(out int id)) return;

            if (_db.DeleteStudent(id))
                Console.WriteLine("Student deleted successfully.");
            else
                Console.WriteLine("Student not found.");

            PressEnterToContinue();
        }

        private bool TryGetId(out int id)
        {
            Console.Write("Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID structure.");
                PressEnterToContinue();
                return false;
            }
            return true;
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}