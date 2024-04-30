// TextExtensions.h

#pragma once

#include "Stdafx.h"
#include "TextRange.h"

using namespace System;
using namespace System::Runtime::CompilerServices;
using namespace System::Linq;
using namespace System::Xml;
using namespace System::Xml::XPath;
using namespace System::Xml::Linq;
using namespace System::Xml::Xsl;
using namespace System::Text;
using namespace System::Text::RegularExpressions;
using namespace System::Collections::Generic;
using namespace Microsoft::Win32;

namespace NAMESPACE_DECL {

	/// <summary>
	/// Contains extension methods for performing additional operations on Text Object Model (TOM) types.
	/// </summary>
	[Extension]
	public ref class TextExtensions abstract sealed {

	public:

		/// <summary>
		/// Appends <paramref name="text"/> to the end of the range.
		/// </summary>
		/// <param name="range">The range to append the text the end of.</param>
		/// <param name="text">The text to append.</param>
		/// <param name="mode">Controls how the start and end positions of the range are affected.</param>
		/// <remarks>
		/// This extension method mimics the behaviour of the 
		/// <see cref="StringBuilder::Append(String^)"/> method of the 
		/// <see cref="StringBuilder"/> class.
		/// </remarks>
		[Extension]
		static void Append(TextRange^ range, String^ text, 
			[Optional, DefaultParameterValue(RangeAppendMode::Collapse)] RangeAppendMode mode
		) {
			if (text == nullptr) text = String::Empty;

			int oldStart = range->Start;
			int oldLength = range->Length;

			range->Collapse(RangePosition::End);
			range->Text = text;
			
			if (mode == RangeAppendMode::Expand) {
				range->Start = oldStart;
			}
			else if (mode == RangeAppendMode::Collapse) {
				range->Collapse(RangePosition::End);
			}
			else {
				range->Start = oldStart;
				range->Length = oldLength;
			}
		}

		/// <summary>
		/// Appends <paramref name="text"/>, followed by a newline character, to the end of the range.
		/// </summary>
		/// <param name="range">The range to append the text the end of.</param>
		/// <param name="text">The text to append.</param>
		/// <param name="mode">Controls how the start and end positions of the range are affected.</param>
		/// <remarks>
		/// This extension method mimics the behaviour of the 
		/// <see cref="StringBuilder::AppendLine(String^)"/> method of the 
		/// <see cref="StringBuilder"/> class.
		/// </remarks>
		[Extension]
		static void AppendLine(TextRange^ range, 
			[Optional, DefaultParameterValue("")] String^ text, 
			[Optional, DefaultParameterValue(RangeAppendMode::Collapse)] RangeAppendMode mode
		) {
			if (text == nullptr) text = String::Empty;
			Append(range, text + Environment::NewLine, mode);
		}

		/// <summary>
		/// Clears the text in the range.
		/// </summary>
		/// <param name="range">The range to clear.</param>
		[Extension]
		static void Clear(TextRange^ range) {
			range->Delete(TextUnit::Character, 0);
		}

		/// <summary>
		/// Inserts <paramref name="text"/> at the specified position within the range.
		/// </summary>
		/// <param name="range">The range to insert the text into.</param>
		/// <param name="index">Character position (relative to the start of the range).</param>
		/// <param name="text">The text to insert.</param>
		/// <param name="mode">Controls how the start and end positions of the range are affected.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="index"/> does not fall within the bounds of the range.
		/// </exception>
		/// <remarks>
		/// This extension method mimics the behaviour of the 
		/// <see cref="StringBuilder::Insert(int,String^)"/> method of the 
		/// <see cref="StringBuilder"/> class.
		/// </remarks>
		[Extension]
		static void Insert(TextRange^ range, int index, String^ text, 
			[Optional, DefaultParameterValue(RangeInsertMode::Expand)] RangeInsertMode mode
		) {
			if ((index < 0) || (index >= range->Length)) throw gcnew ArgumentOutOfRangeException("index");
			if (text == nullptr) text = String::Empty;

			int oldStart = range->Start;
			int oldLength = range->Length;

			range->Collapse(RangePosition::Start);
			if (index > 0) range->MoveStart(TextUnit::Character, index);
			range->Text = text;

			if (mode != RangeInsertMode::Intersect) {
				range->Start = oldStart;
				range->Length = oldLength + text->Length;
			}
		}

		/// <summary>
		/// Removes a sequence of characters from the range.
		/// </summary>
		/// <param name="range">The range to remove the characters from.</param>
		/// <param name="index">Character position (relative to the start of the range) to begin removing characters.</param>
		/// <param name="count">The number of characters to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="index"/> does not fall within the bounds of the 
		/// range, or <paramref name="count"/> is negative, or 
		/// <paramref name="count"/> is greater-than the number of characters 
		/// in the range after <paramref name="index"/>. 
		/// </exception>
		/// <remarks>
		/// This extension method mimics the behaviour of the 
		/// <see cref="StringBuilder::Remove(int,int)"/> method of the 
		/// <see cref="StringBuilder"/> class.
		/// </remarks>
		[Extension]
		static void Remove(TextRange^ range, int index, int count) {
			if ((index < 0) || (index >= range->Length)) throw gcnew ArgumentOutOfRangeException("index");
			if ((count < 0) || (count > (range->Length - index))) throw gcnew ArgumentOutOfRangeException("count");

			int oldStart = range->Start;
			int oldLength = range->Length;

			range->Collapse(RangePosition::Start);
			if (index > 0) range->MoveStart(TextUnit::Character, index);
			if (count > 0) range->Delete(TextUnit::Character, count);
			range->Start = oldStart;
			range->Length = oldLength - count;
		}

#ifdef _TOM2

		/// <summary>
		/// Returns a string containing the Office MathML (OMML) markup for the math text in the range.
		/// </summary>
		/// <param name="range">The range containing the math text.</param>
		/// <param name="mode">Determines how the markup is structured.</param>
		/// <param name="includeProperties">Whether to include document-level math properties.</param>
		/// <returns>
		/// A string containing one or more top-level OMML tags, depending on 
		/// the math content and the value of <paramref name="mode"/> and 
		/// <paramref name="includeProperties"/>.
		/// </returns>
		/// <exception cref="NotSupportedException">
		/// The TOM implementation used by <paramref name="range"/> does not support math.
		/// </exception>
		/// <remarks>
		/// <para>
		/// This method works by converting the Math RTF in the range to OMML, 
		/// as the two representations are semantically identical.
		/// </para>
		/// <para>
		/// Note: Setting the <paramref name="includeProperties"/> parameter to 
		/// true will result in an XML fragment (instead of a document). This 
		/// will also occur if <paramref name="mode"/> is set to 
		/// <see cref="OfficeMathMLMode::Default"/> or 
		/// <see cref="OfficeMathMLMode::Inline"/> and there are multiple math 
		/// zones in the range.
		/// </para>
		/// <seealso href="http://www.ecma-international.org/publications/standards/Ecma-376.htm">Standard ECMA-376: Office Open XML File Formats</seealso>
		/// </remarks>
		/// <example>
		/// The following example demonstrates how to build-up math text and then convert it to OMML: 
		/// <code source="..\Examples\TextExtensions.cs" region="ToOfficeMathML" language="cs" />
		/// </example>
		[Extension]
		static String^ ToOfficeMathML(TextRange^ range, 
			[Optional, DefaultParameterValue(OfficeMathMLMode::Default)] OfficeMathMLMode mode, 
			[Optional, DefaultParameterValue(false)] bool includeProperties
		) {
			// TOM 2 required
			if (range->SupportedVersion == TOMVersion::TOM1) THROW_TOM2_ONLY();
			
			// document element is a WordProcessingML paragraph
			XDocument^ doc = gcnew XDocument(
				gcnew XElement(XNamespace::Get(wordNS) + "p", 
					gcnew XAttribute("xmlns", wordNS),
					gcnew XAttribute(XNamespace::Xmlns + "m", mathNS)
				)
			);

			// get RTF content for range
			String^ rtf = range->Rtf;

			// parse RTF
			OmmlHelper(doc->Root, rtf, 0, range, nullptr);

			// filter element list and convert to string			
			StringBuilder^ sb = gcnew StringBuilder();
			XmlWriter^ xw = nullptr;

			try {
				// resulting XML will be formatted with no declaration at the top
				XmlWriterSettings^ xws = gcnew XmlWriterSettings();
				xws->Encoding = Encoding::Unicode;
				xws->OmitXmlDeclaration = true;
				xws->Indent = true;
				xw = XmlWriter::Create(sb, xws);
				
				// remove math properties element as it is not valid here
				XName^ propertiesName = XNamespace::Get(mathNS) + "mathPr";
				XElement^ properties = doc->Root->Element(propertiesName);
				if (properties == nullptr) 
					properties = gcnew XElement(propertiesName);
				else
					properties->Remove();

				if (includeProperties) {
					// write math properties separately (note: this may create non-conformant XML)
					properties->WriteTo(xw);
				}

				XName^ zoneName =  XNamespace::Get(mathNS) + "math";
				XName^ inlineName =  XNamespace::Get(mathNS) + "oMath";
				XName^ paraName =  XNamespace::Get(mathNS) + "oMathPara";
				XName^ paraPropsName =  XNamespace::Get(mathNS) + "oMathParaPr";

				XElement^ zone = nullptr;
				XElement^ para = nullptr;
				XElement^ paraProps = nullptr;

				switch (mode) {
					case OfficeMathMLMode::Default:
						// copy as-is
						zone = doc->Root->Element(zoneName);
						if (zone != nullptr) {
							for each (XElement^ element in zone->Elements()) {
								element->WriteTo(xw);
							}
						}
						break;

					case OfficeMathMLMode::Inline:
						// return m:oMath elements only (flatten)
						for each (XElement^ element in doc->Root->Descendants(inlineName)) {
							element->WriteTo(xw);
						}
						break;

					case OfficeMathMLMode::Display:
						// create new para
						 para = gcnew XElement(paraName);

						// copy properties from first math para (if any)
						paraProps = Enumerable::FirstOrDefault<XElement^>(doc->Root->Descendants(paraPropsName));
						if (paraProps != nullptr) {
							paraProps->Remove();
							para->Add(paraProps);
						}

						// copy m:oMath elements into para (flatten)
						for each (XElement^ element in Enumerable::ToList<XElement^>(doc->Root->Descendants(inlineName))) {
							element->Remove();
							para->Add(element);
						}
						doc->Root->Add(para);
						para->WriteTo(xw);
						break;

					case OfficeMathMLMode::WordProcessingML:
						// use root element
						doc->Root->WriteTo(xw);
						break;
				}
			}
			finally {
				delete xw;
			}

			return sb->ToString();
		}

		/// <summary>
		/// Returns a string containing the W3C MathML (MML) markup for the math text in the range.
		/// </summary>
		/// <param name="range">The range containing the math text.</param>
		/// <returns>A string containing the MathML markup.</returns>
		/// <exception cref="NotSupportedException">
		/// Microsoft Office is not installed, or the XSL stylesheet (used in the conversion process) was not found.
		/// </exception>
		/// <remarks>
		/// <para>
		/// This method works by transforming the OMML representation of the 
		/// math text to MathML via an XSL stylesheet included with Microsoft 
		/// Office. This method cannot be used if Microsoft Office is not 
		/// installed.
		/// </para>
		/// <para>
		/// Note: If the current process is 32-bit and only the 64-bit version 
		/// of Office is installed, MathML conversion will be unavailable.
		/// </para>
		/// <seealso href="http://www.w3.org/Math/">W3C Math Home</seealso>
		/// </remarks>
		/// <example>
		/// The following example demonstrates how to build-up math text and then convert it to MathML: 
		/// <code source="..\Examples\TextExtensions.cs" region="ToMathML" language="cs" />
		/// </example>
		[Extension]
		static String^ ToMathML(TextRange^ range) {
			String^ xslPath = GetPathToMathXsl();

			Stream^ sTransform = nullptr;
			XmlReader^ xrTransform = nullptr;
			StringReader^ srInput = nullptr;
			XmlWriter^ xwOutput = nullptr;
			
			try {
				// load stylesheet
				XslCompiledTransform^ xslt = gcnew XslCompiledTransform();
				sTransform = File::OpenRead(xslPath);				
				xrTransform = XmlReader::Create(sTransform);					
				xslt->Load(xrTransform);

				// create input (needs a single root element)
				srInput = gcnew StringReader(ToOfficeMathML(range, OfficeMathMLMode::WordProcessingML, false));
				XPathDocument^ doc = gcnew XPathDocument(srInput);
				
				// create output (formatted, no declaration at the top)
				StringBuilder^ sb = gcnew StringBuilder();
				XmlWriterSettings^ xws = xslt->OutputSettings->Clone();
				xws->OmitXmlDeclaration = true;
				xws->Indent = true;
				XmlWriter^ xwOutput = XmlWriter::Create(sb, xws);
				
				// apply XSL transformation
				XsltArgumentList^ args = gcnew XsltArgumentList();
				xslt->Transform(doc, args, xwOutput);

				return sb->ToString();
			}
			finally {
				delete xwOutput;
				delete srInput;
				delete xrTransform;
				delete sTransform;
			}
		}

#endif

	private:
		
#ifdef _TOM2

		// Recursive method that parses Math RTF into OMML
		static int OmmlHelper(XElement^ parent, String^ rtf, int start, TextRange^ range, Dictionary<int,String^>^ fontTable) {
			int i;
			System::Char prev = '\0';
			bool inControlWord = false;
			StringBuilder^ accum = gcnew StringBuilder();
			StringBuilder^ surrogateBuilder = gcnew StringBuilder();
			XElement^ element = nullptr;
			if (fontTable == nullptr) fontTable = gcnew Dictionary<int, String^>();

			// process RTF character by character
			for (i=start; i<rtf->Length; i++) {
				System::Char cur = rtf[i];

				bool flushAccum = false;
				bool ignore = false;
				bool terminate = false;
				bool recurse = false;
				bool controlWordAfter = false;

				if (System::Char::IsWhiteSpace(cur)) {
					if (inControlWord) {
						// signals start of element content
						flushAccum = true;
						ignore = true;
					}

					// ...otherwise just interpret literally
				}
				else if ((cur == '\\') && (prev != '\\')) {
					// start of next control word
					flushAccum = true;
					controlWordAfter = true;
					ignore = true;
				}
				else if ((cur == '{') && (prev != '\\')) {
					// start of a nested block (need to recurse)
					flushAccum = true;
					ignore = true;
					recurse = true;
				}
				else if ((cur == '}') && (prev != '\\')) {
					// end of current block (need to terminate)
					ignore = true;
					terminate = true;
					flushAccum = true;
				}
				else if ((prev == '\\') && ((cur == '\\') || (cur == '{') || (cur == '}'))) {
					// escaped character, interpret as content
					inControlWord = false;
				}

				if (flushAccum) {
					flushAccum = false;

					if (accum->Length > 0) {
						// contains all text since the last delimiter
						String^ text = accum->ToString();
						accum->Remove(0,accum->Length);	

						// anything that isn't a control word is content
						bool processText = !inControlWord;

						if (inControlWord) {
							// process control word
							if (element == nullptr) {
								if (text->StartsWith("rtf")) {
									// root control word, add to root element
									element = parent;
								}
								else {
									if (IsMathRtf(text)) {
										// create element using appropriate name
										element = CreateOmmlTag(text, parent, fontTable);					
										parent->Add(element);
									}
									else if (!text->Equals("*")) {
										// use a dummy element (orphaned content is ignored)
										element = gcnew XElement(text);
									}
								}
							}
							else if (IsMathRtf(text)) {
								// element already created, so create child element
								XElement^ child = CreateOmmlTag(text, parent, fontTable);
								element->Add(child);
							}
							else {
								// check for escaped unicode characters
								if (ProcessUnicodeLiteral(text, surrogateBuilder, i)) {
									String^ content = surrogateBuilder->ToString();
									surrogateBuilder->Remove(0,surrogateBuilder->Length);									
									text = content;

									if (System::Char::IsSurrogatePair(content,0)) {
										// look up folded character for UTF-32 character
										TextRange^ copy = range->Clone();
										if (copy->FindText(content, 0, RangeMatchType::None) > 0) {
											text = copy->GetText(PlainTextFlags::FoldMathAlpha);
										}
										delete copy;
									}

									// need to treat as content
									processText = true;
								}
							}

							inControlWord = false;
						}
						
						if (processText) {
							// set content (depends on element type)
							SetElementContent(element, text, parent, fontTable);
						}
					}
				}

				// next character is part of a control word
				if (controlWordAfter) inControlWord = true;

				if (recurse) {
					// recurse to create children, then continue from end of block
					XElement^ parentOfChild = (element == nullptr) ? parent : element;
					i = OmmlHelper(parentOfChild, rtf, i + 1, range, fontTable);
					prev = '}';
					ignore = true;
					recurse = false;
				}

				if (!ignore) {
					// append to accumulator (normal behaviour)
					accum->Append(cur);
				}

				prev = cur;

				if (terminate) {
					if (element != nullptr) {
						// clean up empty property elements (required in RTF, optional in OMML)
						System::Xml::Linq::Extensions::Remove(element->Descendants(XNamespace::Get(mathNS) + "ctrlPr"));
						if (element->IsEmpty && !element->HasAttributes && (element->Parent != nullptr)) element->Remove();
					}

					// end of block
					break;
				}
			}

			return i;
		}

		// Indicates whether the specified RTF control word translates to an OMML tag
		static bool IsMathRtf(String^ rtfCode) {
			return rtfCode->StartsWith("m");
		}

		// Creates an OMML tag for the specified RTF control word
		static XElement^ CreateOmmlTag(String^ rtfCode, XElement^ parent, Dictionary<int,String^>^ fontTable) {
			// tag999 (usually) translates to <tag m:val="999" />
			Regex^ tagExp = gcnew Regex("([a-zA-z]+)(-?[0-9]+)?");
			Match^ m = tagExp->Match(rtfCode);

			String^ name = m->Groups[1]->Value;
			XElement^ element;

			// only use this trick for math elements
			if (name->StartsWith("m"))
				element = gcnew XElement(XNamespace::Get(mathNS) + name->Substring(1));
			else
				element = gcnew XElement(name);

			if (!String::IsNullOrEmpty(m->Groups[2]->Value)) {
				SetElementContent(element, m->Groups[2]->Value, parent, fontTable);
			}

			return element;
		}

		// Sets the element content, which will either be an attribute value or text value
		static void SetElementContent(XElement^ element, String^ text, XElement^ parent, Dictionary<int,String^>^ fontTable) {
			// process escape sequences
			text = text->Replace("\\\\", "\\")->Replace("\\{", "{")->Replace("\\}", "}");

			XNamespace^ ns = XNamespace::Get(mathNS);

			if (parent->Name->LocalName->Equals("fonttbl") && element->Name->LocalName->StartsWith("f")) {
				// build up font table (some elements refer back to it)
				int key = Int32::Parse(element->Name->LocalName->Substring(1));
				String^ fontName = text->Split(';')[0]->Trim();
				fontTable[key] = fontName;
			}
			else if (parent->Name->Namespace->Equals(ns) && parent->Name->LocalName->EndsWith("Pr")) {
				// transform property values
				if (Enumerable::Contains<String^>(fontValues, element->Name->LocalName)) {
					// lookup value in font table
					int key = Int32::Parse(text);
					text = fontTable[key];
				}
				else if (Enumerable::Contains<String^>(onOffValues, element->Name->LocalName)) {
					// "on"/"off" becomes "1"/"0" for XML
					text = text->Replace("on", "1")->Replace("off", "0");
				}
				
				// set 'val' attribute
				element->SetAttributeValue(ns + "val", text);
			}
			else {
				// append element content (as a run of text)
				element->Add(gcnew XElement(ns + "r", gcnew XElement(ns + "t", gcnew XText(text))));
			}
		}

		// Processes an escaped Unicode character code
		static bool ProcessUnicodeLiteral(String^ rtfCode, StringBuilder^ surrogateBuilder, int% i) {
			// unicode literal in the form \u999? where ? is substitution character
			Match^ m = Regex::Match(rtfCode, "^u(-?[0-9]+).?");
			
			if (m->Success) {
				// according to RTF specification, unicode values are 16-bit signed numbers
				Int16 code = Int16::Parse(m->Groups[1]->Value);
				System::Char c = (System::Char)(UInt16)code;
				surrogateBuilder->Append(c);

				// also append anything after the match (sometimes contains literal content)
				surrogateBuilder->Append(rtfCode->Substring(m->Index + m->Length));

				// if substitution character does not follow immediately, skip over next
				if (Regex::IsMatch(rtfCode, "^u(-?[0-9]+)$")) i++;

				// if character is a low surrogate, continue to accumulate characters
				if (!System::Char::IsHighSurrogate(c)) return true;
			}

			// no match or nothing to process
			return false;
		}

		// Gets the full path to the XSL stylesheet used to convert OMML to MathML
		static String^ GetPathToMathXsl() {
			RegistryKey^ key1 = nullptr;
			RegistryKey^ key2 = nullptr;
			String^ xslPath = nullptr;

			try {
				List<RegistryKey^>^ tryKeys = gcnew List<RegistryKey^>();

				// first try Office reg key (matches processor architecture of current process)
				key1 = Registry::LocalMachine->OpenSubKey(msoRegKey);
				if (key1 != nullptr) tryKeys->Add(key1);

				// then try 32-bit Office reg key (if 32-bit Office on 64-bit process)
				key2 = Registry::LocalMachine->OpenSubKey(msoRegKeyWOW64);
				if (key2 != nullptr) tryKeys->Add(key2);

				if (tryKeys->Count == 0) throw gcnew NotSupportedException("Microsoft Office is not installed on this computer.");

				for each (RegistryKey^ key in tryKeys) {
					// get keys corresponding to version numbers
					SortedList<Decimal, String^>^ verKeyNames = gcnew SortedList<Decimal, String^>();
					for each (String^ sub in key->GetSubKeyNames()) {
						Decimal ver;
						if (Decimal::TryParse(sub, ver)) {
							verKeyNames->Add(ver, sub);
						}
					}

					// go from highest version to lowest
					for (int i=(verKeyNames->Count - 1); i>=0; i--) {
						RegistryKey^ subKey = nullptr;

						try {
							subKey = key->OpenSubKey(verKeyNames->Values[i] + msoSubKey);
							if (subKey != nullptr) {
								// get path value
								String^ installRoot = Convert::ToString(subKey->GetValue(msoPathValue));
								if (!String::IsNullOrEmpty(installRoot)) {
									// see if the XSL stylesheet exists in this folder
									String^ tryPath = Path::Combine(installRoot, xslFilename);
								
									if (File::Exists(tryPath)) {
										xslPath = tryPath;
										break;
									}
								}
							}
						}
						finally {
							delete subKey;
						}
					}
				}
			}
			finally {
				delete key1;
				delete key2;
			}

			if (String::IsNullOrEmpty(xslPath)) throw gcnew NotSupportedException("Unable to locate Microsoft Office XSL stylesheet.");

			return xslPath;
		}

		// XML namespace for Office MathML
		literal String^ mathNS = "http://schemas.openxmlformats.org/officeDocument/2006/math";

		// XML namespace for MS Word
		literal String^ wordNS = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

		// OMML elements that use font lookup values
		static initonly array<String^>^ fontValues = { "mathFont" };

		// OMML elements that use boolean values
		static initonly array<String^>^ onOffValues = { 
			"smallFrac", "dispDef", "wrapRight", "lit", "nor", "aln", "opEmu", 
			"noBreak", "diff", "hideTop", "hideBot", "hideLeft", "hideRight", 
			"strikeH", "strikeV", "strikeBLTR", "strikeTLBR", "grow", 
			"maxDist", "objDist", "plcHide", "supHide", "subHide", "show", 
			"zeroWid", "zeroAsc", "zeroDesc", "transp", "degHide", "alnScr"
		};

		// Registry key for Microsoft Office
		literal String^ msoRegKey = "SOFTWARE\\Microsoft\\Office";

		// Registry key for Microsoft Office (32-bit Office on 64-bit Windows)
		literal String^ msoRegKeyWOW64 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office";

		// Registry sub-key for installation directory
		literal String^ msoSubKey = "\\Common\\InstallRoot";

		// Registry value name for installation directory
		literal String^ msoPathValue = "Path";

		// Name of XSL transformation file
		literal String^ xslFilename = "OMML2MML.XSL";

#endif
	};

} NAMESPACE_CLOSE

