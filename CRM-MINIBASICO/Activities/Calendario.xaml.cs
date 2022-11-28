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
    /// Interaction logic for Calendario.xaml
    /// </summary>
    public partial class Calendario : Page
    {
        public Calendario()
        {
            InitializeComponent();
            setStartDateBox(true);
        }

        private void setStartDateBox(bool isDefault)
        {
            for (int i = 2000; i < 2100; i++)
            {
                this.comboBox_year.Items.Add(i);
            }

            if (isDefault == true)
            {
                DefaultDate();
            }

  
            int month = this.comboBox_mes.SelectedIndex + 1;
            int year = this.comboBox_year.SelectedIndex + 2000;
            if (this.comboBox_year.SelectedItem != null)
                year = Int32.Parse(this.comboBox_year.SelectedItem.ToString());
            if (month == 0)
                month++;

            string dateString = $"1/{month}/{year}";
            DateTime date = Convert.ToDateTime(dateString);
            int firstDow = 0;
            
            switch (date.DayOfWeek.ToString())
            {
                case ("Sunday"):
                    {
                        firstDow = 0;
                        break;
                    }
                case ("Monday"):
                    {
                        firstDow = 1;
                        break;
                    }
                case ("Wednesday"):
                    {
                        firstDow = 2;
                        break;
                    }
                case ("Tuesday"):
                    {
                        firstDow = 3;
                        break;
                    }
                case ("Thursday"):
                    {
                        firstDow = 4;
                        break;
                    }
                case ("Friday"):
                    {
                        firstDow = 5;
                        break;
                    }
                case ("Saturday"):
                    {
                        firstDow = 6;
                        break;
                    }
            }

            this.dateButton10.Content = date.DayOfWeek.ToString();

            int j = 1;
            this.dateButton1.Content = (0 >= firstDow ? j++ : 2);
            this.dateButton2.Content = (1 >= firstDow ? j++ : 2);
            this.dateButton3.Content = (2 >= firstDow ? j++ : 2);
            this.dateButton4.Content = (3 >= firstDow ? j++ : 2);
            this.dateButton5.Content = (4 >= firstDow ? j++ : 2);
            this.dateButton6.Content = (5 >= firstDow ? j++ : 2);
            this.dateButton7.Content = (6 >= firstDow ? j++ : 2);

            
        }

        private void DefaultDate()
        {
            this.comboBox_year.SelectedIndex = 0;
            this.comboBox_mes.SelectedIndex = 0;
        }

        private void Date_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setStartDateBox(false);
        }
    }
}
