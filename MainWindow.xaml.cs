using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Text.RegularExpressions;

namespace LLW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=ATFKPCLAB01\SQLEXPRESS;Initial Catalog=NewDB;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("Select * From Buecher;", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            sdr.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        public void clearData()
        {
            txtISBN.Clear();
            txtTitel.Clear();
            txtVorname.Clear();
            txtNachname.Clear();
            lblID.Content = String.Empty;
        }
        public bool isValid()
        {
            bool checkISBN = txtISBN.Text.Any(char.IsDigit);
            bool checkVorname = txtVorname.Text.Any(char.IsLetter);
            bool checkNachname = txtNachname.Text.Any(char.IsLetter);
            if (txtVorname.Text == string.Empty)
            {
                MessageBox.Show("Vorname benötigt!!");
                return false;
            }
            if (txtNachname.Text == string.Empty)
            {
                MessageBox.Show("Nachname benötigt!!");
                return false;
            }
            if (txtTitel.Text == string.Empty)
            {
                MessageBox.Show("Titel benötigt!!");
                return false;
            }
            if (txtISBN.Text == string.Empty)
            {
                MessageBox.Show("ISBN benötigt!!");
                return false;
            }
            if (checkISBN == false)
            {
                return false;
            }
            if (checkVorname == false)
            {
                return false;
            }
            if (checkNachname == false)
            {
                return false;
            }
            return true;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                lblID.Content = row_selected["ID"].ToString();
                txtISBN.Text = row_selected["ISBN"].ToString();
                txtTitel.Text = row_selected["Titel"].ToString();
                txtVorname.Text = row_selected["Vorname"].ToString();
                txtNachname.Text = row_selected["Nachname"].ToString();
            }
        }

        private void btnLöschen_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete from Buecher where ID = " + lblID.Content + ";", con);
            cmd.ExecuteNonQuery();
            con.Close();
            LoadGrid();
            clearData();
        }

        private void btnHinzufügen_Click(object sender, RoutedEventArgs e)
        {
            if(isValid())
            {
                SqlCommand cmd = new SqlCommand("Insert Into Buecher Values(@ISBN, @Titel, @Vorname, @Nachname);", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ISBN", txtISBN.Text);
                cmd.Parameters.AddWithValue("@Titel", txtTitel.Text);
                cmd.Parameters.AddWithValue("@Vorname", txtVorname.Text);
                cmd.Parameters.AddWithValue("@Nachname", txtNachname.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                LoadGrid();
                clearData();
            }
            else
            {
                MessageBox.Show("Kontrollieren Sie ihre Eingabe!", "Nicht hinzugefügt" , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBearbeiten_Click(object sender, RoutedEventArgs e)
        {
            if(isValid())
            {
            SqlCommand cmd = new SqlCommand("Update Buecher set ISBN = @ISBN, Titel = @Titel, Vorname = @Vorname, Nachname = @Nachname Where ID = @ID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ISBN", txtISBN.Text);
            cmd.Parameters.AddWithValue("@Titel", txtTitel.Text);
            cmd.Parameters.AddWithValue("@Vorname", txtVorname.Text);
            cmd.Parameters.AddWithValue("@Nachname", txtNachname.Text);
            cmd.Parameters.AddWithValue("@ID", lblID.Content);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            LoadGrid();
            clearData();
            }
            else
            {
                MessageBox.Show("Kontrollieren Sie ihre Eingabe!", "Nicht hinzugefügt" , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
