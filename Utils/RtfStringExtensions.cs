#if INCLUDE_RTFLIBRARY

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itenso.Rtf;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;
using System.IO;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Converter.Html;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows.Forms;
using SautinSoft;
using System.Runtime.InteropServices;
using Spire.Doc;

namespace Utils
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ConvertSettings
    {
        public int PreserveTables;             //1 - preserve tables, 0 - transfer to text
        public int PreserveImages;             //1 - preserve images,  0 - skip
        public int PreserveHyperlinks;         //1 - preserve hyperlinks, 0 - skip
        public int PreserveFontFace;           //1 - preserve font faces as in HTML, 0 - all font faces will be as in 'FontFace'
        public int PreserveFontSize;           //1 - preserve font sizes as in HTML, 0 - all font sizes will be as in 'FontSize'
        public int PreserveFontColor;          //1 - preserve font colors as in HTML, 0 - black font color
        public int PreserveBackgroundColor;    //1 - preserve bacground colors as in HTML, 0 - skip
        public int PreserveAlignment;          //1 - preserve alignment as in HTML, 0 - all text will have align as in 'PageAlignment'
        public int PreserveTableWidth;         //1 - preserve width of columns
        public int PreserveNestedTables;       //1 - preserve nested tables, 0 - translate nested tables to plain tables
        public int PageMarginLeft;             //page margin left, mm. For example 10
        public int PageMarginRight;            //page margin right, mm. For example 10
        public int PageMarginTop;              //page margin top, mm. For example 10
        public int PageMarginBottom;           //page margin bottom, mm. For example 10
        public int BorderVisibility;           //table borders: 1 - visible borders, 0 - hidden borders, 2 - as in HTML
        public int PageOrientation;            //page orientation: 0 - Portrait, 1 - Landscape
        public int PageSize;                   //page size: 0 - A4, 1 - A3, 2 - A5, 3 - B5, 4 - Letter, 5 - Legal, 6 - Executive, 7 - Monarh
        public int FontFace;                   //default font face: Arial - 0, Times New Roman - 1, Verdana - 2, Helvetica - 3, Courier - 4, Courier New - 5, Times - 6, Georgia - 7, MS Sans Serif - 8,
                                               //Futura - 9, Arial Narrow - 10, Garamond - 11, Impact - 12, Lucida Console - 13, Tahoma - 14, Inform - 15, Symbol - 16, WingDings - 17, Traditional Arabic - 18
        public int FontSize;                   //default font size, any value from 6 to 72
        public int PageAlignment;              //page alignment: 0 - left, 1 - center, 2 - right, 3 - justify
        public int RtfLanguage;                //RTF language: English - 1033, Albanian - 1052, Belgian - 2067, Bulgarian - 1026, Hungarian - 1038, Danish - 1030, Spanish - 3082, Latvian - 1062, Lithuanian - 1063,
                                               //German - 1031, Netherlands - 1043, Norwegian - 2068, Portuguese - 2070, Romanian - 1048, Russian - 1049, Ukrainian - 1058, Finnish - 1035, French - 1036,
                                               //Czech - 1029, Swedish - 1053, Arabic - 1053, Turkish - 1055, Japanese - 932, SimplifiedChinese - 936, TraditionalChinese - 950, Korean - 949, Thai = 874
        public int Encoding;                   //AutoSelect - 0, ISO-8859-1 - 1, ISO-8859-5 - 2, KOI8-R - 3, Windows-1251 - 4, UTF-8 - 5, Windows-1254 - 6, Windows-1256 - 7,
                                               //Windows-1250 - 8, Windows-1252 - 9, Windows-1253 - 10, Windows-1255 - 11, Windows-1257 - 12, Windows-1258 - 13
        public int OutputTextFormat;           //Output Format: Rtf - 0, Text - 1, Doc - 2 (only file with .doc extension)  
        public int PreservePageBreaks;         //1 - preserve page-breaks
        public int ImageCompatible;            //type of produced images: Word - 0, WordPad - 1
        public int PageNumbers;                //page numbers: 0 - disable, 1 - numbers from first page, 2 - from second page

        public fixed char PageHeader[150];           //page header, any string
        public fixed char PageFooter[150];           //page footer, any string
        public fixed char HtmlPath[650];             //html path for method 'htmltortf_string', will be used for find images (now is not used)
        public int PageNumbersAlignV;          //page numbers vertical align: Top - 4, Bottom - 5
        public int PageNumbersAlignH;          //page numbers horizontal align: 0 - left, 1 - center, 2 - right, 3 - justify
        public int PreserveHR;                 //1 - preseve <hr>, 0 - skip
        public int RtfParts;                   //0 - rtf completely, 1 - only rtf body (to insert inside another rtf files)
        public int CreateTraceFile;            //1 - the component will create trace file, it helps to see how converting goes and shows errors
        public fixed char TraceFilePath[650];        //specifies path for trace file, for example "c:\\Trace.txt"
        public int TableCellPadding;           //specifies table cell padding in pixels, any value from 0 to 10
        public int PreserveHttpImages;         //1 - download remote images, 0 - skip remote images
    };


    public static class RtfStringExtensions
    {
        [DllImport("htmltortf_sautinsoft.dll", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string htmltortf_string([MarshalAs(UnmanagedType.LPStr)] string html, [MarshalAs(UnmanagedType.LPStr)] string rtf, [MarshalAs(UnmanagedType.Struct)] ConvertSettings convertSettings);

        public enum ProgramExitCode
        {
            Successfully = 0,
            InvalidSettings = -1,
            ParseRtf = -2,
            DestinationDirectory = -3,
            InterpretRtf = -4,
            ConvertHtml = -5,
            SaveHtml = -6,
        }

        public static string HtmlToRtf(this string html)
        {
            using (var htmlStream = html.ToStream())
            {
                using (var rtfStream = new MemoryStream())
                {
                    using (var document = new Document(htmlStream, FileFormat.Html))
                    {
                        document.SaveToStream(rtfStream, FileFormat.Rtf);

                        rtfStream.Rewind();

                        return rtfStream.ToText();
                    }
                }
            }
        }

        public static string HtmlToText(this string html)
        {
            using (var htmlStream = html.ToStream())
            {
                using (var rtfStream = new MemoryStream())
                {
                    using (var document = new Document(htmlStream, FileFormat.Html))
                    {
                        document.SaveToStream(rtfStream, FileFormat.Txt);

                        rtfStream.Rewind();

                        return rtfStream.ToText();
                    }
                }
            }
        }


        public static string RtfToHtml(this string str, RtfHtmlConvertSettings htmlConvertSettings, bool throwOnError = false, IRtfParserListener listener = null, IRtfVisualImageAdapter imageAdapter = null, string imageAdapterLogFile = null, RtfImageConvertSettings imageConvertSettings = null)
        {
            IRtfGroup rtfStructure;
            IRtfDocument rtfDocument = null;

            try
            {
                using (var stream = str.ToStream())
                {
                    // parse the rtf structure
                    var structureBuilder = new RtfParserListenerStructureBuilder();
                    var parser = new RtfParser(structureBuilder);

                    parser.IgnoreContentAfterRootGroup = true; // support WordPad documents

                    if (listener != null)
                    {
                        parser.AddParserListener(listener);
                    }

                    parser.Parse(new RtfSource(stream));
                    rtfStructure = structureBuilder.StructureRoot;

                    ThrowOnUnexpectedExitCode();

                    rtfDocument = InterpretRtf(rtfStructure, imageAdapterLogFile, imageConvertSettings, throwOnError);

                    if (throwOnError)
                    {
                        ThrowOnUnexpectedExitCode();
                    }

                    // convert to hmtl

                    string html = ConvertHtml(rtfDocument, imageAdapter, htmlConvertSettings, throwOnError);

                    if (throwOnError)
                    {
                        ThrowOnUnexpectedExitCode();
                    }

                    return html;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("RtfToHtml parser failed with error: {0}", e.Message);
            }

            return null;
        }

        private static string ConvertHtml(IRtfDocument rtfDocument, IRtfVisualImageAdapter imageAdapter, RtfHtmlConvertSettings htmlConvertSettings, bool throwOnError = false)
        {
            string html = null;

            try
            {
                var htmlConverter = new RtfHtmlConverter(rtfDocument, htmlConvertSettings);
                html = htmlConverter.Convert();
            }
            catch (Exception e)
            {
                if (throwOnError)
                {
                    ThrowOnUnexpectedExitCode(e);
                }
            }

            return html;
        }

        private static IRtfDocument InterpretRtf(IRtfGroup rtfStructure, string imageAdapterLogFile = null, RtfImageConvertSettings imageConvertSettings = null, bool throwOnError = false)
        {
            IRtfDocument rtfDocument;
            RtfInterpreterListenerFileLogger interpreterLogger = null;

            try
            {
                // logger
                if (imageAdapterLogFile != null)
                {
                    interpreterLogger = new RtfInterpreterListenerFileLogger(imageAdapterLogFile);
                }

                // image converter
                RtfImageConverter imageConverter = null;

                if (imageConvertSettings != null)
                {
                    imageConverter = new RtfImageConverter(imageConvertSettings);
                }

                // rtf interpreter
                RtfInterpreterSettings interpreterSettings = new RtfInterpreterSettings();
                interpreterSettings.IgnoreDuplicatedFonts = true;
                interpreterSettings.IgnoreUnknownFonts = true;

                // interpret the rtf structure using the extractors
                rtfDocument = RtfInterpreterTool.BuildDoc(rtfStructure, interpreterSettings, interpreterLogger, imageConverter);

            }
            catch (Exception e)
            {
                if (interpreterLogger != null)
                {
                    interpreterLogger.Dispose();
                }

                if (throwOnError)
                {
                    ThrowOnUnexpectedExitCode(e);
                }

                return null;
            }

            return rtfDocument;
        }

        private static void ThrowOnUnexpectedExitCode(Exception ex = null)
        {
            string error = null;

            if (ex != null)
            {
                error = ex.Message;
            }

            if (Environment.ExitCode != (int)ProgramExitCode.Successfully)
            {
                if (error != null)
                {
                    throw new Exception(string.Format("RtfToHtml parser failed with exit code: {0}, Error: {1}", ((ProgramExitCode)Environment.ExitCode).ToString(), error));
                }
                else
                {
                    throw new Exception(string.Format("RtfToHtml parser failed with exit code: {0}", ((ProgramExitCode)Environment.ExitCode).ToString()));
                }
            }
            else if (error != null)
            {
                throw ex;
            }
        }
    }
}

#endif