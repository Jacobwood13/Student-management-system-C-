using Microsoft.Data.Sqlite;

string connectionString = "Data Source=students.db";

var connection = new SqliteConnection(connectionString);

connection.Open();

CreateStudentTable(connection);

// creates a command to create a table in the database
void CreateStudentTable(SqliteConnection connection)
{
    var command = connection.CreateCommand();

    command.CommandText = @"
    CREATE TABLE IF NOT EXISTS Students (
        StudentId INTEGER,
        Name TEXT,
        Course TEXT,
        Grade TEXT
    )";

    command.ExecuteNonQuery();
}

// creates a command to insert a student into the database
void InsertStudent(SqliteConnection connection, int studentId, string name, string course, char grade)
{
    var command = connection.CreateCommand();

     command.CommandText = @"
    INSERT INTO Students
    (StudentId, Name, Course, Grade)
    VALUES ($studentId, $name, $course, $grade)";

    command.Parameters.AddWithValue("$studentId", studentId);
    command.Parameters.AddWithValue("$name", name);
    command.Parameters.AddWithValue("$course", course);
    command.Parameters.AddWithValue("$grade", grade);

    command.ExecuteNonQuery();

}

//creates a method to view all student info in the database
void ViewAllStudents(SqliteConnection connection)
{
    var command = connection.CreateCommand();

    command.CommandText = @"
    SELECT StudentId, Name, Course, Grade
    FROM Students";

    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            int studentId = reader.GetInt32(0);
            string name = reader.GetString(1);
            string course = reader.GetString(2);
            string grade = reader.GetString(3);

            Console.WriteLine($"Student ID: {studentId}, Name: {name}, Course: {course}, Grade: {grade}");
        }
    }
}

void searchStudentById(SqliteConnection connection, int studentId)
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
            string name = reader.GetString(1);
            string course = reader.GetString(2);
            string grade = reader.GetString(3);

            Console.WriteLine($"Student ID: {studentId}, Name: {name}, Course: {course}, Grade: {grade}");
        }
        else
        {
            Console.WriteLine($"No student found with ID: {studentId}");
        }
    }
}

void updateStudentInformation(SqliteConnection connection, int studentId, string name, string course, char grade)
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
    command.Parameters.AddWithValue("$grade", grade);

    int rowsAffected = command.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine($"Student with ID: {studentId} updated successfully.");
    }
    else
    {
        Console.WriteLine($"No student found with ID: {studentId}");
    }
}

void deleteStudent(SqliteConnection connection, int studentId)
{
    var command = connection.CreateCommand();

    command.CommandText = @"
    DELETE FROM Students
    WHERE StudentId = $studentId";

    command.Parameters.AddWithValue("$studentId", studentId);

    int rowsAffected = command.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine($"Student with ID: {studentId} deleted successfully.");
    }
    else
    {
        Console.WriteLine($"No student found with ID: {studentId}");
    }
}