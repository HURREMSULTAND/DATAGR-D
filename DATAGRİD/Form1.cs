using System.Data.SqlClient;
using System.Windows.Forms;
using DATAGRİD.Entities;




namespace DATAGRİD
{
    public partial class Form1 : Form
    {
        Product selectedProduct;


        public Form1()
        {
            InitializeComponent();
            GetList();


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void GetList()
        {
            List<Product> products = new List<Product>();
            SqlConnection connection = new SqlConnection
 ("server=.\\SQLExpress; database=productDb; integrated security=true");

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "select * from Products";

            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var product = new Product();

                product.Id = Convert.ToInt32(dataReader["Id"]);
                product.Name = dataReader["Name"].ToString();
                product.Price = Convert.ToInt32(dataReader["Price"]);

                products.Add(product);
            }

            connection.Close();
            dataGridView1.DataSource = products;


        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (row != null)
                {
                    selectedProduct = new Product();
                    selectedProduct.Id = (int)(row.Cells[0].Value);
                    selectedProduct.Name = row.Cells[1].Value.ToString();
                    selectedProduct.Price = (int)(row.Cells[2].Value);

                    textBox1.Text = selectedProduct.Name;
                    textBox2.Text = selectedProduct.Price.ToString();


                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                var connection = new SqlConnection("server=.\\SQLExpress; database=productDb; integrated security=true;");
                var command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Connection = connection;
                command.CommandText = "update Products set Name=@name where Id=@id";
                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@id", selectedProduct.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();


                selectedProduct.Name = textBox1.Text;
                GetList();
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection connection = new SqlConnection
               ("server=.\\SQLExpress; database=productDb; integrated security=true;");
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "insert into Products (Name,Price) values (@name, @price)";
                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@price", textBox2.Text);


                command.Connection = connection;
                var effectedRows = command.ExecuteNonQuery();

                connection.Close();
                command.Parameters.Clear();
                if (effectedRows > 0)
                {

                }

                textBox1.Text = string.Empty;
                GetList();


            }

        }
    }
}







