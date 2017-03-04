using OCRCore.Business.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OCRCore
{
    public partial class NewFieldTemplate : Form
    {
        public NewFieldTemplate()
        {
            InitializeComponent();
            btnCancel.Click += btnCancel_Click;
            btnOK.Click += btnOK_Click;
        }
        //private string _ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        void btnOK_Click(object sender, EventArgs e)
        {
            var _Name = txtTemplateField.Text;
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("Name", _Name);
            _dic.Add("Decription", _Name);


            var _service = new TemplateService();
            int result = _service.Create_Template(_dic);
            if (result == 1)
            {
                this.Close();
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
