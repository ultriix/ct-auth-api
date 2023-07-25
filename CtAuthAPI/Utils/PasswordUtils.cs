using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace CtAuthAPI.Utils;

public static class PasswordUtils
{
    
    /// <summary>
    /// Encrypt and return a given password.
    /// We store the salt in the hash so we can verify another password later.
    ///
    /// See https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-7.0
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string HashPassword(string password)
    {
        // Generate a 128-bit salt using a sequence of
        // cryptographically strong random bytes.
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
    
        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return $"{ Convert.ToBase64String(salt) }:{ hashed }";
    }

    /// <summary>
    /// Verify an existing hash and new plain password match.
    ///
    /// Uses the original salt to re-hash the new password and compare the hashes.
    /// </summary>
    /// <param name="hashedPassword"></param>
    /// <param name="verifyPassword"></param>
    /// <returns></returns>
    public static bool VerifyPassword(string hashedPassword, string verifyPassword)
    {
        // split the original salt and hash
        string[] parts = hashedPassword.Split(':');
        
        // set the salt from previous to use to verify
        byte[] salt = Convert.FromBase64String(parts[0]);
    
        // re-hash the password using the same settings as HashPassword above
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: verifyPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        // Compare the computed hash with the stored hash
        return hashed == parts[1];
    }
    
}