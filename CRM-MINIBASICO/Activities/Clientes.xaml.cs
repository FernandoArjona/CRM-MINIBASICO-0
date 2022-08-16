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
        int warningLevel = 0;
        bool ignoreWarnings = false;
        bool dataIntegrity;

        SQLiteCommander commander = new SQLiteCommander();
        public Clientes()
        {
            InitializeComponent();
        }

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

        private void IgnoreWarnings_Click(object sender, RoutedEventArgs e)
        {
            if (warningLevel < 3)
                this.clientes_zonaAdvertencias.Visibility = Visibility.Hidden;
            ignoreWarnings = true;
        }
    }
}
