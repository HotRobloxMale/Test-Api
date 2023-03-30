namespace testapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using testapi.Data;

public class Users
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int user_id { get; set; }
    [UniqueLogin(ErrorMessage = "The login is already taken")]
    public string? login {get; set;}
    public string? email { get; set;}
    public string? password { get; set;}
    }
public class UniqueLoginAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dbContext = (ApiDbContext)validationContext.GetService(typeof(ApiDbContext));

        if (dbContext.Users.Any(u => u.login == value.ToString()))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

