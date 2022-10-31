using System;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using System.Windows.Forms;

namespace ControlSAPCSharp.Clases
{
    /// <summary>Clase que permite los controles básicos de otra aplicación</summary>
    public class CAppHandler
    {
        #region <Windows Messages>

        public struct WindowMessages
        {
            /// <summary>Constante del mensaje WM_GETTEXT que permite copiar el texto que corresponde a la ventana a un buffer brindado por el "caller"</summary>
            public const int WM_GETTEXT = 0x000D;
            /// <summary>Determina el largo en caracteres del texto asociado con una ventana</summary>
            public const int WM_GETTEXTLENGTH = 0x000E;
        }

        public struct WindowControl_ListView
        {
            /// <summary>Devuelve el texto de un item de un ListView</summary>
            public const int LVM_GETITEMTEXTA = 0x102D;
        }

        //Información de los Flags: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
        [Flags]
        public enum MouseEventFlags
        {
            MOUSEEVENTF_ABSOLUTE = 0X8000,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_HWHEEL = 0x01000
        }

        #endregion <Windows Messages>

        #region <Variables>

        private bool b_AplicacionIniciada = false;

        private IntPtr ip_handle,
                        ip_HandleChild;

        private StringBuilder sb_NombreVentana = new StringBuilder(),
                               sb_NombreClase = new StringBuilder(),
                               sb_ContenidoVentana = new StringBuilder();

        private string s_NombreVentana = ConfigurationManager.AppSettings["NombreVentanaAEjecutar"];

        //Miembros que almacenaran los criterios de busqueda mientras ésta se ejecuta
        private int parentHandle;
        private Regex className,
                      windowText,
                      process;

        public delegate bool CallbackVentanaEnontrada(int hWnd);

        /*Evento que ocurre cada vez que una ventana fue encontrada coincide los criterios de
         busqueda. El delegado boleano retorna un valor si conicide con la funcion PChildCallBack*/
        private event CallbackVentanaEnontrada VentanaEncontrada;

        private delegate bool CallbackPadre_Hijo(int hWnd, int lParam);

        #endregion <Variables>

        #region <Propeidades>
        
        /// <summary>Indica si la aplicación ya fue iniciada</summary>
        public bool _AplicacionIniciada { get { return b_AplicacionIniciada; } } 

        /// <summary>Devuelve el Handle de la ultima ventana buscada</summary>
        public IntPtr _HandleVentanaActual { get { return this.ip_handle; } }

        /// <summary>Devuelve el handle de la última ventana hija buscada</summary>
        public IntPtr _HandleVentanaHija { get { return ip_HandleChild; } }

        /// <summary>Aplicación que se ejecuta al iniciar</summary>
        public string _AplicacionInicial { get { return ConfigurationManager.AppSettings["AplicacionAEjecutar"]; } }

        /// <summary>Ubicación de la aplicación a ejecutar</summary>
        public string _RutaAplicacionInicial { get { return ConfigurationManager.AppSettings["RutaDeAplicacion"]; } }

        /// <summary>Nombre de la ventana a inicializar</summary>
        public string _NombreVentana { get { return s_NombreVentana; } set { s_NombreVentana = value; } }
        
        /// <summary>Devuelve el nombre de la ultima ventana buscada con el Handle</summary>
        public string _NombreVentanaActual
        {
            get
            {
                //if (this.sb_NombreVentana.Equals(null) || string.IsNullOrEmpty(this.sb_NombreVentana.ToString()))
                //{
                //    this.sb_NombreVentana = new StringBuilder("Sin Registro");
                //    return this.sb_NombreVentana.ToString();
                //}
                //else
                //    return this.sb_NombreVentana.ToString();

                int i_largoNombreVentana = SendMessage((int)ip_handle, WindowMessages.WM_GETTEXTLENGTH, 0, 0);
                this.sb_NombreVentana = new StringBuilder(i_largoNombreVentana + 1);
                SendMessage((int)ip_handle, WindowMessages.WM_GETTEXT, this.sb_NombreVentana.Capacity, this.sb_NombreVentana);

                return sb_NombreVentana.ToString();
            }
        }

