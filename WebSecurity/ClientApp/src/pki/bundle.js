import * as pkijs from "../pkijs.es.js";
import * as asn1js from "https://unpkg.com/asn1js@latest?module";
import * as pvtsutils from "https://unpkg.com/pvtsutils@latest?module";
import * as compressUtil from '../cmp/compress.js';
import * as decompressUtil from '../cmp/decompress.js';

let certificateText = "";
let debugInfo;

function log(type, message) {

  if (!debugInfo) {
    debugInfo = document.getElementById("debug-info");

    if (debugInfo) {
      debugInfo.value = "";
    }
  }

  if (debugInfo) {
    debugInfo.value += `${type}: ${message}` + "\r\n";
  }

  if (window) {
    var event = new CustomEvent("log",
      {
        detail: {
          type: type,
          message: message,
          file: "loggerProxy.ts"
        }
      }
    );

    window.dispatchEvent(event);
  }
  else {
    switch (type) {
      case "trace":
        console.trace(message);
        break;
      case "debug":
        console.debug(message);
        break;
      case "info":
        console.info(message);
        break;
      case "log":
        console.log(message);
        break;
      case "warn":
        console.warn(message);
        break;
      case "error":
        console.error(message);
        break;
      case "fatal":
        console.fatal(message);
        break;
      default:
        debugger;
        throw new Error(`Invalid log type ${type}`);
    }
  }
}

/* eslint-disable deprecation/deprecation */
function toPEM(buffer, tag) {
  return [
    `-----BEGIN ${tag}-----`,
    formatPEM(pvtsutils.Convert.ToBase64(buffer)),
    `-----END ${tag}-----`,
    "",
  ].join("\n");
}
/**
 * Format string in order to have each line with length equal to 64
 * @param pemString String to format
 * @returns Formatted string
 */
function formatPEM(pemString) {
  const PEM_STRING_LENGTH = pemString.length,
    LINE_LENGTH = 64;
  const wrapNeeded = PEM_STRING_LENGTH > LINE_LENGTH;
  if (wrapNeeded) {
    let formattedString = "",
      wrapIndex = 0;
    for (let i = LINE_LENGTH; i < PEM_STRING_LENGTH; i += LINE_LENGTH) {
      formattedString += pemString.substring(wrapIndex, i) + "\r\n";
      wrapIndex = i;
    }
    formattedString += pemString.substring(wrapIndex, PEM_STRING_LENGTH);
    return formattedString;
  } else {
    return pemString;
  }
}
function isNode() {
  return (
    typeof process !== "undefined" &&
    process.versions != null &&
    process.versions.node != null
  );
}
if (isNode()) {
  import("@peculiar/webcrypto").then((peculiarCrypto) => {
    const webcrypto = new peculiarCrypto.Crypto();
    const name = "newEngine";
    pkijs.setEngine(name, new pkijs.CryptoEngine({ name, crypto: webcrypto }));
  });
}

