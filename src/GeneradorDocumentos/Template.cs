/** ------------------------------------------
 * CLASE
 * ---------------------------------------------- 
 * Fichero ..: Template.cs 
 * Clase ....: public class Template
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Esta clase proporciona a partir de una plantilla con
 * la sintaxis deseada de un documento en LaTeX, procesar
 * y generar un formulario para rellenar los campos, y ejecutar
 * pdflatex y convertirlo a un documento pdf.
 * ------------------------------------ 
 * OBSERVACIONES 
 * ------------------------------------
 * En un futuro al comprobar si no esta pdflatex, darle opcion
 * a indicar la ruta de los ejecutables de la distribución miktex,
 * y poner visible la ventana de msdos para ir instalando los paquetes
 * necesarios.
 * 
 * comprabar null, campos vacios.noptio
 * Nombre pestañas
 * pasar docpath actualizado entre forms
 * $[asi opcion con corchetes],sincorchetesconllaver:1:1:comando$
 * falta regex con (( bien hecho
 * poner en try catch
 * ------------------------------------
 * MODIFICACIONES
 * ------------------------------------
 * 24/Nov/2007 : creacion
 * 26/Nov/2007 : añadido comando para insertar seccion --> [nombre:commandSection]
 *             : añadido soporte a listas items --> (begin:itemize(     )end:itemize) 
 *             : añadido soporte para poner argumentos de comandos con [] (por defecto {} )
 * 27/nov/07   : añadido soporte para comentarios en los formas con ##
 * ---------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace DokuGen
{
    [Serializable]
    class Template
    {
        #region Atributos
        //----------------------------------//
        //           ATRIBUTOS              //
        //----------------------------------//

        private string[] _contentBox;  // Almacena el contenido de cada TextBox
        public string[] contentBox
        {
            get { return _contentBox;}
            set { _contentBox = value; }
        }

        private string[] _templateLines; // Almacena el fichero de template
        public string[] templateLines
        {
            get { return _templateLines; }
            set { _templateLines = value; }
        }

        private string _docName;
        public string docName
        {
            get { return _docName; }
            set { _docName = value; }
        }

        // Correspondencia de cada pestaña <--> titulo de seccion para evento TextChanged
        private int[] _vSection = new int[MAX_TABS];

        
        private const string _programTex2pdf = "pdflatex.exe";

        private string _pathDocument  = "";
        private string _folderProject = "";

        private const char _delItem = '$';
        private const char _delOption = ':';
        private const char _delArg = ',';
        private const char _delBigTextBox = '*';
        private const char _delSectionIni = '[';
        private const char _delSectionEnd = ']';
        private const char _delListItemIni = '(';
        private const char _delListItemEnd = ')';

        private char[] _delArgs = { _delArg };
        private char[] _delItems = { _delItem, _delOption };
        private char[] _delListItem = { _delListItemEnd, _delListItemIni, _delOption };
        private char[] _delTagSections = { _delSectionIni, _delSectionEnd, _delOption };

        private const int _nOptions = 4;

        // Ajusto tamaño TextBox por defecto
        private const int _txtWidth  = 90, _txtWidthBig  = 140;
        private const int _txtHeight = 20, _txtHeightBig = 35;

        private const int MAX_TABS = 20;
        private const int MAX_TEXTBOX = 150;

        [NonSerialized]
        private TextBox[] _vTextBox;
        [NonSerialized]
        private TabControl _tabs;


        #endregion


        #region Metodos
        //----------------------------------//
        //           METODOS PUBLICOS       //
        //----------------------------------//

        public Template()
        {
            _contentBox = new string [MAX_TEXTBOX];
        }

        public string ProcessFormOnRuntime(TabControl tabs)
        {
            try
            {
                _vTextBox = new TextBox[MAX_TEXTBOX];

                // Me guardo una referencia para poder añadir eventos a los controles
                _tabs = tabs;
                TabPage myTabPage = tabs.SelectedTab;

                int X = 30, Y = 25, nBox = 0; // Distancia desde la esquina superior izquierda del form
                bool once = true;   // Para labels de informacion
                int indexSection = -1; // Para habilitar Secciones al importar

                GroupBox grp;
                Label lab = new Label();

                // para cada linea de la plantilla
                foreach (string line in _templateLines)
                {
                    // comprobamos si la cadena existe o es un comentario
                    if (line.Length == 0 || line[0] == '%') { Y += (once == false) ? lab.Height + 2 : 0; once = true; }
                    else if (line[0] == _delSectionIni)
                    {
                        // Init distance --> New Tab/Section
                        X = 30; Y = 25;

                        // Separo el nombre de la sección
                        string[] section = line.Split(_delTagSections, StringSplitOptions.RemoveEmptyEntries);

                        string tabName = section[0];
                        myTabPage = new TabPage(tabName);
                        myTabPage.AutoScroll = true;
                        tabs.TabPages.Add(myTabPage);

                        _vTextBox[nBox] = createTextBox("Section", 30, 30, false, tabs.TabPages.IndexOf(myTabPage));
                        grp = createGroupBox("Título de sección", 25, 20);

                        _vTextBox[nBox].Text = tabName; //le asigno el nombre provisional de la plantilla

                        _vTextBox[nBox].TextChanged += new System.EventHandler(this.titleSections_TextChanged);
                        _vSection[tabs.TabPages.IndexOf(myTabPage)] = nBox;

                        _vTextBox[nBox].Width += 90; // Aumento el tamaño del Box
                        Y += grp.Height + 10;       // Ajusto el punto Y actual

                        // Añado el control al groupbox y éste a la pestaña actual
                        grp.Controls.Add(_vTextBox[nBox]);
                        myTabPage.Controls.Add(grp);

                        indexSection = nBox++;

                        Y += (once == false) ? lab.Height + 2 : 0; once = true; //no me gusta un pelo
                    }
                    else if (line[0] == _delItem)
                    {
                        Y += (once == false) ? lab.Height + 2 : 0; once = true;

                        // Parseo opciones de cada item/formula/etiqueta
                        string[] options = line.Split(_delItems, StringSplitOptions.RemoveEmptyEntries);

                        // Parseo las distintos argumentos y opciones del item
                        string[] Args = options[0].Split(_delArgs, StringSplitOptions.RemoveEmptyEntries);
                        //bool isCompulsory = (int.Parse( options[1] ) > 0 ) ? true : false;
                        int nCamposRep = int.Parse(options[2]);

                        int nArgs = Args.Length;
                        bool enabled = false; 

                        for (int i = 0; i < nCamposRep; i++)
                        {
                            bool bigTextBox = false;

                            int grpX = X, grpY = Y;
                            int ctrX = 15, ctrY = 18, initX = ctrX;

                            /////// CREO EL GROUPBOX CONTENEDOR /////////
                            grp = createGroupBox(grpX, grpY);

                            for (int j = 0; j < nArgs; j++)
                            {
                                // Comprueba si es un TextBox multilinea y formatea el titulo sin delBigTextBox
                                string titleBox = isTextBoxBig(Args[j], out bigTextBox);

                                /////// CREO EL LABEL Y AJUSTO LA POSICION /////////
                                Label lbl = createLabel(titleBox, ctrX, ctrY, false);

                                // Ajusto punto eje X actual tras crear el Label
                                ctrX += lbl.Size.Width;

                                /////// CREO EL TEXTBOX Y AJUSTO LA POSICION /////////
                                _vTextBox[nBox] = createTextBox(titleBox, ctrX, ctrY, bigTextBox, i);

                                // Inserta el contenido si hemos importado de fichero los datos
                                enabled = setContent1TextBox(nBox);

                                // Si lo he importado, si hay algun textbox de esa seccion enable, habilito el textbox de seccion
                                if (indexSection > 0 && enabled == true)
                                    _vTextBox[indexSection].Enabled = true;

                                // Ajusto punto eje X actual tras crear el TextBox y añado un margen
                                ctrX += _vTextBox[nBox].Size.Width + 30;

                                ////// AÑADO LOS CONTROLES AL GROUPBOX ////////
                                grp.Controls.Add(lbl);
                                grp.Controls.Add(_vTextBox[nBox]);

                                ////// AÑADO EL GROUPBOX AL TAB   ////////
                                myTabPage.Controls.Add(grp);

                                nBox++;

                                // NO scroll derecha 550
                                if (ctrX >= 420)
                                {
                                    ctrX = initX;
                                    ctrY += (bigTextBox) ? _txtHeightBig : _txtHeight;
                                    ctrY += 15;
                                }
                            }
                            // Inserto la imagen para habilitar/deshabilitar los textBox
                            btnEnableBox btn = new btnEnableBox(grp, myTabPage, enabled);
                            btn.Location = new System.Drawing.Point(grpX - 30, grpY + 15);

                            myTabPage.Controls.Add(btn);

                            Y = grp.Size.Height + grp.Location.Y + 15;
                        }
                    }
                    else if (line.Contains("##"))
                    {
                        string title = line.Remove(0, 2);

                        if (once == true)
                        {
                            lab = createLabel(title, X, Y, true);
                            myTabPage.Controls.Add(lab);
                        }
                        else
                        {
                            lab.Text += Environment.NewLine + title;

                        }
                        once = false;
                    }
                    else
                    {
                        ;
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // generar fichero definitivo
        public string GenerateFileLatex(string folder)
        {
            try
            {
                _folderProject = folder;
                _pathDocument = folder + "\\" + _docName + ".tex";

                StreamWriter sw = new StreamWriter(_pathDocument, false, System.Text.Encoding.GetEncoding(1252));
                string FullTemplateParsed = "";
                int nBox = 0;

                // para cada linea de la plantilla
                foreach (string line in _templateLines)
                {
                    // comprobamos si la cadena existe o es un comentario
                    if (line.Length == 0 || line[0] == '%' || line[0] == '#') { }
                    else
                    {   // ¿es una seccion?
                        if (line[0] == _delSectionIni)
                        {
                            // Parseo opciones de cada item/formula/etiqueta
                            string[] sections = line.Split(_delTagSections, StringSplitOptions.RemoveEmptyEntries);
                            string tagSection = (sections.Length <= 1) ? "\\section" : "\\" + sections[1];

                            FullTemplateParsed += ( _vTextBox[nBox].Enabled )  
                                                  ? tagSection + "{" + _vTextBox[nBox].Text + "}" + Environment.NewLine
                                                  : "";

                            nBox++;
                        }
                        else
                        {   // ¿es un item?
                            if (line[0] == _delItem)
                            {
                                // Parseo opciones de cada item/formula/etiqueta
                                string[] options = line.Split(_delItems, StringSplitOptions.RemoveEmptyEntries);

                                // Compruebo si falta el comando de latex --> campo de solo texto
                                bool noLatexTag = (options.Length == _nOptions - 1) ? true : false;

                                // comprobar null

                                // Parseo las distintos argumentos y opciones del item
                                string[] Args = options[0].Split(_delArgs, StringSplitOptions.RemoveEmptyEntries);
                                bool isCompulsory = (int.Parse(options[1]) > 0) ? true : false;
                                int nCamposRep = int.Parse(options[2]);
                                string tagTex = (noLatexTag == true) ? "" : options[3]; //comando latex sin {}, o vacio para solo texto

                                for (int i = 0; i < nCamposRep; i++)
                                {
                                    if (_vTextBox[nBox].Enabled)
                                    {
                                        FullTemplateParsed += (noLatexTag == true) ? "" : "\\";
                                        FullTemplateParsed += tagTex;
                                    }

                                    for (int j = 0; j < Args.Length; j++)
                                    {
                                        if (_vTextBox[nBox].Enabled)
                                        {
                                            string sIni = "{", sEnd = "}";
                                            if (noLatexTag == true)
                                            {
                                                sIni = "";
                                                sEnd = "" + Environment.NewLine;
                                            }
                                            else if (Args[j].Contains("["))
                                            {
                                                sIni = "["; sEnd = "]";
                                            }

                                            FullTemplateParsed += sIni + _vTextBox[nBox].Text + sEnd;

                                        }

                                        nBox++;
                                    }
                                    FullTemplateParsed += ( _vTextBox[nBox-1].Enabled ) ? Environment.NewLine : ""; 
                                }
                            }
                            else
                            {   // Nueva lista de items
                                if (line.Contains("(") || line.Contains(")"))
                                {
                                    //sacarlo por expresiones regulares
                                    //openList =  regListItemIni.Match(line).Success  ||
                                    //            regListItemEnd.Match(line).Success

                                    string[] list = line.Split(_delListItem, StringSplitOptions.RemoveEmptyEntries);
                                    if (list.Length >= 2)
                                    {
                                        FullTemplateParsed += "\\" + list[0];

                                        for ( int i = 1; i < list.Length; i++)
                                            FullTemplateParsed += "{" + list[i] + "}";

                                        FullTemplateParsed += Environment.NewLine;
                                    }
                                }

                                else
                                    FullTemplateParsed += line + Environment.NewLine;
                            }
                        }
                    }
                }
                sw.Write(FullTemplateParsed);
                sw.Close();

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string runPdfLatex(string folderPdflatex)
        {
            try
            {
                // Creo un directorio temporal para archivos auxiliares en la ruta del proyecto
                DirectoryInfo tempDir = new DirectoryInfo(_folderProject + "\\temp");

                if (tempDir.Exists == false)
                    tempDir.Create();


                System.Diagnostics.Process pPdflatex = new System.Diagnostics.Process();

                //Por si esta en el path o no
                folderPdflatex += (string.IsNullOrEmpty(folderPdflatex)) ? "" : "\\";

                pPdflatex.StartInfo.FileName = folderPdflatex + _programTex2pdf;
                //pPdflatex.StartInfo.Arguments = _docName + ".tex" + " --interaction nonstopmode --aux-directory=temp";
                pPdflatex.StartInfo.Arguments = "\"" + _docName + ".tex" + "\"" + " --interaction nonstopmode --aux-directory=temp --enable-installer";
                //pPdflatex.StartInfo.Arguments = _docName + ".tex" + " --aux-directory=temp";
                pPdflatex.StartInfo.WorkingDirectory = _folderProject;
                pPdflatex.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                pPdflatex.StartInfo.CreateNoWindow = true;

                pPdflatex.Start();

                pPdflatex.WaitForExit(600000);

                if (pPdflatex.HasExited == false)
                {
                    pPdflatex.Kill();
                    return "Plantilla mal formada";
                }

                return "";
            }
             catch(Exception)
            {
                return "No se pudo ejecutar " + _programTex2pdf + " para procesar el documento, \n\n" + 
                        "¿Desea buscar la ruta del programa manualmente?";
            }

            
        }

        public string runPdfViewer()
        {
            try
            {
                System.Diagnostics.Process processRunPdf = new System.Diagnostics.Process();

                processRunPdf.StartInfo.WorkingDirectory = _folderProject;
                processRunPdf.StartInfo.FileName = _docName + ".pdf";

                processRunPdf.Start();

                return "";
            }
            catch (Exception e)
            {
                return "No se pudo mostrar el documento: " + e.Message;
            }
        }


        public void saveContentTextBox()
        {
            for (int i = 0; i < _vTextBox.Length && _vTextBox[i] != null; i++)
            {
                // Pongo un espacio si esta vacio pero habilitado para
                // ahorrarme comprobaciones mas tarde (mas tarde los quito)
                _contentBox[i] = (_vTextBox[i].Enabled) ? 
                                     ( string.IsNullOrEmpty(_vTextBox[i].Text) ) ?
                                        " " : 
                                        _vTextBox[i].Text.ToString() :
                                        "" ;
            }
        }

        //----------------------------------//
        //         METODOS PRIVADOS         //
        //----------------------------------//
        private Label createLabel(string title, int X, int Y, bool autoSize)
        {
            Label lbl = new Label();
            lbl.Text = title;
            lbl.Name = "lbl" + title;
            lbl.Size = new System.Drawing.Size(70, 35);
            lbl.Location = new System.Drawing.Point(X, Y);
            lbl.AutoSize = autoSize;

            return lbl;
        }

        private TextBox createTextBox(string title, int X, int Y, bool bigTextBox, int id)
        {
            int H, W;
            TextBox txt = new TextBox();
            txt.Name = "txt" + title + id;
            
            txt.Enabled = false;

            if (bigTextBox == true)
            {
                W = _txtWidthBig;
                H = _txtHeightBig;

                txt.Multiline = true;
            }
            else
            {   // asignamos valor por defecto
                W  = _txtWidth;
                H  = _txtHeight;
            }
            txt.Size = new System.Drawing.Size(W, H);
            txt.Location = new System.Drawing.Point(X, Y);

            return txt;
        }

        private GroupBox createGroupBox(string title, int X, int Y)
        {
            GroupBox grp = new GroupBox();
            grp.Location = new System.Drawing.Point(X, Y);
            grp.Text = title;
            grp.Name = "Section";

            // Ajusto al textBox de seccion
            grp.Width += 50;
            grp.Height -= 20;

            return grp;
        }

        private GroupBox createGroupBox(int X, int Y)
        {
            GroupBox grp = new GroupBox();
            grp.Location = new System.Drawing.Point(X, Y);

            // Para que la propiedad autoSize pueda aumentar hasta el 
            // tamaño deseado debido a que no se puede contraer
            grp.Width = 10;
            grp.Height = 10;

            grp.AutoSize = true;
            grp.Padding = new Padding(10,10,10,0);

            return grp;
        }

        private string isTextBoxBig(string title, out bool bigTextBox)
        {
            // Comprueba si es un campo multiline
            bigTextBox = title.Contains( _delBigTextBox.ToString());
            char[] delete = { _delBigTextBox, _delSectionIni, _delSectionEnd };

            // elimino:
            //      los indicadores de Campo Multilinia
            //      si hay aparicion de corchetes --> argumento en []
            return title.Trim(delete);
        }

        private void titleSections_TextChanged(object sender, EventArgs e)
        {
            TabPage tab  = _tabs.SelectedTab;
            int indexTab = _tabs.SelectedIndex;
            int indexTextBox = _vSection[indexTab];

            tab.Text = _vTextBox[ indexTextBox ].Text;
        }

        private bool setContent1TextBox(int index)
        {
            // Si se ha cargado de fichero inserto los datos en el box correspondiente
            if (_contentBox != null && string.IsNullOrEmpty(_contentBox[index]) == false)
            {
                _vTextBox[index].Text = (_contentBox[index].StartsWith(" ")) ? "" : _contentBox[index];
                _vTextBox[index].Enabled = true;
            }
            return _vTextBox[index].Enabled;
        }

        #endregion
    }
}
