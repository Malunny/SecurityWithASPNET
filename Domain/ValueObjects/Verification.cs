using System.Security.AccessControl;

namespace Domain.ValueObjects;

public class Verification : ValueObject
{
    public string Code { get; } = Guid.NewGuid().ToString("N")[0..6].ToUpper();
    public DateTime? ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime? VerifiedAt { get; set; } = null;
    public bool IsActive => ExpiresAt == null && VerifiedAt != null;
    
    public Verification() { }

    public void Verify(string code)
    {
        if (IsActive) 
            throw new Exception("Verification is already active");
        
        if (ExpiresAt < DateTime.UtcNow)
            throw new Exception("Verification is expired at " + ExpiresAt);
        
        if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Verification failed");
        
        VerifiedAt = DateTime.UtcNow;
        ExpiresAt = null;
    }
}