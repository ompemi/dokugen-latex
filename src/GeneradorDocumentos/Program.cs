/** ------------------------------------------
 * CLASE 
 * ---------------------------------------------- 
 * Fichero ..: Program.cs 
 * Clase ....: static class Program
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Punto de entrada de la aplicación, inicializa el formulario
 * de seleccionar la ruta del documento
 * ------------------------------------ 
 * MODIFICACIONES
 * ------------------------------------
 * 24/Nov/2007 : creacion
 * ---------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DokuGen
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmSelPath());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la aplicacion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}