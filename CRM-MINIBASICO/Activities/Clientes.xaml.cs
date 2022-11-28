using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        int page = 0;

        SQLiteCommander commander = new SQLiteCommander();
        public Clientes()
        {
            InitializeComponent();
            setContentCollectionTable();
            page = 0;
            this.Buscar_ComboBox.SelectedIndex = 0;
        }

        #region data integrity
        int warningLevel = 0;
        bool ignoreWarnings = false;
        bool dataIntegrity;

        private void RefreshFields()
        {
            matricula = this.clientes_matricula.Text;
            telefono = this.clientes_telefono.Text;
            nombre = this.clientes_nombre.Text;
            email = this.clientes_email.Text;
            nota = this.clientes_nota.Text;
        }

        private bool FieldsAreCorrect()
        {
            return FieldsAreCorrect(false);
        }

        private bool FieldsAreCorrect(bool updating)
        {
            RefreshFields();
            //Fields: MATRICULA
            if (string.IsNullOrEmpty(matricula))
            {
                dataIntegrity = false;
                IssueWarning("El campo de matricula no puede esar vacio", 3);
            }
            string queryText = $"SELECT * FROM CLIENTES WHERE MATRICULA = '{matricula}'";
            
            List<string> identicalIssues = commander.ReadCommand(queryText, "matricula");
            if ((identicalIssues.Count > 0)&&(!updating))
            {
                IssueWarning("La base de datos ya tiene una matricula nombrada " + matricula + ". Favor de cambiar esta matricula por una nueva.", 3);
            }

            //Fields: TELEFONO
            if (!CheckIfNumeric(telefono) && (ignoreWarnings == false))
            {
                this.clientes_telefono_block.FontWeight = FontWeights.Bold;
                this.clientes_telefono_block.Background = Brushes.Red;
                IssueWarning("El campo de telefono debe de estar vacio o solo tener numeros", 2);
            }

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

            ToggleView();
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

            this.clientes_matricula.Text    = clientes_matriculas[buttonClicked + (page*10)];
            this.clientes_nombre.Text       = clientes_nombres[buttonClicked + (page * 10)];
            this.clientes_email.Text        = clientes_emails[buttonClicked + (page * 10)];
            this.clientes_telefono.Text     = clientes_telefonos[buttonClicked + (page * 10)];
            this.clientes_nota.Text         = clientes_notas[buttonClicked + (page * 10)];
        }

        private void setContentCollectionTable()
        {
            setContentCollectionTable("SELECT * FROM CLIENTES");
        }

        private void setContentCollectionTable(string query)
        {
            int itemCount = page * 10;
            SQLiteCommander commander = new SQLiteCommander();
            List<string> clientes = commander.ReadCommand(query, "NOMBRE");

            //nombres
            if (clientes.Count >= itemCount + 1)
                contentEntry_1.Text = clientes[itemCount + 0];
            else
                contentEntry_1.Text = "";
            if (clientes.Count >= itemCount + 2)
                contentEntry_2.Text = clientes[itemCount + 1];
            else
                contentEntry_2.Text = "";
            if (clientes.Count >= itemCount + 3)
                contentEntry_3.Text = clientes[itemCount + 2];
            else
                contentEntry_3.Text = "";
            if (clientes.Count >= itemCount + 4)
                contentEntry_4.Text = clientes[itemCount + 3];
            else
                contentEntry_4.Text = "";
            if (clientes.Count >= itemCount + 5)
                contentEntry_5.Text = clientes[itemCount + 4];
            else
                contentEntry_5.Text = "";
            if (clientes.Count >= itemCount + 6)
                contentEntry_6.Text = clientes[itemCount + 5];
            else
                contentEntry_6.Text = "";
            if (clientes.Count >= itemCount + 7)
                contentEntry_7.Text = clientes[itemCount + 6];
            else
                contentEntry_7.Text = "";
            if (clientes.Count >= itemCount + 8)
                contentEntry_8.Text = clientes[itemCount + 7];
            else
                contentEntry_8.Text = "";
            if (clientes.Count >= itemCount + 9)
                contentEntry_9.Text = clientes[itemCount + 8];
            else
                contentEntry_9.Text = "";
            if (clientes.Count >= itemCount + 10)
                contentEntry_10.Text = clientes[itemCount + 9];
            else
                contentEntry_10.Text = "";

            //matricula
            List<string> matriculas = commander.ReadCommand(query, "MATRICULA");
            if (matriculas.Count >= itemCount + 1)
                matriculaEntry_1.Text = matriculas[itemCount + 0];
            else
                matriculaEntry_1.Text = "";
            if (matriculas.Count >= itemCount + 2)
                matriculaEntry_2.Text = matriculas[itemCount + 1];
            else
                matriculaEntry_2.Text = "";
            if (matriculas.Count >= itemCount + 3)
                matriculaEntry_3.Text = matriculas[itemCount + 2];
            else
                matriculaEntry_3.Text = "";
            if (matriculas.Count >= itemCount + 4)
                matriculaEntry_4.Text = matriculas[itemCount + 3];
            else
                matriculaEntry_4.Text = "";
            if (matriculas.Count >= itemCount + 5)
                matriculaEntry_5.Text = matriculas[itemCount + 4];
            else
                matriculaEntry_5.Text = "";
            if (matriculas.Count >= itemCount + 6)
                matriculaEntry_6.Text = matriculas[itemCount + 5];
            else
                matriculaEntry_6.Text = "";
            if (matriculas.Count >= itemCount + 7)
                matriculaEntry_7.Text = matriculas[itemCount + 6];
            else
                matriculaEntry_7.Text = "";
            if (matriculas.Count >= itemCount + 8)
                matriculaEntry_8.Text = matriculas[itemCount + 7];
            else
                matriculaEntry_8.Text = "";
            if (matriculas.Count >= itemCount + 9)
                matriculaEntry_9.Text = matriculas[itemCount + 8];
            else
                matriculaEntry_9.Text = "";
            if (matriculas.Count >= itemCount + 10)
                matriculaEntry_10.Text = matriculas[itemCount + 9];
            else
                matriculaEntry_10.Text = "";

            //correo
            List<string> correos = commander.ReadCommand(query, "EMAIL");
            if (correos.Count >= itemCount + 1)
                correoEntry_1.Text = correos[itemCount + 0];
            else
                correoEntry_1.Text = "";
            if (correos.Count >= itemCount + 2)
                correoEntry_2.Text = correos[itemCount + 1];
            else
                correoEntry_2.Text = "";
            if (correos.Count >= itemCount + 3)
                correoEntry_3.Text = correos[itemCount + 2];
            else
                correoEntry_3.Text = "";
            if (correos.Count >= itemCount + 4)
                correoEntry_4.Text = correos[itemCount + 3];
            else
                correoEntry_4.Text = "";
            if (correos.Count >= itemCount + 5)
                correoEntry_5.Text = correos[itemCount + 4];
            else
                correoEntry_5.Text = "";
            if (correos.Count >= itemCount + 6)
                correoEntry_6.Text = correos[itemCount + 5];
            else
                correoEntry_6.Text = "";
            if (correos.Count >= itemCount + 7)
                correoEntry_7.Text = correos[itemCount + 6];
            else
                correoEntry_7.Text = "";
            if (correos.Count >= itemCount + 8)
                correoEntry_8.Text = correos[itemCount + 7];
            else
                correoEntry_8.Text = "";
            if (correos.Count >= itemCount + 9)
                correoEntry_9.Text = correos[itemCount + 8];
            else
                correoEntry_9.Text = "";
            if (correos.Count >= itemCount + 10)
                correoEntry_10.Text = correos[itemCount + 9];
            else
                correoEntry_10.Text = "";

            //telefono
            List<string> telefonos = commander.ReadCommand(query, "TELEFONO");
            if (telefonos.Count >= itemCount + 1)
                telefonoEntry_1.Text = telefonos[itemCount + 0];
            else
                telefonoEntry_1.Text = "";
            if (telefonos.Count >= itemCount + 2)
                telefonoEntry_2.Text = telefonos[itemCount + 1];
            else
                telefonoEntry_2.Text = "";
            if (telefonos.Count >= itemCount + 3)
                telefonoEntry_3.Text = telefonos[itemCount + 2];
            else
                telefonoEntry_3.Text = "";
            if (telefonos.Count >= itemCount + 4)
                telefonoEntry_4.Text = telefonos[itemCount + 3];
            else
                telefonoEntry_4.Text = "";
            if (telefonos.Count >= itemCount + 5)
                telefonoEntry_5.Text = telefonos[itemCount + 4];
            else
                telefonoEntry_5.Text = "";
            if (telefonos.Count >= itemCount + 6)
                telefonoEntry_6.Text = telefonos[itemCount + 5];
            else
                telefonoEntry_6.Text = "";
            if (telefonos.Count >= itemCount + 7)
                telefonoEntry_7.Text = telefonos[itemCount + 6];
            else
                telefonoEntry_7.Text = "";
            if (telefonos.Count >= itemCount + 8)
                telefonoEntry_8.Text = telefonos[itemCount + 7];
            else
                telefonoEntry_8.Text = "";
            if (telefonos.Count >= itemCount + 9)
                telefonoEntry_9.Text = telefonos[itemCount + 8];
            else
                telefonoEntry_9.Text = "";
            if (telefonos.Count >= itemCount + 10)
                telefonoEntry_10.Text = telefonos[itemCount + 9];
            else
                telefonoEntry_10.Text = "";

            string page_number_text = (itemCount + 1) + " a " + (itemCount + 10);
            this.page_number.Text = page_number_text;
        }

        private void ToggleView_Click(object sender, RoutedEventArgs e)
        {
            ToggleView();
        }

        private void ToggleView()
        {
            if (this.MainContentGrid.Visibility == Visibility.Visible)
            {
                this.MainContentGrid.Visibility = Visibility.Hidden;
                this.Bar_Menu_List_Options.Visibility = Visibility.Visible;
                this.Bar_Menu_Buttons.Visibility = Visibility.Hidden;
                this.CollectionTableGrid.Visibility = Visibility.Visible;

            }
            else
            {
                this.MainContentGrid.Visibility = Visibility.Visible;
                this.Bar_Menu_Buttons.Visibility = Visibility.Visible;
                this.Bar_Menu_List_Options.Visibility = Visibility.Hidden;
                this.CollectionTableGrid.Visibility = Visibility.Hidden;
            }
        }

        private void LeftArrow_Click(object sender, RoutedEventArgs e)
        {
            if ((page - 1) >= 0)
            {
                page--;
                setContentCollectionTable();
            }
            
        }

        private void RightArrow_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM CLIENTES";
            SQLiteCommander commander = new SQLiteCommander();
            List<string> clientes = commander.ReadCommand(query, "NOMBRE");
            int pageCap = (page + 1) * 10;
            if (clientes.Count >= pageCap)
            {
                page++;
                setContentCollectionTable();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            IssueWarning("Estas a punto de borrar este articulo. Ignora advertencias y vuelve a intentar si quieres proseguir.", 2);
            if (ignoreWarnings == true)
            {
                string query = "DELETE FROM CLIENTES WHERE MATRICULA = '" + this.clientes_matricula.Text + "'";
                SQLiteCommander commander = new SQLiteCommander();
                commander.WriteCommand(query);
                this.clientes_matricula.Text = "";
                this.clientes_nombre.Text = "";
                this.clientes_email.Text = "";
                this.clientes_nota.Text = "";
                this.clientes_telefono.Text = "";
                this.clientes_advertencias.Text = "";
                IssueWarning("Articulo borrado exitosamente.", 1);
                setContentCollectionTable();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            warningLevel = 0;
            dataIntegrity = true;
            this.clientes_advertencias.Text = "";
            this.clientes_zonaAdvertencias.Background = Brushes.White;
            this.clientes_zonaAdvertencias.Visibility = Visibility.Hidden;

            warningLevel = 0;
            dataIntegrity = true;
            this.clientes_advertencias.Text = "";
            this.clientes_zonaAdvertencias.Background = Brushes.White;
            this.clientes_zonaAdvertencias.Visibility = Visibility.Hidden;

            
            RefreshFields();
            commander.WriteCommand($"UPDATE CLIENTES SET MATRICULA = '{matricula}', NOMBRE = '{nombre}', EMAIL = '{email}', TELEFONO = '{telefono}', NOTA ='{nota}' WHERE MATRICULA = '{matricula}'");
            IssueWarning("Se han actualizado los campos exitosamente", 1);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = this.Buscar_TextBox.Text;
            string selectedTerm = this.Buscar_ComboBox.Text;
            if (!selectedTerm.Equals("Buscar por..."))
            {
                string queryText = $"SELECT * FROM CLIENTES WHERE {selectedTerm} LIKE '%{query}%'";
                setContentCollectionTable(queryText);
            }
            else
            {
                setContentCollectionTable();
            }
        }

        private void AutogenerarMatricula_Click(object sender, RoutedEventArgs e)
        {
            string letters = "abcdefghijklmnopqrstuvwxy";
            Random rnd = new Random();
            int num = rnd.Next(1000, 2000);
            char[] randomPicks = new char[4];
            char[] chars = letters.ToCharArray();
            randomPicks[0] = chars[rnd.Next(0, 23)];
            randomPicks[1] = chars[rnd.Next(0, 23)];
            randomPicks[2] = chars[rnd.Next(0, 23)];
            randomPicks[3] = chars[rnd.Next(0, 23)];
            string numstring = num.ToString();
            char[] numChars = numstring.ToCharArray();
            string id = $"{randomPicks[0]}{numChars[0]}{randomPicks[1]}{numChars[1]}{randomPicks[2]}{numChars[2]}{randomPicks[3]}{numChars[3]}";
            this.clientes_matricula.Text = id;
        }
    }
}