async function createCertificate$1(hashAlg, signAlg) {
  const certificate = new pkijs.Certificate();
  const crypto = pkijs.getCrypto(true);
  //#region Put a static values
  certificate.version = 2;
  certificate.serialNumber = new asn1js.Integer({ value: 1 });
  certificate.issuer.typesAndValues.push(
    new pkijs.AttributeTypeAndValue({
      type: "2.5.4.6",
      value: new asn1js.PrintableString({ value: "RU" }),
    })
  );
  certificate.issuer.typesAndValues.push(
    new pkijs.AttributeTypeAndValue({
      type: "2.5.4.3",
      value: new asn1js.BmpString({ value: "Test" }),
    })
  );
  certificate.subject.typesAndValues.push(
    new pkijs.AttributeTypeAndValue({
      type: "2.5.4.6",
      value: new asn1js.PrintableString({ value: "RU" }),
    })
  );
  certificate.subject.typesAndValues.push(
    new pkijs.AttributeTypeAndValue({
      type: "2.5.4.3",
      value: new asn1js.BmpString({ value: "Test" }),
    })
  );
  certificate.notBefore.value = new Date();
  certificate.notAfter.value = new Date();
  certificate.notAfter.value.setFullYear(
    certificate.notAfter.value.getFullYear() + 1
  );
  certificate.extensions = []; // Extensions are not a part of certificate by default, it's an optional array
  //#region "BasicConstraints" extension
  const basicConstr = new pkijs.BasicConstraints({
    cA: true,
    pathLenConstraint: 3,
  });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "2.5.29.19",
      critical: true,
      extnValue: basicConstr.toSchema().toBER(false),
      parsedValue: basicConstr, // Parsed value for well-known extensions
    })
  );
  //#endregion
  //#region "KeyUsage" extension
  const bitArray = new ArrayBuffer(1);
  const bitView = new Uint8Array(bitArray);
  bitView[0] |= 0x02; // Key usage "cRLSign" flag
  bitView[0] |= 0x04; // Key usage "keyCertSign" flag
  const keyUsage = new asn1js.BitString({ valueHex: bitArray });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "2.5.29.15",
      critical: false,
      extnValue: keyUsage.toBER(false),
      parsedValue: keyUsage, // Parsed value for well-known extensions
    })
  );
  //#endregion
  //#region "ExtendedKeyUsage" extension
  const extKeyUsage = new pkijs.ExtKeyUsage({
    keyPurposes: [
      "2.5.29.37.0",
      "1.3.6.1.5.5.7.3.1",
      "1.3.6.1.5.5.7.3.2",
      "1.3.6.1.5.5.7.3.3",
      "1.3.6.1.5.5.7.3.4",
      "1.3.6.1.5.5.7.3.8",
      "1.3.6.1.5.5.7.3.9",
      "1.3.6.1.4.1.311.10.3.1",
      "1.3.6.1.4.1.311.10.3.4", // Microsoft Encrypted File System
    ],
  });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "2.5.29.37",
      critical: false,
      extnValue: extKeyUsage.toSchema().toBER(false),
      parsedValue: extKeyUsage, // Parsed value for well-known extensions
    })
  );
  //#region Microsoft-specific extensions
  const certType = new asn1js.Utf8String({ value: "certType" });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "1.3.6.1.4.1.311.20.2",
      critical: false,
      extnValue: certType.toBER(false),
      parsedValue: certType, // Parsed value for well-known extensions
    })
  );
  const prevHash = new asn1js.OctetString({
    valueHex: new Uint8Array([
      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    ]).buffer,
  });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "1.3.6.1.4.1.311.21.2",
      critical: false,
      extnValue: prevHash.toBER(false),
      parsedValue: prevHash, // Parsed value for well-known extensions
    })
  );
  const certificateTemplate = new pkijs.CertificateTemplate({
    templateID: "1.1.1.1.1.1",
    templateMajorVersion: 10,
    templateMinorVersion: 20,
  });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "1.3.6.1.4.1.311.21.7",
      critical: false,
      extnValue: certificateTemplate.toSchema().toBER(false),
      parsedValue: certificateTemplate, // Parsed value for well-known extensions
    })
  );
  const caVersion = new pkijs.CAVersion({
    certificateIndex: 10,
    keyIndex: 20,
  });
  certificate.extensions.push(
    new pkijs.Extension({
      extnID: "1.3.6.1.4.1.311.21.1",
      critical: false,
      extnValue: caVersion.toSchema().toBER(false),
      parsedValue: caVersion, // Parsed value for well-known extensions
    })
  );
  //#endregion
  //#endregion
  //#region Create a new key pair
  //#region Get default algorithm parameters for key generation
  const algorithm = pkijs.getAlgorithmParameters(signAlg, "generateKey");
  if ("hash" in algorithm.algorithm) algorithm.algorithm.hash.name = hashAlg;
  //#endregion
  const { privateKey, publicKey } = await crypto.generateKey(
    algorithm.algorithm,
    true,
    algorithm.usages
  );
  //#endregion
  //#region Exporting public key into "subjectPublicKeyInfo" value of certificate
  await certificate.subjectPublicKeyInfo.importKey(publicKey);
  //#endregion
  //#region Signing final certificate
  await certificate.sign(privateKey, hashAlg);
  //#endregion
  return {
    certificate,
    certificateBuffer: certificate.toSchema(true).toBER(false),
    privateKeyBuffer: await crypto.exportKey("pkcs8", privateKey),
  };
}
async function verifyCertificate$1(
  certificateBuffer,
  intermediateCertificates,
  trustedCertificates,
  crls
) {
  //#region Major activities
  //#region Initial check
  if (certificateBuffer.byteLength === 0) return { result: false };
  //#endregion
  //#region Decode existing CERT
  const certificate = pkijs.Certificate.fromBER(certificateBuffer);
  //#endregion
  //#region Create certificate's array (end-user certificate + intermediate certificates)
  const certificates = [];
  certificates.push(...intermediateCertificates);
  certificates.push(certificate);
  //#endregion
  //#region Make a copy of trusted certificates array
  const trustedCerts = [];
  trustedCerts.push(...trustedCertificates);
  //#endregion
  //#region Create new X.509 certificate chain object
  const certChainVerificationEngine =
    new pkijs.CertificateChainValidationEngine({
      trustedCerts,
      certs: certificates,
      crls,
    });
  //#endregion
  // Verify CERT
  return certChainVerificationEngine.verify();
  //#endregion
}

