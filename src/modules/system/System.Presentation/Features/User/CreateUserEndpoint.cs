﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Application.Features.Users.CreateUser;
using System.Domain.Features.Identity;
using System.Presentation.Common;

namespace System.Presentation.Features.User;

internal sealed class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/createuser", async (CreateUserDto request, ISender sender) =>
        {
            Result<int> result = await sender.Send(new CreateUserCommand(request));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.User);
    }

}