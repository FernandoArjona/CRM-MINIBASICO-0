using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;


namespace CRM_MINIBASICO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*
         Basic data entry of all activities has been completed.
        TODO: 
        -Data security protocols (i.e. making sure that only numbers are entered in number fields)
        -Special data entries (adding content to ComboBoxes from the DB, check-uncheck system for the TicketSoporte activity)
        -Design the work for the next patch, with the objective of bringing readability features.
        -Code optimization

        TESTING FOR GITHUB VERSION MANAGEMENT
         */
        public MainWindow()
        {
            InitializeComponent();
            SQLiteCommander commander = new SQLiteCommander();
            /*
            commander.Command("CREATE TABLE IF NOT EXISTS CLIENTES (MATRICULA TEXT PRIMARY KEY, NOMBRE TEXT, EMAIL TEXT, TELEFONO TEXT, NOTA TEXT);");
            commander.Command("CREATE TABLE IF NOT EXISTS CITAS (TITULO TEXT, ASISTENTES TEXT, FECHA TEXT, DESCRIPCION TEXT);");
            commander.Command("CREATE TABLE IF NOT EXISTS CATALOGO (NOMBRE TEXT, MATRICULA TEXT, DESCRIPCION TEXT, EXISTENCIAS TINYINT, PRECIO_VENTA DOUBLE, PRECIO_COMPRA DOUBLE, DESCUENTO DOUBLE, PROVEEDOR TEXT);");
            commander.Command("CREATE TABLE IF NOT EXISTS COTIZACION (CONCEPTO TEXT, COTIZACION DOUBLE, MATRICULA_CLIENTE TEXT);");
            commander.Command("CREATE TABLE IF NOT EXISTS TICKETSOPORTE (TITULO TEXT, CLEINTE TEXT, DESCRIPCION TEXT, ESTATUS TEXT);");*/
        }

        private void Leads_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Leads();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            //Page1 = Clientes.xaml class
            this.Main_Page.Content = new Clientes();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Catalogo_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Catalogo();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Citas_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Citas();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Calendario_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Calendario();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Cotizaciones_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Cotizacion();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Tickets_Soporte_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new TicketSoporte();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Actividades_Click(object sender, RoutedEventArgs e)
        {
            this.Main_Page.Content = new Actividades();
            this.Main_Page.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Historial_Movimientos_Click(object sender, RoutedEventArgs e)
        {
        }


    }
}
