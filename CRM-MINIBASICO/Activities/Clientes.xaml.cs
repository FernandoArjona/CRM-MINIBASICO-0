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
        string matricula;
        string nombre;
        string email;
        string telefono;
        string nota;

        SQLiteCommander commander = new SQLiteCommander();
        public Clientes()
        {
            InitializeComponent();
            setContentCollectionTable();
        }

        #region data integrity
        int warningLevel = 0;
        bool ignoreWarnings = false;
        bool dataIntegrity;

        private bool FieldsAreCorrect()
        {
            //Fields: MATRICULA
            matricula = this.clientes_matricula.Text;
            if (string.IsNullOrEmpty(matricula))
            {
                dataIntegrity = false;
                IssueWarning("El campo de matricula no puede esar vacio", 3);
            }
            string queryText = "SELECT * FROM CLIENTES WHERE MATRICULA =" + matricula;
            /*
             * TO-DO: FIX THIS
             * List<string> identicalIssues = commander.ReadCommand(queryText, "matricula");
            if (identicalIssues.Count > 0)
            {
                IssueWarning("La base de datos ya tiene una matricula nombrada " + matricula + ". Favor de cambiar esta matricula por una nueva.", 3);
            }*/

            //Fields: TELEFONO
            telefono = this.clientes_telefono.Text;
            if (!CheckIfNumeric(telefono) && (ignoreWarnings == false))
            {
                this.clientes_telefono_block.FontWeight = FontWeights.Bold;
                this.clientes_telefono_block.Background = Brushes.Red;
                IssueWarning("El campo de telefono debe de estar vacio o solo tener numeros", 2);
            }

            //Fields: NOMBRE, EMAIL, & NOTA
            nombre = this.clientes_nombre.Text;
            email = this.clientes_email.Text;
            nota = this.clientes_nota.Text;


            //Check if warnings can be ignored
            if ((warningLevel <= 2) && (ignoreWarnings == true))
                dataIntegrity = true;

            return dataIntegrity;
        }
        
        private bool CheckIfNumeric(string text)
        {
            return double.TryParse(text, out _) || (text == string.Empty);
        }
       
        private void IssueWarning(string warning, int level)
        {
            /*
             * Issues a warning to the warning zone fields in case that the user makes a mistake.
             *Level 1 warnings may be completely disregarded, and serve only as notifications
             *such as "task done successfully!"
             *Level 2 warnings must flag for a lack of data integrity,
             *unless minor warnings are explicitly set to be ignored by the user.
             *Level 3 warnings must flag for a lack of data integrity, and cannot be disregarded.
             *level 4 warnings must flag for a lack of data integrity, cannot be disregarded, and
             *are to a level of error that should not be possible to be reached. This represents
             a serious flaw in the system.
            */

            this.clientes_zonaAdvertencias.Visibility = Visibility.Visible;
            if (level > warningLevel)
                warningLevel = level;

            switch (level)
            {
                case 1:
                    {
                        if (level >= warningLevel)
                            this.clientes_advertencias.Background = Brushes.Gray;
                        this.clientes_advertencias.Text += "\n [Notificación:]" + warning;
                        break;
                    }
                case 2:
                    {
                        if ((level >= warningLevel)&&(ignoreWarnings == false))
                        {
                            this.clientes_advertencias.Background = Brushes.Yellow;
                            this.clientes_advertencias.Text += "\n [Advertencia nivel 2:]" + warning;
                            dataIntegrity = false;
                        }
                        break;
                    }
                case 3:
                    {
                        if (level >= warningLevel)
                            this.clientes_advertencias.Background = Brushes.Red;
                        this.clientes_advertencias.Text += "\n [Advertencia nivel 3:]" + warning + "(No se pueden ignorar advertencias de nivel 3)";
                        dataIntegrity = false;
                        break;
                    }
                default:
                    {
                        this.clientes_advertencias.Background = Brushes.Purple;
                        this.clientes_advertencias.Text += "\n [Advertencia nivel 4:]" + warning + "De alguna forma, haz entrado a un nivel de advertencia que no deberia de ser posible. Contacta al autor del proyecto.";
                        dataIntegrity = false;
                        break;
                    }
            }
        }
        #endregion

        #region click
        private void IgnoreWarnings_Click(object sender, RoutedEventArgs e)
        {
            if (warningLevel < 3)
                this.clientes_zonaAdvertencias.Visibility = Visibility.Hidden;
            ignoreWarnings = true;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            warningLevel = 0;
            dataIntegrity = true;
            this.clientes_advertencias.Text = "";
            this.clientes_zonaAdvertencias.Background = Brushes.White;
            this.clientes_zonaAdvertencias.Visibility = Visibility.Hidden;
            if (FieldsAreCorrect())
            {
                commander.WriteCommand($"INSERT OR IGNORE INTO CLIENTES VALUES ('{matricula}', '{nombre}', '{email}', '{telefono}', '{nota}')");
                IssueWarning("Se han guardado los campos exitosamente", 1);
            }
        }
        #endregion

        private void btn_view_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string buttonName = button.Name;

            this.CollectionTableGrid.Visibility = Visibility.Hidden;
            this.MainContentGrid.Visibility = Visibility.Visible;
            int buttonClicked = 0;
            switch (buttonName)
            {
                case "btn_view1":
                    {
                        buttonClicked = 1;
                        break;
                    }
                case "btn_view2":
                    {
                        buttonClicked = 2;
                        break;
                    }
                case "btn_view3":
                    {
                        buttonClicked = 3;
                        break;
                    }
                case "btn_view4":
                    {
                        buttonClicked = 4;
                        break;
                    }
                case "btn_view5":
                    {
                        buttonClicked = 5;
                        break;
                    }
                case "btn_view6":
                    {
                        buttonClicked = 6;
                        break;
                    }
                case "btn_view7":
                    {
                        buttonClicked = 7;
                        break;
                    }
                case "btn_view8":
                    {
                        buttonClicked = 8;
                        break;
                    }
                case "btn_view9":
                    {
                        buttonClicked = 9;
                        break;
                    }
                case "btn_view10":
                    {
                        buttonClicked = 10;
                        break;
                    }
                default:
                    {
                        buttonClicked = 1;
                        break;
                    }
            }
            buttonClicked--;

            string query = "SELECT * FROM CLIENTES";
            SQLiteCommander commander           = new SQLiteCommander();
            List<string> clientes_matriculas    = commander.ReadCommand(query, "MATRICULA");
            List<string> clientes_nombres       = commander.ReadCommand(query, "NOMBRE");
            List<string> clientes_emails        = commander.ReadCommand(query, "EMAIL");
            List<string> clientes_telefonos     = commander.ReadCommand(query, "TELEFONO");
            List<string> clientes_notas         = commander.ReadCommand(query, "NOTA");

            this.clientes_matricula.Text    = clientes_matriculas[buttonClicked];
            this.clientes_nombre.Text       = clientes_nombres[buttonClicked];
            this.clientes_email.Text        = clientes_emails[buttonClicked];
            this.clientes_telefono.Text     = clientes_telefonos[buttonClicked];
            this.clientes_nota.Text         = clientes_notas[buttonClicked];
            /*
            this.clientes_matricula.Text    = clientes_matriculas[buttonClicked];
            this.clientes_nombre.Text       = clientes_nombres[buttonClicked];
            this.clientes_email.Text        = clientes_emails[buttonClicked];
            this.clientes_telefono.Text     = clientes_telefonos[buttonClicked];
            this.clientes_nota.Text         = clientes_notas[buttonClicked];*/
        }

        int contentCount = 0;
        private void setContentCollectionTable()
        {
            string query = "SELECT * FROM CLIENTES";
            SQLiteCommander commander = new SQLiteCommander();
            List<string> clientes = commander.ReadCommand(query, "NOMBRE");
            
            if (clientes.Count >= contentCount + 1)
                contentEntry_1.Text = clientes[0];
            if (clientes.Count >= contentCount + 2)
                contentEntry_2.Text = clientes[1];
            if (clientes.Count >= contentCount + 3)
                contentEntry_3.Text = clientes[2];
            if (clientes.Count >= contentCount + 4)
                contentEntry_4.Text = clientes[3];
            if (clientes.Count >= contentCount + 5)
                contentEntry_5.Text = clientes[4];
            if (clientes.Count >= contentCount + 6)
                contentEntry_6.Text = clientes[5];
            if (clientes.Count >= contentCount + 7)
                contentEntry_7.Text = clientes[6];
            if (clientes.Count >= contentCount + 8)
                contentEntry_8.Text = clientes[7];
            if (clientes.Count >= contentCount + 9)
                contentEntry_9.Text = clientes[8];
            if (clientes.Count >= contentCount + 10)
                contentEntry_10.Text = clientes[9];


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainContentGrid.Visibility == Visibility.Visible)
            {
                this.MainContentGrid.Visibility = Visibility.Hidden;
                this.CollectionTableGrid.Visibility = Visibility.Visible;
            }
            else
            {
                this.MainContentGrid.Visibility = Visibility.Visible;
                this.CollectionTableGrid.Visibility = Visibility.Hidden;
            }
        }
    }
}
