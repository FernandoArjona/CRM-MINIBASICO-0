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
    /// Interaction logic for Clientes.xaml
    /// </summary>
    public partial class Clientes : Page
    {
        string field0;
        string field1;
        string field2;
        string field3;
        string field4;

        SQLiteCommander commander = new SQLiteCommander();
        public Clientes()
        {
            InitializeComponent();
        }

        private void RefreshFields()
        {
            field0 = this.matricula.Text;
            field1 = this.clientes_name.Text;
            field2 = this.clientes_email.Text;
            field3 = this.clientes_phone.Text;
            field4 = this.clientes_note.Text;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RefreshFields();
            commander.Command($"INSERT OR IGNORE INTO CLIENTES VALUES ('{field0}', '{field1}', '{field2}', '{field3}', '{field4}')");
        }
    }
}
