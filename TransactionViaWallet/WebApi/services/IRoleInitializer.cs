using Microsoft.AspNetCore.Identity;

public interface IRoleInitializer
{
    Task InitializeRoles();
}

public class RoleInitializer : IRoleInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializer(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task InitializeRoles()
    {
        string[] roleNames = { "User", "Admin" }; // Ajoutez d'autres rôles si nécessaire
        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