        /// <summary>Devuelve el nombre de la última clase buscada</summary>
        public string _NombreClaseActual
        {
            get
            {
                //if (this.sb_NombreClase.Equals(null) || string.IsNullOrEmpty(this.sb_NombreClase.ToString()))
                //{
                //    sb_NombreClase = new StringBuilder("Sin Registro");
                //    return sb_NombreClase.ToString();
                //}
                //else
                //    return sb_NombreClase.ToString();

                this.sb_NombreClase = new StringBuilder(256);
                GetClassName((int)ip_handle, sb_NombreClase, sb_NombreClase.Capacity);

                return sb_NombreClase.ToString();
            }
        }

        #endregion <Propiedades>

        #region <Funciones "User32">

        /// <summary>
        /// Copia el texto de la barra de titulo de la ventana especificada (si cuenta con uno) en un buffer.
        /// Si la ventana especificada es un control, el texto de éste es copiado.
        /// </summary>
        /// <param name="hWnd">Handle de la ventana o control que contiene el texto</param>
        /// <param name="TextoDeVentana">Buffer donde será almacenado el texto</param>
        /// <param name="cont">Maximo número de caracteres a copiar en el buffer. Si el texto excede este limite, éste es truncado</param>
        [DllImport("User32.dll")]
        private static extern void GetWindowText(int hWnd, StringBuilder TextoDeVentana, int cont);

        /// <summary>
        /// Obtiene el nombre de la clase de la ventana especificada por el Handle de la misma
        /// </summary>
        /// <param name="hWnd">Handle de la ventana e indirectamente la clase a la cual la ventana pertenece</param>
        /// <param name="BufferNombreVentana">Buffer donde se almacenara el nombre de la clase</param>
        /// <param name="Contador">Tamaño del buffer lpClassName en caracteres. Éste debe ser lo suficientemente largo para que incluya el caracter terminador nulo, de otro
        /// modo el nombre la cadena de caracteres es truncado a un maximo de caracteres nMaxCount -1</param>
        [DllImport("User32.dll")]
        private static extern void GetClassName(int hWnd, StringBuilder BufferNombreClase, int Contador);

        /// <summary>
        /// Envia el mensaje especificado de una ventana a otra y no retorna ningun valor hasta que el "procedure" de la ventana haya procesado el mensaje
        /// </summary>
        /// <param name="hWnd">Handle de la ventana con la que se quiere comunicar</param>
        /// <param name="Msg">Mensaje a ser envíado (Windows Messages)</param>
        /// <param name="wParam">Información adicional del mensaje</param>
        /// <param name="lparam">Buffer donde se almacena la información solicitada</param>
        [DllImport("User32.dll")]
        private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, StringBuilder lparam);

