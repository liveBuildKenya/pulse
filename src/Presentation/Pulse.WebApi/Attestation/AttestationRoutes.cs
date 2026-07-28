using Fido2NetLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pulse.WebAuthn.Application.Attestation.Models;
using Pulse.WebAuthn.Application.Attestation.Services;
using System.Threading;

namespace Pulse.WebApi.Attestation
{
    /// <summary>
    /// Represents the registration of new credentials
    /// </summary>
    public static class AttestationRoutes
    {
        public static void MapAttestationRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var routeTag = "Attestation";

            endpointRouteBuilder.MapGet("/attestation/options",
                ([FromQuery] AttestationOptionsRequestModel? attestationOptionsRequestModel,
                [FromServices] IAttestationUseCase attestationUseCase) =>
                attestationUseCase.MakeAttestationOptions(new AttestationOptionsRequestModel { Username = username, DisplayName = displayName }))
                .WithTags(routeTag);

            endpointRouteBuilder.MapPost("/attestation",
                async ([FromBody] AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse,
                [FromServices] IAttestationUseCase attestationService,
                CancellationToken cancellationToken) => await attestationService.MakeAttestation(authenticatorAttestationRawResponse, cancellationToken))
                .WithTags(routeTag);
        }
    }
}
