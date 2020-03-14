using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFrmDPS
{
    public partial class FrmDetailData : Form
    {
        DataTable dt;
        public FrmDetailData(DataTable dt1)
        {
            InitializeComponent();
            this.dt = dt1;
        }

        private void FrmDetailData_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Add(dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["ItemCode"].ToString();
                    dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["Locator"].ToString();
                    dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["RequireQuantity"].ToString();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection drCollection = dataGridView1.SelectedRows;
                foreach (DataGridViewRow dr in drCollection)
                {
                    string productionName = dr.Cells[0].Value.ToString();
                    string str = string.Format("select * from T_PRODUCTION_DETAILS where proctionName='{0}'", productionName);   
                    
                }
               
            }
            catch(Exception ex)
            { 
            
            }
        }
    }
}
