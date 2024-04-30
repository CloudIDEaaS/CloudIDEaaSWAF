/**
 * Decompress a binary string representation with browser native APIs in to a normal js string
 *
 * @param binary - Binary string that should be decompressed, e.g. the output from `atob`
 * @param encoding - Decompression algorithm to use
 * @returns The decompressed string
 */
export declare function decompressRaw(binary: string, encoding: CompressionFormat): Promise<string>;
/**
 * Decompress a string representation with browser native APIs in to a normal js string
 *
 * @param data - String that should be decompressed
 * @param encoding - Decompression algorithm to use
 * @returns The decompressed string
 */
export declare function decompress(data: string, encoding: CompressionFormat): Promise<string>;
