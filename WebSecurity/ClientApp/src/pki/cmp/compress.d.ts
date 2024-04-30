/**
 * Compress a string with browser native APIs into a binary string representation
 *
 * @param data - Input string that should be compressed
 * @param encoding - Compression algorithm to use
 * @returns The compressed binary string
 */
export declare function compressRaw(data: string, encoding: CompressionFormat): Promise<string>;
/**
 *	Compress a string with browser native APIs into a string representation
 *
 * @param data - Input string that should be compressed
 * @param encoding - Compression algorithm to use
 * @returns The compressed string
 */
export declare function compress(data: string, encoding: CompressionFormat): Promise<string>;
