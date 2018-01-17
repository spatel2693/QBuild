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
using System.IO;

namespace QBuild
{
    public partial class Form1 : Form
    {
        static string path = Path.GetFullPath(Environment.CurrentDirectory);
        static string databaseName = "QBuild.mdf";
        SqlConnection con = new SqlConnection(@"Data Source=(Localdb)\MSSQLLocalDB;AttachDbFilename=" + path + @"\" + databaseName + ";Integrated Security=True; Connect Timeout = 30");


        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("THE PATH IS :::::::: " + path);
        }

        private void populateData(object sender, EventArgs e)
        {
            try
            {
                con.Open();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            treeView1.Nodes.Clear();

            SqlCommand cm = new SqlCommand("SELECT PARENT_NAME, COMPONENT_NAME FROM bom ", con);


            try
            {

                SqlDataReader dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    TreeNode node = new TreeNode(dr["PARENT_NAME"].ToString());
                    // TreeNode node2 = new TreeNode(dr["COMPONENT_NAME"].ToString());

                    node.Nodes.Add(dr["COMPONENT_NAME"].ToString());
                    treeView1.Nodes.Add(node);


                }
                button1.Enabled = false;
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayData();
        }
        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapt = new SqlDataAdapter("SELECT PARENT_NAME, COMPONENT_NAME, PART_NUMBER, TITLE, QUANTITY, TYPE, ITEM, MATERIAL FROM bom JOIN part ON bom.COMPONENT_NAME = part.NAME", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string selected_node;
            string selected_parent;

            if (e.Node.Parent == null)
            {
                selected_node = e.Node.Text;
                selected_parent = "No Parent";
                label3.Text = selected_parent;
            }
            else
            {
                selected_node = e.Node.Text;
                selected_parent = e.Node.Parent.Text;
                label3.Text = selected_parent + "/" + selected_node;

            }


            label4.Text = selected_node;

        }
    }
}
