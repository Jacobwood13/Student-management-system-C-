
using System;
using Microsoft.Data.Sqlite;

namespace StudentManagementSystem
{
    class Program
    {
        private static readonly string ConnectionString = "Data Source=students.db";

        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                CreateStudentTable(connection);
            }

            Run();
        }

        static void Run()
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
                    case "1":
                        ShowViewStudentsPage();
                        break;

                    case "2":
                        ShowAddStudentPage();
                        break;

                    case "3":
                        ShowSearchStudentPage();
                        break;

                    case "4":
                        ShowUpdateStudentPage();
                        break;

                    case "5":
                        ShowDeleteStudentPage();
                        break;

                    case "6":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ShowViewStudentsPage()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                ViewAllStudents(connection);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void ShowAddStudentPage()
        {
            Console.Clear();

            Console.Write("Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadLine();
                return;
            }

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Course: ");
            string course = Console.ReadLine() ?? "";

            Console.Write("Grade: ");
            char grade = (Console.ReadLine() ?? "F")[0];

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                InsertStudent(connection, studentId, name, course, grade);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void ShowSearchStudentPage()
        {
            Console.Clear();

            Console.Write("Enter Student ID: ");

            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                SearchStudentById(connection, studentId);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void ShowUpdateStudentPage()
        {
            Console.Clear();

            Console.Write("Student ID: ");

            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadLine();
                return;
            }

            Console.Write("New Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("New Course: ");
            string course = Console.ReadLine() ?? "";

            Console.Write("New Grade: ");
            char grade = (Console.ReadLine() ?? "F")[0];

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                UpdateStudentInformation(connection, studentId, name, course, grade);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void ShowDeleteStudentPage()
        {
            Console.Clear();

            Console.Write("Student ID to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                DeleteStudent(connection, studentId);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void CreateStudentTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                StudentId INTEGER PRIMARY KEY,
                Name TEXT,
                Course TEXT,
                Grade TEXT
            )";

            command.ExecuteNonQuery();
        }

        static void InsertStudent(SqliteConnection connection,
            int studentId,
            string name,
            string course,
            char grade)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
            INSERT INTO Students
            (StudentId, Name, Course, Grade)
            VALUES
            ($studentId, $name, $course, $grade)";

            command.Parameters.AddWithValue("$studentId", studentId);
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$course", course);
            command.Parameters.AddWithValue("$grade", grade.ToString());

            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Student added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ViewAllStudents(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT StudentId, Name, Course, Grade FROM Students";

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("No students found.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"ID: {reader.GetInt32(0)} | " +
                        $"Name: {reader.GetString(1)} | " +
                        $"Course: {reader.GetString(2)} | " +
                        $"Grade: {reader.GetString(3)}");
                }
            }
        }

        static void SearchStudentById(
            SqliteConnection connection,
            int studentId)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
            SELECT StudentId, Name, Course, Grade
            FROM Students
            WHERE StudentId = $studentId";

            command.Parameters.AddWithValue("$studentId", studentId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetInt32(0)}");
                    Console.WriteLine($"Name: {reader.GetString(1)}");
                    Console.WriteLine($"Course: {reader.GetString(2)}");
                    Console.WriteLine($"Grade: {reader.GetString(3)}");
                }
                else
                {
                    Console.WriteLine("Student not found.");
                }
            }
        }

        static void UpdateStudentInformation(
            SqliteConnection connection,
            int studentId,
            string name,
            string course,
            char grade)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
            UPDATE Students
            SET Name = $name,
                Course = $course,
                Grade = $grade
            WHERE StudentId = $studentId";

            command.Parameters.AddWithValue("$studentId", studentId);
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$course", course);
            command.Parameters.AddWithValue("$grade", grade.ToString());

            int rows = command.ExecuteNonQuery();

            Console.WriteLine(
                rows > 0
                    ? "Student updated successfully."
                    : "Student not found.");
        }

        static void DeleteStudent(
            SqliteConnection connection,
            int studentId)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
            DELETE FROM Students
            WHERE StudentId = $studentId";

            command.Parameters.AddWithValue("$studentId", studentId);

            int rows = command.ExecuteNonQuery();

            Console.WriteLine(
                rows > 0
                    ? "Student deleted successfully."
                    : "Student not found.");
        }
    }
}




