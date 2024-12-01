
function isWebAssemblySupported() {
    try {
        if (typeof WebAssembly === 'object' && typeof WebAssembly.instantiateStreaming === 'function') {
            // Prefer streaming compilation for better performance
            return WebAssembly.instantiateStreaming(fetch('BlazorNet8RenderModes.Client.wasm')).then(result => {
                return result.instance instanceof WebAssembly.Instance;
            });
        } else if (typeof WebAssembly === 'object' && typeof WebAssembly.instantiate === 'function') {
            // Fallback to traditional compilation
            const module = new WebAssembly.Module(Uint8Array.of(0x0, 0x61, 0x73, 0x6d, 0x01, 0x00, 0x00, 0x00));
            return WebAssembly.instantiate(module).then(result => {
                return result.instance instanceof WebAssembly.Instance;
            });
        }
    } catch (e) {
        console.error('WebAssembly is not supported:', e);
    }
    return false;
}

function isWebAssemblyReady() {

    try {
        // Check WebAssembly module readiness
        return typeof WebAssembly !== 'undefined' && WebAssembly.instantiateStreaming;
    } catch (error) {
        console.error("Error checking WebAssembly readiness:", error);
        return false;
    }
};