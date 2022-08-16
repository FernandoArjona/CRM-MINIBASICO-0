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
    /// Interaction logic for Leads.xaml
    /// </summary>
    public partial class Leads : Page
    {
        string name;
        string email;
        string phone;
        string note;
        SQLiteCommander commander = new SQLiteCommander();

        public Leads()
        {
            InitializeComponent();
            //SQLiteCommander builder = new SQLiteCommander();
            //builder.Command(@"CREATE TABLE LEADS(NAME TEXT, MAIL TEXT, TEL TEXT, NOTE TEXT)");
        }

        private void RefreshFields()
        {
            name = this.Leads_name.Text;
            email = this.Leads_email.Text;
            phone = this.Leads_phone.Text;
            note = this.Leads_note.Text;
        }

        private void AddLead_Click(object sender, RoutedEventArgs e)
        {
            RefreshFields();
            commander.WriteCommand($"INSERT OR IGNORE INTO LEADS VALUES ('{name}', '{email}', '{phone}', '{note}')");
        }
    }
}
