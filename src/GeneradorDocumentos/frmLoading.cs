/** ------------------------------------------
 * CLASE PARCIAL 
 * ---------------------------------------------- 
 * Fichero ..: frmLoading.cs 
 * Clase ....: Partial Class frmLoading
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Esta clase proporciona un formulario de espera
 * mientras se ejecuta en background la tarea de 
 * procesado de documentos
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
    public partial class frmLoading : Form
    {
        private BackgroundWorker _bw;

        public frmLoading(BackgroundWorker bw)
        {
            InitializeComponent();
            _bw = bw;
        }

        private void frmLoading_FormClosed(object sender, FormClosedEventArgs e)
        {
            _bw.CancelAsync();
        }
    }
}