using SistemaGestionTareas.ApplicationCore.Abstractions;

namespace SistemaGestionTareas.ApplicationCore.Entities
{
    public class User:DomainEntity<string>
    {
        public string Firstname { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public ICollection<TaskItem> TaskItems { get; set; } = [];

        private User() { }

        public User(string firstName,string lastname,string email)
        {
            Firstname = firstName;
            LastName = lastname;
            Email = email;
        }
    }
}
