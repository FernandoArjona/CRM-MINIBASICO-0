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

namespace CRM_MINIBASICO.Activities
{
    /// <summary>
    /// Interaction logic for Orden.xaml
    /// </summary>
    public partial class Orden : Page
    {
        string concepto_            = "";
        string matricula_           = "";
        string precio_venta_        = "";
        string pago_cubierto_        = "";
        string fecha_entrega_       = "";
        string nota_             = "";
        string cliente_nombre_      = "";
        string cliente_matricula_   = "";
        int page = 0;

        SQLiteCommander commander = new SQLiteCommander();

        public Orden()
        {
            InitializeComponent();
            setStartDateBox();
            setContentCollectionTable();
        }

        private void setStartDateBox()
        {
            for (int i = 2000; i < 2100; i++)
            {
                this.comboBox_year.Items.Add(i);
            }
            for (int i = 0; i < 24; i++)
            {
                this.comboBox_hora.Items.Add(i);
            }
            for (int i = 0; i < 60; i++)
            {
                this.comboBox_minutos.Items.Add(i);
            }
            this.comboBox_year.SelectedIndex    = 0;
            this.comboBox_mes.SelectedIndex     = 0;
            this.comboBox_hora.SelectedIndex    = 0;
            this.comboBox_minutos.SelectedIndex = 0;

            Date_SelectionChanged();

            this.comboBox_dia.SelectedIndex = 0;
        }

        private string ParseDateString()
        {
            int day = this.comboBox_dia.SelectedIndex + 1;
            int month = this.comboBox_mes.SelectedIndex + 1;
            int year = 2000;
            if (this.comboBox_year.SelectedItem != null)
                year = Int32.Parse(this.comboBox_year.SelectedItem.ToString());
            int hour = 0;

            if (this.comboBox_hora.SelectedItem != null)
                hour = Int32.Parse(this.comboBox_hora.SelectedItem.ToString());

            int minutes = 0;
            if (this.comboBox_minutos.SelectedItem != null)
                minutes = Int32.Parse(this.comboBox_minutos.SelectedItem.ToString());

            if (day == 0)
                day++;

            if (month == 0)
                month++;

            return ($"{day}/{month}/{year} {((hour < 10)? ("0"+hour) : ("" + hour))}:{((minutes < 10) ? ("0" + minutes) : ("" + minutes))}:00");
        }

        private void RefreshFields()
        {

            concepto_ = this.concepto.Text;
            matricula_ = this.matricula.Text;
            precio_venta_ = this.precio_venta.Text;
            pago_cubierto_ = this.pago_cliente.Text;
            fecha_entrega_ = ParseDateString();
            nota_ = this.nota.Text;
            cliente_nombre_ = this.cliente_nombre.Text;
            cliente_matricula_ = this.cliente_matricula.Text;
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
                string com = $"INSERT OR IGNORE INTO ORDENES VALUES ('{concepto_}', " +
                    $"'{matricula_}', {precio_venta_}, {pago_cubierto_}, '{fecha_entrega_}', " +
                    $"'{nota_}', '{cliente_nombre_}', {cliente_matricula_})";
                commander.WriteCommand(com);
                IssueWarning("Se han guardado los campos exitosamente", 1);
            }
        }

        private void Date_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int daysInMonth = 31;

            this.comboBox_dia.Items.Clear();
            
            for (int i = 1; i <= daysInMonth; i++)
            {
                this.comboBox_dia.Items.Add(i);
            }

