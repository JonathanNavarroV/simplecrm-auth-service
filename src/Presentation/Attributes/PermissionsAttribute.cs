using Microsoft.AspNetCore.Authorization;

namespace Presentation.Attributes;

/// <summary>
/// Atributo para declarar permisos de forma concisa en endpoints.
/// Se usa como: [Permissions("USERS:READ")]
/// La resolución de la política la realiza un PolicyProvider personalizado.
/// </summary>
public class PermissionsAttribute : AuthorizeAttribute
{
    public PermissionsAttribute(string permission)
    {
        Policy = $"Permission:{permission}";
    }
}
