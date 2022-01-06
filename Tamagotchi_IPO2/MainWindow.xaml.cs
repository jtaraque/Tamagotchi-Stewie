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


using System.IO;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Tamagotchi_IPO2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random r = new Random();
        DispatcherTimer TimerPrincipal;



        /*---------------- Atributos y objetos ----------------*/

        double decremento = 1.0;
        string nombre;
        double puntuacion = 0.0;
        double puntuacionPasiva = 0.0;
        int clicksPasivos = 0;
        int clicks = 0;

        double puntuacionPasivaDescansar = 0.5;
        double puntuacionPasivaAlimentar = 0.5;   //En un principio los objetos equipados iban a dar atributos para aguantar más tiempo, incrementando la jugabilidad
        double puntuacionPasivaJugar = 0.5;

        bool pocaEnergiaTerminada = true;
        bool pocoAlimentoTerminada = true;
        bool pocaDiversionTerminada = true;

        bool pausado = false;



        Desbloqueable ShortsAmarillos = new Desbloqueable("Pantalones Amarillos");
        Desbloqueable ShortsCeleste = new Desbloqueable("Pantalones Celestes");
        Desbloqueable ShortsAzules = new Desbloqueable("Pantalones Azules");
        Desbloqueable ShortsMulticolor = new Desbloqueable("Pantalones Multicolor");
        Desbloqueable ShortsNaranjas = new Desbloqueable("Pantalones Naranjas");
        Desbloqueable ShortsVerdes = new Desbloqueable("Pantalones Verdes");
        Desbloqueable ShortsVioletas = new Desbloqueable("Pantalones Violetas");
        Desbloqueable TshirtAmarilla = new Desbloqueable("Camiseta Amarilla");
        Desbloqueable TshirtAzul = new Desbloqueable("Camiseta Azul");
        Desbloqueable TshirtCeleste = new Desbloqueable("Camiseta Celeste");
        Desbloqueable TshirtMulticolor = new Desbloqueable("Camiseta Multicolor");
        Desbloqueable TshirtRoja = new Desbloqueable("Camiseta Roja");
        Desbloqueable TshirtVerde = new Desbloqueable("Camiseta Verde");
        Desbloqueable MedallaBronce = new Desbloqueable("Medalla de bronce");
        Desbloqueable MedallaPlata = new Desbloqueable("Medalla de plata");
        Desbloqueable MedallaOro = new Desbloqueable("Medalla de oro");
        Desbloqueable CursorPlata = new Desbloqueable("Cursor de plata");
        Desbloqueable CursorOro = new Desbloqueable("Cursor de oro");
        Desbloqueable Rapido = new Desbloqueable("Eliminacion rapida");
        Desbloqueable Aguante = new Desbloqueable("Mucho aguante");
        Desbloqueable Simpsons = new Desbloqueable("Fondo de los Simpsons");
        Desbloqueable Stereo = new Desbloqueable("Fondo Stereo");
        Desbloqueable Basura = new Desbloqueable("Fondo sucio");
        Desbloqueable casa = new Desbloqueable("Fondo casa");







        /*---------------- Lógica Principal ----------------*/

        public MainWindow()
        {
            InitializeComponent();
            VentanaBienvenida pantallaInicio = new VentanaBienvenida(this);
            pantallaInicio.ShowDialog();

            TimerPrincipal = new DispatcherTimer();
            TimerPrincipal.Interval = TimeSpan.FromMilliseconds(2000);
            TimerPrincipal.Tick += new EventHandler(reloj);

            TimerPrincipal.Start();
        }

            private void reloj(object sender, EventArgs e)
            {
            if (pausado == false)
            {
                this.pbJugar.Value -= decremento;
                this.pbDescansar.Value -= decremento;
                this.pbAlimentar.Value -= decremento;


                decremento += 1;

                ComprobarDesbloqueables();

                if (pbDescansar.Value <= 10 && pocaEnergiaTerminada == true)
                {
                    Storyboard sbPocaEnergia = (Storyboard)this.Resources["PocaEnergia"];
                    sbPocaEnergia.Completed += new EventHandler(finPocaEnergia);
                    sbPocaEnergia.Begin();
                }

                if (pbAlimentar.Value <= 10 && pocoAlimentoTerminada == true) {
                    Storyboard sbPocoAlimento = (Storyboard)this.Resources["PocoAlimento"];
                    sbPocoAlimento.Completed += new EventHandler(finPocoAlimento);
                    sbPocoAlimento.Begin();
                }

                if (pbJugar.Value <= 10 && pocaDiversionTerminada == true)
                {
                    Storyboard sbPocaDiversion = (Storyboard)this.Resources["PocaDiversion"];
                    sbPocaDiversion.Completed += new EventHandler(finPocaDiversion);
                    sbPocaDiversion.Begin();
                }




                if (pbAlimentar.Value == 0 || pbJugar.Value == 0 || pbDescansar.Value == 0)
                {
                    TimerPrincipal.Stop();
                    this.btnAlimentar.IsEnabled = false;
                    this.btnDescansar.IsEnabled = false;
                    this.btnJugar.IsEnabled = false;

                    this.puntuacionPasiva = puntuacionPasivaAlimentar + puntuacionPasivaDescansar + puntuacionPasivaJugar;
                    this.puntuacion = decremento * 10 + puntuacionPasiva + clicks + clicksPasivos * 0.05;
                    this.lblPuntuacion.Content = "Puntuación total: " + puntuacion + "\n\n" +
                                                 "Segundos aguantados: " + this.decremento + "\n" +
                                                 "Puntuación pasiva: " + puntuacionPasiva + "\n" +
                                                 "Número de clicks: " + this.clicks + "\n" +
                                                 "Clicks Pasivos: " + this.clicksPasivos;
                    this.lblPuntuacion.Visibility = Visibility.Visible;

                    Label nuevoRanking = new Label();
                    nuevoRanking.FontSize = 16;
                    nuevoRanking.Content = this.nombre + " - " + this.puntuacion;
                    this.RankingPanel.Children.Add(nuevoRanking);
                }

            }
            else
            {
                lblPausa.Content = "El juego se encuentra pausado, vuelve al área de los botones cuando estés listo.";
            }
            }


        /*---------------- Comportamiento ----------------*/

        private void CambiarShorts(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush ShortsSelectedColor = new SolidColorBrush();

            switch (((Image)sender).Name)
            {
                case "ImShortAmarillo":
                    if (ShortsAmarillos.Desbloqueado == true) { 
                    ShortsSelectedColor.Color = Color.FromArgb(255, 193, 170, 0); 
                    Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                     lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImShortAzul":
                    if (ShortsAzules.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, 48, 11, 152);
                    Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImShortCeleste":
                    if (ShortsCeleste.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, 0, 202, 205);
                    Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                     lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImShortMorado":
                    if (ShortsVioletas.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, 174, 11, 135);
                        Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;

                case "ImShortNaranja":
                    if (ShortsNaranjas.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, 252, 111, 0);
                        Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImShortVerde":
                    if (ShortsVerdes.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, 0, 102, 23);
                        Pantalon.Fill = ShortsSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";

                    }
                    break;
                case "ImShortMulticolor":
                    if (ShortsMulticolor.Desbloqueado == true)
                    {
                        ShortsSelectedColor.Color = Color.FromArgb(255, Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)));
                        Pantalon.Fill = ShortsSelectedColor;  //Color aleatorio
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";

                    }
                    break;
            }
        }

        private void CambiarTshirt(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush TshirtSelectedColor = new SolidColorBrush();

            switch (((Image)sender).Name)
            {
                case "ImTshirtAmarilla":
                    if (TshirtAmarilla.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, 227, 231, 0);
                        Camiseta.Fill = TshirtSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImTshirtAzul":
                    if (TshirtAzul.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, 23, 28, 189);
                        Camiseta.Fill = TshirtSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImTshirtCeleste":
                    if (TshirtCeleste.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, 0, 231, 202);
                        Camiseta.Fill = TshirtSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImTshirtRoja":
                    if (TshirtRoja.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, 215, 25, 55);
                        Camiseta.Fill = TshirtSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImTshirtVerde":
                    if (TshirtVerde.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, 0, 231, 0);
                        Camiseta.Fill = TshirtSelectedColor;
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
                case "ImTshirtMulticolor":
                    if (TshirtMulticolor.Desbloqueado == true)
                    {
                        TshirtSelectedColor.Color = Color.FromArgb(255, Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)));
                        Camiseta.Fill = TshirtSelectedColor;  //Color aleatorio
                    }
                    else
                    {
                        lblInformacion.Text = "¡Objeto no desbloqueado!";
                    }
                    break;
            }
        }

        private void CambiarFondo(object sender, MouseButtonEventArgs e)
        {
            switch (((Image)sender).Name)
            {
                case "ImFondoBar":
                    Fondo.Source = ((Image)sender).Source;

                    break;
                case "ImFondoHabitacionStewie":
                    Fondo.Source = ((Image)sender).Source;

                    break;
                case "ImFondoSalon":
                    Fondo.Source = ((Image)sender).Source;

                    break;
                case "ImFondoParque":
                    Fondo.Source = ((Image)sender).Source;

                    break;
                case "ImFondoBasura":
                    if (Basura.Desbloqueado == true)
                    {
                        Fondo.Source = ImFondoBasura.Source;

                    }
                    else
                    {
                        lblInformacion.Text = "¡Fondo no desbloqueado!";
                    }
                    break;
                case "ImFondocasa":
                    if (casa.Desbloqueado == true)
                    {
                        Fondo.Source = ImFondoCasa.Source;

                    }
                    else
                    {
                        lblInformacion.Text = "¡Fondo no desbloqueado!";
                    }
                    break;
                case "ImFondoStereo":
                    if (Stereo.Desbloqueado == true)
                    {
                        Fondo.Source = ImFondoStereo.Source;

                    }
                    else
                    {
                        lblInformacion.Text = "¡Fondo no desbloqueado!";
                    }
                    break;
                case "ImFondoSimpsons":
                    if (Simpsons.Desbloqueado == true)
                    {
                        Fondo.Source = ImFondoSimpsons.Source;

                    }
                    else
                    {
                        lblInformacion.Text = "¡Fondo no desbloqueado!";
                    }
                    break;
            }
        

        }

        private void Descansar(object sender, RoutedEventArgs e)
        {
            Storyboard sbCerrarOjos = (Storyboard)this.Resources["CerrarOjos"];
            sbCerrarOjos.Completed += new EventHandler(finDescansar);
            btnDescansar.IsEnabled = false;
            sbCerrarOjos.Begin();
            this.pbDescansar.Value += 10;
            this.clicks++;


        }

        private void finDescansar(object sender, EventArgs e)
        {
            btnDescansar.IsEnabled = true;

        }

        private void Alimentar(object sender, RoutedEventArgs e)
        {
            Storyboard sbComer = (Storyboard)this.Resources["Comer"];
            sbComer.Completed += new EventHandler(finAlimentar);
            btnAlimentar.IsEnabled = false;
            sbComer.Begin();
            this.pbAlimentar.Value += 10;
            this.clicks++;
        }

        private void finAlimentar(object sender, EventArgs e)
        {
            btnAlimentar.IsEnabled = true;

        }

        private void Jugar(object sender, RoutedEventArgs e)
        {
            Storyboard sbJugar = (Storyboard)this.Resources["Jugar"];
            sbJugar.Begin();
            sbJugar.Completed += new EventHandler(finJugar);
            btnJugar.IsEnabled = false;
            sbJugar.Begin();
            this.pbJugar.Value += 10;
            this.clicks++;

        }

        private void finJugar(object sender, EventArgs e)
        {
            btnJugar.IsEnabled = true;

        }

        private void DescansarPasivo(object sender, MouseEventArgs e)
        {
            this.pbDescansar.Value += puntuacionPasivaDescansar;
            this.clicksPasivos++;

        }

        private void AlimentarPasivo(object sender, MouseEventArgs e)
        {
            this.pbAlimentar.Value += puntuacionPasivaAlimentar;
            this.clicksPasivos++;

        }

        private void JugarPasivo(object sender, MouseEventArgs e)
        {
            this.pbJugar.Value += puntuacionPasivaJugar;
            this.clicksPasivos++;

        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
            this.lblInformacion.Text = "Bienvenido " + this.nombre;
        }

        private void PararTimer(object sender, MouseEventArgs e)
        {
            this.pausado = true;
            this.ImPlay.Visibility = Visibility.Hidden;
            this.ImPausa.Visibility = Visibility.Visible;
        }

        private void ContinuarTimer(object sender, MouseEventArgs e)
        {
            this.pausado = false;
            this.lblPausa.Content = "";
            this.ImPausa.Visibility = Visibility.Hidden;
            this.ImPlay.Visibility = Visibility.Visible;

        }

        private void finPocaEnergia(object sender, EventArgs e)
        {
            pocaEnergiaTerminada = true;

        }

        private void finPocoAlimento(object sender, EventArgs e)
        {
            pocoAlimentoTerminada = true;

        }

        private void finPocaDiversion(object sender, EventArgs e)
        {
            pocaDiversionTerminada = true;

        }

        private void ComprobarDesbloqueables()
        {
            if (decremento >= 28)
            {
                this.ImShortMulticolor.Source = new BitmapImage(new Uri(@"/shortsMulticolor.png", UriKind.Relative));
                ShortsMulticolor.Desbloqueado = true;
                lblInformacion.Text = "Se ha desbloqueado el objeto " + ShortsMulticolor.Nombre;
            }

            if (clicks >= 70)
            {
                this.ImShortAmarillo.Source = new BitmapImage(new Uri(@"/shortsAmarillos.png", UriKind.Relative));
                ShortsAmarillos.Desbloqueado = true;
                this.ImTshirtAmarilla.Source = new BitmapImage(new Uri(@"/tshirtAmarilla.png", UriKind.Relative));
                TshirtAmarilla.Desbloqueado = true;
                lblInformacion.Text = "Se han desbloqueado los objetos " + ShortsAmarillos.Nombre + " y " + TshirtAmarilla.Nombre;
            }

            if (clicksPasivos >= 550)
            {
                this.ImTshirtMulticolor.Source = new BitmapImage(new Uri(@"/tshirtMulticolor.png", UriKind.Relative));
                TshirtMulticolor.Desbloqueado = true;
                lblInformacion.Text = "Se ha desbloqueado el objeto " + TshirtMulticolor.Nombre;
            }

            if (clicksPasivos >= 70)
            {
                this.ImShortCeleste.Source = new BitmapImage(new Uri(@"/shortsAzulClaro.png", UriKind.Relative));
                ShortsCeleste.Desbloqueado = true;
                this.ImTshirtCeleste.Source = new BitmapImage(new Uri(@"/tshirtAzulClaro.png", UriKind.Relative));
                TshirtCeleste.Desbloqueado = true;
                lblInformacion.Text = "Se han desbloqueado los objetos " + ShortsCeleste.Nombre + " y " + TshirtCeleste.Nombre;
            }

            if (clicks > 40)
            {
                this.ImShortNaranja.Source = new BitmapImage(new Uri(@"/shortsNaranjas.png", UriKind.Relative));
                ShortsNaranjas.Desbloqueado = true;
                lblInformacion.Text = "Se ha desbloqueado el objeto " + ShortsNaranjas.Nombre;
            }

            if (clicks >= 20 && clicksPasivos >= 300)
            {
                this.ImShortVerde.Source = new BitmapImage(new Uri(@"/shortsVerde.png", UriKind.Relative));
                ShortsVerdes.Desbloqueado = true;
                this.ImShortMorado.Source = new BitmapImage(new Uri(@"/shortsVioletas.png", UriKind.Relative));
                ShortsVioletas.Desbloqueado = true;
                lblInformacion.Text = "Se han desbloqueado los objetos " + ShortsVerdes.Nombre + " y " + ShortsVioletas.Nombre;
            }

            if (decremento >= 18 && clicks >= 80)
            {
                this.ImTshirtVerde.Source = new BitmapImage(new Uri(@"/tshirtVerde.png", UriKind.Relative));
                TshirtVerde.Desbloqueado = true;
                this.ImTshirtRoja.Source = new BitmapImage(new Uri(@"/tshirtRoja.png", UriKind.Relative));
                TshirtRoja.Desbloqueado = true;
                lblInformacion.Text = "Se han desbloqueado los objetos " + TshirtVerde.Nombre + " y " + TshirtRoja.Nombre;
            }

            if (decremento >= 20 && clicks >= 80 && clicksPasivos >= 400)
            {
                this.ImTshirtAzul.Source = new BitmapImage(new Uri(@"/tshirtAzul.png", UriKind.Relative));
                TshirtAzul.Desbloqueado = true;
                this.ImShortAzul.Source = new BitmapImage(new Uri(@"/shortsAzules.png", UriKind.Relative));
                ShortsAzules.Desbloqueado = true;
                lblInformacion.Text = "Se han desbloqueado los objetos " + TshirtAzul.Nombre + " y " + ShortsAzules.Nombre;
            }
            if (decremento >= 14)
            {
                this.ImRapido.Source = new BitmapImage(new Uri(@"/clock.png", UriKind.Relative));
                Rapido.Desbloqueado = true;
            }

            if (decremento >= 28)
            {
                this.ImAguante.Source = new BitmapImage(new Uri(@"/slow-motion.png", UriKind.Relative));
                Aguante.Desbloqueado = true;
            }

            if (clicks >= 30)
            {
                this.ImBronce.Source = new BitmapImage(new Uri(@"/bronze-medal.png", UriKind.Relative));
                MedallaBronce.Desbloqueado = true;
            }

            if (clicks >= 50)
            {
                this.ImPlata.Source = new BitmapImage(new Uri(@"/silver-medal.png", UriKind.Relative));
                MedallaPlata.Desbloqueado = true;
            }

            if (clicks >= 80)
            {
                this.ImOro.Source = new BitmapImage(new Uri(@"/gold-medal.png", UriKind.Relative));
                MedallaOro.Desbloqueado = true;
            }

            if (clicksPasivos >= 400)
            {
                this.ImCursorPlata.Source = new BitmapImage(new Uri(@"/cursorPlata.png", UriKind.Relative));
                CursorPlata.Desbloqueado = true;
            }

            if (clicksPasivos >= 600)
            {
                this.ImCursorOro.Source = new BitmapImage(new Uri(@"/cursorOro.png", UriKind.Relative));
                CursorOro.Desbloqueado = true;
            }
            if (pausado == true && ShortsMulticolor.Desbloqueado == true && TshirtMulticolor.Desbloqueado == true) { 
            this.ImFondoSimpsons.Source = new BitmapImage(new Uri(@"/FondoSimpsons.jpg", UriKind.Relative));
                Simpsons.Desbloqueado = true;
            }

            if (ShortsAmarillos.Desbloqueado == true && TshirtAmarilla.Desbloqueado == true) {
                this.ImFondoStereo.Source = new BitmapImage(new Uri(@"/FondoStereo.jpg", UriKind.Relative));
                Stereo.Desbloqueado = true;
            }

            if (clicksPasivos >= 400) {
                this.ImFondoBasura.Source = new BitmapImage(new Uri(@"/FondoBasura.jpg", UriKind.Relative));
                Basura.Desbloqueado = true;
            }

            if (clicks == 0 && clicksPasivos >= 150) {
                this.ImFondoCasa.Source = new BitmapImage(new Uri(@"/FondoCasa.jpg", UriKind.Relative));
                casa.Desbloqueado = true;
            }
        }
        }
}
