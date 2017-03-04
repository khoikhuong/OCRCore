using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OCRCore
{
    // Smart OCR SDK namespace
    using OCRCore.App_Start;
    using System.IO;
    using Common.Helper;
    using System.Threading;
    using Business.Service;
    using log4net;
    using System.Reflection;

    public partial class DemoForm : Form
    {
        static ILog LOGGER = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region "Members"
        private delegate void UpdateProgressDelegate(int value);
        private UpdateProgressDelegate m_UpdateProgress;
        private bool m_bCancelRequested;
        private bool m_bActiveRecognition;
        private int m_nSelectedPageIndex;
        private string m_strModulePath;
        private string m_Username = SmartOcrSdkExports.LIB_USER;//"Acom";
        private string m_Key = SmartOcrSdkExports.LIB_KEY;//"41D5B-F352C-CQ41A-CB178-215D7-71E3F-2626223KCF11B5";
        private Dictionary<int, int?> m_Dictionary_Page = new Dictionary<int, int?>();
        #endregion

        #region drawing
        private Point startPos;
        private Point currentPos;
        private bool drawing;
        private Dictionary<string, int> _dictionary = new Dictionary<string, int>();
        List<Rectangle> myRectangles = new List<Rectangle>();

        private Rectangle getRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }

        private void RemoveRegion(object sender, EventArgs e)
        {
            Remove();
            btnExtract.Enabled = false;
        }

        private void Remove()
        {
            myRectangles.Clear();
            pbPreview.Invalidate();
            //  listBox1.Items.Clear();
        }

        void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;
                var rect = getRectangle();

                if (rect.Width > 0 && rect.Height > 0)
                {
                    myRectangles.Add(rect);
                    btnExtract.Enabled = true;
                    btnRemoveRegion.Enabled = true;
                }

                SmartOcrSdkExports.TOCR_RECT tocr_Rect;
                tocr_Rect.m_nBottom = rect.Bottom;
                tocr_Rect.m_nLeft = rect.Left;
                tocr_Rect.m_nRight = rect.Right;
                tocr_Rect.m_nTop = rect.Top;

                //  MessageBox.Show(tocr_Rect.m_nBottom + " " + tocr_Rect.m_nLeft + " " + tocr_Rect.m_nRight + " " + tocr_Rect.m_nTop);
                int res = SmartOcrSdkExports.OCR_DeleteAllRegionsInPage(m_nSelectedPageIndex);
                res = SmartOcrSdkExports.OCR_AddCustomRegionInPage(m_nSelectedPageIndex, SmartOcrSdkExports.OCR_RT_TEXT, tocr_Rect);

                res = SmartOcrSdkExports.OCR_RecognizeRegion(m_nSelectedPageIndex, 0, SmartOcrSdkExports.OCR_RT_TEXT, null, IntPtr.Zero);

                int nRegionType_1 = SmartOcrSdkExports.OCR_RT_UNDEFINED;
                SmartOcrSdkExports.TOCR_RECT rectRegion = new SmartOcrSdkExports.TOCR_RECT();
                SmartOcrSdkExports.OCR_GetRegionInfo(m_nSelectedPageIndex, 0, ref nRegionType_1, ref rectRegion);

                int regionCount = SmartOcrSdkExports.OCR_GetRegionCountInPage(m_nSelectedPageIndex);
                StringBuilder sbText = new StringBuilder();

                for (int i = 0; i < regionCount; i++)
                {
                    // First calculate how much space do we need
                    int length = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, null, 0);
                    if (length != SmartOcrSdkExports.RES_ERROR)
                    {
                        StringBuilder sbBuffer = new StringBuilder(length);
                        res = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, sbBuffer, length);
                        if (res != SmartOcrSdkExports.RES_ERROR)
                        {
                            sbText.Append(sbBuffer.ToString());
                            sbText.Append(System.Environment.NewLine);
                            sbText.Append(System.Environment.NewLine);

                        }
                        foreach (Control c in panel3.Controls)
                        {
                            if (c is TextBox)
                            {
                                TextBox TextBoxControl = (TextBox)c;
                                if (TextBoxControl.Focused)
                                {
                                    TextBoxControl.Text = sbText.ToString();
                                    string txt = TextBoxControl.Name;
                                    Int64 Region_ID = Convert.ToInt64(txt.Split('_')[1]);
                                }

                                if (string.IsNullOrEmpty(TextBoxControl.Text))
                                {
                                    TextBoxControl.Focus();
                                    break;
                                }
                            }
                        }
                    }
                }

                pbPreview.Invalidate();
            }
        }

        private void pbPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentPos = startPos = e.Location;
                drawing = true;
            }
        }

        private void pbPreview_MouseMove(object sender, MouseEventArgs e)
        {
            currentPos = e.Location;
            if (drawing) pbPreview.Invalidate();
        }
        Graphics graphics;
        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            if (myRectangles.Count > 0) e.Graphics.DrawRectangles(Pens.Blue, myRectangles.ToArray());
            if (drawing) e.Graphics.DrawRectangle(Pens.Red, getRectangle());
            graphics = e.Graphics;
        }
        #endregion

        #region "Ctor"
        public DemoForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            int h = Screen.PrimaryScreen.WorkingArea.Height;
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = h;
            this.Width = w;
            pbPreview.Width = panel1.Width;
            pbPreview.Height = panel1.Height;
            panel3.Height = h;
            panel3.Width = w - (panel1.Width + panel2.Width);
            pbPreview.Height = h;


            //  LoadDynamicInput();

            pbPreview.MouseUp += OnMouseUp;
            btnLoad.Click += btnLoad_Click;
            btnRecognizeSelected.Click += btnRecognizeSelected_Click;
            btnRecognize.Click += btnRecognize_Click;
            btnSave.Click += btnSave_Click;
            btnExtract.Click += ExtractData;
            btnRemoveRegion.Click += RemoveRegion;
            btnChooseFile.Click += btnChooseFile_Click;
            btnTemplate.Click += btnTemplate_Click;
            lstLoadedImages.SelectedIndexChanged += lstLoadedImages_SelectedIndexChanged;

            btnExtract.Enabled = false;
            button1.Hide();
            // btnRecognize.Enabled = false;
            //btnSave.Enabled = false;
            //cbOutputFormats.Enabled = false;

            btnSave.Hide();
            btnRecognize.Hide();
            cbOutputFormats.Hide();
            lblOutputFormat.Hide();
            btnCancel.Hide();
            btnRecognizeSelected.Hide();
            m_bCancelRequested = false;
            m_bActiveRecognition = false;
            m_UpdateProgress = new UpdateProgressDelegate(UpdateProgressSafe);
            cbOutputFormats.SelectedIndex = 2;
            m_nSelectedPageIndex = -1;


            #region "Init the OCR SDK"
            FileInfo exeFileInfo = new FileInfo(Application.ExecutablePath);
            m_strModulePath = exeFileInfo.DirectoryName;
            string strRedistPath = System.IO.Path.Combine(this.m_strModulePath, SmartOcrSdkExports.LIB_PATH);// m_strModulePath + "\\";
            OcrData m_OCR = new OcrData();
            m_OCR.OCR_Init(m_Username, m_Key, strRedistPath);
            #endregion
        }

        void btnTemplate_Click(object sender, EventArgs e)
        {
            var frmTemplate = new DemoTemplateForm();
            frmTemplate.MaximizeBox = false;
            frmTemplate.Show(this);
        }

        void btnChooseFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(openFileDialog.FileName);
                string fileName = openFileDialog.FileName;
                int pageSelectIndex = 0;
                int nPageCount_1 = SmartOcrSdkExports.OCR_GetPageCount();
                if (ext == ".pdf")
                {
                    SmartOcrSdkExports.OCR_AddPagesFromPDF(fileName, null, IntPtr.Zero);
                }
                else
                {
                    SmartOcrSdkExports.OCR_AddPageFromImage(fileName);
                }
                int nPageCount = SmartOcrSdkExports.OCR_GetPageCount() - nPageCount_1;
                int index = lstLoadedImages.Items.Count;

                if (nPageCount > 1)
                {
                    for (int i = 0; i < nPageCount; i++)
                    {
                        pageSelectIndex++;
                        string filePath = "";
                        filePath = fileName + "_page" + pageSelectIndex;
                        lstLoadedImages.Items.Add(filePath);
                    }
                }
                else
                {
                    lstLoadedImages.Items.Add(fileName);
                }
                // Automatically select the first item
                if (lstLoadedImages.SelectedItem == null)
                {
                    lstLoadedImages.SelectedIndex = 0;
                }
                else
                {
                    lstLoadedImages.SelectedIndex = lstLoadedImages.Items.Count - nPageCount;
                }
            }
        }
        #endregion

        void RecognizeSelectedThreadProc()
        {
            SmartOcrSdkExports.OCR_RecognizePage(m_nSelectedPageIndex, new OcrProgressCallbackDelegate(OcrProgressCallback), IntPtr.Zero);
            prgRecognition.Invoke(m_UpdateProgress, new object[] { 0 });
            m_bActiveRecognition = false;
        }

        void RecognizeAllThreadProc()
        {
            SmartOcrSdkExports.OCR_RecognizeAllPages(new OcrProgressCallbackDelegate(OcrProgressCallback), IntPtr.Zero);
            prgRecognition.Invoke(m_UpdateProgress, new object[] { 0 });
            m_bActiveRecognition = false;
        }

        int OcrProgressCallback(int nEvent, double dProgress, IntPtr pCallbackData)
        {
            if (nEvent == SmartOcrSdkExports.OCR_TT_RECOGNIZE || nEvent == SmartOcrSdkExports.OCR_TT_RECOGNIZE_ALL || nEvent == SmartOcrSdkExports.OCR_TT_LOAD_PDF)
            {
                prgRecognition.Invoke(m_UpdateProgress, new object[] { (int)dProgress });
            }
            return m_bCancelRequested == true ? -1 : 1;
        }

        int OcrScanCallback(int nEvent, IntPtr hBitmap, IntPtr pCallbackData)
        {
            if (nEvent == SmartOcrSdkExports.OCR_SE_IMAGE_SCANNED && hBitmap != IntPtr.Zero)
            {
                SmartOcrSdkExports.OCR_AddPageFromHBITMAP(hBitmap, -1, -1);
            }
            return 1;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            const string strFilters = "*.png;*.bmp;*.tif;*.tiff;*.jpg;*.jpeg;*.gif";
            string[] arrFilters = strFilters.Split(';');
            foreach (string strFilter in arrFilters)
            {
                // Get files for each filter
                string[] arrImgFiles = Directory.GetFiles(m_strModulePath + "\\Test_Images", strFilter);

                // Load files
                foreach (string strImg in arrImgFiles)
                {
                    #region Tuan
                    // OCR_AddPageFromImage_W
                    // DeleteAllRegionsInPage  ( int  nPageIndex ) 

                    //   OCR_DeleteRegionInPage  ( int  nPageIndex,  int  nRegionIndex  )   : Xóa 1 vùng cho 1 trang cụ thể

                    //ExternalGetRecognizedText

                    //OCR_LoadRegionTemplate_A

                    //OCR_RecognizeRegions   

                    // OCR_SaveRegionTemplate_W
                    #endregion

                    int res = SmartOcrSdkExports.OCR_AddPageFromImage(strImg);
                    // Add to the list only if we successfully loaded the image 
                    if (res != SmartOcrSdkExports.RES_ERROR)
                    {
                        lstLoadedImages.Items.Add(strImg);

                        // Automatically select the first item
                        if (lstLoadedImages.SelectedItem == null)
                        {
                            lstLoadedImages.SelectedIndex = 0;
                        }
                    }
                }
            }

            if (lstLoadedImages.SelectedItem == null)
            {
                MessageBox.Show("Could not find the test images.\n" +
                                "The demo application searches for test images in \"Test_Images\" folder.\n" +
                                "This folder must be located in the same directory with the application.");
            }
            #region
            // Demo: Scan
            // - Init scan, should be called only once
            // SmartOcrSdkExports.OCR_InitScan();
            //
            /* Additional scan tune if required
            int len = SmartOcrSdkExports.OCR_GetScanners(null, 0);
            if (len > 0)
            {
                StringBuilder sb = new StringBuilder(len);
                SmartOcrSdkExports.OCR_GetScanners(sb, len);
                string[] arrScanners = sb.ToString().Replace("\r", "").Split('\n');
                if(arrScanners.Length > 0)
                    SmartOcrSdkExports.OCR_SetActiveScanner(arrScanners[0]);
            }
            */
            // - Perform scan (async)
            // SmartOcrSdkExports.OCR_PerformScan(new OcrScanCallbackDelegate(OcrScanCallback), 0, IntPtr.Zero);
            #endregion
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (m_bActiveRecognition == false)
            {
                if (lstLoadedImages.Items.Count > 0)
                {
                    prgRecognition.Value = 0;
                    m_bCancelRequested = false;
                    m_bActiveRecognition = true;

                    // Start the recognize all pages thread
                    Thread threadRecognition = new Thread(new ThreadStart(RecognizeAllThreadProc));
                    threadRecognition.Start();
                }
            }
        }

        private void btnRecognizeSelected_Click(object sender, EventArgs e)
        {
            if (m_bActiveRecognition == false)
            {
                int nPageCount = SmartOcrSdkExports.OCR_GetPageCount();
                //m_nSelectedPageIndex = lstLoadedImages.Items.Count > 0 ? lstLoadedImages.SelectedIndex : -1;

                m_nSelectedPageIndex = lstLoadedImages.Items.Count > 0 ? 0 : -1;
                if (m_nSelectedPageIndex >= 0)
                {
                    btnLoad.Enabled = false;
                    btnRecognizeSelected.Enabled = false;
                    btnChooseFile.Enabled = false;
                    lstLoadedImages.Enabled = false;
                    btnRemoveRegion.Enabled = false;
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                    panel3.Enabled = false;

                    m_bCancelRequested = false;
                    m_bActiveRecognition = true;
                    prgRecognition.Value = 0;
                    Thread threadRecognition = new Thread(new ThreadStart(RecognizeSelectedThreadProc));
                    threadRecognition.Start();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_bActiveRecognition)
                return;
            int res = SmartOcrSdkExports.OCR_ExportDocument(m_strModulePath + "\\ExportedFile",
                                                            cbOutputFormats.SelectedIndex + 1, 0, 0, 1);
            if (res != SmartOcrSdkExports.RES_ERROR)
            {
                MessageBox.Show("The file is successfully saved to: \n\"" + m_strModulePath + "\\\"");
            }
            else
            {
                MessageBox.Show("Failed to save." +
                    " Please make sure that the application is started with Administrator privileges" +
                    " and there is at least one recognized page.");
            }


        }

        private void ExtractData(object sender, EventArgs e)
        {

            var fileName = "KBLAAOCR_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".txt";
            string strRedistPath = m_strModulePath + "\\ExportedFile\\" + fileName;
            FileInfo saveFile = new FileInfo(strRedistPath);
            StreamWriter writer = null;
            bool flag = false;
            if (myRectangles.Count > 0)
            {
                writer = saveFile.CreateText();
                flag = true;
            }
            foreach (var rect in myRectangles)
            {
                SmartOcrSdkExports.TOCR_RECT tocr_Rect;
                tocr_Rect.m_nBottom = rect.Bottom;
                tocr_Rect.m_nLeft = rect.Left;
                tocr_Rect.m_nRight = rect.Right;
                tocr_Rect.m_nTop = rect.Top;

                int res = SmartOcrSdkExports.OCR_DeleteAllRegionsInPage(m_nSelectedPageIndex);
                res = SmartOcrSdkExports.OCR_AddCustomRegionInPage(m_nSelectedPageIndex, SmartOcrSdkExports.OCR_RT_TEXT, tocr_Rect);
                res = SmartOcrSdkExports.OCR_RecognizeRegion(m_nSelectedPageIndex, 0, SmartOcrSdkExports.OCR_RT_TEXT, null, IntPtr.Zero);

                int nRegionType_1 = SmartOcrSdkExports.OCR_RT_UNDEFINED;
                SmartOcrSdkExports.TOCR_RECT rectRegion = new SmartOcrSdkExports.TOCR_RECT();
                SmartOcrSdkExports.OCR_GetRegionInfo(m_nSelectedPageIndex, 0, ref nRegionType_1, ref rectRegion);

                int regionCount = SmartOcrSdkExports.OCR_GetRegionCountInPage(m_nSelectedPageIndex);
                StringBuilder sbText = new StringBuilder();

                for (int i = 0; i < regionCount; i++)
                {
                    // First calculate how much space do we need
                    int length = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, null, 0);
                    if (length != SmartOcrSdkExports.RES_ERROR)
                    {
                        StringBuilder sbBuffer = new StringBuilder(length);
                        res = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, sbBuffer, length);
                        if (res != SmartOcrSdkExports.RES_ERROR)
                        {
                            sbText.Append(sbBuffer.ToString());
                            sbText.Append(System.Environment.NewLine);
                            sbText.Append(System.Environment.NewLine);
                            writer.Write(sbText.ToString());

                        }
                    }
                }
            }
            if (flag)
            {
                writer.Close();
                string msg = "Data was extracted and imported to file which put , " + strRedistPath + ", Please browse for view !";
                MessageBox.Show(msg);
                btnExtract.Enabled = false;
            }

        }

        private void DemoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release the OCR engine.
            // Use try-catch block in order to guarantee application exit when 
            // SmartOCR_SDK.dll is not in the same folder with the application
            try
            {
                // Release the OCR engine
                SmartOcrSdkExports.OCR_ReleaseAll();
            }
            catch (System.Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bCancelRequested = true;
        }

        private void lstLoadedImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLoadedImages.SelectedItem != null)
            {
                if (pbPreview.Image != null)
                    pbPreview.Image.Dispose();

                #region note
                //int nPageCount = SmartOcrSdkExports.OCR_GetPageCount();
                //if (nPageCount >= 0)
                //{
                //    SmartOcrSdkExports.OCR_RemoveAllPages();
                //}

                //string ext = Path.GetExtension(lstLoadedImages.SelectedItem.ToString());
                //string filePath = lstLoadedImages.SelectedItem.ToString();
                //string[] page = ext.Split('_');
                //int pageIndex = 0, index  = 0;
                //if (page != null)
                //{
                //    ext = page[0].ToString();
                //    if(page.Length > 1)
                //    {
                //        string strPage = page[1];
                //        index = strPage.Length;
                //        pageIndex = (Convert.ToInt16(strPage.Substring(4))) - 1;
                //        filePath = filePath.Replace("_" + strPage, "");
                //    }

                //}
                //if (ext == ".pdf")
                //{
                //    SmartOcrSdkExports.OCR_AddPagesFromPDF(filePath, null, IntPtr.Zero);

                //    #region Note 
                //    //PdfDocument doc = new PdfDocument();
                //    //doc.LoadFromFile(filePath);
                //    //Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                //    //pbPreview.Image = emf;
                //    #endregion
                //    ExtentionFile = 0;
                //}
                //else
                //{
                //    SmartOcrSdkExports.OCR_AddPageFromImage(filePath);
                //   // pbPreview.Image = Bitmap.FromFile(filePath);
                //    ExtentionFile = 0;
                //}
                #endregion
                Remove();
                if (m_bActiveRecognition == false)
                {
                    m_nSelectedPageIndex = lstLoadedImages.Items.Count > 0 ? lstLoadedImages.SelectedIndex : -1;
                    if (m_nSelectedPageIndex >= 0)
                    {
                        LoadDynamicInput();

                        IntPtr hBmp = IntPtr.Zero;

                        int res = SmartOcrSdkExports.OCR_GetHBITMAP_FromPageImage(m_nSelectedPageIndex, ref hBmp);
                        if (res != SmartOcrSdkExports.RES_ERROR)
                        {
                            pbPreview.Image = Bitmap.FromHbitmap(hBmp);

                            #region "Resize image"
                            //Image MyImage = Bitmap.FromHbitmap(hBmp);

                            //Bitmap MyBitMap = new Bitmap(MyImage, panel1.Width + 200, panel1.Height + 200);

                            //Graphics Graphic = Graphics.FromImage(MyBitMap);

                            //Graphic.InterpolationMode = InterpolationMode.High;

                            //pbPreview.Image = MyBitMap;
                            #endregion
                        }

                        if (!m_Dictionary_Page.ContainsKey(m_nSelectedPageIndex))
                        {
                            btnLoad.Enabled = false;
                            btnRecognizeSelected.Enabled = false;
                            btnChooseFile.Enabled = false;
                            lstLoadedImages.Enabled = false;
                            btnRemoveRegion.Enabled = false;
                            panel1.Enabled = false;
                            panel2.Enabled = false;
                            panel3.Enabled = false;

                            m_bCancelRequested = false;
                            m_bActiveRecognition = true;
                            prgRecognition.Value = 0;

                            //@Tuan: Nen xem xet cai nay
                            Thread threadRecognition = new Thread(new ThreadStart(RecognizeSelectedThreadProc));
                            threadRecognition.Start();
                        }
                        else
                        {
                            var Template_ID = CheckExitsTemplate();
                            if (Template_ID.HasValue)
                            {
                                LoadDynamicInput(Template_ID.Value);
                                LoadDynamicRectangle(Template_ID.Value);
                            }
                            else
                            {
                                MessageBox.Show("Template Incorrect");
                                if (MessageBox.Show("Bạn có muốn tạo template cho file?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    var m_Path = lstLoadedImages.SelectedItem.ToString();
                                    string m_Extention = Path.GetExtension(m_Path);
                                    string[] m_Str = m_Extention.Split('_');

                                    if (m_Str.Length > 1)
                                        m_Path = m_Path.Replace("_" + m_Str[1], "");
                                    FileTemplateOCR_DTO.m_Path = m_Path;
                                    var frmTemplate = new DemoTemplateForm();
                                    frmTemplate.MaximizeBox = false;
                                    frmTemplate.Show(this);
                                }
                            }
                        }
                    }
                }
            }
        }


        void UpdateProgressSafe(int value)
        {
            if (value == 0)
            {
                btnLoad.Enabled = true;
                btnRecognizeSelected.Enabled = true;
                btnChooseFile.Enabled = true;
                lstLoadedImages.Enabled = true;
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;

                IntPtr hBmp = IntPtr.Zero;
                int res = SmartOcrSdkExports.OCR_GetHBITMAP_FromPageImage(m_nSelectedPageIndex, ref hBmp);
                if (res != SmartOcrSdkExports.RES_ERROR)
                {
                    pbPreview.Image = Bitmap.FromHbitmap(hBmp);
                }
                if (!m_Dictionary_Page.ContainsKey(m_nSelectedPageIndex))
                    m_Dictionary_Page.Add(m_nSelectedPageIndex, null);

                var Template_ID = CheckExitsTemplate();

                if (Template_ID.HasValue)
                {
                    LoadDynamicInput(Template_ID.Value);
                    LoadDynamicRectangle(Template_ID.Value);
                }
                else
                {
                    MessageBox.Show("Template Incorrect");
                    if (MessageBox.Show("Bạn có muốn tạo template cho file?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var m_Path = lstLoadedImages.SelectedItem.ToString();
                        string m_Extention = Path.GetExtension(m_Path);
                        string[] m_Str = m_Extention.Split('_');

                        if (m_Str.Length > 1)
                            m_Path = m_Path.Replace("_" + m_Str[1], "");
                        FileTemplateOCR_DTO.m_Path = m_Path;
                        var frmTemplate = new DemoTemplateForm();
                        frmTemplate.MaximizeBox = false;
                        frmTemplate.Show(this);
                    }
                }
            }
            prgRecognition.Value = value;
        }

        //private string _ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        public void LoadDynamicInput(int template_ID = 0)
        {
            panel3.Controls.Clear();
            var _service = new TemplateService();
            var _dic = new Dictionary<string, object>();
            _dic.Add("Template_ID", template_ID);
            var _dataTable = _service.Region_SelectByTemplateID(_dic);
            if (_dataTable != null && _dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in _dataTable.Rows)
                {
                    Label label = new Label();
                    int count = panel3.Controls.OfType<Label>().ToList().Count;
                    label.Location = new Point(10, (25 * count) + 2);
                    label.Size = new Size(40, 20);
                    label.Name = "label_" + (count + 1);
                    label.Text = row["Label"].ToString();
                    panel3.Controls.Add(label);

                    TextBox textbox = new TextBox();
                    count = panel3.Controls.OfType<TextBox>().ToList().Count;
                    textbox.Location = new System.Drawing.Point(60, 25 * count);
                    textbox.Size = new System.Drawing.Size(250, 20);
                    textbox.Name = "textbox_" + row["ID"];
                    panel3.Controls.Add(textbox);
                }
            }
        }

        void LoadDynamicRectangle(int template_ID = 0)
        {
            Remove();
            var _service = new TemplateService();
            var _dic = new Dictionary<string, object>();
            _dic.Add("Template_ID", template_ID);
            var _dataTable = _service.Region_SelectByTemplateID(_dic);
            if (_dataTable != null && _dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in _dataTable.Rows)
                {
                    int m_Bottom = Convert.ToInt32(row["Rect_Bottom"]);
                    int m_Top = Convert.ToInt32(row["Rect_Top"]);
                    int m_Left = Convert.ToInt32(row["Rect_Left"]);
                    int m_Right = Convert.ToInt32(row["Rect_Right"]);
                    int X = Convert.ToInt32(row["X"]);
                    int Y = Convert.ToInt32(row["Y"]);
                    int Width = Convert.ToInt32(row["Width"]);
                    int Height = Convert.ToInt32(row["Height"]);

                    SmartOcrSdkExports.TOCR_RECT tocr_Rect;
                    tocr_Rect.m_nTop = m_Top;
                    tocr_Rect.m_nBottom = m_Bottom;
                    tocr_Rect.m_nLeft = m_Left;
                    tocr_Rect.m_nRight = m_Right;

                    string value = ExtractDataOCR(tocr_Rect);
                    if (!string.IsNullOrEmpty(value))
                    {
                        Rectangle rect = new Rectangle();
                        rect.X = X;
                        rect.Y = Y;
                        rect.Width = Width;
                        rect.Height = Height;

                        myRectangles.Add(rect);

                        foreach (Control c in panel3.Controls)
                        {
                            if (c is TextBox)
                            {
                                TextBox TextBoxControl = (TextBox)c;
                                if (TextBoxControl.Name.Contains("textbox_" + row["ID"]))
                                {
                                    TextBoxControl.Text = value;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateRegion(Rectangle _rectangle, Int64 Region_ID)
        {
            if (fUpdate_Region)
            {
                var _dic = new Dictionary<string, object>();
                _dic.Add("Rect_Bottom", _rectangle.Bottom);
                _dic.Add("Rect_Top", _rectangle.Top);
                _dic.Add("Rect_Left", _rectangle.Left);
                _dic.Add("Rect_Right", _rectangle.Right);
                _dic.Add("X", _rectangle.X);
                _dic.Add("Y", _rectangle.Y);
                _dic.Add("Width", _rectangle.Width);
                _dic.Add("Height", _rectangle.Height);
                _dic.Add("ID", Region_ID);
                var _service = new TemplateService();
                _service.Update_Region(_dic);
            }
            else
            {
                MessageBox.Show("Template Incorrect");
            }

        }

        string ExtractDataOCR(SmartOcrSdkExports.TOCR_RECT tocr_Rect)
        {
            int res = SmartOcrSdkExports.OCR_DeleteAllRegionsInPage(m_nSelectedPageIndex);
            res = SmartOcrSdkExports.OCR_AddCustomRegionInPage(m_nSelectedPageIndex, SmartOcrSdkExports.OCR_RT_TEXT, tocr_Rect);

            res = SmartOcrSdkExports.OCR_RecognizeRegion(m_nSelectedPageIndex, 0, SmartOcrSdkExports.OCR_RT_TEXT, null, IntPtr.Zero);
            int nRegionType_1 = SmartOcrSdkExports.OCR_RT_UNDEFINED;
            // SmartOcrSdkExports.TOCR_RECT rectRegion = new SmartOcrSdkExports.TOCR_RECT();
            SmartOcrSdkExports.OCR_GetRegionInfo(m_nSelectedPageIndex, 0, ref nRegionType_1, ref tocr_Rect);

            int regionCount = SmartOcrSdkExports.OCR_GetRegionCountInPage(m_nSelectedPageIndex);
            StringBuilder sbText = new StringBuilder();

            for (int i = 0; i < regionCount; i++)
            {
                // First calculate how much space do we need
                int length = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, null, 0);
                if (length != SmartOcrSdkExports.RES_ERROR)
                {
                    StringBuilder sbBuffer = new StringBuilder(length);
                    res = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, sbBuffer, length);
                    if (res != SmartOcrSdkExports.RES_ERROR)
                    {
                        sbText.Append(sbBuffer.ToString());
                        sbText.Append(System.Environment.NewLine);
                        sbText.Append(System.Environment.NewLine);
                    }
                }
            }
            return sbText.ToString();
        }

        private bool fUpdate_Region = true;

        void CheckAndUpdateTemplate(Dictionary<string, object> _dic)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var _service = new TemplateService();
            int result = _service.CheckAndUpdateTemplate(_dic);
            fUpdate_Region = true;
            if (result > 0)
            {
                LoadDynamicRectangle();
                string fileName = Path.GetFileNameWithoutExtension(lstLoadedImages.SelectedItem.ToString());
                _dictionary.Add(fileName, result);

                watch.Stop();
                LOGGER.InfoFormat("CheckAndUpdateTemplate --> {0} seconds", (long)watch.Elapsed.TotalSeconds);
            }
            else if (result == -2)
            {
                MessageBox.Show("Template Incorrect");
                fUpdate_Region = false;
            }
        }

        public int? CheckExitsTemplate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var _service = new TemplateService();
            var data = _service.Template_Select();
            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    int m_Bottom = Convert.ToInt32(row["Rect_Bottom"]);
                    int m_Top = Convert.ToInt32(row["Rect_Top"]);
                    int m_Left = Convert.ToInt32(row["Rect_Left"]);
                    int m_Right = Convert.ToInt32(row["Rect_Right"]);
                    string Markers = row["Marked"].ToString();
                    int Template_ID = Convert.ToInt32(row["ID"]);
                    int m_Sort = Convert.ToInt32(row["Sort"]);

                    //Rectangle rect = new Rectangle();
                    //rect.X = Convert.ToInt32(row["X"]);
                    //rect.Y = Convert.ToInt32(row["Y"]);
                    //rect.Width = Convert.ToInt32(row["Width"]);
                    //rect.Height = Convert.ToInt32(row["Height"]);

                    //myRectangles.Add(rect);

                    SmartOcrSdkExports.TOCR_RECT tocr_Rect;
                    tocr_Rect.m_nTop = m_Top;
                    tocr_Rect.m_nBottom = m_Bottom;
                    tocr_Rect.m_nLeft = m_Left;
                    tocr_Rect.m_nRight = m_Right;
                    string value = ExtractDataOCR(tocr_Rect);

                    if (value.Replace("\r\n", "").Equals(Markers.Replace("\r\n", "")))
                    {
                        if (m_Dictionary_Page.ContainsKey(m_nSelectedPageIndex))
                            m_Dictionary_Page[m_nSelectedPageIndex] = m_Sort;

                        watch.Stop();
                        LOGGER.InfoFormat("CheckExitsTemplate --> {0} seconds", (long)watch.Elapsed.TotalSeconds);

                        return Template_ID;
                    }
                }
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SmartOcrSdkExports.OCR_SetRecognitionMode(SmartOcrSdkExports.OCR_RM_FORM);
            // int ress = SmartOcrSdkExports.CAP_AddSampleFieldData(SmartOcrSdkExports.CAP_FF_TEXT, "TOTAL");

            // ress = SmartOcrSdkExports.CAP_AddSampleFieldDataByName("TOTAL", "1000");

            //// ress = SmartOcrSdkExports.CAP_RegisterCustomField("TOTAL", "tbInvoice", SmartOcrSdkExports.CAP_FF_TEXT, null, null, null);
            //  int length_v2 = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, 0, null, 0);

            // StringBuilder sbBuffer_v2 = new StringBuilder(length_v2);
            //          //   res = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, sbBuffer, length);
            // ress =  SmartOcrSdkExports.CAP_FilterTextAs("TOTAL", SmartOcrSdkExports.CAP_FF_TEXT, sbBuffer_v2, length_v2, null, null);
            // MessageBox.Show(ress + "");

            DataTable dbTable = new DataTable();
            try
            {

                //Adding columns to datatable
                dbTable.Columns.Add("UNIT PRICE", typeof(string));
                dbTable.Columns.Add("TOTAL", typeof(string));


                // Init the OCR SDK
                //if (!Init()) return;

                // Add pages
                //SmartOcrSdkExports.OCR_AddPagesFromPDF("C:\\Temp\\W-2.pdf", null, IntPtr.Zero);
                // SmartOcrSdkExports.OCR_AddPageFromImage("C:\\Temp\\test.png");

                //set region mode:OCR_RM_FORM
                SmartOcrSdkExports.OCR_SetRecognitionMode(SmartOcrSdkExports.OCR_RM_FORM);
                SmartOcrSdkExports.TOCR_RECT rectRegion = new SmartOcrSdkExports.TOCR_RECT();
                //rectRegion.m_nLeft = 1778;
                //rectRegion.m_nTop = 1387;
                //rectRegion.m_nRight = 2375;
                //rectRegion.m_nBottom = 2036; 

                rectRegion.m_nLeft = 507;
                rectRegion.m_nTop = 1130;
                rectRegion.m_nRight = 1216;
                rectRegion.m_nBottom = 1643;

                int res = SmartOcrSdkExports.OCR_DeleteAllRegionsInPage(m_nSelectedPageIndex);
                res = SmartOcrSdkExports.OCR_AddCustomRegionInPage(m_nSelectedPageIndex, SmartOcrSdkExports.OCR_RT_TEXT, rectRegion);

                res = SmartOcrSdkExports.OCR_RecognizeRegion(m_nSelectedPageIndex, 0, SmartOcrSdkExports.OCR_RT_TEXT, null, IntPtr.Zero);
                int nRegionType_1 = SmartOcrSdkExports.OCR_RT_UNDEFINED;
                //SmartOcrSdkExports.TOCR_RECT rectRegion = new SmartOcrSdkExports.TOCR_RECT();
                SmartOcrSdkExports.OCR_GetRegionInfo(m_nSelectedPageIndex, 0, ref nRegionType_1, ref rectRegion);

                int regionCount = SmartOcrSdkExports.OCR_GetRegionCountInPage(m_nSelectedPageIndex);
                StringBuilder sbText = new StringBuilder();

                for (int i = 0; i < regionCount; i++)
                {
                    // First calculate how much space do we need
                    int length = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, null, 0);
                    if (length != SmartOcrSdkExports.RES_ERROR)
                    {
                        StringBuilder sbBuffer = new StringBuilder(length);
                        res = SmartOcrSdkExports.OCR_GetRecognizedTextFromPageRegion(m_nSelectedPageIndex, i, sbBuffer, length);
                        if (res != SmartOcrSdkExports.RES_ERROR)
                        {
                            sbText.Append(sbBuffer.ToString());
                            sbText.Append(System.Environment.NewLine);
                            sbText.Append(System.Environment.NewLine);
                        }

                    }
                }
                MessageBox.Show(sbText.ToString());
                //DataColumnCollection columns = dbTable.Columns;
                //sbText.Replace(columns.ToString(), "");
                foreach (DataColumn dc in dbTable.Columns)
                {
                    if (sbText.ToString().Contains(dc.ColumnName))
                    {
                        sbText.Replace(dc.ColumnName, "");
                    }

                }

                List<String> items = new List<String>(sbText.ToString().Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));


                for (var i = 0; i < items.Count; i++)
                {
                    List<String> items_2 = new List<String>(items[i].ToString().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    if (items_2.Count > 1)
                    {
                        DataRow row;
                        row = dbTable.NewRow();
                        row["UNIT PRICE"] = items_2[0];
                        row["TOTAL"] = items_2[1];
                        dbTable.Rows.Add(row);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // Release the OCR - can be called when your application is closing 
                SmartOcrSdkExports.OCR_ReleaseAll();
            }
            dataGridView1.DataSource = dbTable;
        }

    }

    internal class OcrData
    {
        static ILog LOGGER = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int nRegionIndex { get; set; }
        public SmartOcrSdkExports.TOCR_RECT rect;
        public string sRegionText { get; set; }

        public OcrData() { }
        public OcrData(int nRegionIndex, SmartOcrSdkExports.TOCR_RECT rect, string sRegionText)
        {
            this.nRegionIndex = nRegionIndex;
            this.rect = rect;
            this.sRegionText = sRegionText;
        }

        public void OCR_Init(string strUserName = null, string strLicenseKey = null, string strIntsallPath = null, string strTempDir = null)
        {
            int res = SmartOcrSdkExports.RES_ERROR;
            try
            {
                if (Directory.Exists(strIntsallPath) == false)
                {
                    strIntsallPath = "";
                }

                var watchOCR_Init = System.Diagnostics.Stopwatch.StartNew();
                res = SmartOcrSdkExports.OCR_Init(strUserName, strLicenseKey, strIntsallPath, strTempDir);
                watchOCR_Init.Stop();
                LOGGER.InfoFormat("OCR_Init --> {0} seconds", (long)watchOCR_Init.Elapsed.TotalSeconds);
                
                SmartOcrSdkExports.OCR_SetRecognitionMode(SmartOcrSdkExports.OCR_RM_FORM);
            }
            catch (System.DllNotFoundException ex)
            {
                LOGGER.Error(ex);
                MessageBox.Show("System.DllNotFoundException occurred.\n\n" +
                                "Please make sure that SmartOCR_SDK.dll is in the same folder with your executable.\n\n" +
                                "Exception description: \n" + ex.Message, "SmartOCR_SDK.dll not found");
            }
            catch (System.BadImageFormatException ex)
            {
                LOGGER.Error(ex);
                MessageBox.Show("System.BadImageFormatException occurred.\n\n" +
                                "Please make sure to build your project with platform target \'x86\' instead of \'Any CPU\'.\n\n" +
                                "Exception description: \n" + ex.Message, "System.BadImageFormatException exception");
            }
            if (res == SmartOcrSdkExports.RES_ERROR)
            {
                MessageBox.Show("Could not initialize SmartOCR SDK.\n" +
                                "Please make sure to place redistributables in the folder of your executable.", "Smart OCR init error");
            }
        }
    }

    internal static class FileTemplateOCR_DTO
    {
        public static string m_Path { get; set; }
    }
}
