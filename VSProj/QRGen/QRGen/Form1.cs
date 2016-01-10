using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.Office.Interop.Excel;

namespace QRGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path = null;
        string excel_path = null;

        private void btnGen_Click(object sender, EventArgs e)
        {

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            string str;
            int rCnt = 0;
            //int cCnt = 0;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(excel_path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
            {
                str = (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2;
                str = str.ToUpper();
                //MessageBox.Show(str);
                if (!SaveQR(str))
                {
                    MessageBox.Show("Ошибка в строке: "+rCnt.ToString() + " ("+ str +")");
                }
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Готово!");
        }


        private bool SaveQR(string value)
        {
            try
            {
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap qrcode = encoder.Encode(value);
                //path = "C:\\Users\\Art\\Desktop\\qrcodes\\" + value + ".png";
                using (var newBitmap = new Bitmap(qrcode))
                {
                    newBitmap.Save(path + "\\" + value + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch 
            {
                return false;
            }
            

            return true;
        
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgFolder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dlgFolder.SelectedPath;
                lblPath.Text = path;
                panel2.Enabled = true;
            }
        }

        private void dlgFolder_HelpRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Выберите папку");
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgFile.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                excel_path = dlgFile.FileName;
                lblFilepath.Text = excel_path;
                panel3.Enabled = true;
            }
        }


    }
}
