namespace OCRCore
{
    partial class DemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnRemoveRegion = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.lblOutputFormat = new System.Windows.Forms.Label();
            this.cbOutputFormats = new System.Windows.Forms.ComboBox();
            this.btnRecognizeSelected = new System.Windows.Forms.Button();
            this.lblLoadedImages = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lstLoadedImages = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.prgRecognition = new System.Windows.Forms.ProgressBar();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.SystemColors.Control;
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPreview.Location = new System.Drawing.Point(3, 0);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(172, 542);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPreview.TabIndex = 6;
            this.pbPreview.TabStop = false;
            this.pbPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPreview_Paint);
            this.pbPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbPreview_MouseDown);
            this.pbPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbPreview_MouseMove);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.pbPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(481, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 542);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnTemplate);
            this.panel2.Controls.Add(this.btnChooseFile);
            this.panel2.Controls.Add(this.btnRemoveRegion);
            this.panel2.Controls.Add(this.btnExtract);
            this.panel2.Controls.Add(this.lblOutputFormat);
            this.panel2.Controls.Add(this.cbOutputFormats);
            this.panel2.Controls.Add(this.btnRecognizeSelected);
            this.panel2.Controls.Add(this.lblLoadedImages);
            this.panel2.Controls.Add(this.lblProgress);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.lstLoadedImages);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.prgRecognition);
            this.panel2.Controls.Add(this.btnRecognize);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(172, 542);
            this.panel2.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 407);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(10, 93);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(136, 23);
            this.btnTemplate.TabIndex = 28;
            this.btnTemplate.Text = "Tạo Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(10, 436);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(137, 23);
            this.btnChooseFile.TabIndex = 27;
            this.btnChooseFile.Text = "Add File";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            // 
            // btnRemoveRegion
            // 
            this.btnRemoveRegion.Location = new System.Drawing.Point(12, 64);
            this.btnRemoveRegion.Name = "btnRemoveRegion";
            this.btnRemoveRegion.Size = new System.Drawing.Size(134, 23);
            this.btnRemoveRegion.TabIndex = 26;
            this.btnRemoveRegion.Text = "Clear Region";
            this.btnRemoveRegion.UseVisualStyleBackColor = true;
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(12, 35);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(134, 23);
            this.btnExtract.TabIndex = 25;
            this.btnExtract.Text = "Extract Data";
            this.btnExtract.UseVisualStyleBackColor = true;
            // 
            // lblOutputFormat
            // 
            this.lblOutputFormat.AutoSize = true;
            this.lblOutputFormat.Location = new System.Drawing.Point(11, 216);
            this.lblOutputFormat.Name = "lblOutputFormat";
            this.lblOutputFormat.Size = new System.Drawing.Size(126, 13);
            this.lblOutputFormat.TabIndex = 24;
            this.lblOutputFormat.Text = "Output Document Format";
            // 
            // cbOutputFormats
            // 
            this.cbOutputFormats.FormattingEnabled = true;
            this.cbOutputFormats.Items.AddRange(new object[] {
            "RTF",
            "DOC",
            "PDF",
            "HTML",
            "TXT"});
            this.cbOutputFormats.Location = new System.Drawing.Point(10, 232);
            this.cbOutputFormats.Name = "cbOutputFormats";
            this.cbOutputFormats.Size = new System.Drawing.Size(136, 21);
            this.cbOutputFormats.TabIndex = 23;
            // 
            // btnRecognizeSelected
            // 
            this.btnRecognizeSelected.Location = new System.Drawing.Point(10, 122);
            this.btnRecognizeSelected.Name = "btnRecognizeSelected";
            this.btnRecognizeSelected.Size = new System.Drawing.Size(137, 23);
            this.btnRecognizeSelected.TabIndex = 22;
            this.btnRecognizeSelected.Text = "Recognize Selected";
            this.btnRecognizeSelected.UseVisualStyleBackColor = true;
            // 
            // lblLoadedImages
            // 
            this.lblLoadedImages.AutoSize = true;
            this.lblLoadedImages.Location = new System.Drawing.Point(10, 265);
            this.lblLoadedImages.Name = "lblLoadedImages";
            this.lblLoadedImages.Size = new System.Drawing.Size(62, 13);
            this.lblLoadedImages.TabIndex = 21;
            this.lblLoadedImages.Text = "Loaded File";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 459);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(48, 13);
            this.lblProgress.TabIndex = 20;
            this.lblProgress.Text = "Progress";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(137, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // lstLoadedImages
            // 
            this.lstLoadedImages.FormattingEnabled = true;
            this.lstLoadedImages.HorizontalScrollbar = true;
            this.lstLoadedImages.Location = new System.Drawing.Point(10, 281);
            this.lstLoadedImages.Name = "lstLoadedImages";
            this.lstLoadedImages.Size = new System.Drawing.Size(137, 108);
            this.lstLoadedImages.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(20, 504);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // prgRecognition
            // 
            this.prgRecognition.Location = new System.Drawing.Point(10, 475);
            this.prgRecognition.Name = "prgRecognition";
            this.prgRecognition.Size = new System.Drawing.Size(137, 20);
            this.prgRecognition.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgRecognition.TabIndex = 17;
            // 
            // btnRecognize
            // 
            this.btnRecognize.Location = new System.Drawing.Point(10, 151);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(137, 23);
            this.btnRecognize.TabIndex = 15;
            this.btnRecognize.Text = "Recognize All";
            this.btnRecognize.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(137, 23);
            this.btnLoad.TabIndex = 14;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(178, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(303, 542);
            this.panel3.TabIndex = 17;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 360);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(293, 170);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Visible = false;
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 542);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DemoForm";
            this.Text = "KBLAAOCR  Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DemoForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblOutputFormat;
        private System.Windows.Forms.ComboBox cbOutputFormats;
        private System.Windows.Forms.Button btnRecognizeSelected;
        private System.Windows.Forms.Label lblLoadedImages;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lstLoadedImages;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar prgRecognition;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnRemoveRegion;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}