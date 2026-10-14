namespace Pulse.Web.Common
{
    public class AssertionHandler : DelegatingHandler
    {
        private readonly TemporaryStorage _storage;
        public AssertionHandler(TemporaryStorage storage)
        {
            _storage = storage;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            // 🔹 Intercept request
            if (!string.IsNullOrEmpty(_storage.AssertionKey))
            {
                request.Headers.Add("X-Assertion-Options-Key", _storage.AssertionKey);
            }
            if (!string.IsNullOrEmpty(_storage.AttestationKey))
            {
                request.Headers.Add("X-Attestation-Options-Key", _storage.AttestationKey);
            }

            // Forward request down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            // 🔹 Intercept response
            if (response.Headers.TryGetValues("X-Assertion-Options-Key", out var assertion))
            {
                _storage.AssertionKey = assertion.FirstOrDefault();

            }
            if (response.Headers.TryGetValues("X-Attestation-Options-Key", out var attestation))
            {
                _storage.AttestationKey = attestation.FirstOrDefault();

            }

            // You could also log, transform, or even replace the response here
            return response;
        }
    }
}
