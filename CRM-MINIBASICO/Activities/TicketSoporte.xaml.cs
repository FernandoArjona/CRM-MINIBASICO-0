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
    /// Interaction logic for TicketSoporte.xaml
    /// </summary>
    public partial class TicketSoporte : Page
    {
        public TicketSoporte()
        {
            InitializeComponent();
        }

        string field1 = "";
        string field2 = "";
        string field3 = "";
        string field4 = "";
        SQLiteCommander commander = new SQLiteCommander();

        private void RefreshFields()
        {
            field1 = this.titulo.Text;
            field2 = this.cliente.Text;
            field4 = this.descripcion.Text;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RefreshFields();
            commander.Command($"INSERT OR IGNORE INTO TICKETSOPORTE VALUES ('{field1}', '{field2}', '{field3}', '{field4}')");
        }
    }
}
