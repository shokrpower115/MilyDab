using System.ComponentModel.DataAnnotations;

namespace ProServi.Application.DTOs.Auth;

public class UpdateUserDto
{
    [StringLength(100, MinimumLength = 3)]
    public string FullName { get; set; }

    [Phone]
    public string Phone { get; set; }
}
