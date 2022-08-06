using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRM_MINIBASICO
{
    /// <summary>
    /// Interaction logic for Catalogo.xaml
    /// </summary>
    public partial class Catalogo : Page
    {
        string field1 = "";
        string field2 = "";
        string field3 = "";
        int field4 = 0;
        long field5 = 0;
        long field6 = 0;
        double field7 = 0;
        string field8 = "";

        SQLiteCommander commander = new SQLiteCommander();

        public Catalogo()
        {
            InitializeComponent();
        }

        private void RefreshFields()
        {
            field1 = this.nombre.Text;
            field2 = this.matricula.Text;
            field3 = this.descripcion.Text;
            field4 = Int32.Parse(this.existencias.Text);
            field5 = (long)Convert.ToDouble(this.precio_venta.Text);
            field6 = (long)Convert.ToDouble(this.precio_compra.Text);
            field7 = (double)Convert.ToDouble(this.descuento_permitido.Text);
            field8 = this.proveedor.Text;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RefreshFields();
            commander.Command($"INSERT OR IGNORE INTO CATALOGO VALUES ('{field1}', '{field2}', '{field3}', {field4}, {field5}, {field6}, {field7}, '{field8}')");
        }
    }
}
