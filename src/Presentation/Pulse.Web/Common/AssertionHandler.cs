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

            // Forward request down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            // 🔹 Intercept response
            if (response.Headers.TryGetValues("X-Assertion-Options-Key", out var values))
            {
                _storage.AssertionKey = values.FirstOrDefault();

            }

            // You could also log, transform, or even replace the response here
            return response;
        }
    }
}
