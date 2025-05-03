using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id;
        [ForeignKey("Users")]
        public User users;
        public int UserId;
        public bool IsCompleted;
        public string Description;
        public User User;
    }
}
