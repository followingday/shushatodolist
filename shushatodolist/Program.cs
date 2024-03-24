using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TodoApp
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=tasks.db");
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new TodoContext())
            {
                db.Database.EnsureCreated();

                while (true)
                {
                    Console.WriteLine("TODO List");
                    Console.WriteLine("1. Add Task");
                    Console.WriteLine("2. Delete Task");
                    Console.WriteLine("3. Mark Task as Completed");
                    Console.WriteLine("4. List Tasks");
                    Console.WriteLine("5. Mark All Tasks as Completed");
                    Console.WriteLine("6. Delete All Tasks");
                    Console.WriteLine("7. Exit");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddTask(db);
                            break;
                        case "2":
                            DeleteTask(db);
                            break;
                        case "3":
                            MarkAsCompleted(db);
                            break;
                        case "4":
                            ListTasks(db);
                            break;
                        case "5":
                            MarkAllTasksCompleted(db);
                            break;
                        case "6":
                            DeleteAllTasks(db);
                            break;
                        case "7":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        static void AddTask(TodoContext db)
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();
            var task = new TaskItem { Description = description, IsCompleted = false };
            db.Tasks.Add(task);
            db.SaveChanges();
            Console.WriteLine("Task added successfully.");
        }

        static void DeleteTask(TodoContext db)
        {
            Console.Write("Enter task ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var task = db.Tasks.Find(id);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                    Console.WriteLine("Task deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Please enter a valid number.");
            }
        }

        static void MarkAsCompleted(TodoContext db)
        {
            Console.Write("Enter task ID to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var task = db.Tasks.Find(id);
                if (task != null)
                {
                    task.IsCompleted = true;
                    db.SaveChanges();
                    Console.WriteLine("Task marked as completed.");
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Please enter a valid number.");
            }
        }

        static void ListTasks(TodoContext db)
        {
            var tasks = db.Tasks.ToList();
            if (tasks.Any())
            {
                Console.WriteLine("Tasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"- ID: {task.Id} [{(task.IsCompleted ? "X" : " ")}] {task.Description}");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }

        static void MarkAllTasksCompleted(TodoContext db)
        {
            var tasks = db.Tasks.ToList();
            foreach (var task in tasks)
            {
                task.IsCompleted = true;
            }
            db.SaveChanges();
            Console.WriteLine("All tasks marked as completed.");
        }

        static void DeleteAllTasks(TodoContext db)
        {
            db.Tasks.RemoveRange(db.Tasks);
            db.SaveChanges();
            Console.WriteLine("All tasks deleted successfully.");
        }
    }
}
