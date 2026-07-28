using Fido2NetLib;
using Microsoft.AspNetCore.Http;
using Pulse.WebAuthn.Application.Attestation.Models;

namespace Pulse.WebAuthn.Application.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service interface
    /// </summary>
    public interface IAttestationUseCase
    {
        /// <summary>
        /// Makes the attestation options 
        /// </summary>
        /// <param name="attestationOptionsRequestModel">Attestation options request model</param>
        /// <returns>Attestation options</returns>
        IResult MakeAttestationOptions(AttestationOptionsRequestModel attestationOptionsRequestModel);

        /// <summary>
        /// Makes the attestation
        /// </summary>
        /// <param name="authenticatorAttestationRawResponse">Authenticator attestation Raw response</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Attestation Result</returns>
        Task<IResult> MakeAttestation(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken);
    }
}
