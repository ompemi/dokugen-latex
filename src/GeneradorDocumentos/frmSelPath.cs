/** ------------------------------------------
 * CLASE PARCIAL 
 * ---------------------------------------------- 
 * Fichero ..: frmSelPath.cs 
 * Clase ....: Partial Class frmSelPath
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Esta clase genera un formulario que es el inicio 
 * de la aplicacion, donde se pide la ruta donde 
 * quieres guardar el documento
 * ------------------------------------ 
 * OBSERVACIONES 
 * ------------------------------------
 * Mas texto o algo para que no este tan vacio
 * 
 * SplashScreen ss = new SplashScreen(@"..\..\..\images\SplashScreen.png");
 * ss.ShowSplashScreen(1000);
 * ------------------------------------
 * MODIFICACIONES
 * ------------------------------------
 * 24/Nov/2007 : creacion
 * ---------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DokuGen
{
    public partial class frmSelPath : Form
    {
        private string _pathDoc;

        public static Form staticVarFormSelPath = null;
        private frmSelTemplate _frmTemp;

        //
        //  Constructor del formulario
        //
        public frmSelPath()
        {
            InitializeComponent();
        }

        //
        //  Boton examinar directorio para guardar proyecto
        //
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBrowse.Text = folderBrowserDialog1.SelectedPath;
                _pathDoc = txtBrowse.Text;
            }
        }

        //
        //  Evento para cargar ruta por defecto en textbox
        //
        private void frmSelPath_Load(object sender, EventArgs e)
        {
            // Inicializo ruta por defecto para guardar proyecto --> escritorio
            string desktopPath = Uri.UnescapeDataString(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            this.folderBrowserDialog1.SelectedPath = desktopPath;
            _pathDoc = desktopPath;
            txtBrowse.Text = desktopPath;
        }

        private void btnNextFromPath_Click(object sender, EventArgs e)
        {
            // Guardo una referencia a la instancia actual
            staticVarFormSelPath = this;

            this.Hide();

            if (frmSelTemplate.staticVarFormSelTemp == null)
            {
                _frmTemp = new frmSelTemplate();
                frmSelTemplate.setPathDoc(_pathDoc); ;
                _frmTemp.Show();
            }
            else
            {
               //No se porque no puedo usar get/set con la variable estatica
                 frmSelTemplate.setPathDoc(_pathDoc);
                _frmTemp.Show();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}