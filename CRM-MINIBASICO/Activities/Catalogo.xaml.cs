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
        //TODO: Delete proveedor from SQL database
        string nombre_ = "";
        string matricula_ = "";
        string descripcion_ = "";
        string existencias_ = "";
        string precio_venta_ = "";
        string precio_compra_ = "";
        string descuento_ = "";
        int page = 0;

        SQLiteCommander commander = new SQLiteCommander();

        public Catalogo()
        {
            InitializeComponent();
            setContentCollectionTable();
        }

        private void RefreshFields()
        {
            nombre_         = this.nombre.Text;
            matricula_      = this.matricula.Text;
            descripcion_    = this.descripcion.Text;
            existencias_    = this.existencias.Text;
            precio_venta_   = this.precio_venta.Text;
            precio_compra_  = this.precio_compra.Text;
            descuento_      = this.descuento_permitido.Text;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            warningLevel = 0;
            dataIntegrity = true;
            this.advertencias.Text = "";
            this.zonaAdvertencias.Background = Brushes.White;
            this.zonaAdvertencias.Visibility = Visibility.Hidden;
            if (FieldsAreCorrect())
            {
                commander.WriteCommand($"INSERT OR IGNORE INTO CATALOGO VALUES ('{nombre_}', '{matricula_}', '{descripcion_}', {existencias_}, {precio_venta_}, {precio_compra_}, {descuento_})");
                IssueWarning("Se han guardado los campos exitosamente", 1);
            }
        }

        #region data integrity
        int warningLevel = 0;
        bool ignoreWarnings = false;
        bool dataIntegrity;


        private bool FieldsAreCorrect()
        {
            return FieldsAreCorrect(false);
        }

        private bool FieldsAreCorrect(bool updating)
        {
            RefreshFields();
            //Fields: NOMBRE
            if (string.IsNullOrEmpty(nombre_))
            {
                dataIntegrity = false;
                IssueWarning("El campo de matricula no puede esar vacio", 3);
            }

            //Fields: MATRICULA
            if (string.IsNullOrEmpty(matricula_))
            {
                dataIntegrity = false;
                IssueWarning("El campo de matricula no puede esar vacio", 3);
            }
            string queryText = $"SELECT * FROM CATALOGO WHERE MATRICULA = '{matricula}'";

            List<string> identicalIssues = commander.ReadCommand(queryText, "matricula");
            if ((identicalIssues.Count > 0) && (!updating))
            {
                IssueWarning("La base de datos ya tiene una matricula nombrada " + matricula_ + ". Favor de cambiar esta matricula por una nueva.", 3);
            }

            //Fields: EXISTENCIAS
            if (!CheckIfNumeric(existencias_))
            {
                this.existencias.FontWeight = FontWeights.Bold;
                this.existencias.Background = Brushes.Red;
                IssueWarning("El campo de existencias debe de estar vacio o solo tener numeros", 3);
            }

            //Fields: PRECIO DE VENTA
            if (!CheckIfNumeric(precio_venta_) && (ignoreWarnings == false))
            {
                this.precio_venta.FontWeight = FontWeights.Bold;
                this.precio_venta.Background = Brushes.Red;
                IssueWarning("El campo de precio de venta debe de estar vacio o solo tener numeros", 3);
            }

            //Fields: PRECIO DE COMPRA
            if (!CheckIfNumeric(precio_compra_) && (ignoreWarnings == false))
            {
                this.precio_compra.FontWeight = FontWeights.Bold;
                this.precio_compra.Background = Brushes.Red;
                IssueWarning("El campo de precio de compra debe de estar vacio o solo tener numeros", 3);
            }

            //Fields: DESCUENTO
            if (!CheckIfNumeric(descuento_) && (ignoreWarnings == false))
            {
                this.existencias.FontWeight = FontWeights.Bold;
                this.existencias.Background = Brushes.Red;
                IssueWarning("El campo de descuentos debe de estar vacio o solo tener numeros", 3);
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

            this.zonaAdvertencias.Visibility = Visibility.Visible;
            if (level > warningLevel)
                warningLevel = level;

            switch (level)
            {
                case 1:
                    {
                        if (level >= warningLevel)
                            this.advertencias.Background = Brushes.Gray;
                        this.advertencias.Text += "\n [Notificación:]" + warning;
                        break;
                    }
                case 2:
                    {
                        if ((level >= warningLevel) && (ignoreWarnings == false))
                        {
                            this.advertencias.Background = Brushes.Yellow;
                            this.advertencias.Text += "\n [Advertencia nivel 2:]" + warning;
                            dataIntegrity = false;
                        }
                        break;
                    }
                case 3:
                    {
                        if (level >= warningLevel)
                            this.advertencias.Background = Brushes.Red;
                        this.advertencias.Text += "\n [Advertencia nivel 3:]" + warning + "(No se pueden ignorar advertencias de nivel 3)";
                        dataIntegrity = false;
                        break;
                    }
                default:
                    {
                        this.advertencias.Background = Brushes.Purple;
                        this.advertencias.Text += "\n [Advertencia nivel 4:]" + warning + "De alguna forma, haz entrado a un nivel de advertencia que no deberia de ser posible. Contacta al autor del proyecto.";
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
                this.zonaAdvertencias.Visibility = Visibility.Hidden;
            ignoreWarnings = true;
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

            
            string query = "SELECT * FROM CATALOGO";
            SQLiteCommander commander   = new SQLiteCommander();

            List<string> nombres        = commander.ReadCommand(query, "NOMBRE");
            List<string> matriculas     = commander.ReadCommand(query, "MATRICULA");
            List<string> descripcions   = commander.ReadCommand(query, "DESCRIPCION");
            List<int> existencias       = commander.ReadCommandInt(query, "EXISTENCIAS");
            List<double> precio_ventas  = commander.ReadCommandDouble(query, "PRECIO_VENTA");
            List<double> precio_compras = commander.ReadCommandDouble(query, "PRECIO_COMPRA");
            List<double> descuentos     = commander.ReadCommandDouble(query, "DESCUENTO");


            this.nombre.Text                = nombres[buttonClicked + (page*10)];
            this.matricula.Text             = matriculas[buttonClicked + (page*10)];
            this.descripcion.Text           = descripcions[buttonClicked + (page*10)];
            this.existencias.Text           = existencias[buttonClicked + (page*10)].ToString();
            this.precio_venta.Text          = precio_ventas[buttonClicked + (page*10)].ToString();
            this.precio_compra.Text         = precio_compras[buttonClicked + (page*10)].ToString();
            this.descuento_permitido.Text   = descuentos[buttonClicked + (page * 10)].ToString();
        }

        private void setContentCollectionTable()
        {
            setContentCollectionTable("SELECT * FROM CATALOGO");
        }

        private void setContentCollectionTable(string query)
        {
            int itemCount = page * 10;
            SQLiteCommander commander = new SQLiteCommander();
            List<string> catalogos = commander.ReadCommand(query, "NOMBRE");

            //nombres
            if (catalogos.Count >= itemCount + 1)
                nombreEntry_1.Text = catalogos[itemCount + 0];
            else
                nombreEntry_1.Text = "";
            if (catalogos.Count >= itemCount + 2)
                nombreEntry_2.Text = catalogos[itemCount + 1];
            else
                nombreEntry_2.Text = "";
            if (catalogos.Count >= itemCount + 3)
                nombreEntry_3.Text = catalogos[itemCount + 2];
            else
                nombreEntry_3.Text = "";
            if (catalogos.Count >= itemCount + 4)
                nombreEntry_4.Text = catalogos[itemCount + 3];
            else
                nombreEntry_4.Text = "";
            if (catalogos.Count >= itemCount + 5)
                nombreEntry_5.Text = catalogos[itemCount + 4];
            else
                nombreEntry_5.Text = "";
            if (catalogos.Count >= itemCount + 6)
                nombreEntry_6.Text = catalogos[itemCount + 5];
            else
                nombreEntry_6.Text = "";
            if (catalogos.Count >= itemCount + 7)
                nombreEntry_7.Text = catalogos[itemCount + 6];
            else
                nombreEntry_7.Text = "";
            if (catalogos.Count >= itemCount + 8)
                nombreEntry_8.Text = catalogos[itemCount + 7];
            else
                nombreEntry_8.Text = "";
            if (catalogos.Count >= itemCount + 9)
                nombreEntry_9.Text = catalogos[itemCount + 8];
            else
                nombreEntry_9.Text = "";
            if (catalogos.Count >= itemCount + 10)
                nombreEntry_10.Text = catalogos[itemCount + 9];
            else
                nombreEntry_10.Text = "";

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
            string query = "SELECT * FROM CATALOGO";
            SQLiteCommander commander = new SQLiteCommander();
            List<string> clientes = commander.ReadCommand(query, "MATRICULA");
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
                string query = "DELETE FROM CATALOGO WHERE MATRICULA = '" + this.matricula.Text + "'";
                SQLiteCommander commander = new SQLiteCommander();
                commander.WriteCommand(query);
                this.matricula.Text = "";
                this.nombre.Text = "";
                this.descripcion.Text = "";
                this.precio_venta.Text = "";
                this.precio_compra.Text = "";
                this.descuento_permitido.Text = "";
                IssueWarning("Articulo borrado exitosamente.", 1);
                setContentCollectionTable();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            warningLevel = 0;
            dataIntegrity = true;
            this.advertencias.Text = "";
            this.zonaAdvertencias.Background = Brushes.White;
            this.zonaAdvertencias.Visibility = Visibility.Hidden;

            warningLevel = 0;
            dataIntegrity = true;
            this.advertencias.Text = "";
            this.zonaAdvertencias.Background = Brushes.White;
            this.zonaAdvertencias.Visibility = Visibility.Hidden;


            RefreshFields();
            commander.WriteCommand($"UPDATE CATALOGO SET NOMBRE = '{nombre_}', PRECIO_VENTA = {precio_venta_}, PRECIO_COMPRA = {precio_compra_}," +
                $" DESCUENTO = {descuento_}, DESCRIPCION ='{descripcion_}' WHERE MATRICULA = '{matricula_}'");
            IssueWarning("Se han actualizado los campos exitosamente", 1);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = this.Buscar_TextBox.Text;
            string selectedTerm = this.Buscar_ComboBox.Text;
            if (!selectedTerm.Equals("Buscar por..."))
            {
                string queryText = $"SELECT * FROM CATALOGO WHERE {selectedTerm} LIKE '%{query}%'";
                setContentCollectionTable(queryText);
            }
            else
            {
                setContentCollectionTable();
            }
        }

        private void autogenerarMatricula_Click(object sender, RoutedEventArgs e)
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
            this.matricula.Text = id;
        }
    }
}
