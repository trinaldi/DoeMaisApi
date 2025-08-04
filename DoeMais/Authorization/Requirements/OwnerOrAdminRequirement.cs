using Microsoft.AspNetCore.Authorization;

namespace DoeMais.Authorization.Requirements;

public class OwnerOrAdminRequirement : IAuthorizationRequirement { }