function getElement(id, type) {
  const el = document.getElementById(id);
  if (!el) {
    throw new Error(`Element with id '${id}' does not exist`);
  }
  if (type && el.nodeName.toLowerCase() !== type) {
    throw new TypeError(
      `Element '${el.nodeName}' is not requested type '${type}'`
    );
  }
  return el;
}
class Assert {
  static ok(value, message) {
    if (!value) {
      throw new Error(message || "Value is empty");
    }
  }
}
function handleFileBrowse$1(evt, cb) {
  Assert.ok(evt.target);
  const target = evt.target;
  const currentFiles = target.files;
  Assert.ok(currentFiles);
  const tempReader = new FileReader();
  tempReader.onload = (event) => {
    Assert.ok(event.target);
    const file = event.target.result;
    if (!(file instanceof ArrayBuffer)) {
      throw new Error("incorrect type of the file. Must be ArrayBuffer");
    }
    cb(file);
  };
  if (currentFiles.length) {
    tempReader.readAsArrayBuffer(currentFiles[0]);
  }
}
function decodePEM(pem, tag = "[A-Z0-9 ]+") {
  const pattern = new RegExp(
    `-{5}BEGIN ${tag}-{5}([a-zA-Z0-9=+\\/\\n\\r]+)-{5}END ${tag}-{5}`,
    "g"
  );
  const res = [];
  let matches = null;
  // eslint-disable-next-line no-cond-assign
  while ((matches = pattern.exec(pem))) {
    const base64 = matches[1].replace(/\r/g, "").replace(/\n/g, "");
    res.push(pvtsutils.Convert.FromBase64(base64));
  }
  return res;
}
function parseCertificate$1(source) {
  const buffers = [];
  const buffer = pvtsutils.BufferSourceConverter.toArrayBuffer(source);
  const pem = pvtsutils.Convert.ToBinary(buffer);
  if (/----BEGIN CERTIFICATE-----/.test(pem)) {
    buffers.push(...decodePEM(pem, "CERTIFICATE"));
  } else {
    buffers.push(buffer);
  }
  const res = [];
  for (const item of buffers) {
    res.push(pkijs.Certificate.fromBER(item));
  }
  return res;
}
function processError(e, message) {
  log("error", e);
  alert(`${message}.See developer console for more details`);
}

let certificateBuffer = new ArrayBuffer(0); // ArrayBuffer with loaded or created CERT
let rsaPublicKey;

