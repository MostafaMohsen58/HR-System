using System.Text.Json.Serialization;
namespace HRManagementSystem.BL.DTOs.AuthDTO
{
    public class UserDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}