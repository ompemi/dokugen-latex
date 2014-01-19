/** ------------------------------------------
 * CLASE PARCIAL 
 * ---------------------------------------------- 
 * Fichero ..: frmMain.cs 
 * Clase ....: Partial Class frmMain
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Esta clase proporciona el formulario base para generar en 
 * tiempo de ejecución los distintos textbox de cada plantilla
 * ------------------------------------ 
 * OBSERVACIONES 
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
    public partial class frmMain : Form
    {
        #region Atributos
        //----------------------------------//
        //           ATRIBUTOS              //
        //----------------------------------//

        private Template _temp;
        private frmLoading _f;

        private string _pathDoc      = "";
        private string _pathTemplate = "";
        private string _pathPdflatex = "";
        private string _nameTemplate = "";

        // Indica si se han importado datos de fichero
        private bool _saved = false;
        public bool Saved
        {
            get { return _saved;  }
            set { _saved = value; }
        }

        #endregion

        #region Métodos
        //----------------------------------//
        //           METODOS                //
        //----------------------------------//

        //
        //  Constructor formulario
        //
        public frmMain(string pathDoc, string pathTemplate)
        {
            _pathDoc = pathDoc;
            _pathTemplate = pathTemplate;

            InitializeComponent();
        }

        private string serializeTemplate()
        {
            try
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.Stream stream = new System.IO.FileStream(_pathDoc + "\\" + _nameTemplate + ".dat", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                formatter.Serialize(stream, _temp);
                stream.Close();
                stream.Dispose();

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string deserializeTemplate()
        {
            try
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.Stream stream = new System.IO.FileStream(_pathTemplate, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                _temp = (Template)formatter.Deserialize(stream);
                stream.Close();
                stream.Dispose();

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Delegado para modificar propiedades de otro proceso
        public delegate void delegEnable(Form frm);

        // Bloquea el formulario principal
        public void setNotEnabledForm(Form frm)
        {
            frm.Enabled = false;
        }

        #endregion

        #region Eventos
        //----------------------------------//
        //           EVENTOS                //
        //----------------------------------//

        //
        //  Boton Generar documento
        //
        private void btnGenerateDoc_Click(object sender, EventArgs e)
        {
            // Muestro la barra de progreso
            _f = new frmLoading(backgroundWorker1);
            _f.Show();

            // Se ejecuta en background las tareas para generar el documento
            backgroundWorker1.RunWorkerAsync();
        }

        //
        //  Evento que realiza tareas en background
        //
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Realizo el trabajo en otro thread
            string msg = _temp.GenerateFileLatex(_pathDoc);
            if (string.IsNullOrEmpty(msg) == false)
            {
                e.Cancel = true;
                //poner aki mensaje dde no se pudo no haya
                MessageBox.Show("Plantilla mal realizada: \n" + msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Si deshabilito antes el frmMain no procesa bien los textBox des/habilitados:
            //      Invoco de manera asincrona el metodo para deshabilitar el form
            //      desde el subproceso que creo el control
            BeginInvoke(new delegEnable(setNotEnabledForm), new object[] { this });

            string msg2 = _temp.runPdfLatex(_pathPdflatex);
            if (string.IsNullOrEmpty(msg2) == false)
            {
                e.Result = msg2 as object;

                return;
            }

            if (backgroundWorker1.CancellationPending == true) { e.Cancel = true; }
        }

        //
        //  Evento al finalizar las tareas en background
        //
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Restablezco el control al form principal
                this.Enabled = true;
                _f.Close();

                if (e.Result != null)
                {
                    DialogResult res = MessageBox.Show(e.Result.ToString(), "No se encuentra el archivo",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button1);

                    if (res == DialogResult.Yes)
                    {
                        FolderBrowserDialog fb = new FolderBrowserDialog();

                        // Inicializo ruta por defecto
                        string programFilesPath = Uri.UnescapeDataString(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                        fb.SelectedPath = programFilesPath;
                        fb.ShowNewFolderButton = false;

                        fb.Description = "Seleccione el directorio del programa pdflatex. Por defecto se encuentra en:\nC:/Archivos de programa/Miktex/miktex/bin";
                        fb.ShowDialog();
                        _pathPdflatex = fb.SelectedPath;

                        // Guardar configuración en archivos para posteriores ejecuciones
                    }
                }
                else if (e.Cancelled)
                {
                    // El usuario cancela la operación
                    this.Focus();
                }
                else if (e.Error != null)
                {
                    // Ha habido un error
                    string msg = "Mensaje de error: " + e.Error.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string pathDoc = _pathDoc + "\\" + _temp.docName + ".pdf";
                    if (System.IO.File.Exists(pathDoc) == false)
                        MessageBox.Show("No existe el fichero:\n " + _pathDoc + "\\" + _temp.docName + ".pdf", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        MessageBox.Show("Documento generado correctamente en: \n\n" + _pathDoc + "\\" + _temp.docName + ".pdf", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string msg = _temp.runPdfViewer();
                        if (string.IsNullOrEmpty(msg) == false)
                            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                ;
            }

        }

        //
        //  Evento al cargar el form
        //
        private void frmMain_Load(object sender, EventArgs e)
        {
            string msg = ""; // Mensaje de error

            try
            {
                if (_saved)
                {
                    // Importo la clase Template
                    msg = deserializeTemplate();
                    if (string.IsNullOrEmpty(msg) == false)
                        throw new Exception();

                    _nameTemplate = _temp.docName;
                }
                else
                {
                    // da error porque coje toda la ruta si no tiene plantilla_
                    // al ver el pdf
                    if (_pathTemplate.Contains("plantilla_"))
                    {
                        _nameTemplate = _pathTemplate;
                        int index = _nameTemplate.IndexOf("plantilla_") + 10;
                        _nameTemplate = _nameTemplate.Remove(0, index);
                        _nameTemplate = _nameTemplate.Remove(_nameTemplate.Length - 4);
                    }
                    else
                        _nameTemplate = "Documento";

                    // Genero dinamicamente los distintos controles a partir del template
                    _temp = new Template();
                    _temp.docName = _nameTemplate;

                    // Almaceno el fichero plantilla desde el fichero (si esta serializado ya estan los datos)
                    _temp.templateLines = System.IO.File.ReadAllLines(_pathTemplate, System.Text.Encoding.GetEncoding(1252));
                }

                this.Text = "Dokugen - " + _nameTemplate;

                msg = this._temp.ProcessFormOnRuntime(tabControlMain);
                if (string.IsNullOrEmpty(msg) == false)
                    throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo procesar la plantilla: \n" + msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        //  Evento para volver al form de elegir plantilla
        //
        private void btnSelTemp_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Si eliges una nueva plantilla pierdes los datos introducidos,\n\n¿Desea seguir con la operación?", "Información",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult.Yes == res)
            {
                frmSelTemplate.staticVarFormSelTemp.Show();
                this.Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Inserto el contenido de los TextBox en un string[]
            _temp.saveContentTextBox();  

            // Serializa la clase template para despues importarla
           string msg = serializeTemplate();
           if (string.IsNullOrEmpty(msg) == true)
               MessageBox.Show("Plantilla guardada correctamente en:\n\n" + _pathDoc + "\\" + _nameTemplate + ".dat\n\n Para importar los datos sólo tienes que abrirlo como plantilla", 
                   "Información",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
           else
               MessageBox.Show("No se pudo guardar la plantilla: \n" + msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //
        // Finaliza el programa
        //
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("¿Quiere guardar los datos introducidos en el documento antes de salir?",
                        "Información",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

            switch (res)
            {
                case DialogResult.Yes:
                    //llamar evento clicksave
                    this.btnSave_Click(this, new EventArgs());
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        #endregion


    }
}