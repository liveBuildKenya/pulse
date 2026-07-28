export function getClientCapabilities() {
    const getCapabilities = window.PublicKeyCredential?.getClientCapabilities;

    return typeof getCapabilities === "function"
        ? getCapabilities.call(window.PublicKeyCredential)
        : {};
}
