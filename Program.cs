using System;

namespace StudentManagementSystem
{
    internal class StudentManagementApp
    {
        private const string ConnectionString = "Data Source=students.db";

        [STAThread]
        static void Main(string[] args)
        {
            // Creating the database manager assembly unit
            DatabaseManager dbManager = new DatabaseManager(ConnectionString);

            // Injecting the database manager dependency unit into UI controller
            UserInterface ui = new UserInterface(dbManager);

            // Now safely run application processing flow loops
            ui.Run();
        }
    }
}