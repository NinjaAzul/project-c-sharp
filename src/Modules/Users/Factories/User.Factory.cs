// using Project_C_Sharp.Modules.Users.Entities;
// using Project_C_Sharp.Shared.Exceptions;

// namespace Project_C_Sharp.Modules.Users.Factories;

// public class UserFactory
// {
//     public static User Create(string name, string email, string password)
//     {
//         // temp user for validation
//         var tempUser = new User(name, email, password);

//         // validate fields
//         var basicValidator = new UserValidator();
//         var basicValidationResult = basicValidator.Validate(tempUser);

//         if (!basicValidationResult.IsValid)
//         {
//             throw new BadRequestException(basicValidationResult.Errors);
//         }

//         // validate password
//             var passwordValidator = new UserValidator();
//         var passwordValidationResult = passwordValidator.Validate(password);

//         if (!passwordValidationResult.IsValid)
//         {
//             throw new BadRequestException(passwordValidationResult.Errors);
//         }

//         // if pass the validations, create the final user with the hashed password
//         return new User(
//             name: name,
//             email: email,
//             password: BCrypt.Net.BCrypt.HashPassword(password)
//         );
//     }
// }