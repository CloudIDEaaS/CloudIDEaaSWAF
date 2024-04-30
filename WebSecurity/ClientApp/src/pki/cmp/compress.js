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
 * Compress a string with browser native APIs into a binary string representation
 *
 * @param data - Input string that should be compressed
 * @param encoding - Compression algorithm to use
 * @returns The compressed binary string
 */
export function compressRaw(data, encoding) {
    return __awaiter(this, void 0, void 0, function* () {
        // stream the string through the compressor
        const stream = new Blob([new TextEncoder().encode(data)]).stream().pipeThrough(new CompressionStream(encoding));
        // convert the stream to an array buffer
        const buffer = yield new Response(stream).arrayBuffer();
        // convert the array buffer to a binary string
        return Array.from(new Uint8Array(buffer), (x) => String.fromCodePoint(x)).join('');
    });
}
/**
 *	Compress a string with browser native APIs into a string representation
 *
 * @param data - Input string that should be compressed
 * @param encoding - Compression algorithm to use
 * @returns The compressed string
 */
export function compress(data, encoding) {
    return __awaiter(this, void 0, void 0, function* () {
        return btoa(yield compressRaw(data, encoding));
    });
}