        /// <summary>
        /// Envia el mensaje especificado de una ventana a otra y no retorna ningun valor hasta que el "procedure" de la ventana haya procesado el mensaje
        /// </summary>
        /// <param name="hWnd">Handle de la ventana con la que se quiere comunicar</param>
        /// <param name="Msg">Mensaje a ser envíado (Windows Messages)</param>
        /// <param name="wParam">Información adicional del mensaje</param>
        /// <param name="lparam">Información adicional del mensaje</param>
        [DllImport("User32.dll")]
        private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);
        
        /// <summary>
        /// Devuelve el identificador del Hilo que creo la ventana especificada, opcionalmente, también devuelve el identificador del proceso que creaó la ventana
        /// </summary>
        /// <param name="hWnd">Handle de la ventana</param>
        /// <param name="lpdwProcessId">Puntero a una variable que recibe el identificador del proceso. Si este parámetro no es NULL, esta funicion copia el identificador
        /// del proceso a la variable, de otra forma, no lo hace</param>
        /// <returns>Devuelve el valor del identificador del hilo que creó la ventana</returns>
        [DllImport("User32.dll")]
        private static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        /// <summary>
        /// Enumera las ventanas hijas que pertenecen a la ventana padre especificada al pasar el "handle" a cada una de las ventanas hijas.
        /// Con esta funcion se especfica se puede especificar un parametro que referencie el handle de la ventana padre.
        /// </summary>
        /// <param name="hWndParent">Handle de la ventana parde a quien las ventanas hijas serán enumeradas</param>
        /// <param name="lpEnumFunc">Puntero a una función Callback definida</param>
        /// <param name="lParam">Valor que recibirá los datos de la función callback</param>
        /// <returns></returns>
        [DllImport("user32.Dll")]
        private static extern Boolean EnumChildWindows(int hWndParent, CallbackPadre_Hijo lpEnumFunc, int lParam);

        /// <summary>
        /// Ocasiona que el handle especificado sea una ventana Top-Level, esta funión no busca las ventanas hijas.
        /// </summary>
        /// <param name="lpClassName">Nombre de la clase. Si lpClassName apunta a un string, se está especificando el el nombre de la clase de la ventana.
        /// Si lpClassName es NULL, encuentra cuqluier ventana cuyo titulo coincida con el lpWindowName</param>
        /// <param name="lpWindowName">Nombre de la ventana. Si el parametro es NULL, todos los nombres de las ventanas coinciden</param>
        /// <returns>HWND. Si la función es ejecutada correctamente, el valor devuelto es el handle de la ventana que se especificó en los parametros, 
        /// si la función falla, el valor devuelto es "NULL"</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Trae el hilo que creo la ventana especificada a primer plano y la activa. La entrada de teclado es direccionada a la ventana.
        /// El sistema asigna ligeramente una mayor prioridad al hilo que creo la ventana en el primer plano.
        /// </summary>
        /// <param name="hWnd">Handle de la ventana que debería estar activa y traida a primer plano</param>
        /// <returns>Bool. Si la ventana es traida al primr plano, el valor devuelto es diferente a cero.
        /// Si la ventana no es traida a primer plano, el valor devueto es cero</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(int hWnd);

        /// <summary>
        /// Busca el handle hija por medio de la clase y nombre de la ventana hija (Spy++), brindando el handle de la ventana padre
        /// </summary>
        /// <param name="hWndParent">Handle de la ventana padre</param>
        /// <param name="hWndChildAfter">Handle de la ventana hija (buffer)</param>
        /// <param name="lpszClass">Nombre de la clase hija que se busca</param>
        /// <param name="lpszWindow">Nombre de la ventana hija que se busca</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(int hWndParent, int hWndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// Funcion que sintetiza el movimiento de mouse y clics
        /// </summary>
        /// <param name="dwFlags">Controla varios aspectos del movimiento del mouse y botones de clic. Este parametro corresponde al uso de Flags</param>
        /// <param name="dx">Posición absoluta del mouse a lo largo del eje X o la cantidad de movimiento desde la última vez que el evento fue 
        /// generado (segun el Flag utilizado)</param>
        /// <param name="dy">Posición absoluta del mouse a lo largo del eje Y o la cantidad de movimiento desde la ultima vez que el evento mouse 
        /// fue utilizado</param>
        /// <param name="dwData">Depende del "Flag" éste buffer de datos es llenado</param>
        /// <param name="dwExtraInfo">Información adicional dependiendo del "Flag"</param>
        [DllImport("User32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        #endregion <Funciones "User32">

        #region <Funciones Privadas>

        /// <summary>
        /// Esta función es llamada cada vez que la ventana es encontrada por 
        /// </summary>
        /// <param name="handle">Handle de la ventana buscada</param>
        /// <param name="lParam">Parametro de ventana</param>
        /// <returns></returns>
        private bool CallBackEnumVentanasHijas(int handle, int lParam)
        {
            //Si el nombre de la clase fue brindado, se revisa si existe la ventana
            if (this.className != null)
            {
                StringBuilder sbClase = new StringBuilder(256);
                GetClassName(handle, sbClase, sbClase.Capacity);

                //Si no se encuentra la ventana, retorna el valor "true" para continuar con la siguiente ventana
                if(!this.className.IsMatch(sbClase.ToString()))
                    return true;
            }

            //Si se brindó el nombre de la ventana, la busca por este medio
            if(this.windowText != null)
            {
                //Obtiene el nombre y largo de caracteres de las ventanas abiertas
                int ilargoNombreVentana = SendMessage(handle, WindowMessages.WM_GETTEXTLENGTH, 0, 0);
                StringBuilder sbnombreDeVentana = new StringBuilder(ilargoNombreVentana + 1);

                //Se comunica con cada una de las ventanas para obtener su nombre
                SendMessage(handle, WindowMessages.WM_GETTEXT, sbnombreDeVentana.Capacity, sbnombreDeVentana);

                //Si no encuentra la ventana, retorna el valor de "true" para continuar con la siguiente ventana
                if (!this.windowText.IsMatch(sbnombreDeVentana.ToString()))
                    return true;
            }

            //Si nombre del proceso fue brindado, busca la ventana por este medio
            if (this.process != null)
            {
                int iprocesoID;
                GetWindowThreadProcessId(handle, out iprocesoID);

                //Con el proceso obtenido, lo podeoms usar en .NET para obtener el objeto "Proceso"
                Process p = Process.GetProcessById(iprocesoID);


                if (!this.process.IsMatch(p.ProcessName))
                    return true;
            }

            return VentanaEncontrada(handle);
        }

        #endregion <Funciones Privadas>

        #region <Funciones Públicas>

        /// <summary>
        /// Permite buscar una ventana onjetivo para poder trabajar con ésta
        /// </summary>
        /// <param name="parentHandle">Handle de la ventana padre (opcional, colocar 0 si no se conoce)</param>
        /// <param name="NombreClase">Nombre de la clase de la ventana a buscar (opcional, colocar "null" si no se conoce, se recomienda usar MicrosoftSpy++)</param>
        /// <param name="TextoVentana">Nombre que aparece en la barra de la ventana o en el administrador de tareas (opcional, de no conocerse colocar "null")</param>
        /// <param name="Proceso">Nombre del proceso (opconal, de no conocerse, colocar "null")(</param>
        /// <param name="BuscarVentana">Función callback que realiza la busqueda</param>
        public void EncontrarVentana(int parentHandle, Regex NombreClase, Regex TextoVentana, Regex Proceso, CallbackVentanaEnontrada BuscarVentana)
        {
            this.parentHandle = parentHandle;
            this.className = NombreClase;
            this.windowText = TextoVentana;
            this.process = Proceso;

            //Agrega el CallBackVentanaEncontrada al evento VentanaEnontrada
            VentanaEncontrada = BuscarVentana;

            //Invoca la funcion EnumChildWindow
            EnumChildWindows(parentHandle, new CallbackPadre_Hijo(CallBackEnumVentanasHijas), 0);
        }

        /// <summary>
        /// Ocasiona que el handle especificado sea una ventana Top-Level, esta funión no busca las ventanas hijas.
        /// </summary>
        /// <param name="lpClassName">Nombre de la clase. Si lpClassName apunta a un string, se está especificando el el nombre de la clase de la ventana.
        /// Si lpClassName es NULL, encuentra cuqluier ventana cuyo titulo coincida con el lpWindowName</param>
        /// <param name="lpWindowName">Nombre de la ventana. Si el parametro es NULL, todos los nombres de las ventanas coinciden</param>
        /// <returns>HWND. Si la función es ejecutada correctamente, el valor devuelto es el handle de la ventana que se especificó en los parametros, 
        /// si la función falla, el valor devuelto es "NULL"</returns>
        public IntPtr EncontrarVentana(string lpClassName, string lpWindowName)
        {
            this.ip_handle = FindWindow(lpClassName, lpWindowName);

            if (ip_handle != IntPtr.Zero)
            {
                int i_largoNombreVentana = SendMessage((int)ip_handle, WindowMessages.WM_GETTEXTLENGTH, 0, 0);
                this.sb_NombreVentana = new StringBuilder(i_largoNombreVentana + 1);
                SendMessage((int)ip_handle, WindowMessages.WM_GETTEXT, this.sb_NombreVentana.Capacity, this.sb_NombreVentana);

                this.sb_NombreClase = new StringBuilder(256);
                GetClassName((int)ip_handle, sb_NombreClase, sb_NombreClase.Capacity);
            }
            
            return ip_handle;
        }

        /// <summary>
        /// Busca la ventana hija dentro de la ventana padre especificada por el nombre de su clase o nombre de ventana.
        /// Por defecto, el nombre de ventana es null.
        /// </summary>
        /// <param name="hWndParent">Handle de la ventana padre</param>
        /// <param name="NombreClaseHija">Nombre de la clase hija contenida por el handle padre</param>
        /// <param name="NombreVentanaHija">Nombre de la ventana hija contenida por el handle padre</param>
        /// <returns></returns>
        public IntPtr BuscarVentanaHija(IntPtr hWndParent, string NombreClaseHija, string NombreVentanaHija = null)
        {
            ip_HandleChild = IntPtr.Zero;

            for (int i = 0; ip_HandleChild == IntPtr.Zero; i++)
            {
                ip_HandleChild = FindWindowEx((int)hWndParent, (int)this.ip_HandleChild, NombreClaseHija, NombreVentanaHija);
            }

            if(ip_HandleChild != IntPtr.Zero)
                SetForegroundWindow((int)ip_HandleChild);

            //SendKeys.SendWait("100 EP1 ERP Production");
            //SendKeys.SendWait("{ENTER}");

            return ip_HandleChild;
        }

        public string ContenidoDeVentana(IntPtr HandleVentana)
        {
            int i_largoNombreVentana = SendMessage((int)HandleVentana, WindowMessages.WM_GETTEXTLENGTH, 0, 0);
            this.sb_ContenidoVentana = new StringBuilder(i_largoNombreVentana + 1);
            SendMessage((int)HandleVentana, WindowMessages.WM_GETTEXT, this.sb_ContenidoVentana.Capacity, this.sb_ContenidoVentana);

            return sb_ContenidoVentana.ToString();
        }

        /// <summary>Trae al frente la ventana especificada al frente</summary>
        /// <param name="HandleVentanaPadre">Handle de la ventana que se desea traer al frente</param>
        public void VentanaAPrimerPlano(IntPtr HandleVentanaPadre)
        {
            SetForegroundWindow((int)HandleVentanaPadre);
        }

        /// <summary>
        /// Inicializa la aplicación especificada
        /// </summary>
        /// <param name="RutaAplicacion">Ruta donde se encuentra el archivo</param>
        /// <param name="NombreAplicacion">Nombre del archivo y su extensión</param>
        public void AbrirAplicacion(string RutaAplicacion, string NombreAplicacion)
        {
            NombreAplicacion = NombreAplicacion.Substring(0, NombreAplicacion.IndexOf("."));
            Process[] ComprobarAplicacion = Process.GetProcessesByName(NombreAplicacion);

            //Corroboro que no exita una aplicación previamente abierta
            if (ComprobarAplicacion.Length < 1)
            {
                using (Process aplicacionAEjecutar = new Process())
                {
                    ProcessStartInfo nombreAplicacionEjecutar = new ProcessStartInfo(RutaAplicacion + "\\" + NombreAplicacion + ".exe");
                    nombreAplicacionEjecutar.UseShellExecute = false;

                    aplicacionAEjecutar.StartInfo = nombreAplicacionEjecutar;

                    aplicacionAEjecutar.Start();

                    //Espera a que el proceso termine de cargar
                    aplicacionAEjecutar.WaitForInputIdle();

                    b_AplicacionIniciada = true;
                }
            }
            else
                EncontrarVentana(null, _NombreVentana);
        }

        #region <Mouse>

        /// <summary>
        /// Ejecuta un clic Derecho en las coordenadas especificadas
        /// </summary>
        /// <param name="Coordenadas">Coordenadas donde se desea hacer clic</param>
        public void ClicDerecho(System.Drawing.Point Coordenadas)
        {
            Cursor.Position = Coordenadas;
            mouse_event((int)MouseEventFlags.MOUSEEVENTF_RIGHTDOWN, Coordenadas.X, Coordenadas.Y, 0, 0);
            mouse_event((int)MouseEventFlags.MOUSEEVENTF_RIGHTUP, Coordenadas.X, Coordenadas.Y, 0, 0);
        }

        /// <summary>
        /// Ejecuta un clic Izquierdo en las coordenadas especificadas
        /// </summary>
        /// <param name="Coordenadas">Coordenadas donde se desea hacer clic</param>
        public void ClicIzquierdo(System.Drawing.Point Coordenadas)
        {
            Cursor.Position = Coordenadas;
            mouse_event((int)MouseEventFlags.MOUSEEVENTF_LEFTDOWN, Coordenadas.X, Coordenadas.Y, 0, 0);
            mouse_event((int)MouseEventFlags.MOUSEEVENTF_LEFTUP, Coordenadas.X, Coordenadas.Y, 0, 0);
        }

        /// <summary>
        /// Posiciona el mouse en las coordenadas especificadas
        /// </summary>
        /// <param name="Coordenadas">Coordenadas donde se desea posicionar el mouse</param>
        public void PosicionarMouse(System.Drawing.Point Coordenadas)
        {
            Cursor.Position = Coordenadas;
        }

        #endregion

        #endregion <Funciones Públicas>
    }
}
