using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalOdevApp.Model
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class FirestoreService
    {
        private FirestoreDb _firestoreDb;

        public FirestoreService()
        {
            // Firebase konsolundan indirdiğiniz json dosyasının yolunu belirtin
            string filepath = Path.Combine(AppContext.BaseDirectory,"Resources","mauiapp-1bbc9-firebase-adminsdk-z2bys-a60d341b0c.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            _firestoreDb = FirestoreDb.Create("mauiapp-1bbc9");
        }
        public async Task<List<TodoItem>> GetTodosAsync()
        {
            var todoCollection = _firestoreDb.Collection("todos");
            var snapshot = await todoCollection.OrderBy("CreatedAt").GetSnapshotAsync();

            return snapshot.Documents.Select(doc => new TodoItem
            {
                Id = doc.Id,
                Title = doc.GetValue<string>("Title"),
                Description = doc.GetValue<string>("Description"),
                IsCompleted = doc.GetValue<bool>("IsCompleted"),
                CreatedAt = doc.GetValue<DateTime>("CreatedAt")
            }).ToList();
        }

        public async Task AddTodoAsync(TodoItem todo)
        {
            var todoCollection = _firestoreDb.Collection("todos");
            await todoCollection.AddAsync(new Dictionary<string, object>
        {
            { "Title", todo.Title },
            { "Description", todo.Description },
            { "IsCompleted", todo.IsCompleted },
            { "CreatedAt", DateTime.UtcNow }
        });
        }

        public async Task UpdateTodoAsync(TodoItem todo)
        {
            var todoDoc = _firestoreDb.Collection("todos").Document(todo.Id);
            await todoDoc.UpdateAsync(new Dictionary<string, object>
        {
            { "Title", todo.Title },
            { "Description", todo.Description },
            { "IsCompleted", todo.IsCompleted }
        });
        }

        public async Task DeleteTodoAsync(string todoId)
        {
            var todoDoc = _firestoreDb.Collection("todos").Document(todoId);
            await todoDoc.DeleteAsync();
        }
    }
}
