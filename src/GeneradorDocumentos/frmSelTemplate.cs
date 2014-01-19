/** ------------------------------------------
 * CLASE PARCIAL 
 * ---------------------------------------------- 
 * Fichero ..: frmSelTemplate.cs 
 * Clase ....: Partial Class frmSelTemplate
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Seleccionas la plantilla LaTeX que quieras o puedes
 * volver a elegir la ruta donde se guarda el proyecto
 * ------------------------------------ 
 * OBSERVACIONES 
 * ------------------------------------
 * Poner un ejemplo en pdf de cada plantilla
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
    public partial class frmSelTemplate : Form
    {
        private string _pathTemplate, _path, _name;

        public static string _pathDoc;
        public static Form staticVarFormSelTemp = null;

        public frmSelTemplate()
        {
            InitializeComponent();
        }

        //
        //  Boton examinar archivo para elegir plantilla
        //
        private void btnChooseTemplate_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Application.StartupPath + "\\templates\\";
            this.openFileDialog1.Filter = "Archivo de plantilla (plantilla_*.txt)|*.txt|Plantillas guardadas (*.dat)|*.dat|Todos los archivos (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 1;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.FileName = "";


            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _pathTemplate = this.openFileDialog1.FileName;
                this.txtTemplate.Text = _pathTemplate;
                getPreview();
            }
        }

        private void getPreview()
        {
            if (_pathTemplate.Contains("plantilla_"))
            {
                _name = _pathTemplate;
                int index = _name.IndexOf("plantilla_") + 10;
                _name = _name.Remove(0, index);
                _name = _name.Remove(_name.Length - 4);

                _path = Application.StartupPath + "\\examples\\";
                string preview = _path + _name + ".pdf";
                if (System.IO.File.Exists(preview))
                {
                    btnPreview.Visible = true;
                    
                }
                else
                    btnPreview.Visible = false;
            }



        }

        private void btnNextFromTemp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_pathTemplate))
                MessageBox.Show("Seleccione una plantilla antes de dar el siguiente paso", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                try
                {
                    // Guardo una referencia a la instancia actual
                    staticVarFormSelTemp = this;

                    this.Hide(); 

                    frmMain frm = new frmMain(_pathDoc, _pathTemplate);

                    bool saved = false;
                    if (_pathTemplate.Contains(".dat"))
                    {
                        saved = true;
                    }

                    frm.Saved = saved;
                    frm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo procesar la plantilla: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBefFromTemp_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmSelPath.staticVarFormSelPath.Show();
            frmSelPath.staticVarFormSelPath.Focus();            
        }

        public static void setPathDoc(string pathDoc)
        {
            _pathDoc = pathDoc;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process processRunPdf = new System.Diagnostics.Process();

            processRunPdf.StartInfo.WorkingDirectory = _path;
            processRunPdf.StartInfo.FileName = _name + ".pdf";

            processRunPdf.Start();
        }


    }
}