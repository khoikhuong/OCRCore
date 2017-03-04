using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OCRCore.App_Start
{
    #region "Callback Delegates"
    
    // Delegate for progress callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrProgressCallbackDelegate(int nEvent, double dProgress, IntPtr pCallbackData);

    // Delegate for scan callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrScanCallbackDelegate(int nEvent, IntPtr hBitmap, IntPtr pCallbackData);

    // Delegate for log callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrLogCallbackDelegate([In] [MarshalAsAttribute(UnmanagedType.LPWStr)] string pwchLogMsg, IntPtr pCallbackData);

    // Delegate for region callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrRegionCallbackDelegate(int nPageIndex, int nRegionIndex, int nCode, IntPtr pCallbackData);

    // Delegate for page callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrPageCallbackDelegate(int nPageIndex, int nCode, IntPtr pCallbackData);

    // Delegate for document callback function
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate int OcrDocumentCallbackDelegate(int nDocumentIndex, int nDocPageIndex, int nCode, IntPtr pCallbackData);
    
    #endregion

    public static class SmartOcrSdkExports
    {
        public const string LIB_PATH = "Redistributables\\";
        public const string LIB_USER = "Acom";
        public const string LIB_KEY = "41D5B-F352C-CQ41A-CB178-215D7-71E3F-2626223KCF11B5";

        private const string strDllName = LIB_PATH + "SmartOCR_SDK.dll";

        #region "Constants"

        // Result codes
        public const int RES_SUCCESS = 0;
        public const int RES_ERROR = -1;
        public const int RES_ERROR_FNF = -2;
        public const int RES_ABORT = -3;

        // Document recognition modes
        public const int OCR_RM_DOCUMENT = 0;
        public const int OCR_RM_FORM = 1;

        // Supported output document formats
        public const int DFT_RTF = 1;
        public const int DFT_DOC = 2;
        public const int DFT_PDF = 3;
        public const int DFT_HTML = 4;
        public const int DFT_TXT = 5;

        // Region types
        public const int OCR_RT_UNDEFINED = -1;
        public const int OCR_RT_IMAGE = 1;
        public const int OCR_RT_TEXT = 2;
        public const int OCR_RT_TABLE = 3;
        public const int OCR_RT_VIRTUAL = 4;

        // Task types
        public const int OCR_TT_AUTOROTATE = 1;
        public const int OCR_TT_LOCATE = 2;
        public const int OCR_TT_PREPROCESS = 3;
        public const int OCR_TT_AUTOROTATE_ALL = 4;
        public const int OCR_TT_LOCATE_ALL = 5;
        public const int OCR_TT_PREPROCESS_ALL = 6;
        public const int OCR_TT_RECOGNIZE = 7;
        public const int OCR_TT_RECOGNIZE_ALL = 8;
        public const int OCR_TT_LOAD_PDF = 9;

        // Script types
        public const int OCR_ST_NONE = 0;
        public const int OCR_ST_SUBSCRIPT = 1;
        public const int OCR_ST_SUPERSCRIPT = 2;

        // Font types
        public const int OCR_FT_UNKNOWN = 0;
        public const int OCR_FT_SERIF = 1;
        public const int OCR_FT_SAN_SERIF = 2;
        public const int OCR_FT_MONOSPACED = 3;

        // Paragraph alignments
        public const int OCR_PA_UNKNOWN	= 0;
        public const int OCR_PA_LEFT = 1;
        public const int OCR_PA_RIGHT = 2;
        public const int OCR_PA_CENTERED = 3;
        public const int OCR_PA_JUSTIFIED = 4;

        // Scanner events
        public const int OCR_SE_SCANNER_READY = 1;
        public const int OCR_SE_IMAGE_SCANNED = 2;
        public const int OCR_SE_SCANNER_CLOSE = 3;
        public const int OCR_SE_GENERIC_ERROR = -1;

        // Recognition predefined character sets
        public const int OCR_RCS_ALL = 0;
        public const int OCR_RCS_ALPHA = 1;
        public const int OCR_RCS_NUMERIC = 2;
        public const int OCR_RCS_ALPHA_NUMERIC = 3;
        public const int OCR_RCS_MONEY = 4;

        // Glyph splitting modes
        public const int OCR_GS_ENABLED = 0;
        public const int OCR_GS_DISABLED = 1;

		// Recognition language IDs
		public const int OCR_LANG_ENGLISH = 1;
		public const int OCR_LANG_GERMAN = 2;
		public const int OCR_LANG_FRENCH = 3;
		public const int OCR_LANG_SPANISH = 4;
        public const int OCR_LANG_ITALIAN = 5;
        public const int OCR_LANG_RUSSIAN = 6;
        public const int OCR_LANG_BULGARIAN = 7;
        public const int OCR_LANG_CZECH = 8;
        public const int OCR_LANG_POLISH = 9;
        public const int OCR_LANG_HUNGARIAN = 10;
        public const int OCR_LANG_UKRAINIAN = 11;
        public const int OCR_LANG_ROMANIAN = 12;
        public const int OCR_LANG_TURKISH = 13;
        public const int OCR_LANG_PORTUGUESE = 14;
        public const int OCR_LANG_DUTCH = 15;
        public const int OCR_LANG_DANISH = 16;
        public const int OCR_LANG_FINNISH = 17;
        public const int OCR_LANG_SWEDISH = 18;
        public const int OCR_LANG_NORWEGIAN = 19;

        // Font variations
        public const int OCR_FONTVAR_ALL = 0;
        public const int OCR_FONTVAR_NORMAL = 1;
        public const int OCR_FONTVAR_BOLD = 2;
        public const int OCR_FONTVAR_ITALIC = 4;

        // Custom glyph types
        public const int OCR_CGLYPH_TYPE_NORMAL = 0;
        public const int OCR_CGLYPH_TYPE_BOLD = 1;
        public const int OCR_CGLYPH_TYPE_ITALIC = 2;
        public const int OCR_CGLYPH_TYPE_BOLD_IT = 3;

        // Log verbosity levels
        public const int OCR_LOG_VERBOSITY_DISABLED = 0;
        public const int OCR_LOG_VERBOSITY_VERY_LOW = 1;
        public const int OCR_LOG_VERBOSITY_LOW = 2;
        public const int OCR_LOG_VERBOSITY_NORMAL = 3;
        public const int OCR_LOG_VERBOSITY_HIGH = 4;
        public const int OCR_LOG_VERBOSITY_VERY_HIGH = 5;

        // Page image quality levels
        public const int OCR_PIQ_UNKNOWN = -1;
        public const int OCR_PIQ_LOW = 1;
        public const int OCR_PIQ_AVERAGE = 2;
        public const int OCR_PIQ_HIGH = 3;

        // Output image formats
        public const int OCR_IF_PNG = 1;
        public const int OCR_IF_JPEG = 2;
        public const int OCR_IF_TIFF = 3;
        public const int OCR_IF_GIF = 4;
        public const int OCR_IF_BMP = 5;

        // OCR option codes
        public const int OCR_OPTION_ENABLE_AUTO_ORIENTATION = 1;            // Boolean
        public const int OCR_OPTION_ENABLE_ADVANCED_TEXT_VALIDATION = 2;    // Boolean
        public const int OCR_OPTION_ENABLE_ADVANCED_REC_CACHE = 3;          // Boolean
        public const int OCR_OPTION_ENABLE_SIMULTANEOUS_MULTI_OCR = 4;      // Boolean
        public const int OCR_OPTION_GRAPHICS_CLEANUP_MODE = 5;              // Integer - 0 - disabled, 1 - normal, 2 - aggressive
        public const int OCR_OPTION_DETECT_TEXT_ONLY = 6;                   // Boolean
        public const int OCR_OPTION_MIN_TEMPLATE_MATCH_PERCENT = 7;         // Integer - %
        public const int OCR_OPTION_ENABLE_VECTOR_PDF_PROCESSING = 8;       // Boolean
        public const int OCR_OPTION_ENABLE_HYBRID_REGION_TEMPLATES = 9;     // Boolean
        public const int OCR_OPTION_ENABLE_REC_RESULTS_FILTRATION = 10;     // Boolean
        public const int OCR_OPTION_ENABLE_AUTO_PAGE_IMAGE_DESPECKLE = 11;  // Boolean

        // Page callback codes
        public const int OCR_PCC_PAGE_LOADED = 1;
        public const int OCR_PCC_PAGE_BEFORE_REMOVE = 2;
        public const int OCR_PCC_PAGE_REMOVED = 4;

        // Image transformation options
        public const int OCR_IMG_TRANSFORM_NONE = 0;
        public const int OCR_IMG_TRANSFORM_CROP = 1;
        public const int OCR_IMG_TRANSFORM_RESIZE = 2;
        public const int OCR_IMG_TRANSFORM_ROTATE = 4;
        public const int OCR_IMG_TRANSFORM_INC_CONTRAST = 8;

        // Interpolation types
        public const int OCR_INTERPOLATE_BICUBIC_H = 1;
        public const int OCR_INTERPOLATE_BICUBIC_S = 2;
        public const int OCR_INTERPOLATE_BOX = 3;
        public const int OCR_INTERPOLATE_HERMIT = 4;
        public const int OCR_INTERPOLATE_HAMING = 5;
        public const int OCR_INTERPOLATE_HANING = 6;
        public const int OCR_INTERPOLATE_SINC = 7;
        public const int OCR_INTERPOLATE_BLACKMAN = 8;
        public const int OCR_INTERPOLATE_BSPLINE = 9;

        // Text filter types
        public const int CAP_TFT_DATE = 0;
        public const int CAP_TFT_CURRENCY = 1;
        public const int CAP_TFT_INVOICENUM = 2;
        public const int CAP_TFT_DISCOUNT = 3;
        public const int CAP_TFT_CUSTOM = 1000;

        // Field data types
        public const int CAP_FDT_ALL = 0;
        public const int CAP_FDT_VENDORS = 1;
        public const int CAP_FDT_ITEMS = 2;

        // Region callback codes
        public const int CAP_RCC_REGION_CREATED = 1;
        public const int CAP_RCC_REGION_REMOVED = 2;
        public const int CAP_RCC_RECT_CHANGED = 4;
        public const int CAP_RCC_NAME_CHANGED = 8;
        public const int CAP_RCC_TYPE_CHANGED = 16;
        public const int CAP_RCC_CHARSET_CHANGED = 32;
        public const int CAP_RCC_REGION_BEFORE_CREATE = 64;
        public const int CAP_RCC_REGION_BEFORE_REMOVE = 128;
        public const int CAP_RCC_REGION_CONTENT_CHANGED = 256;
        public const int CAP_RCC_REGION_TEMPLATE_CHANGED = 512;

        // Capture layer document types
        public const int CAP_DOC_TYPE_UNKNOWN = -1;
        public const int CAP_DOC_TYPE_INVOICE = 1;
        public const int CAP_DOC_TYPE_ORDER = 2;
        public const int CAP_DOC_TYPE_PACKING_SLIP = 3;
        public const int CAP_DOC_TYPE_UPS_LABEL = 4;
        public const int CAP_DOC_TYPE_CONTRACT = 5;

        // Capture layer option codes
        public const int CAP_OPTION_SUPPORT_MULTIROW = 1;                   // Boolean
        public const int CAP_OPTION_SKIP_LOW_QUALITY_IMAGES = 2;            // Boolean
        public const int CAP_OPTION_RECOGNIZE_ONLY_TEMPLATE_FIELDS = 3;     // Boolean
        public const int CAP_OPTION_SUPPORT_MULTIPAGE = 4;                  // Boolean
        public const int CAP_OPTION_ENABLE_FULL_REC_FOR_INACTIVE_PAGES = 5; // Boolean

        // Capture layer field format types
        public const int CAP_FF_TEXT = 0;
        public const int CAP_FF_BOOL = 1;
        public const int CAP_FF_INTEGER = 2;
        public const int CAP_FF_DOUBLE = 3;
        public const int CAP_FF_MONEY = 4;
        public const int CAP_FF_DATE = 5;

        // Document callback codes
        public const int CAP_DCC_DOC_INSERTED = 1;
        public const int CAP_DCC_DOC_BEFORE_REMOVE = 2;
        public const int CAP_DCC_DOC_REMOVED = 4;
        public const int CAP_DCC_DOC_TYPE_CHANGED = 8;
        public const int CAP_DCC_DOC_PAGE_INSERTED = 16;
        public const int CAP_DCC_DOC_PAGE_BEFORE_REMOVE = 32;
        public const int CAP_DCC_DOC_PAGE_REMOVED = 64;

        #endregion

        #region "DataTypes"

        // Integer rectangle structure
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct TOCR_RECT
        {
            public int m_nLeft;
            public int m_nTop;
            public int m_nRight;
            public int m_nBottom;
        }

        // Real rectangle structure
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct TOCR_DOUBLE_RECT
        {
	        public double m_dLeft;
            public double m_dTop;
            public double m_dRight;
            public double m_dBottom;
        }

        // Structure for text properties
        public const int MAX_FONT_NAME_LENGTH = 256;
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct TextProperties
        {
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public bool m_bBold;

            [MarshalAsAttribute(UnmanagedType.Bool)]
            public bool m_bItalic;

            public int m_nTextColor;
            public int m_nBackgroundColor;

            [MarshalAsAttribute(UnmanagedType.Bool)]
            public bool m_bUnderlined;

            public int m_nScriptType;
            public double m_dFontSize;
            public int m_nFontType;
            public double m_dXScaleFactor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FONT_NAME_LENGTH)]
            public string m_wchFontName;
        }

        // Structure for paragraph properties
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct ParagraphProperties
        {
            public double m_dRelativeLineSpacing;
            public double m_dAbsoluteLineSpacing;
            public int m_nParagraphAlignment;
            public double m_dLeftBasePos;
            public double m_dRightBasePos;
            public double m_dAfterParagraphSpacing;
            public double m_dFirstLineIndent;
            public double m_dTabSize;
            public int m_nBaseTabs;
        }

        // Structure for character information
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct CharInfo
        {
            public ushort m_wchCode;
            public TOCR_DOUBLE_RECT m_BoundingRect;
            public int m_nTextPropertiesIndex;
            public int m_nRowIndex;
            public int m_nConfidence;
        }

        // Image transformation params
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct ImageTransformParams
        {
            public int m_nTransformOptions;
            public int m_nNewWidth;
            public int m_nNewHeight;
            public int m_nCropLeft;
            public int m_nCropTop;
            public int m_nCropRight;
            public int m_nCropBottom;
            public double m_dAngleDegreesRotate;
            public double m_dGammaCorection;
            public int m_nInterpolationType;
        }

        #endregion

        #region "OCR SDK Layer API Functions"

        // Init SDK
        [DllImport(strDllName, EntryPoint = "OCR_Init_W", 
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_Init([In] string strUserName,
                                          [In] string strLicenseKey,
                                          [In] string strIntsallPath,
                                          [In] string strTempDir);
      
        // Release SDK
        [DllImport(strDllName, EntryPoint = "OCR_ReleaseAllAndDestroySDK", 
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ReleaseAll();

        // Set recognition mode to OCR_RM_DOCUMENT (default) or OCR_RM_FORM
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionMode",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRecognitionMode(int nRecognitionMode);

        // Set recognition character set to 'nRecognitionCharset' must be 'OCR_RCS_ALL' (default), 'OCR_RCS_ALPHA', 'OCR_RCS_NUMERIC' or 'OCR_RCS_ALPHA_NUMERIC'
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionCharset",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRecognitionCharset(int nRecognitionCharset, int nPageIndex, int nRegionIndex);

        // Set a custom character set for recognition
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionCustomCharset",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SetRecognitionCustomCharset([In] string strAllowedChars, int nPageIndex, int nRegionIndex);

        // Set glyph splitting mode to 'OCR_GS_ENABLED' (default) or 'OCR_GS_DISABLED'
        [DllImport(strDllName, EntryPoint = "OCR_SetGlyphSplittingMode",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetGlyphSplittingMode(int nGlyphSplittingMode);

        // Set global text to image conversion trigger. Valid values: [0 - 100]. Default value: 85. 
        // Pass 0 in order to disable dynamic text to image conversion.
        [DllImport(strDllName, EntryPoint = "OCR_SetTextToImageConversionTrigger",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetTextToImageConversionTrigger(int nRecognitionMode);

        // Add page from image file
        [DllImport(strDllName, EntryPoint = "OCR_AddPageFromImage_W", 
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_AddPageFromImage([In] string strImagePath);

        // Add pages from PDF file
        [DllImport(strDllName, EntryPoint = "OCR_AddPagesFromPDF_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_AddPagesFromPDF([In] string strPdfPath, OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Add page from HBITMAP
        [DllImport(strDllName, EntryPoint = "OCR_AddPageFromHBITMAP",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AddPageFromHBITMAP(IntPtr hBitmap, int nDpiX, int nDpiY);

        // Add page from memory bitmap buffer
        [DllImport(strDllName, EntryPoint = "OCR_AddPageFromMemoryBitmap",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AddPageFromMemoryBitmap([In] byte [] bmpBuffer, int nBufferSize);

        // Recognize specified page
        [DllImport(strDllName, EntryPoint = "OCR_RecognizePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RecognizePage(int nPageIndex, OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Recognize all pages
        [DllImport(strDllName, EntryPoint = "OCR_RecognizeAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RecognizeAllPages(OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Recognize the content of a given text region
        [DllImport(strDllName, EntryPoint = "OCR_RecognizeRegion",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RecognizeRegion(int nPageIndex, int nRegionIndex, int nAlwaysAsText,
                                                     OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Recognize the contents of specified text regions
        [DllImport(strDllName, EntryPoint = "OCR_RecognizeRegions",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RecognizeRegions(int nPageIndex, int[] arrRegionIndexes, int nRegionIndexCount,
                                                      OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Export document
        [DllImport(strDllName, EntryPoint = "OCR_ExportDocument_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExportDocument([In] string strPath, int nDocType, int nOptions, 
                                                    int nSplitPages, int nAddExtension);

        // Export specific page
        [DllImport(strDllName, EntryPoint = "OCR_ExportPage_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExportPage(int nPageIndex, [In] string strPath, int nDocType, 
                                                int nOptions, int nAddExtension);

        // Get recognized text
        [DllImport(strDllName, EntryPoint = "OCR_GetRecognizedText_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetRecognizedText(StringBuilder sbTextBuffer, int nResultBufferSize);

        // Get recognized text from specified page
        [DllImport(strDllName, EntryPoint = "OCR_GetRecognizedTextFromPage_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetRecognizedTextFromPage(int nPageIndex, StringBuilder sbTextBuffer, 
                                                               int nResultBufferSize);

        // Get recognized text from specified page region
        [DllImport(strDllName, EntryPoint = "OCR_GetRecognizedTextFromPageRegion_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetRecognizedTextFromPageRegion(int nPageIndex, int nRegionIndex,
                                                                     StringBuilder sbTextBuffer, int nResultBufferSize);
 
        // Remove specified page from document
        [DllImport(strDllName, EntryPoint = "OCR_RemovePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RemovePage(int nPageIndex);

        // Remove all pages from document
        [DllImport(strDllName, EntryPoint = "OCR_RemoveAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RemoveAllPages();

        // Get total page count in document
        [DllImport(strDllName, EntryPoint = "OCR_GetPageCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageCount();
        
        // Autorotate specified page
        [DllImport(strDllName, EntryPoint = "OCR_AutorotatePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AutorotatePage(int nPageIndex, OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Locate regions in specified page
        [DllImport(strDllName, EntryPoint = "OCR_LocateRegionsInPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_LocateRegionsInPage(int nPageIndex, OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Preprocess page
        [DllImport(strDllName, EntryPoint = "OCR_PreprocessPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_PreprocessPage(int nPageIndex, OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Autorotate all pages
        [DllImport(strDllName, EntryPoint = "OCR_AutorotateAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AutorotateAllPages(OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Locate regions in all pages
        [DllImport(strDllName, EntryPoint = "OCR_LocateRegionsInAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_LocateRegionsInAllPages(OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Preprocess all pages
        [DllImport(strDllName, EntryPoint = "OCR_PreprocessAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_PreprocessAllPages(OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Rotate page image to specific angle
        [DllImportAttribute(strDllName, EntryPoint = "OCR_RotatePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RotatePage(int nPageIndex, int nAngleDegrees);

        // Get region count in specified page
        [DllImport(strDllName, EntryPoint = "OCR_GetRegionCountInPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetRegionCountInPage(int nPageIndex);
        
        // Delete all regions in page
        [DllImport(strDllName, EntryPoint = "OCR_DeleteAllRegionsInPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_DeleteAllRegionsInPage(int nPageIndex);

        // Delete specified region in page
        [DllImport(strDllName, EntryPoint = "OCR_DeleteRegionInPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_DeleteRegionInPage(int nPageIndex, int nRegionIndex);

        // Get region info
        [DllImport(strDllName, EntryPoint = "OCR_GetRegionInfo",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetRegionInfo(int nPageIndex, int nRegionIndex, 
                                                   ref int nRegionType, ref TOCR_RECT rectRegion);

        // Add region in page
        [DllImport(strDllName, EntryPoint = "OCR_AddCustomRegionInPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AddCustomRegionInPage(int nPageIndex, int nRegionType, TOCR_RECT rectRegion);

        // Set region type of a region in page
        [DllImport(strDllName, EntryPoint = "OCR_SetRegionType",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRegionType(int nPageIndex, int nRegionIndex, int nRegionType);

        // Set the rectangle of a given region
        [DllImport(strDllName, EntryPoint = "OCR_SetRegionRect",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRegionRect(int nPageIndex, int nRegionIndex, TOCR_RECT rectRegion, Boolean bUpdateText);

        // Set region name, useful for form recognition
        [DllImport(strDllName, EntryPoint = "OCR_SetRegionName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SetRegionName(int nPageIndex, int nRegionIndex, [In] string strRegionName);

        // Get the name of a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetRegionName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetRegionName(int nPageIndex, int nRegionIndex, StringBuilder sbNameBuffer, int nNameBufferSize);

        // Find region in given page by its name and get its index
        [DllImport(strDllName, EntryPoint = "OCR_FindRegionByName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_FindRegionByName(int nPageIndex, [In] string strRegionName);

        // Get HBITMAP from page image
        [DllImport(strDllName, EntryPoint = "OCR_GetHBITMAP_FromPageImage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetHBITMAP_FromPageImage(int nPageIndex, ref IntPtr hBitmap);

        // Get text row count from a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionRowCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionRowCount(int nPageIndex, int nRegionIndex);

        // Get paragraph properties index for a given text row
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionRowParagraphIndex",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionRowParagraphIndex(int nPageIndex, int nRegionIndex, int nRowIndex);

        // Get text properties count for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetTextPropertiesCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetTextPropertiesCount(int nPageIndex, int nRegionIndex);

        // Get paragraph properties count for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetParagraphPropertiesCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetParagraphPropertiesCount(int nPageIndex, int nRegionIndex);

        // Get all text properties for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetTextProperties",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetTextProperties(int nPageIndex, int nRegionIndex, 
                                                       [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] 
                                                       [In, Out] TextProperties[] pTextProperties, int nAllocatedElementCount);


        // Get all paragraph properties for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetParagraphProperties",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetParagraphProperties(int nPageIndex, int nRegionIndex,
                                                            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] 
                                                            [In, Out] ParagraphProperties [] pParaProperties, int nAllocatedElementCount);

        // Get recognized character count for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionCharacterCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionCharacterCount(int nPageIndex, int nRegionIndex);

        // Get recognized character count for a given text row
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionRowCharacterCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionRowCharacterCount(int nPageIndex, int nRegionIndex, int nRowIndex);

        // Get recognized alternate character count for a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionAltCharacterCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionAltCharacterCount(int nPageIndex, int nRegionIndex);

        // Get info about recognized characters from a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionCharacterInfo",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionCharacterInfo(int nPageIndex, int nRegionIndex,
                                                                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] 
                                                                [In, Out] CharInfo [] pCharInfoBuffer, int nAllocatedElementCount);

        // Get info about recognized characters from a given text row
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionRowCharacterInfo",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionRowCharacterInfo(int nPageIndex, int nRegionIndex, int nRowIndex, 
                                                                   [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 4)] 
                                                                   [In, Out] CharInfo[] pCharInfoBuffer, int nAllocatedElementCount);

        // Get info about recognized alternate characters from a given region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionAltCharacterInfo",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionAltCharacterInfo(int nPageIndex, int nRegionIndex,
                                                                   [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] 
                                                                   [In, Out] CharInfo[] pCharInfoBuffer, int nAllocatedElementCount);

        // Enable or disable the usage of default dictionary
        [DllImport(strDllName, EntryPoint = "OCR_UseDefaultDictionary",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_UseDefaultDictionary([MarshalAs(UnmanagedType.Bool)] bool bUseDefaultDictionary);

        // Determine if the default dictionary is in use
        [DllImport(strDllName, EntryPoint = "OCR_IsDefaultDictionaryUsed",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OCR_IsDefaultDictionaryUsed();

        // Load a specific user dictionary
        [DllImport(strDllName, EntryPoint = "OCR_LoadUserDictionary_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_LoadUserDictionary([In] string pwchFileName);

        // Save the user dictionary to file
        [DllImport(strDllName, EntryPoint = "OCR_SaveUserDictionary_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SaveUserDictionary([In] string pwchFileName);

        // Add a word to user dictionary
        [DllImport(strDllName, EntryPoint = "OCR_AddWord_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_AddWord([In] string pwchWord);

        // Check if word is in dictionary
        [DllImport(strDllName, EntryPoint = "OCR_IsWordInDictionary_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OCR_IsWordInDictionary([In] string pwchWord);

        // Check if word is in dictionary (the difference in letter case will be ignored)
        [DllImport(strDllName, EntryPoint = "OCR_IsWordInDictionaryIgnoreCase_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OCR_IsWordInDictionaryIgnoreCase([In] string pwchWord);

        // Clear the content of user dictionary
        [DllImport(strDllName, EntryPoint = "OCR_ClearUserDictionary",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ClearUserDictionary();

        // Load region template from external file
        [DllImport(strDllName, EntryPoint = "OCR_LoadRegionTemplate_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_LoadRegionTemplate([In] string pwchFile, int nPageIndex);

        // Save region template to external file
        [DllImport(strDllName, EntryPoint = "OCR_SaveRegionTemplate_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SaveRegionTemplate([In] string pwchFile, int nPageIndex);

        // Save region template to external file with additional control over page template ownership
        [DllImport(strDllName, EntryPoint = "OCR_SaveRegionTemplateEx_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SaveRegionTemplateEx([In] string pwchFile, int nPageIndex, [MarshalAs(UnmanagedType.Bool)] bool bUpdatePageTemplate);

        // Get match rate in percents between given region template and specific page
        [DllImport(strDllName, EntryPoint = "OCR_GetRegionTemplateMatchRate_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetRegionTemplateMatchRate([In] string pwchFile, int nPageIndex);

        // Get template transformation as XY translation for a given page
        [DllImport(strDllName, EntryPoint = "OCR_GetTemplateTransformation",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetTemplateTransformation(int nPageIndex, ref int pTranslationX, ref int pTranslationY);

        // Set selection state of a given page
        [DllImport(strDllName, EntryPoint = "OCR_SelectPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SelectPage(int nPageIndex, [MarshalAs(UnmanagedType.Bool)] bool bSelect);

        // Set selection state of all pages
        [DllImport(strDllName, EntryPoint = "OCR_SelectAllPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SelectAllPages([MarshalAs(UnmanagedType.Bool)] bool bSelect);

        // Recognize selected pages 
        [DllImport(strDllName, EntryPoint = "OCR_RecognizeSelectedPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_RecognizeSelectedPages(OcrProgressCallbackDelegate callback, IntPtr pCallbackData);

        // Export selected pages if they are recognized
        [DllImport(strDllName, EntryPoint = "OCR_ExportSelectedPages_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExportSelectedPages([In] string strPath, int nDocType, int nOptions,
                                                         int nSplitPages, int nAddExtension);

        // Check if a given page is blank (does not contain text)
        [DllImport(strDllName, EntryPoint = "OCR_RecognizeRegions",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OCR_IsBlankPage(int nPageIndex);

        // Init PDF loader for loading PDF files in worker thread
        [DllImport(strDllName, EntryPoint = "OCR_InitPdfLoader",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_InitPdfLoader();

        // Release PDF loader from a worker thread used to load PDF files
        [DllImport(strDllName, EntryPoint = "OCR_ReleasePdfLoader",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ReleasePdfLoader();

        // Init scanner interface
        [DllImport(strDllName, EntryPoint = "OCR_InitScan",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_InitScan();

        // Get installed scanner identifiers
        [DllImport(strDllName, EntryPoint = "OCR_GetScanners",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetScanners(StringBuilder sbBuffer, int nBufferSize);

        // Set active scanner by ID
        [DllImport(strDllName, EntryPoint = "OCR_SetActiveScanner",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SetActiveScanner([In] string pwchScannerID);

        // Perform asynchronous scan
        [DllImport(strDllName, EntryPoint = "OCR_PerformScan",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_PerformScan(OcrScanCallbackDelegate callback, int nOptions, IntPtr pCallbackData);

        // Get page image size in pixels
        [DllImport(strDllName, EntryPoint = "OCR_GetPageSize",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageSize(int nPageIndex, ref int pPageWidth, ref int pPageHeight);

        // Auto-resize all regions from given page so their bounding box fit exactly to their text content
        [DllImport(strDllName, EntryPoint = "OCR_AutofitPageRegions",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AutofitPageRegions(int nPageIndex);

        // Auto-resize specific regions of given page so the bounding box of the region fits exactly to its text content
        [DllImport(strDllName, EntryPoint = "OCR_AutofitRegion",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AutofitRegion(int nPageIndex, int nRegionIndex);

        // Specify log callback delegate that will receive log information from the SDK according to specified log verbosity
        [DllImport(strDllName, EntryPoint = "OCR_SetLogCallback",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetLogCallback(OcrLogCallbackDelegate callback, IntPtr pCallbackData, int nLogVerbosity);

        // Add custom glyph to recognition cache from HBITMAP
        [DllImport(strDllName, EntryPoint = "OCR_AddCustomGlyphFromHandle",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_AddCustomGlyphFromHandle(IntPtr hBitmapGlyph, char wchGlyphCode, int nOptions);

        // Add custom glyph to recognition cache from file
        [DllImport(strDllName, EntryPoint = "OCR_AddCustomGlyphFromFile_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_AddCustomGlyphFromFile([In] string pwchFile, char wchGlyphCode, int nOptions);

        // Clear all added custom glyphs
        [DllImport(strDllName, EntryPoint = "OCR_ClearCustomGlyphs",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ClearCustomGlyphs();

        // Enable or disable automatic advanced text validation after recognition
        [DllImport(strDllName, EntryPoint = "OCR_EnableAdvancedTextValidation",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_EnableAdvancedTextValidation([MarshalAs(UnmanagedType.Bool)] bool bEnable);

        // Enable or disable dynamic advanced recognition cache usage
        [DllImport(strDllName, EntryPoint = "OCR_EnableAdvancedCache",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_EnableAdvancedCache([MarshalAs(UnmanagedType.Bool)] bool bEnable);

        // Perform advanced text validation on recognition results from specific page
        [DllImport(strDllName, EntryPoint = "OCR_PerformAdvancedTextValidation",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_PerformAdvancedTextValidation(int nPageIndex);

        // Automatically generate region template from recognition results of given page
        [DllImport(strDllName, EntryPoint = "OCR_AutoGenerateRegionTemplate",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_AutoGenerateRegionTemplate(int nPageIndex);

        // Calculate page image rotation angle and orientation position
        [DllImport(strDllName, EntryPoint = "OCR_CalcPageRotation",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_CalcPageRotation(int nPageIndex, ref int pOrientation, ref double pRotation, 
                                                      [MarshalAs(UnmanagedType.Bool)] bool bUseCofidenceThreshold);

        // Specify path to external recognizer
        [DllImport(strDllName, EntryPoint = "OCR_ExternalSetRecognizerPath_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExternalSetRecognizerPath([In] string pwchFile);

        // Recognize page using the specified external recognizer
        [DllImport(strDllName, EntryPoint = "OCR_ExternalRecognizePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ExternalRecognizePage(int nPageIndex);

        // Recognize page region using the specified external recognizer
        [DllImport(strDllName, EntryPoint = "OCR_ExternalRecognizeRegion",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_ExternalRecognizeRegion(int nPageIndex, int nRegionIndex);

        // Get text recognized by the external recognizer ( Note Tuan )
        [DllImport(strDllName, EntryPoint = "OCR_ExternalGetRecognizedText_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExternalGetRecognizedText(StringBuilder sbResultBuffer, int nResultBufferSize);		

		// Primary recognition language control
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionLanguage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRecognitionLanguage(int nLangID);

        // Secondary recognition language control
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionLanguage2",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetRecognitionLanguage2(int nLangID);

        // Specify locale for recognition and region template generation
        [DllImport(strDllName, EntryPoint = "OCR_SetRecognitionLocale_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SetRecognitionLocale([In] string pwchLocale, [In] string pwchUserData);

        // Get font count used in recognition
        [DllImport(strDllName, EntryPoint = "OCR_GetFontCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetFontCount();

        // Get font index of specific recognized character
        [DllImport(strDllName, EntryPoint = "OCR_GetCharFontIndex",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetCharFontIndex(int nPageIndex, int nRegionIndex, int nCharIndex);

        // Extract font name from font index
        [DllImport(strDllName, EntryPoint = "OCR_GetFontName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_GetFontName(int nFontIndex, StringBuilder sbFontNameBuffer, int nFontNameBufferSize);

        // Enable specific variations of given font specified by its name
        [DllImport(strDllName, EntryPoint = "OCR_EnableFont_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_EnableFont(int nPageIndex, int nRegionIndex, [In] string pwchFontName, int nFontVariations);

        // Disable specific variations of given font specified by its name
        [DllImport(strDllName, EntryPoint = "OCR_DisableFont_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_DisableFont(int nPageIndex, int nRegionIndex, [In] string pwchFontName, int nFontVariations);

        // Disable specific character variations of given font specified by its name
        [DllImport(strDllName, EntryPoint = "OCR_DisableFontChars_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_DisableFontChars([In] string pwchFontName, [In] string pwchChars, int nFontVariations);

        // Specify space correction factor
        [DllImport(strDllName, EntryPoint = "OCR_SetSpaceCorrectionFactor",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetSpaceCorrectionFactor(int nPageIndex, int nRegionIndex, double dSpaceCorrectionFactor);

        // Get page image quality estimation
        [DllImport(strDllName, EntryPoint = "OCR_SetSpaceCorrectionFactor",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageImageQuality(int nPageIndex);

        // Control OCR level options, see 'OCR_OPTION_*' codes
        [DllImport(strDllName, EntryPoint = "OCR_SetOption",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetOption(int nOptionCode, int nValue);

        // Page data serialization to file in internal format
        [DllImport(strDllName, EntryPoint = "OCR_SavePageData_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SavePageData(int nPageIndex, [In] string strFilePath);

        // Page data serialization to file in internal format with embedded image
        [DllImport(strDllName, EntryPoint = "OCR_SavePageDataWithImage_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_SavePageDataWithImage(int nPageIndex, [In] string strFilePath);

        // Page data serialization from file in internal format
        [DllImport(strDllName, EntryPoint = "OCR_LoadPageData_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_LoadPageData(int nPageIndex, [In] string strFilePath);

        // Get horizontal and vertical resolution of page image
        [DllImport(strDllName, EntryPoint = "OCR_GetPageImageResolution",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageImageResolution(int nPageIndex, ref int pDpiX, ref int pDpiY);

        // Export page as image file
        [DllImport(strDllName, EntryPoint = "OCR_ExportPageAsImage_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OCR_ExportPageAsImage(int nPageIndex, [In] string pwchOutputFileName, int nImageFormat, int nOptions);

        // Get confidence level for specific page region
        [DllImport(strDllName, EntryPoint = "OCR_GetPageRegionConfidence",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageRegionConfidence(int nPageIndex, int nRegionIndex);

        // Specify page callback delegate that will be called for different page level operations, see page callback codes 'OCR_PCC_*'
        [DllImport(strDllName, EntryPoint = "OCR_SetPageCallback",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_SetPageCallback(OcrPageCallbackDelegate callback, IntPtr pCallbackData);

        // Check if page data is extracted after loading from readable format like vector PDF file
        [DllImport(strDllName, EntryPoint = "OCR_IsPageDataExtracted",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool OCR_IsPageDataExtracted(int nPageIndex);

        // Get transformed version of the page image, see 'ImageTransformParams' structure
        [DllImport(strDllName, EntryPoint = "OCR_GetPageImageTransformed",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OCR_GetPageImageTransformed(int nPageIndex, ref IntPtr hBitmap, ref ImageTransformParams pImgTransformParams);

        #endregion

        #region "Capture SDK Layer API Functions"

        // Filter input text using one of the predefined types 'CAP_TFT_*'
        [DllImport(strDllName, EntryPoint = "CAP_FilterTextAs_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_FilterTextAs([In] string pwchTextIn, int nFilterAs, StringBuilder sbBufferOut, int nBufferSize, 
                                                  [In] byte [] pArrConfidencesIn, [In, Out] byte [] pArrConfidencesOut);

        // Validate input text using one of the predefined types 'CAP_TFT_*'
        [DllImport(strDllName, EntryPoint = "CAP_ValidateTextAs_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_ValidateTextAs([In] string pwchTextIn, int nValidateAs);

        // Filter input text using custom type name
        [DllImport(strDllName, EntryPoint = "CAP_FilterTextByName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_FilterTextByName([In] string pwchTextIn, [In] string pwchTypeName, StringBuilder sbBufferOut, int nBufferSize,
                                                      [In] byte[] pArrConfidencesIn, [In, Out] byte[] pArrConfidencesOut);

        // Validate input text using custom type name
        [DllImport(strDllName, EntryPoint = "CAP_ValidateTextByName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_ValidateTextByName([In] string pwchTextIn, [In] string pwchTypeName);

        // Add sample list field data using one of the predefined field data types 'CAP_FDT*'
        [DllImport(strDllName, EntryPoint = "CAP_AddSampleFieldData_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_AddSampleFieldData(int nTypeFieldData, [In] string pwchFieldData);

        // Remove sample list field data using one of the predefined field data types 'CAP_FDT*'
        [DllImport(strDllName, EntryPoint = "CAP_RemoveSampleFieldData_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_RemoveSampleFieldData(int nTypeFieldData, [In] string pwchFieldData);

        // Add sample list field data using custom field data type name
        [DllImport(strDllName, EntryPoint = "CAP_AddSampleFieldDataByName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_AddSampleFieldDataByName([In] string pwchFieldName, [In] string pwchFieldData);

        // Remove sample list field data using custom field data type name
        [DllImport(strDllName, EntryPoint = "CAP_RemoveSampleFieldDataByName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_RemoveSampleFieldDataByName([In] string pwchFieldName, [In] string pwchFieldData);

        // Find best matching region template for given page
        [DllImport(strDllName, EntryPoint = "CAP_FindBestRegionTemplate_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_FindBestRegionTemplate(int nPageIndex, [In] string pwchPath, StringBuilder sbBufferFileFound, int nBufferSize);

        // Specify region callback delegate that will be called for different region level operations
        [DllImport(strDllName, EntryPoint = "CAP_SetRegionCallback",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetRegionCallback(OcrRegionCallbackDelegate pfnRegionCallback, IntPtr pCallbackData);

        // Synchronize destination page with the source page
        [DllImport(strDllName, EntryPoint = "CAP_SynchronizePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SynchronizePage(int nPageIndexDest, int nPageIndexSrc, [MarshalAs(UnmanagedType.Bool)] bool bSyncRect);

        // Specify working folder with region templates for the Capture Layer
        [DllImport(strDllName, EntryPoint = "CAP_SetWorkingFolder_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_SetWorkingFolder([In] string pwchWorkingFolder);

        // Get matched region template name for specific page
        [DllImport(strDllName, EntryPoint = "CAP_GetRegionTemplateName_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_GetRegionTemplateName(int nPageIndex, StringBuilder sbBufferTemplateName, int nBufferSize);

        // Set user text content for specific region
        [DllImport(strDllName, EntryPoint = "CAP_SetRegionText_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_SetRegionText(int nPageIndex, int nRegionIndex, [In] string pwchRegionText);

        // Set user text content for specific region with additional control if the user text must be used as output text
        [DllImport(strDllName, EntryPoint = "CAP_SetRegionTextEx_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_SetRegionTextEx(int nPageIndex, int nRegionIndex, [In] string pwchRegionText, [MarshalAs(UnmanagedType.Bool)] bool bSetAsOutputText);

        // Process specific page using Capture Layer page processing procedure
        [DllImport(strDllName, EntryPoint = "CAP_ProcessPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_ProcessPage(int nPageIndex, [MarshalAs(UnmanagedType.Bool)] bool bAutoGenerate, 
                                                 OcrPageCallbackDelegate pfnProgressCallback, IntPtr pCallbackData);

        // Update recognition cache for specific region
        [DllImport(strDllName, EntryPoint = "CAP_UpdateCache",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_UpdateCache(int nPageIndex, int nRegionIndex);

        // Update recognition cache for specific pages
        [DllImport(strDllName, EntryPoint = "CAP_UpdateCacheForPages",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_UpdateCacheForPages([In] int[] arrPageIndexes, int nPageIndexCount);

        // Update item table structure from specific page
        [DllImport(strDllName, EntryPoint = "CAP_UpdateItemTable",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_UpdateItemTable(int nPageIndex, [In] int[] arrRowRegionIndexes, int nRowRegionIndexCount);

        // Control Capture Layer options, see 'CAP_OPTION_*' codes
        [DllImport(strDllName, EntryPoint = "CAP_SetOption",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetOption(int nOptionCode, int nValue);

        // Register custom field type for processing and auto-detection
        [DllImport(strDllName, EntryPoint = "CAP_RegisterCustomField_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_RegisterCustomField([In] string pwchFieldName, [In] string pwchTableName, int nFormat, 
                                                         [In] string pwchFormula, [In] string pwchListSampleHeaders, [In] string pwchRegexValidator);

        // Register custom field type 
        [DllImport(strDllName, EntryPoint = "CAP_UnregisterCustomField_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_UnregisterCustomField([In] string pwchFieldName);

        // Specify document callback delegate that will be called for different Capture Layer document level operations
        [DllImport(strDllName, EntryPoint = "CAP_SetDocumentCallback",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetDocumentCallback(OcrDocumentCallbackDelegate pfnDocumentCallback, IntPtr pCallbackData);

        // Get the count of Capture Layer documents
        [DllImport(strDllName, EntryPoint = "CAP_GetDocumentCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_GetDocumentCount();

        // Get the type of specific Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_GetDocumentType",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_GetDocumentType(int nCapDocumentIndex);

        // Get the type of specific Capture Layer document by page index
        [DllImport(strDllName, EntryPoint = "CAP_GetDocumentTypeByPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_GetDocumentTypeByPage(int nOcrPageIndex);

        // Set the type of specific Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_SetDocumentType",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetDocumentType(int nCapDocumentIndex, int nCapDocumentType);

        // Set the type of specific Capture Layer document by page index
        [DllImport(strDllName, EntryPoint = "CAP_SetDocumentTypeByPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetDocumentTypeByPage(int nOcrPageIndex, int nCapDocumentType);

        // Get page count in specific Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_DocumentGetPageCount",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_DocumentGetPageCount(int nCapDocumentIndex);

        // Insert page in Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_DocumentInsertPage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_DocumentInsertPage(int nCapDocumentIndex, int nOcrPageIndex, int nDocPageIndex);

        // Remove page from Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_DocumentRemovePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_DocumentRemovePage(int nCapDocumentIndex, int nDocPageIndex);

        // Insert new Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_InsertDocument",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_InsertDocument(int nCapDocumentIndex);

        // Remove specific Capture Layer document
        [DllImport(strDllName, EntryPoint = "CAP_RemoveDocument",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_RemoveDocument(int nCapDocumentIndex);

        // Move page from source Capture Layer document to destination document
        [DllImport(strDllName, EntryPoint = "CAP_MovePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_MovePage(int nCapDocumentIndexSrc, int nDocPageIndexSrc, int nCapDocumentIndexDest, int nDocPageIndexDest);

        // Convert Capture Layer document index and document page index to OCR Layer page index
        [DllImport(strDllName, EntryPoint = "CAP_GetOcrPageIndex",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_GetOcrPageIndex(int nCapDocIndex, int nDocPageIndex);

        // Convert OCR Layer page index to Capture Layer document index and document page index
        [DllImport(strDllName, EntryPoint = "CAP_GetPageIndexInCapDocument",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_GetPageIndexInCapDocument(int nOcrPageIndex, ref int pnCapDocumentIndex, ref int pnDocPageIndex);

        // Remove all Capture Layer documents
        [DllImport(strDllName, EntryPoint = "CAP_RemoveAllDocuments",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_RemoveAllDocuments();

        // Specify active page for the Capture Layer
        [DllImport(strDllName, EntryPoint = "CAP_SetActivePage",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CAP_SetActivePage(int nPageIndex);

        // Automatically assign and/or resize region based on user input
        [DllImport(strDllName, EntryPoint = "CAP_AutoAssignResizeRegion_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_AutoAssignResizeRegion(int nPageIndex, [In] string pwchFieldName, [In] string pwchFieldText, int nOptRegionIndex);

        // Get geographic location (on page image) hints for specific field type
        [DllImport(strDllName, EntryPoint = "CAP_GetFieldGeoHints_W",
         ExactSpelling = false, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CAP_GetFieldGeoHints(int nPageIndex, [In] string pwchFieldName,
                                                      [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] 
                                                      [In, Out] TOCR_RECT [] pArrHintRects, int nArrElemCount);

        #endregion
    }
}
