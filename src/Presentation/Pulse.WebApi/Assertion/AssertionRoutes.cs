using Fido2NetLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pulse.WebAuthn.Application.Assertion.Models;
using Pulse.WebAuthn.Application.Assertion.Services;
using System.Threading;

namespace Pulse.WebApi.Assertion
{
    /// <summary>
    /// Represents the assertion routes.
    /// </summary>
    public static class AssertionRoutes
    {
        public static void MapAssertionRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var tag = "Assertion";
            endpointRouteBuilder.MapGet("/assertion/option", ([FromServices] IAssertionUseCase assertionUseCase,
                [FromQuery] string username,
                [FromQuery] string userVerification) => assertionUseCase.MakeAssertionOptions(new AssertionOptionsRequestModel { Username = username, UserVerification = userVerification }))
                .WithTags(tag);

            endpointRouteBuilder.MapPost("/assertion", async ([FromServices] IAssertionUseCase assertionUseCase,
                [FromBody] AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse,
                CancellationToken cancellationToken) => await assertionUseCase.MakeAssertion(authenticatorAssertionRawResponse, cancellationToken))
                .WithTags(tag);
        }
    }
}