const trustedCertificates = []; // Array of root certificates from "CA Bundle"
const intermediateCertificates = []; // Array of intermediate certificates
const crls = []; // Array of CRLs for all certificates (trusted + intermediate)
let hashAlg = "SHA-1";
let signAlg = "RSASSA-PKCS1-v1_5";
function parseCertificate() {
  //#region Initial check
  if (certificateBuffer.byteLength === 0) {
    alert("Nothing to parse!");
    return;
  }
  //#endregion
  //#region Decode existing X.509 certificate
  const certificates = parseCertificate$1(certificateBuffer);
  if (!certificates.length) {
    throw new Error("Cannot get certificate from the file");
  }
  const certificate = certificates[0];
  //#endregion
  //#region Put information about X.509 certificate issuer
  const rdnmap = {
    "2.5.4.6": "C",
    "2.5.4.10": "O",
    "2.5.4.11": "OU",
    "2.5.4.3": "CN",
    "2.5.4.7": "L",
    "2.5.4.8": "ST",
    "2.5.4.12": "T",
    "2.5.4.42": "GN",
    "2.5.4.43": "I",
    "2.5.4.4": "SN",
    "1.2.840.113549.1.9.1": "E-mail",
  };
  //#region get information about X.509 certificate subject
  for (const typeAndValue of certificate.subject.typesAndValues) {
    let typeval = rdnmap[typeAndValue.type];
    if (typeof typeval === "undefined") typeval = typeAndValue.type;
    const subjval = typeAndValue.value.valueBlock.value;
  }
  //#endregion
  //#region Put information about X.509 certificate serial number
  var cert_serial_number = pvtsutils.Convert.ToHex(
    certificate.serialNumber.valueBlock.valueHexView
  );
  //#endregion
  //#region Put information about issuance date
  var cert_not_before = certificate.notBefore.value.toString();
  //#endregion
  //#region Put information about expiration date
  var cert_not_after = certificate.notAfter.value.toString();
  //#endregion
  //#region Put information about subject public key size
  let publicKeySize = "< unknown >";
  if (
    certificate.subjectPublicKeyInfo.algorithm.algorithmId.indexOf(
      "1.2.840.113549"
    ) !== -1
  ) {
    rsaPublicKey = pkijs.RSAPublicKey.fromBER(
      certificate.subjectPublicKeyInfo.subjectPublicKey.valueBlock.valueHexView
    );
    const modulusView = rsaPublicKey.modulus.valueBlock.valueHexView;
    let modulusBitLength = 0;
    if (modulusView[0] === 0x00)
      modulusBitLength =
        (rsaPublicKey.modulus.valueBlock.valueHexView.byteLength - 1) * 8;
    else
      modulusBitLength =
        rsaPublicKey.modulus.valueBlock.valueHexView.byteLength * 8;
    publicKeySize = modulusBitLength.toString();
  }
  var cert_keysize = publicKeySize;
  //#endregion
  //#region Put information about signature algorithm
  const algomap = {
    "1.2.840.113549.1.1.2": "MD2 with RSA",
    "1.2.840.113549.1.1.4": "MD5 with RSA",
    "1.2.840.10040.4.3": "SHA1 with DSA",
    "1.2.840.10045.4.1": "SHA1 with ECDSA",
    "1.2.840.10045.4.3.2": "SHA256 with ECDSA",
    "1.2.840.10045.4.3.3": "SHA384 with ECDSA",
    "1.2.840.10045.4.3.4": "SHA512 with ECDSA",
    "1.2.840.113549.1.1.10": "RSA-PSS",
    "1.2.840.113549.1.1.5": "SHA1 with RSA",
    "1.2.840.113549.1.1.14": "SHA224 with RSA",
    "1.2.840.113549.1.1.11": "SHA256 with RSA",
    "1.2.840.113549.1.1.12": "SHA384 with RSA",
    "1.2.840.113549.1.1.13": "SHA512 with RSA",
  }; // array mapping of common algorithm OIDs and corresponding types
  let signatureAlgorithm = algomap[certificate.signatureAlgorithm.algorithmId];
  if (typeof signatureAlgorithm === "undefined")
    signatureAlgorithm = certificate.signatureAlgorithm.algorithmId;
  else
    signatureAlgorithm = `${signatureAlgorithm} (${certificate.signatureAlgorithm.algorithmId})`;
  var cert_sign_algo = signatureAlgorithm;
  //#endregion
  //#region Put information about certificate extensions
  if (certificate.extensions) {
    for (let i = 0; i < certificate.extensions.length; i++) {
      var ext = certificate.extensions[i].extnID;
    }
  }
  //#endregion
}
async function createCertificate() {
  try {
    const cert = await createCertificate$1(hashAlg, signAlg);
    certificateBuffer = cert.certificateBuffer;
    trustedCertificates.push(cert.certificate);
    parseCertificate();
    console.info("Certificate created successfully!");
    console.info("Private key exported successfully!");
    getElement("new_signed_data").innerHTML = [
      toPEM(cert.certificateBuffer, "CERTIFICATE"),
      toPEM(cert.privateKeyBuffer, "PRIVATE KEY"),
    ].join("\n\n");
    alert("Certificate created successfully!");
  } catch (error) {
    processError(error, "Error on Certificate creation");
  }
}
async function verifyCertificate() {
  try {
    const chainStatus = await verifyCertificate$1(
      certificateBuffer,
      intermediateCertificates,
      trustedCertificates,
      crls
    );
    alert(`Verification result: ${chainStatus.result}`);
  } catch (e) {
    processError(e, "Error on Certificate verifying");
  }
}

