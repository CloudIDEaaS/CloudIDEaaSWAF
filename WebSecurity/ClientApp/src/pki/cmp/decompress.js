var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
/**
 * Decompress a binary string representation with browser native APIs in to a normal js string
 *
 * @param binary - Binary string that should be decompressed, e.g. the output from `atob`
 * @param encoding - Decompression algorithm to use
 * @returns The decompressed string
 */
export function decompressRaw(binary, encoding) {
    return __awaiter(this, void 0, void 0, function* () {
        // stream the string through the decompressor
        const stream = new Blob([Uint8Array.from(binary, (m) => m.codePointAt(0))])
            .stream()
            .pipeThrough(new DecompressionStream(encoding));
        // convert the stream to a string
        return new Response(stream).text();
    });
}
/**
 * Decompress a string representation with browser native APIs in to a normal js string
 *
 * @param data - String that should be decompressed
 * @param encoding - Decompression algorithm to use
 * @returns The decompressed string
 */
export function decompress(data, encoding) {
    return __awaiter(this, void 0, void 0, function* () {
        return decompressRaw(atob(data), encoding);
    });
}
