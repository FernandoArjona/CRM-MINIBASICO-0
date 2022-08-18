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
    /// Interaction logic for Cotizacion.xaml
    /// </summary>
    public partial class Cotizacion : Page
    {
        string field1 = "";
        long field2 = 0;
        string field3 = "";
        SQLiteCommander commander = new SQLiteCommander();

        public Cotizacion()
        {
            InitializeComponent();
        }

        private void RefreshFields()
        {
            field1 = this.concepto.Text;
            field2 = (long)Convert.ToDouble(this.cotizacion.Text);
            field3 = this.cliente_matricula.Text;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RefreshFields();
            commander.WriteCommand($"INSERT OR IGNORE INTO COTIZACIONES VALUES ('{field1}', {field2}, '{field3}')");
        }
    }


}
