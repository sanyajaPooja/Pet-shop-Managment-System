using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace pet_shop
{
    public partial class Billings : Form
    {
        public Billings()
        {
            InitializeComponent();
            //EmpNameLbl.Text = Login.Employee;
            GetCustomers();
            DisplayProduct();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Radhu\Documents\PetShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void GetCustomers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTbl",Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId",typeof(int));
            dt.Load(Rdr);
            CustIdCb.ValueMember = "CustId";
            CustIdCb.DataSource = dt;
            Con.Close();
        }
        private void DisplayProduct()
        {
            Con.Open();
            String Query = "Select * from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void CustIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustName();
        }
        private void GetCustName()
        {
            try
            {
                Con.Open();
                string query = "SELECT CustName FROM CustomerTbl WHERE CustId = @CustId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@CustId", CustIdCb.SelectedValue); 

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    CustNameTb.Text = dt.Rows[0]["CustName"].ToString();
                }
                else
                {
                  
                    CustNameTb.Text = "Customer not found";
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
            //Con.Open();
            //String Query = "Select * from CustomerTbl where CustId='" + CustIdCb.SelectedValue.ToString();
            //SqlCommand cmd = new SqlCommand(Query, Con);
            //DataTable dt = new DataTable();
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //sda.Fill(dt);
            //foreach(DataRow dr in dt.Rows)
            //{
            //    CustNameTb.Text = dr["CustNameTb"].ToString();
            //}
            //Con.Close();
        }

        private void UpdateStock()
        {
            try
            {
                int Stock = 0; 
                int key = 0;   

                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Upadate ProductTbl set PrQty=@PQ where PrId=@Pkey", Con);
                cmd.Parameters.AddWithValue("@PQ", NewQty);
                cmd.Parameters.AddWithValue("@Pkey",key);
                cmd.ExecuteNonQuery();
                Con.Close();
                DisplayProduct();
            }
            catch(Exception Ex)
            {

                MessageBox.Show(Ex.Message);
            }
            }
        int n = 0, GrdTotal = 0;
        int Stock,key = 0;
        private void SaveBtn_Click(object sender, EventArgs e)//add to bill
        {
            if(QtyTb.Text==""|| Convert.ToInt32(QtyTb.Text)>Stock)
            {
                MessageBox.Show("No Enough In House");
           }
            else if(QtyTb.Text==""||key == 0)
            {
                MessageBox.Show("Missing information");
            }
            else
            {

                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);

                
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = PrNameTb.Text;
                newRow.Cells[2].Value = QtyTb.Text;
                newRow.Cells[3].Value = PrPriceTb.Text;
                newRow.Cells[4].Value = total;
                GrdTotal = GrdTotal + total;
                BillDGV.Rows.Add(newRow);
                n++;
                TotalLbl.Text = "Rs" + GrdTotal;
                UpdateStock();
                Reset();


            }
        }

        private void label2_Click(object sender, EventArgs e)//home page
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)//product page
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)//employees page
        {
            Employees Obj = new Employees();
            Obj.Show();
            this.Hide();
        }
        private void Reset()///reset page
        {
            PrNameTb.Text = "";
            QtyTb.Text = "";
            PrPriceTb.Text = "";
            Stock = 0;
            key = 0;
        }
        private void EditBtn_Click(object sender, EventArgs e)//reset button
        {
            Reset();
        }

        private void label8_Click(object sender, EventArgs e)//logout
        {
            this.Hide();

            // Show the login form
            Login loginForm = new Login();
            loginForm.ShowDialog();

            // Close the application if login form is closed
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)//customers page
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }
            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//product bill
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ProductsDGV.Rows[e.RowIndex];
                PrNameTb.Text = row.Cells[1].Value.ToString();
                Stock= Convert.ToInt32(row.Cells[3].Value.ToString());
                
                PrPriceTb.Text = row.Cells[4].Value.ToString();



                key = Convert.ToInt32(row.Cells[0].Value.ToString());
            }
            //PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            //Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
            //PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            //if (PrNameTb.Text == "")
            //{
            //    key = 0;
            //}
            //else
            //{
            //    key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
            //}
        }
    }
}
