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
                ([FromQuery] string? username,
                [FromQuery] string? residentKeyRequirement,
                [FromQuery] string? userVerificationRequirement,
                [FromQuery] string? authenticatorAttachment,
                [FromQuery] string? attestationConveyancePreference,
                [FromServices] IAttestationUseCase attestationUseCase) =>
                {
                    var model = new AttestationOptionsRequestModel
                    {
                        Username = username ?? string.Empty,
                        ResidentKeyRequirement = System.Enum.TryParse<Fido2NetLib.Objects.ResidentKeyRequirement>(residentKeyRequirement, true, out var rk) ? rk : Fido2NetLib.Objects.ResidentKeyRequirement.Preferred,
                        UserVerificationRequirement = System.Enum.TryParse<Fido2NetLib.Objects.UserVerificationRequirement>(userVerificationRequirement, true, out var uv) ? uv : Fido2NetLib.Objects.UserVerificationRequirement.Preferred,
                        AuthenticatorAttachment = System.Enum.TryParse<Fido2NetLib.Objects.AuthenticatorAttachment>(authenticatorAttachment, true, out var aa) ? aa : null,
                        AttestationConveyancePreference = System.Enum.TryParse<Fido2NetLib.Objects.AttestationConveyancePreference>(attestationConveyancePreference, true, out var ac) ? ac : Fido2NetLib.Objects.AttestationConveyancePreference.None
                    };
                    return attestationUseCase.MakeAttestationOptions(model);
                })
                .WithTags(routeTag);

            endpointRouteBuilder.MapPost("/attestation",
                async ([FromBody] AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse,
                [FromServices] IAttestationUseCase attestationService,
                CancellationToken cancellationToken) => await attestationService.MakeAttestation(authenticatorAttestationRawResponse, cancellationToken))
                .WithTags(routeTag);
        }
    }
}
