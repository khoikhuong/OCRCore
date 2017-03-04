using OCRCore.App_Start;
using OCRCore.Business.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRCore
{
    public partial class DemoTemplateForm : Form
    {
        #region "Member"
        private delegate void UpdateProgressDelegate(int value);
        private UpdateProgressDelegate m_UpdateProgress;
        private Dictionary<int, int> m_Dictionary_Page = new Dictionary<int, int>();
        private bool m_bCancelRequested;
        private bool m_bActiveRecognition;
        private int m_nSelectedPageIndex;

        private string m_strPath;
        private int m_PageOld = 0;
        private int m_Page = 0;


        private Point startPos;
        private Point currentPos;
        private bool drawing;
        List<Rectangle> myRectangles = new List<Rectangle>();
        private Rectangle T_Rectangle, R_Rectangle;

        private Rectangle getRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }
        #endregion

        public DemoTemplateForm()
        {
            InitializeComponent();

            btnAdd.Click += btnAdd_Click;
            btnUpdateTemplate.Click += btnUpdateTemplate_Click;
            pbPreview.MouseUp += OnMouseUp;
            pbPreview.MouseDown += pbPreview_MouseDown;
            pbPreview.MouseMove += pbPreview_MouseMove;
            pbPreview.Paint += pbPreview_Paint;

            lstLoadedImages.SelectedIndexChanged += lstLoadedImages_SelectedIndexChanged;
            dataGridView1.RowHeaderMouseClick += dataGridView1_RowHeaderMouseClick;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.RowHeaderMouseClick += dataGridView2_RowHeaderMouseClick;
            //Add Columns
            AddItemListView();
            //  dataGridView();

            btnChooseFile.Click += btnChooseFile_Click;


            this.WindowState = FormWindowState.Maximized;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            int h = Screen.PrimaryScreen.WorkingArea.Height;
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = h;
            this.Width = w;
            panel3.Height = h;
            panel1.Height = h;
            panel2.Height = h;
            panel3.Width = w - (panel1.Width + panel2.Width);
            pbPreview.Width = panel3.Width;
            pbPreview.Height = h;

            //pbPreview.Height = h;

            m_bCancelRequested = false;
            m_bActiveRecognition = false;
            m_UpdateProgress = new UpdateProgressDelegate(UpdateProgressSafe);
            m_nSelectedPageIndex = -1;

            // Lấy số page hiện tại
            m_Page = SmartOcrSdkExports.OCR_GetPageCount();
            if (!string.IsNullOrEmpty(FileTemplateOCR_DTO.m_Path))
            {
                AddFromPageOrc(FileTemplateOCR_DTO.m_Path);
                FileTemplateOCR_DTO.m_Path = string.Empty;
            }
        }

        void btnUpdateTemplate_Click(object sender, EventArgs e)
        {

            //code to execute
            //dgvIcbSubsInfo.Rows[currentRow].Selected = true;
            var m_Dic = new Dictionary<string, object>();
            var Template_ID = Convert.ToInt32(txtTemplate_ID.Text);
            if (Template_ID > 0)
            {
                int currentRow = dataGridView2.CurrentCell.RowIndex;
                int iColumn = dataGridView2.CurrentCell.ColumnIndex;
                if (T_Rectangle.Width != 0 && T_Rectangle.Height != 0)
                {
                    m_Dic.Add("X", T_Rectangle.X);
                    m_Dic.Add("Y", T_Rectangle.Y);
                    m_Dic.Add("Width", T_Rectangle.Width);
                    m_Dic.Add("Height", T_Rectangle.Height);
                    m_Dic.Add("Marked", txtMarkers.Text);
                    m_Dic.Add("Rect_Bottom", T_Rectangle.Bottom);
                    m_Dic.Add("Rect_Left", T_Rectangle.Left);
                    m_Dic.Add("Rect_Top", T_Rectangle.Top);
                    m_Dic.Add("Rect_Right", T_Rectangle.Right);
                }
                m_Dic.Add("ID", Template_ID);
                m_Dic.Add("Path", m_strPath);
                m_Dic.Add("Name", txtTemplateName.Text);
                m_Dic.Add("Sort", mericSort.Value);
                var _service = new TemplateService();
                var result = _service.Update_Template(m_Dic);
                if (result == 1)
                {
                    MessageBox.Show("Cập nhập thành công");
                    AddItemListView();
                    dataGridView2.ClearSelection();
                    // dataGridView2.Rows[iColumn].Visible = false;
                    dataGridView2.CurrentCell = dataGridView2.Rows[currentRow].Cells[iColumn];
                    dataGridView2.Rows[currentRow].Selected = true;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng để cập nhập");
            }

        }

        private void Remove()
        {
            myRectangles.Clear();
            pbPreview.Invalidate();
        }

        void lstLoadedImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLoadedImages.SelectedItem != null)
            {
                if (pbPreview.Image != null)
                    pbPreview.Image.Dispose();
                Remove();
                if (m_bActiveRecognition == false)
                {
                    m_nSelectedPageIndex = lstLoadedImages.Items.Count > 0 ? (m_Page + lstLoadedImages.SelectedIndex) : -1;
                    if (m_nSelectedPageIndex >= 0)
                    {
                        IntPtr hBmp = IntPtr.Zero;
                        int res = SmartOcrSdkExports.OCR_GetHBITMAP_FromPageImage(m_nSelectedPageIndex, ref hBmp);
                        if (res != SmartOcrSdkExports.RES_ERROR)
                        {
                            pbPreview.Image = Bitmap.FromHbitmap(hBmp);
                        }
                        var Template_ID = Convert.ToInt32(txtTemplate_ID.Text);
                        if (!m_Dictionary_Page.ContainsKey(m_nSelectedPageIndex))
                        {
                            m_Dictionary_Page.Add(m_nSelectedPageIndex, Template_ID);
                            btnChooseFile.Enabled = false;
                            lstLoadedImages.Enabled = false;
                            panel1.Enabled = false;
                            panel2.Enabled = false;

                            m_bCancelRequested = false;
                            m_bActiveRecognition = true;
                            prgRecognition.Value = 0;
                            Thread threadRecognition = new Thread(new ThreadStart(RecognizeSelectedThreadProc));
                            threadRecognition.Start();
                        }
                    }
                }
            }
        }

        #region "Processing OCR"
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

        void UpdateProgressSafe(int value)
        {
            if (value == 0)
            {
                btnChooseFile.Enabled = true;
                lstLoadedImages.Enabled = true;
                panel1.Enabled = true;
                panel2.Enabled = true;
                //panel3.Enabled = true;

                IntPtr hBmp = IntPtr.Zero;

                int res = SmartOcrSdkExports.OCR_GetHBITMAP_FromPageImage(m_nSelectedPageIndex, ref hBmp);
                if (res != SmartOcrSdkExports.RES_ERROR)
                {
                    pbPreview.Image = Bitmap.FromHbitmap(hBmp);
                }
            }
            prgRecognition.Value = value;
        }
        #endregion

        public void AddFromPageOrc(string m_FileName)
        {
            string ext = Path.GetExtension(m_FileName);

            m_strPath = m_FileName;
            int pageSelectIndex = 0;
            m_PageOld = SmartOcrSdkExports.OCR_GetPageCount();
            if (m_PageOld > m_Page)
            {
                for (int i = m_Page; i < m_PageOld; i++)
                {
                    var nPageIndex = m_Page;
                    SmartOcrSdkExports.OCR_RemovePage(nPageIndex);
                }
                lstLoadedImages.Items.Clear();
            }
            if (ext == ".pdf")
            {
                SmartOcrSdkExports.OCR_AddPagesFromPDF(m_FileName, null, IntPtr.Zero);
            }
            else
            {
                SmartOcrSdkExports.OCR_AddPageFromImage(m_FileName);
            }
            int nPageCount = SmartOcrSdkExports.OCR_GetPageCount() - m_Page;
            int index = lstLoadedImages.Items.Count;

            if (nPageCount > 1)
            {
                for (int i = 0; i < nPageCount; i++)
                {
                    pageSelectIndex++;
                    string filePath = "";
                    filePath = m_FileName + "_page" + pageSelectIndex;
                    lstLoadedImages.Items.Add(filePath);
                }
            }
            else
            {
                lstLoadedImages.Items.Add(m_FileName);
            }
            // Automatically select the first item
            if (lstLoadedImages.SelectedItem == null)
            {
                lstLoadedImages.SelectedIndex = 0;
            }
        }
        // chọn file cho template
        void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                AddFromPageOrc(fileName);
            }
        }


        private OCR_Template_DTO GetTemplateByID(int TemplateId)
        {
            var _dic = new Dictionary<string, object>();
            _dic.Add("ID", TemplateId);
            var _service = new TemplateService();
            DataTable mTable = _service.Template_SelectById(_dic);
            if (mTable.Rows.Count > 0)
            {
                DataRow m_Row = mTable.Rows[0];
                var data = new OCR_Template_DTO
                {
                    ID = Convert.ToInt32(m_Row["ID"]),
                    Sort = Convert.ToInt32(m_Row["Sort"]),
                    Marked = m_Row["Marked"].ToString(),
                    Name = m_Row["Name"].ToString(),
                    X = Convert.ToInt32(m_Row["X"]),
                    Y = Convert.ToInt32(m_Row["Y"]),
                    Width = Convert.ToInt32(m_Row["Width"]),
                    Height = Convert.ToInt32(m_Row["Height"]),
                    Rect_Bottom = Convert.ToInt32(m_Row["Rect_Bottom"]),
                    Rect_Top = Convert.ToInt32(m_Row["Rect_Top"]),
                    Rect_Left = Convert.ToInt32(m_Row["Rect_Left"]),
                    Rect_Right = Convert.ToInt32(m_Row["Rect_Right"]),
                    m_Path = m_Row["Path"].ToString(),
                };
                return data;
            }
            return null;
        }
        void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Template_ID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
            var data = GetTemplateByID(Template_ID);
            if (data != null)
            {
                btnAddField.Enabled = true;
                label5.Text = data.Name;
                txtTemplateName.Text = data.Name;
                txtMarkers.Text = data.Marked;
                mericSort.Value = data.Sort;
                txtTemplate_ID.Text = data.ID.ToString();

                #region "Add file in listbox"
                if (File.Exists(data.m_Path))
                {
                    #region "Document comment"
                    //string ext = Path.GetExtension(m_File);
                    //string fileName = m_File;
                    //m_strPath = fileName;
                    //int pageSelectIndex = 0;
                    //m_PageOld = SmartOcrSdkExports.OCR_GetPageCount();
                    //if (m_PageOld > m_Page)
                    //{

                    //    for (int i = m_Page; i < m_PageOld; i++)
                    //    {
                    //        var nPageIndex = m_Page;
                    //        int res = SmartOcrSdkExports.OCR_RemovePage(nPageIndex);
                    //    }
                    //    lstLoadedImages.Items.Clear();
                    //}

                    //if (ext == ".pdf")
                    //{
                    //    SmartOcrSdkExports.OCR_AddPagesFromPDF(fileName, null, IntPtr.Zero);
                    //}
                    //else
                    //{
                    //    SmartOcrSdkExports.OCR_AddPageFromImage(fileName);
                    //}
                    //int nPageCount = SmartOcrSdkExports.OCR_GetPageCount() - m_Page;
                    //int index = lstLoadedImages.Items.Count;

                    //if (nPageCount > 1)
                    //{
                    //    for (int i = 0; i < nPageCount; i++)
                    //    {
                    //        pageSelectIndex++;
                    //        string filePath = "";
                    //        filePath = fileName + "_page" + pageSelectIndex;
                    //        lstLoadedImages.Items.Add(filePath);
                    //    }
                    //}
                    //else
                    //{
                    //    lstLoadedImages.Items.Add(fileName);
                    //}
                    //if (lstLoadedImages.SelectedItem == null)
                    //{
                    //    lstLoadedImages.SelectedIndex = 0;
                    //}
                    #endregion

                    AddFromPageOrc(data.m_Path);
                    Rectangle rect = new Rectangle();
                    rect.X = data.X;
                    rect.Y = data.Y;
                    rect.Width = data.Width;
                    rect.Height = data.Height;
                    myRectangles.Add(rect);
                }
                else
                {
                    MessageBox.Show("File không tồn tại !!!!");
                }
                #endregion
                dataGridView(Template_ID);
            }
        }
        int Template_ID = 0;
        void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                this.dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);

            int row = e.RowIndex;
            int col = e.ColumnIndex;
            if (dataGridView2.CurrentCell is System.Windows.Forms.DataGridViewCheckBoxCell)
            {

                DataGridViewCell cell = dataGridView2.CurrentCell;

                DataGridViewCheckBoxCell checkCell =
                    (DataGridViewCheckBoxCell)dataGridView2.Rows[row].Cells[col];

                bool newBool = (bool)checkCell.EditingCellFormattedValue;
                if (!newBool)
                {
                    AddItemListView();
                }
                else
                {
                    DataGridViewTextBoxCell textBoxCell = (DataGridViewTextBoxCell)dataGridView2.Rows[row].Cells[0];
                    var Id = Convert.ToInt16(textBoxCell.Value);
                    var _dic = new Dictionary<string, object>();
                    _dic.Add("ID", Id);
                    var _service = new TemplateService();
                    _service.Update_StatusTemplate(_dic);
                    AddItemListView();
                }
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {

            if (txtTemplateName.Text != "" && txtMarkers.Text != "")
            {
                var _dic = new Dictionary<string, object>();
                var rect = T_Rectangle;

                var m_Sort = mericSort.Value;

                _dic.Add("Name", txtTemplateName.Text);
                _dic.Add("Marked", txtMarkers.Text.Replace("\r\n", ""));
                _dic.Add("Rect_Bottom", rect.Bottom);
                _dic.Add("Rect_Left", rect.Left);
                _dic.Add("Rect_Top", rect.Top);
                _dic.Add("Rect_Right", rect.Right);
                _dic.Add("Path", m_strPath);
                _dic.Add("Sort", m_Sort);
                _dic.Add("X", rect.X);
                _dic.Add("Y", rect.Y);
                _dic.Add("Width", rect.Width);
                _dic.Add("Height", rect.Height);
                var _service = new TemplateService();
                var result = _service.Create_Template(_dic);
                if (result != -1)
                {
                    MessageBox.Show("Record Inserted Successfully");

                    AddItemListView();
                    txtTemplate_ID.Text = result.ToString();
                    label5.Text = txtTemplateName.Text;
                    dataGridView(result);

                    Int32 index = dataGridView2.Rows.Count - 1;
                    int iColumn = dataGridView2.CurrentCell.ColumnIndex;
                    dataGridView2.ClearSelection();
                    dataGridView2.CurrentCell = dataGridView2.Rows[index].Cells[iColumn];
                    dataGridView2.Rows[index].Selected = true;
                    btnAddField.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }
        DataTable _dataTable = new DataTable();
        public void dataGridView(int template_ID = 0)
        {
            dataGridView1.DataSource = null;
            _dataTable.Clear();
            var _dic = new Dictionary<string, object>();
            _dic.Add("Template_ID", template_ID);
            var _service = new TemplateService();
            _dataTable = _service.Region_SelectByTemplateID(_dic);

            //Set AutoGenerateColumns False
            dataGridView1.AutoGenerateColumns = false;
            //Set Columns Count
            dataGridView1.ColumnCount = 6;

            dataGridView1.Columns[0].Name = "Label";
            dataGridView1.Columns[0].HeaderText = "Region Name";
            dataGridView1.Columns[0].DataPropertyName = "Label";

            dataGridView1.Columns[1].HeaderText = "ID";
            dataGridView1.Columns[1].Name = "ID";
            dataGridView1.Columns[1].DataPropertyName = "ID";
            dataGridView1.Columns[1].Visible = false;

            dataGridView1.Columns[2].Name = "Rec_tBottom";
            dataGridView1.Columns[2].HeaderText = "RectBottom";
            dataGridView1.Columns[2].DataPropertyName = "Rect_Bottom";


            dataGridView1.Columns[3].Name = "Rect_Top";
            dataGridView1.Columns[3].HeaderText = "RectTop";
            dataGridView1.Columns[3].DataPropertyName = "Rect_Top";

            dataGridView1.Columns[4].Name = "Rect_Left";
            dataGridView1.Columns[4].HeaderText = "RectLeft";
            dataGridView1.Columns[4].DataPropertyName = "Rect_Left";

            dataGridView1.Columns[5].Name = "Rect_Right";
            dataGridView1.Columns[5].HeaderText = "RectRight";
            dataGridView1.Columns[5].DataPropertyName = "Rect_Right";
            if (_dataTable != null && _dataTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = _dataTable;
            }
            dataGridView1.Refresh();
        }

        //private string _ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        public void AddItemListView()
        {
            var _service = new TemplateService();
            var _dataTable = _service.Template_Select();
            if (_dataTable != null && _dataTable.Rows.Count > 0)
            {
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.DataSource = _dataTable;
            }

        }
        private void ClearData()
        {
            textBox1.Text = "";
            txtResult.Text = "";
            ID = 0;
        }
        int ID = 0;
        private void btnAddField_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && txtResult.Text != "")
            {
                var _dic = new Dictionary<string, object>();
                var rect = R_Rectangle;
                var _Template_ID = Convert.ToInt32(txtTemplate_ID.Text);
                _dic.Add("Lable", textBox1.Text);
                _dic.Add("Rect_Bottom", rect.Bottom);
                _dic.Add("Rect_Top", rect.Top);
                _dic.Add("Rect_Left", rect.Left);
                _dic.Add("Rect_Right", rect.Right);
                _dic.Add("X", rect.X);
                _dic.Add("Y", rect.Y);
                _dic.Add("Width", rect.Width);
                _dic.Add("Height", rect.Height);

                _dic.Add("TemplateID", _Template_ID);
                var _service = new TemplateService();
                var result = _service.Create_Region(_dic);
                if (result == 1)
                {
                    MessageBox.Show("Record Inserted Successfully");
                    dataGridView(_Template_ID);
                    ClearData();
                    btnUpdate.Enabled = false;
                    btnFieldRemove.Enabled = false;
                    Remove();
                }
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }

        //dataGridView1 RowHeaderMouseClick Event  
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            btnUpdate.Enabled = true;
            btnFieldRemove.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                var _dic = new Dictionary<string, object>();
                _dic.Add("Lable", textBox1.Text);
                _dic.Add("ID", ID);
                var _service = new TemplateService();
                var result = _service.Create_Region(_dic);
                if (result == 1)
                {
                    MessageBox.Show("Record Updated Successfully");
                    dataGridView(Template_ID);
                    ClearData();
                    ID = 0;
                    btnUpdate.Enabled = false;
                    btnFieldRemove.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }


        #region drawing
        void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;
                var rect = getRectangle();
                if (rect.Width > 0 && rect.Height > 0)
                {
                    myRectangles.Add(rect);
                }

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

                        }
                        if (txtMarkers.Focused)
                        {
                            txtMarkers.Text = sbText.ToString();
                            T_Rectangle = new Rectangle();
                            T_Rectangle = rect;
                        }
                        else if (txtResult.Focused)
                        {
                            txtResult.Text = sbText.ToString();
                            R_Rectangle = new Rectangle();
                            R_Rectangle = rect;
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

        private void TemplateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Release the OCR engine
                int nPageCount = SmartOcrSdkExports.OCR_GetPageCount();// -m_PageOld;
                for (int i = m_Page; i < nPageCount; i++)
                {
                    var nPageIndex = m_Page;
                    int res = SmartOcrSdkExports.OCR_RemovePage(nPageIndex);
                }
            }
            catch (System.Exception)
            {

            }
        }

    }

    internal class OCR_Template_DTO
    {
        public string Name { get; set; }
        public string Marked { get; set; }
        public int Sort { get; set; }
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rect_Bottom { get; set; }
        public int Rect_Top { get; set; }
        public int Rect_Left { get; set; }
        public int Rect_Right { get; set; }
        public string m_Path { get; set; }
    }
}