function str2ab(str) {
  var buf = new ArrayBuffer(str.length); // 2 bytes for each char
  var bufView = new Uint8Array(buf);
  for (var i = 0, strLen = str.length; i < strLen; i++) {
    bufView[i] = str.charCodeAt(i);
  }
  return buf;
}

function textToArrayBuffer(arrayText) {
  var buffer = new ArrayBuffer(arrayText.length);
  var view = new Uint8Array(buffer);
  var length = view.length;

  for (var i = 0; i < length; i++) {
    view[i] = arrayText[i].charCodeAt(0);
  }

  return buffer;
}

function textToArray(arrayText) {
  var length = arrayText.length;
  var array = new Array(length)

  for (var i = 0; i < length; i++) {
    array[i] = arrayText[i].charCodeAt(0);
  }

  return array;
}

function textToArrayReverse(arrayText) {
  var length = arrayText.length;
  var array = new Array(length)

  for (var i = 0; i < length; i++) {
    array[i] = arrayText[i].charCodeAt(0);
  }

  array.reverse();

  return array;
}

function arrayToText(array) {
  return new Uint8Array(array).reduce((p, c) => p + String.fromCharCode(c), '');
}

function debuggerBreak() {
  debugger;
}

function escapeRegExp(string) {
  return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
}

