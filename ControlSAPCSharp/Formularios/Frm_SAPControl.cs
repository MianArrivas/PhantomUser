using System;
using System.Windows.Forms;

using ControlSAPCSharp.Clases;

namespace ControlSAPCSharp
{
    public partial class Frm__CtrlSAP : Form
    {
        #region <Llamado de clases>

        CAppHandler cah_Ventana = new CAppHandler();

        #endregion <Llamado de clases>

        #region <Variables>

        IntPtr ip_Handle;

        #endregion <Variables>

        #region <Constructor>

        public Frm__CtrlSAP()
        {
            InitializeComponent();
        }

        #endregion <Constructor>

        #region <Funciones Privadas>

        private void AgregarLog(string TextLog)
        {
            TxtBx1_Log.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " - " + TextLog); 
        }

        private void Funcion1()
        {
            ip_Handle = cah_Ventana.EncontrarVentana(null, "SAP Logon 750");

            AgregarLog("Handle de SAP Logon: " + ip_Handle);
            AgregarLog("Nombre de la clase: " + cah_Ventana._NombreClaseActual);
            AgregarLog("Nombre de la ventana: " + cah_Ventana._NombreVentanaActual);

            cah_Ventana.VentanaAPrimerPlano(cah_Ventana._HandleVentanaActual);

            cah_Ventana.BuscarVentanaHija(cah_Ventana._HandleVentanaActual, "Edit");
            AgregarLog("Buscando ventana Hija \"Edit\": " + cah_Ventana._HandleVentanaHija);
        }

        #endregion <Funciones Privadas>

        private void Btn1_IniciarControl_Click(object sender, EventArgs e)
        {
            //Funcion1();

            cah_Ventana.VentanaAPrimerPlano(cah_Ventana._HandleVentanaActual);

            ip_Handle = cah_Ventana.EncontrarVentana(null, cah_Ventana._NombreVentana);
            AgregarLog("Handle de la ventana buscada: " + ip_Handle);

            cah_Ventana.BuscarVentanaHija(cah_Ventana._HandleVentanaActual, "Edit");

            AgregarLog("Contenido de la ventana: \n" + cah_Ventana.ContenidoDeVentana(cah_Ventana._HandleVentanaHija));

            if (cah_Ventana.ContenidoDeVentana(cah_Ventana._HandleVentanaHija) != "100 EP1 ERP Production")
            {
                for (; cah_Ventana.ContenidoDeVentana(cah_Ventana._HandleVentanaHija) != ""; )
                    SendKeys.SendWait("{BACKSPACE}");

                AgregarLog("Buscando transaccion");
                cah_Ventana.BuscarVentanaHija(cah_Ventana._HandleVentanaActual, "Edit");
                SendKeys.SendWait("100 EP1 ERP Production");
            }

            AgregarLog("Entrando a transaccion");
            cah_Ventana.BuscarVentanaHija(cah_Ventana._HandleVentanaActual, "Static");
            SendKeys.SendWait("{ENTER}");

            System.Threading.Thread.Sleep(8500);
            ip_Handle = cah_Ventana.EncontrarVentana(null, "Información sobre documentos entrantes");
            if(ip_Handle != IntPtr.Zero)
                SendKeys.SendWait("{ENTER}");

            System.Threading.Thread.Sleep(3500);
            AgregarLog("Entrando a la transacción ZIAS...");
            System.Drawing.Point coordenada = new System.Drawing.Point(100, 50);
            cah_Ventana.ClicDerecho(coordenada);
            SendKeys.SendWait("ZIAS");
            SendKeys.SendWait("{ENTER}");

            System.Threading.Thread.Sleep(5500);
            coordenada = new System.Drawing.Point(250, 250);
            cah_Ventana.ClicDerecho(coordenada);
            SendKeys.SendWait("6080");
            SendKeys.SendWait("{TAB}");
            SendKeys.SendWait("IPBL01");
            SendKeys.SendWait("{TAB}");

            AgregarLog("Abriendo libro de turnos..");
            SendKeys.SendWait("{F6}");

            //System.Threading.Thread.Sleep(3500);
            //AgregarLog("Buscando nota de cabecera");
            //coordenada = new System.Drawing.Point(350, 250);
            //cah_Ventana.ClicDerecho(coordenada);
            //SendKeys.SendWait("Esto es una prueba para verificar si es posble escribir en la cabecera");
        }

        private void Btn2_HacerClic_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MskTxtBx1_CoordX.Text) && !string.IsNullOrEmpty(MskTxtBx2_CoordY.Text))
            {
                int i_coordX = Convert.ToInt32(MskTxtBx1_CoordX.Text),
                i_coordY = Convert.ToInt32(MskTxtBx2_CoordY.Text);

                System.Drawing.Point coordenada = new System.Drawing.Point(i_coordX, i_coordY);

                AgregarLog("Haciendo clic en coordenadas: " + i_coordX + ", " + i_coordY);

                //cah_Ventana.ClicDerecho(coordenada);
                //cah_Ventana.ClicIzquierdo(coordenada);
                cah_Ventana.PosicionarMouse(coordenada);
            }
            else
                AgregarLog("No hay coordenadas");
        }

        private void Btn3_AbrirAplicacion_Click(object sender, EventArgs e)
        {
            cah_Ventana.AbrirAplicacion(cah_Ventana._RutaAplicacionInicial, cah_Ventana._AplicacionInicial);
            //AgregarLog("Handle de la app: " + cah_Ventana._HandleVentanaActual);
            //AgregarLog("Nombre de la ventana: " + cah_Ventana._NombreVentanaActual);
            //AgregarLog("Nombre de la clase: " + cah_Ventana._NombreClaseActual);

            System.Threading.Thread.Sleep(10000);
        }
    }
}

/*
 
                SetForegroundWindow((int)ip_handle);
                ip_HandleChild = FindWindowEx((int)ip_handle, (int)ip_HandleChild, "Edit", null);
                SetForegroundWindow((int)ip_HandleChild);
                SendKeys.Send ("100 ERP");
 */