            this.comboBox_dia.SelectedIndex = 0;

        }

        private void Date_SelectionChanged()
        {
            this.comboBox_year.SelectedIndex = 0;
            this.comboBox_mes.SelectedIndex = 0;
            this.comboBox_dia.SelectedIndex = 1;
            this.comboBox_hora.SelectedIndex = 0;
            this.comboBox_minutos.SelectedIndex = 0;
            //int selectedMonth = this.comboBox_mes.SelectedIndex + 1;
            //int selectedYear = Int32.Parse(this.comboBox_year.SelectedItem.ToString());
            //int daysInMonth = System.DateTime.DaysInMonth(selectedYear, selectedMonth);
            //
            //this.comboBox_dia.Items.Clear();
            //
            //for (int i = 1; i <= daysInMonth; i++)
            //{
            //    this.comboBox_dia.Items.Add(i);
            //}
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
            if (string.IsNullOrEmpty(concepto_))
            {
                IssueWarning("El campo de concepto no puede esar vacio", 3);
            }

            //Fields: MATRICULA
            if (string.IsNullOrEmpty(matricula_))
            {
                IssueWarning("El campo de matricula no puede esar vacio", 3);
            }
            string queryText = "SELECT * FROM ORDENES WHERE MATRICULA = " + matricula_;


            List<string> identicalIssues = commander.ReadCommand(queryText, "matricula");
            if ((identicalIssues.Count > 0) && (!updating))
            {
                IssueWarning("La base de datos ya tiene una matricula nombrada " + matricula_ + ". Favor de cambiar esta matricula por una nueva.", 3);
            }

            //Fields: PRECIO DE VENTA, PAGO CUBIERTO

            if (!CheckIfNumeric(precio_venta_))
            {
                this.precio_venta.FontWeight = FontWeights.Bold;
                this.precio_venta.Background = Brushes.Red;
                IssueWarning("El campo de precio de venta debe de estar vacio o solo tener numeros", 3);
            }else if (Double.Parse(precio_venta_) < 0)
            {
                double correction = Double.Parse(precio_venta_) * -1;
                precio_venta_ = correction.ToString();
                IssueWarning("El precio de venta se presentaba como negativo. Se ha convertido a positivo.", 1);
            }


            if (!CheckIfNumeric(pago_cubierto_))
            {
                this.pago_cliente.FontWeight = FontWeights.Bold;
                this.pago_cliente.Background = Brushes.Red;
                IssueWarning("El campo de precio de venta debe de estar vacio o solo tener numeros", 3);
            }
            else if (Double.Parse(pago_cubierto_) < 0)
            {
                double correction = Double.Parse(precio_venta_) * -1;
                precio_venta_ = correction.ToString();
                IssueWarning("El pago cubierto se presentaba como negativo. Se ha convertido a positivo.", 1);
            }

            //Fields: nota
            if (string.IsNullOrEmpty(nota_))
            {
                IssueWarning("El campo de nota no puede estar sin seleccionar", 3);
            }


            if (string.IsNullOrEmpty(cliente_nombre_) && (ignoreWarnings == false))
            {
                dataIntegrity = false;
                IssueWarning("El campo de nombre de cliente esta vacio. Recomendamos ingresar un nombre.", 2);
            }

            if (string.IsNullOrEmpty(cliente_matricula_) && (ignoreWarnings == false))
            {
                dataIntegrity = false;
                IssueWarning("El campo de nombre de cliente esta vacio. Recomendamos ingresar una matricula.", 2);
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


            string query = "SELECT * FROM ORDENES";
            SQLiteCommander commander = new SQLiteCommander();

            List<string> Lconcepto_              = commander.ReadCommand(query, "CONCEPTO");
            List<string> Lmatricula_             = commander.ReadCommand(query, "MATRICULA");
            List<double> Lprecio_venta_          = commander.ReadCommandDouble(query, "PRECIO_VENTA");
            List<double> Lpago_cubierto_         = commander.ReadCommandDouble(query, "PRECIO_CUBIERTO");
            //List<DateTime> Lfecha_entrega_       = commander.ReadCommandDate(query, "FECHA_ENTREGA");
            List<string> Lfecha_entrega_         = commander.ReadCommand(query, "FECHA_ENTREGA");
            List<string> Lnota_               = commander.ReadCommand(query, "nota");
            List<string> Lcliente_nombre_        = commander.ReadCommand(query, "CLIENTE_NOMBRE");
            List<string> Lcliente_matricula_     = commander.ReadCommand(query, "CLIENTE_MATRICULA");

            this.concepto.Text                  = Lconcepto_[buttonClicked + (page * 10)];
            this.matricula.Text                 = Lmatricula_[buttonClicked + (page * 10)];
            this.precio_venta.Text              = Lprecio_venta_[buttonClicked + (page * 10)].ToString();
            this.pago_cliente.Text              = Lpago_cubierto_[buttonClicked + (page * 10)].ToString();
            this.nota.Text           = Lnota_[buttonClicked + (page * 10)];
            this.cliente_nombre.Text            = Lcliente_nombre_[buttonClicked + (page * 10)];
            this.cliente_matricula.Text         = Lcliente_matricula_[buttonClicked + (page * 10)];

            //DATE
            string dateString = Lfecha_entrega_[buttonClicked + (page * 10)];
            DateTime date = Convert.ToDateTime(dateString);
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            int hour = date.Hour;
            int minute = date.Minute;

            this.comboBox_dia.SelectedIndex = day;
            this.comboBox_mes.SelectedIndex = month;
            this.comboBox_year.SelectedIndex = year;
            this.comboBox_hora.SelectedIndex = hour;
            this.comboBox_minutos.SelectedIndex = minute;

        }

        private void setContentCollectionTable()
        {
            setContentCollectionTable("SELECT * FROM ORDENES");
        }

        private void setContentCollectionTable(string query)
        {
            int itemCount = page * 10;
            SQLiteCommander commander = new SQLiteCommander();
            List<string> Lconcepto_ = commander.ReadCommand(query, "CONCEPTO");
            List<string> Lmatricula_ = commander.ReadCommand(query, "MATRICULA");
            List<double> Lprecio_venta_ = commander.ReadCommandDouble(query, "PRECIO_VENTA");
            List<double> Lpago_cubierto_ = commander.ReadCommandDouble(query, "PRECIO_CUBIERTO");
            //List<DateTime> Lfecha_entrega_ = commander.ReadCommandDate(query, "FECHA_ENTREGA");
            List<string> Lfecha_entrega_ = commander.ReadCommand(query, "FECHA_ENTREGA");
            List<string> Lnota_ = commander.ReadCommand(query, "nota");
            List<string> Lcliente_nombre_ = commander.ReadCommand(query, "CLIENTE_NOMBRE");
            List<string> Lcliente_matricula_ = commander.ReadCommand(query, "CLIENTE_MATRICULA");

            //conceptos
            if (Lconcepto_.Count >= itemCount + 1)
                conceptoEntry_1.Text = Lconcepto_[itemCount + 0];
            else
                conceptoEntry_1.Text = "";
            if (Lconcepto_.Count >= itemCount + 2)
                conceptoEntry_2.Text = Lconcepto_[itemCount + 1];
            else
                conceptoEntry_2.Text = "";
            if (Lconcepto_.Count >= itemCount + 3)
                conceptoEntry_3.Text = Lconcepto_[itemCount + 2];
            else
                conceptoEntry_3.Text = "";
            if (Lconcepto_.Count >= itemCount + 4)
                conceptoEntry_4.Text = Lconcepto_[itemCount + 3];
            else
                conceptoEntry_4.Text = "";
            if (Lconcepto_.Count >= itemCount + 5)
                conceptoEntry_5.Text = Lconcepto_[itemCount + 4];
            else
                conceptoEntry_5.Text = "";
            if (Lconcepto_.Count >= itemCount + 6)
                conceptoEntry_6.Text = Lconcepto_[itemCount + 5];
            else
                conceptoEntry_6.Text = "";
            if (Lconcepto_.Count >= itemCount + 7)
                conceptoEntry_7.Text = Lconcepto_[itemCount + 6];
            else
                conceptoEntry_7.Text = "";
            if (Lconcepto_.Count >= itemCount + 8)
                conceptoEntry_8.Text = Lconcepto_[itemCount + 7];
            else
                conceptoEntry_8.Text = "";
            if (Lconcepto_.Count >= itemCount + 9)
                conceptoEntry_9.Text = Lconcepto_[itemCount + 8];
            else
                conceptoEntry_9.Text = "";
            if (Lconcepto_.Count >= itemCount + 10)
                conceptoEntry_10.Text = Lconcepto_[itemCount + 9];
            else
                conceptoEntry_10.Text = "";

            //matricula
            if (Lmatricula_.Count >= itemCount + 1)
                matriculaEntry_1.Text = Lmatricula_[itemCount + 0];
            else
                matriculaEntry_1.Text = "";
            if (Lmatricula_.Count >= itemCount + 2)
                matriculaEntry_2.Text = Lmatricula_[itemCount + 1];
            else
                matriculaEntry_2.Text = "";
            if (Lmatricula_.Count >= itemCount + 3)
                matriculaEntry_3.Text = Lmatricula_[itemCount + 2];
            else
                matriculaEntry_3.Text = "";
            if (Lmatricula_.Count >= itemCount + 4)
                matriculaEntry_4.Text = Lmatricula_[itemCount + 3];
            else
                matriculaEntry_4.Text = "";
            if (Lmatricula_.Count >= itemCount + 5)
                matriculaEntry_5.Text = Lmatricula_[itemCount + 4];
            else
                matriculaEntry_5.Text = "";
            if (Lmatricula_.Count >= itemCount + 6)
                matriculaEntry_6.Text = Lmatricula_[itemCount + 5];
            else
                matriculaEntry_6.Text = "";
            if (Lmatricula_.Count >= itemCount + 7)
                matriculaEntry_7.Text = Lmatricula_[itemCount + 6];
            else
                matriculaEntry_7.Text = "";
            if (Lmatricula_.Count >= itemCount + 8)
                matriculaEntry_8.Text = Lmatricula_[itemCount + 7];
            else
                matriculaEntry_8.Text = "";
            if (Lmatricula_.Count >= itemCount + 9)
                matriculaEntry_9.Text = Lmatricula_[itemCount + 8];
            else
                matriculaEntry_9.Text = "";
            if (Lmatricula_.Count >= itemCount + 10)
                matriculaEntry_10.Text = Lmatricula_[itemCount + 9];
            else
                matriculaEntry_10.Text = "";

            //Lprecio_venta
            if (Lprecio_venta_.Count >= itemCount + 1)
                precioVentaEntry_1.Text = Lprecio_venta_[itemCount + 0].ToString();
            else
                precioVentaEntry_1.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 2)
                precioVentaEntry_2.Text = Lprecio_venta_[itemCount + 1].ToString();
            else
                precioVentaEntry_2.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 3)
                precioVentaEntry_3.Text = Lprecio_venta_[itemCount + 2].ToString();
            else
                precioVentaEntry_3.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 4)
                precioVentaEntry_4.Text = Lprecio_venta_[itemCount + 3].ToString();
            else
                precioVentaEntry_4.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 5)
                precioVentaEntry_5.Text = Lprecio_venta_[itemCount + 4].ToString();
            else
                precioVentaEntry_5.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 6)
                precioVentaEntry_6.Text = Lprecio_venta_[itemCount + 5].ToString();
            else
                precioVentaEntry_6.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 7)
                precioVentaEntry_7.Text = Lprecio_venta_[itemCount + 6].ToString();
            else
                precioVentaEntry_7.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 8)
                precioVentaEntry_8.Text = Lprecio_venta_[itemCount + 7].ToString();
            else
                precioVentaEntry_8.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 9)
                precioVentaEntry_9.Text = Lprecio_venta_[itemCount + 8].ToString();
            else
                precioVentaEntry_9.Text = "";
            if (Lprecio_venta_.Count >= itemCount + 10)
                precioVentaEntry_10.Text = Lprecio_venta_[itemCount + 9].ToString();
            else
                precioVentaEntry_10.Text = "";

            //Lpago_cubierto
            if (Lpago_cubierto_.Count >= itemCount + 1)
                pagoCubiertoEntry_1.Text = Lpago_cubierto_[itemCount + 0].ToString();
            else
                pagoCubiertoEntry_1.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 2)
                pagoCubiertoEntry_2.Text = Lpago_cubierto_[itemCount + 1].ToString();
            else
                pagoCubiertoEntry_2.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 3)
                pagoCubiertoEntry_3.Text = Lpago_cubierto_[itemCount + 2].ToString();
            else
                pagoCubiertoEntry_3.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 4)
                pagoCubiertoEntry_4.Text = Lpago_cubierto_[itemCount + 3].ToString();
            else
                pagoCubiertoEntry_4.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 5)
                pagoCubiertoEntry_5.Text = Lpago_cubierto_[itemCount + 4].ToString();
            else
                pagoCubiertoEntry_5.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 6)
                pagoCubiertoEntry_6.Text = Lpago_cubierto_[itemCount + 5].ToString();
            else
                pagoCubiertoEntry_6.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 7)
                pagoCubiertoEntry_7.Text = Lpago_cubierto_[itemCount + 6].ToString();
            else
                pagoCubiertoEntry_7.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 8)
                pagoCubiertoEntry_8.Text = Lpago_cubierto_[itemCount + 7].ToString();
            else
                pagoCubiertoEntry_8.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 9)
                pagoCubiertoEntry_9.Text = Lpago_cubierto_[itemCount + 8].ToString();
            else
                pagoCubiertoEntry_9.Text = "";
            if (Lpago_cubierto_.Count >= itemCount + 10)
                pagoCubiertoEntry_10.Text = Lpago_cubierto_[itemCount + 9].ToString();
            else
                pagoCubiertoEntry_10.Text = "";        

            //Lcliente_nombre_
            if (Lcliente_nombre_.Count >= itemCount + 1)
                clienteNombreEntry_1.Text = Lcliente_nombre_[itemCount + 0].ToString();
            else
                clienteNombreEntry_1.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 2)
                clienteNombreEntry_2.Text = Lcliente_nombre_[itemCount + 1].ToString();
            else
                clienteNombreEntry_2.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 3)
                clienteNombreEntry_3.Text = Lcliente_nombre_[itemCount + 2].ToString();
            else
                clienteNombreEntry_3.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 4)
                clienteNombreEntry_4.Text = Lcliente_nombre_[itemCount + 3].ToString();
            else
                clienteNombreEntry_4.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 5)
                clienteNombreEntry_5.Text = Lcliente_nombre_[itemCount + 4].ToString();
            else
                clienteNombreEntry_5.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 6)
                clienteNombreEntry_6.Text = Lcliente_nombre_[itemCount + 5].ToString();
            else
                clienteNombreEntry_6.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 7)
                clienteNombreEntry_7.Text = Lcliente_nombre_[itemCount + 6].ToString();
            else
                clienteNombreEntry_7.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 8)
                clienteNombreEntry_8.Text = Lcliente_nombre_[itemCount + 7].ToString();
            else
                clienteNombreEntry_8.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 9)
                clienteNombreEntry_9.Text = Lcliente_nombre_[itemCount + 8].ToString();
            else
                clienteNombreEntry_9.Text = "";
            if (Lcliente_nombre_.Count >= itemCount + 10)
                clienteNombreEntry_10.Text = Lcliente_nombre_[itemCount + 9].ToString();
            else
                clienteNombreEntry_10.Text = "";

            //Lcliente_matricula_
            if (Lcliente_matricula_.Count >= itemCount + 1)
                clienteMatriculaEntry_1.Text = Lcliente_matricula_[itemCount + 0].ToString();
            else
                clienteMatriculaEntry_1.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 2)
                clienteMatriculaEntry_2.Text = Lcliente_matricula_[itemCount + 1].ToString();
            else
                clienteMatriculaEntry_2.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 3)
                clienteMatriculaEntry_3.Text = Lcliente_matricula_[itemCount + 2].ToString();
            else
                clienteMatriculaEntry_3.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 4)
                clienteMatriculaEntry_4.Text = Lcliente_matricula_[itemCount + 3].ToString();
            else
                clienteMatriculaEntry_4.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 5)
                clienteMatriculaEntry_5.Text = Lcliente_matricula_[itemCount + 4].ToString();
            else
                clienteMatriculaEntry_5.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 6)
                clienteMatriculaEntry_6.Text = Lcliente_matricula_[itemCount + 5].ToString();
            else
                clienteMatriculaEntry_6.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 7)
                clienteMatriculaEntry_7.Text = Lcliente_matricula_[itemCount + 6].ToString();
            else
                clienteMatriculaEntry_7.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 8)
                clienteMatriculaEntry_8.Text = Lcliente_matricula_[itemCount + 7].ToString();
            else
                clienteMatriculaEntry_8.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 9)
                clienteMatriculaEntry_9.Text = Lcliente_matricula_[itemCount + 8].ToString();
            else
                clienteMatriculaEntry_9.Text = "";
            if (Lcliente_matricula_.Count >= itemCount + 10)
                clienteMatriculaEntry_10.Text = Lcliente_matricula_[itemCount + 9].ToString();
            else
                clienteMatriculaEntry_10.Text = "";

            //Lfecha
            if (Lfecha_entrega_.Count >= itemCount + 1)
                FechaEntregaEntry_1.Text = Lfecha_entrega_[itemCount + 0].ToString();
            else
                FechaEntregaEntry_1.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 2)
                FechaEntregaEntry_2.Text = Lfecha_entrega_[itemCount + 1].ToString();
            else
                FechaEntregaEntry_2.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 3)
                FechaEntregaEntry_3.Text = Lfecha_entrega_[itemCount + 2].ToString();
            else
                FechaEntregaEntry_3.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 4)
                FechaEntregaEntry_4.Text = Lfecha_entrega_[itemCount + 3].ToString();
            else
                FechaEntregaEntry_4.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 5)
                FechaEntregaEntry_5.Text = Lfecha_entrega_[itemCount + 4].ToString();
            else
                FechaEntregaEntry_5.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 6)
                FechaEntregaEntry_6.Text = Lfecha_entrega_[itemCount + 5].ToString();
            else
                FechaEntregaEntry_6.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 7)
                FechaEntregaEntry_7.Text = Lfecha_entrega_[itemCount + 6].ToString();
            else
                FechaEntregaEntry_7.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 8)
                FechaEntregaEntry_8.Text = Lfecha_entrega_[itemCount + 7].ToString();
            else
                FechaEntregaEntry_8.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 9)
                FechaEntregaEntry_9.Text = Lfecha_entrega_[itemCount + 8].ToString();
            else
                FechaEntregaEntry_9.Text = "";
            if (Lfecha_entrega_.Count >= itemCount + 10)
                FechaEntregaEntry_10.Text = Lfecha_entrega_[itemCount + 9].ToString();
            else
                FechaEntregaEntry_10.Text = "";

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
            string query = "SELECT * FROM ORDENES";
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
                string query = "DELETE FROM ORDENES WHERE MATRICULA = '" + this.matricula.Text + "'";
                SQLiteCommander commander = new SQLiteCommander();
                commander.WriteCommand(query);
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
            //"NOMBRE TEXT, MATRICULA TEXT, DESCRIPCION TEXT, EXISTENCIAS INT, PRECIO_VENTA DOUBLE, PRECIO_COMPRA DOUBLE, DESCUENTO DOUBLE)";
            /*
            commander.WriteCommand($"UPDATE CATALOGO SET NOMBRE = '{nombre_}', PRECIO_VENTA = {precio_venta_}, PRECIO_COMPRA = {precio_compra_}," +
                $" DESCUENTO = {descuento_}, DESCRIPCION ='{descripcion_}' WHERE MATRICULA = '{matricula_}'");*/
            IssueWarning("Se han actualizado los campos exitosamente", 1);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = this.Buscar_TextBox.Text;
            string selectedTerm = this.Buscar_ComboBox.Text;
            if (!selectedTerm.Equals("Buscar por..."))
            {
                string queryText = $"SELECT * FROM ORDENES WHERE {selectedTerm} LIKE '%{query}%'";
                setContentCollectionTable(queryText);
            }
            else
            {
                setContentCollectionTable();
            }
        }
    }
}
