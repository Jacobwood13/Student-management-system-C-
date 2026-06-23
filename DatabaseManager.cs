using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace StudentManagementSystem
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
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

        public bool InsertStudent(Student student)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Students (StudentId, Name, Course, Grade)
                VALUES ($studentId, $name, $course, $grade)";

            command.Parameters.AddWithValue("$studentId", student.StudentId);
            command.Parameters.AddWithValue("$name", student.Name);
            command.Parameters.AddWithValue("$course", student.Course);
            command.Parameters.AddWithValue("$grade", student.Grade.ToString());

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT StudentId, Name, Course, Grade FROM Students";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Course = reader.GetString(2),
                    Grade = reader.GetString(3)[0]
                });
            }
            return students;
        }

        public Student? GetStudentById(int studentId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT StudentId, Name, Course, Grade FROM Students WHERE StudentId = $studentId";
            command.Parameters.AddWithValue("$studentId", studentId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Student
                {
                    StudentId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Course = reader.GetString(2),
                    Grade = reader.GetString(3)[0]
                };
            }
            return null;
        }

        public bool UpdateStudent(Student student)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Students
                SET Name = $name, Course = $course, Grade = $grade
                WHERE StudentId = $studentId";

            command.Parameters.AddWithValue("$studentId", student.StudentId);
            command.Parameters.AddWithValue("$name", student.Name);
            command.Parameters.AddWithValue("$course", student.Course);
            command.Parameters.AddWithValue("$grade", student.Grade.ToString());

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteStudent(int studentId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Students WHERE StudentId = $studentId";
            command.Parameters.AddWithValue("$studentId", studentId);

            return command.ExecuteNonQuery() > 0;
        }
    }
}