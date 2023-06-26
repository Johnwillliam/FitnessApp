using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class User
    {
        public User()
        {
            FavoriteExercises = new List<FavoriteExercise>();
        }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<FavoriteExercise> FavoriteExercises { get; set; }
    }
}
