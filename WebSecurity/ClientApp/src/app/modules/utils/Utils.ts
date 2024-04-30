export class Utils {
    public static expandPath(path: string): string {

        var replaced = path.replace(/%([^%]+)%/g, function (_, n) {
            return process.env[n];
        });

        return replaced;
    }

    public static sleep(time): Promise<{}> {
        return new Promise((resolve) => setTimeout(resolve, time));
    }

    public static explicitSleep = (milliseconds: number): void => {
        let startTime = Date.now();
        let currentTime = Date.now();

        while (currentTime - startTime < milliseconds) {
            currentTime = Date.now();
        }
    }


    public static checksum(data: string): number {

        let checksum = 0;

        // Iterate through each character in the data
        for (let i = 0; i < data.length; i++) {

            let product = (data.charCodeAt(i) & 0x7F);
            product *= i + 1;

            checksum += Utils.unsigned(product);
        }

        // Ensure the checksum is within 
        return checksum;
    }

    public static ab2str(buf: ArrayBuffer): string {
        return String.fromCharCode.apply(null, new Uint16Array(buf));
    }

    public static str2ab(str: string): ArrayBuffer {

        var buf = new ArrayBuffer(str.length * 2); // 2 bytes for each char
        var bufView = new Uint16Array(buf);

        for (var i = 0, strLen = str.length; i < strLen; i++) {
            bufView[i] = str.charCodeAt(i);
        }

        return buf;
    }

    public static textToArrayBuffer(arrayText) {
        var buffer = new ArrayBuffer(arrayText.length);
        var view = new Uint8Array(buffer);
        var length = view.length;

        for (var i = 0; i < length; i++) {
            view[i] = arrayText[i].charCodeAt(0);
        }

        return buffer;
    }

    public static textToArray(arrayText) {
        var length = arrayText.length;
        var array = new Array(length)

        for (var i = 0; i < length; i++) {
            array[i] = arrayText[i].charCodeAt(0);
        }

        return array;
    }

    public static textToArrayReverse(arrayText) {
        var length = arrayText.length;
        var array = new Array(length)

        for (var i = 0; i < length; i++) {
            array[i] = arrayText[i].charCodeAt(0);
        }

        array.reverse();

        return array;
    }

    public static arrayToText(array) {
        return new Uint8Array(array).reduce((p, c) => p + String.fromCharCode(c), '');
    }

    public static debuggerBreak() {
        debugger;
    }

    public static escapeRegExp(string) {
        return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
    }

    public static replaceAll(str, find, replace) {
        return str.replace(new RegExp(Utils.escapeRegExp(find), 'g'), replace);
    }

    public static unsigned(dec) {
        return dec >>> 0;
    }

    public static dec2bin(dec) {
        return Utils.unsigned(dec).toString(2);
    }

    public static bin2deca(bin) {

        let dec;
        let xCheck = "";  // kn todo - remove this and below

        dec = bin.match(/.{1,8}/g).reduce((partialResult, p, _) => {
            let num = parseInt(p, 2);
            let result = String.fromCharCode(num);

            xCheck += result;

            return partialResult + result;
        }, "");

        return dec;
    }

    public static bin2dec(bin) {

        let dec;
        let byteLength = (bin.length / 8);
        let numberOfBits;

        dec = bin.match(/.{1,8}/g).reduceRight((partialResult, p, index) => {
            let num = parseInt(p, 2) << ((byteLength - (index + 1)) * 8);
            let result = num | partialResult;

            return result;
        }, 0);

        return Utils.unsigned(dec);
    }

    public static atobin(a) {

        let xCheck = "";  // kn todo - remove this and below

        a = a.split("").reduce((partialResult, c, _) => {
            let bin = Utils.dec2bin(c.charCodeAt(0)).padStart(8, "0");

            xCheck += bin + " ";

            return partialResult + bin;
        }, "");

        return a;
    }

    public static xor(mask, x) {

        let length;
        let xCheck = "";  // kn todo - remove this and below
        let result;

        x = Utils.atobin(x);
        length = x.length
        mask = mask.padEnd(length, "0");

        result = mask.split("").reduce((partialResult, c, index) => {
            if (index < x.length) {
                let n = parseInt(x.substr(index, 1));
                let cn = parseInt(c);
                let xor = n ^ cn;

                xCheck += xor.toString();

                return partialResult + xor.toString();
            }
        }, "");

        return Utils.bin2dec(result);
    }

    public static xora(mask, x) {

        let length = x.length;
        let xCheck = "";  // kn todo - remove this and below
        let result;

        x = Utils.atobin(x);
        length = x.length
        mask = mask.padEnd(length, "0");

        result = mask.split("").reduce((partialResult, c, index) => {
            if (index < x.length) {
                let n = parseInt(x.substr(index, 1));
                let cn = parseInt(c);
                let xor = n ^ cn;

                xCheck += xor.toString();

                return partialResult + xor.toString();
            }
        }, "");

        return Utils.bin2deca(result);
    }

    public static mask(mask, x) {

        let length = x.length;
        mask = mask.padEnd(length, "0");

        return mask.split("").reduceRight((partialResult, _, index) => {
            if (index < x.length && x.charAt(x.length - index - 1) === "1") {
                return partialResult + "1";
            }
            else {
                return partialResult + "0";
            }
        }, "");
    }

    public static isNumeric(str) {
        if (typeof str != "string") return false // we only process strings!  
        let num = <any>str;
        return !isNaN(num) && // use type coercion to parse the _entirety_ of the string (`parseFloat` alone does not do this)...
            !isNaN(parseFloat(str)) // ...and ensure strings of whitespace fail
    }

    public static arrayToHexText(bytes, maxLength) {

        let debugBytes = "";
        let limit = Math.min(bytes.byteLength, maxLength)
        let length;

        for (let x = 0; x < limit; x++) {
            debugBytes = debugBytes.concat(bytes[x].toString(16));
            debugBytes = debugBytes.concat(", ");
        }

        if (limit < maxLength) {
            debugBytes = debugBytes.concat(", ...");
        }
        else {
            length = debugBytes.length;
            debugBytes = debugBytes.substring(0, length - 2);
        }

        return debugBytes;
    }
}