function replaceAll(str, find, replace) {
  return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

export function unsigned(dec) {
  return dec >>> 0;
}

export function dec2bin(dec) {
  return unsigned(dec).toString(2);
}

export function bin2deca(bin) {

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

export function bin2dec(bin) {

  let dec;
  let byteLength = (bin.length / 8);
  let numberOfBits;

  dec = bin.match(/.{1,8}/g).reduceRight((partialResult, p, index) => {
    let num = parseInt(p, 2) << ((byteLength - (index + 1)) * 8);
    let result = num | partialResult;

    return result;
  }, 0);

  return unsigned(dec);
}

export function atobin(a) {

  let xCheck = "";  // kn todo - remove this and below

  a = a.split("").reduce((partialResult, c, _) => {
    let bin = dec2bin(c.charCodeAt(0)).padStart(8, "0");

    xCheck += bin + " ";

    return partialResult + bin;
  }, "");

  return a;
}

export function xor(mask, x) {

  let length;
  let xCheck = "";  // kn todo - remove this and below
  let result;

  x = atobin(x);
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

  return bin2dec(result);
}

export function xora(mask, x) {

  let length = x.length;
  let xCheck = "";  // kn todo - remove this and below
  let result;

  x = atobin(x);
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

  return bin2deca(result);
}

export function mask(mask, x) {

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

export function isNumeric(str) {
  if (typeof str != "string") return false // we only process strings!  
  return !isNaN(str) && // use type coercion to parse the _entirety_ of the string (`parseFloat` alone does not do this)...
    !isNaN(parseFloat(str)) // ...and ensure strings of whitespace fail
}

export function arrayToHexText(bytes, maxLength) {

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

export async function handle(n) {
  const promise = new Promise(async (resolve, reject) => {
    const breakFunc = debuggerBreak;
    if (!breakFunc) {
    }

    log("debug", "Entering bundle handler --------------------------------------------------------------------------------------------------------------------------");

    if (!window.navigator.onLine) {
      throw "Cannot continue, no internet connection!";
    }

    log("debug", "Checking connection");

    const response = await fetch("https://api.ipify.org?format=json");
    const data = await response.json();

    if (Array.isArray(n)) {

      let url;
      let headers;
      let body;

      [url, headers, body] = n;

      const keys = headers.keys();
      let header = keys.next();

      while (header.value) {

        let key = header.value;
        let value = headers.get(header.value);

        header = keys.next();
      }
    }

    let metaUniqueId = document.querySelector("meta[name='useruniqueid']")
    let ip = data.ip;
    let ipn = parseInt(replaceAll(ip, ".", ""));

    if (metaUniqueId) {

      log("debug", "Found meta tag");

      let content = metaUniqueId.getAttribute("content");

      if (isNumeric(content)) {
        n = parseInt(content);
      }
    }

    if (debugInfo) {
      debugInfo.value += `n (unique number from argument or meta unique id tag): ${n}` + "\r\n";
      debugInfo.value += `ip: ${ip}` + "\r\n";
      debugInfo.value += `ipn (ip without dots): ${ipn}` + "\r\n";
    }

    let xor = n ^ ipn;
    let ms = dec2bin(n ^ ipn);
    let test = false; // kn - todo remove, test only

    if (debugInfo) {
      debugInfo.value += `xor (n xor'd with ipn): ${xor}` + "\r\n";
      debugInfo.value += `ms (n xor'd with ipn, converted to binary string): ${ms}` + "\r\n";
    }

    if (test) {
      let i = "70.162.253.91";
      let i2 = replaceAll(i, ".", "")
      let ipn2 = parseInt(i2);
      let msCheck = dec2bin(n ^ ipn2);

      console.debug(ms);
      console.debug(msCheck);
      console.debug(`Mask matches ${ms == msCheck}`);
    }

    log("debug", "Looking for text from header");

    if (window.performance && performance.getEntriesByType) {

      let navTiming = performance.getEntriesByType('navigation');

      if (navTiming.length > 0) {
        let serverTiming = navTiming[0].serverTiming

        if (serverTiming && serverTiming.length > 0) {
          for (let i = 0; i < serverTiming.length; i++) {
            if (serverTiming[i].name == "ertcay") {

              let base64regex = /^([0-9a-zA-Z+/]{4})*(([0-9a-zA-Z+/]{2}==)|([0-9a-zA-Z+/]{3}=))?$/;

              log("debug", "Found text from header");

              certificateText = decodeURIComponent(serverTiming[i].description).replace(/["']/g, "");

              if (debugInfo) {
                debugInfo.value += `certificateText from header:\r\n\r\n${certificateText}\r\n\r\n`;
              }

              if (!base64regex.test(certificateText)) {

                let error = `ertcay is not valid base64, value:${certificateText}`;
                log("error", error);
                reject(error);
              }
            }
            else {
              console.debug(`${serverTiming[i].name} = ${serverTiming[i].description}`)
            }
          }
        }
      }
    }

    if (!certificateText) {

      log("debug", "Looking for text from block");

      let certBlock = document.getElementById("cert-block");

      if (certBlock) {
        certificateText = certBlock.value;

        if (debugInfo) {
          debugInfo.value += `certificateText from cert-block:\r\n\r\n ${certificateText}` + "\r\n\r\n";
        }
      }
      else {

        let error = "No headers or cert block";

        log("error", error);
        reject(error);
      }
    }

    log("debug", "array reverse");

    let certificateArray = textToArrayReverse(certificateText);
    let k;
    let c;
    let t;

    if (debugInfo) {
      debugInfo.value += `certificateArray (reversed): ${arrayToHexText(certificateArray, 25)} \r\n`;
    }

    log("debug", "to buffer");

    certificateBuffer = textToArrayBuffer(atob(certificateText));
    c = btoa(xora(ms, arrayToText(certificateArray)));

    log("debug", "compressing");

    c = await compressUtil.compress(c, "deflate-raw");

    if (debugInfo) {
      debugInfo.value += `certificate (reversed, xor'd w/ ms, base64 encoded, then compressed and base64 encoded again): \r\n\r\n${c}\r\n\r\n`;
    }

    // kn - todo remove, test only

    if (test) {
      let c2 = await decompressUtil.decompress(c, "deflate-raw")
      let b2 = xora(ms, atob(c2));
      let a2 = textToArrayReverse(b2);

      c2 = arrayToText(a2);

      console.debug(certificateText);
      console.debug(c2);
      console.debug(`Cert matches ${c2 == certificateText}`);
    }

    try {

      log("debug", "parsing");

      parseCertificate();
    }
    catch (e) {
      log("error", e);
      reject(e);
    }

    k = rsaPublicKey.toString();
    t = Date.now();

    if (debugInfo) {
      debugInfo.value += `\r\npublicKey: ${k}\r\n`;
      debugInfo.value += `\r\nclientCert: ${c}\r\n`;
      debugInfo.value += `\r\nnavigationStart: ${t.toString()}\r\n`;
    }

    resolve([k, c, t]);

    log("debug", "Exiting bundle handler --------------------------------------------------------------------------------------------------------------------------");

  }, (e) => {

    log("error", e);
    reject(e);

    throw e;
  });

  return promise;
}
