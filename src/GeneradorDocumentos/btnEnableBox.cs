/** ------------------------------------------
 * CONTROL DE USUARIO 
 * ---------------------------------------------- 
 * Fichero ..: btnEnableBox.cs 
 * Clase ....: Partial Class btnEnableBox : UserControl
 * Lenguaje .: C# 2.0
 * Autor ....: Omar Pera Mira 
 * Email  ...: campbell.sx@gmail.com
 * ------------------------------------ 
 *  SUMARIO 
 * ------------------------------------ 
 * Esta clase proporciona un control con un boton
 * que como fondo tiene una imagen y la cambia al
 * apretarlo.
 * ------------------------------------ 
 * OBSERVACIONES 
 * ------------------------------------
 * Estan agregados los eventos para des/habilitar
 * los textbox segun el boton
 * ------------------------------------ 
 * MODIFICACIONES
 * ------------------------------------
 * 24/Nov/2007 : creacion
 * ---------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DokuGen
{
    public partial class btnEnableBox : UserControl
    {
        private ImageList iconImages = new ImageList();
        private GroupBox _grp;
        private TabPage _tab;

        // Default --> imagen -> yes, enabled = false
        private int indexCurrImage = 0;

        public btnEnableBox(GroupBox grp, TabPage tab, bool enable)
        {
            InitializeComponent();

            _grp = grp;
            _tab = tab;

            iconImages.ColorDepth = ColorDepth.Depth8Bit;
            iconImages.ImageSize = new Size(16, 16);

            // Obtengo todos los nombre de imagen de un directorio
            string[] iconFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\images\\enabled\\");

            if (iconFiles.Length < 2)
            {
                MessageBox.Show("No se han encontrado las imagenes para habilitar/deshabilitar los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int cnt = 0, indexInitImage = 0, indexNo = 0, indexYes = 0;

            // Crea un objeto imagen para cada icono y lo añade a listImage
            foreach (string iconFile in iconFiles)
            {
                iconImages.Images.Add(Image.FromFile(iconFile));

                if (iconFile.Contains("no") == true)
                {
                    indexNo = cnt;

                }
                else
                    indexYes = cnt;

                cnt++;
            }

            indexInitImage = (enable == false) ? indexNo : indexYes;
            indexCurrImage = indexInitImage;

               // Specify the layout style of the background image. Tile is the default. button1.BackgroundImageLayout = ImageLayout.Center; // Make the button the same size as the image. button1.Size = button1.BackgroundImage.Size;
            btnEnable.ImageList  = iconImages;
            btnEnable.ImageIndex = indexInitImage;

            btnEnable.Size = btnEnable.Image.Size;
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in _grp.Controls)
                if (ctr.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ctr.Enabled = (ctr.Enabled == false) ? true : false;
                }

            Control grpBoxSection = new Control();
            bool enableSection = false;

            
            foreach (Control ctr in _tab.Controls)
            {
                if (ctr.GetType().ToString() == "System.Windows.Forms.GroupBox")
                {
                    if (ctr.Name.Contains("Section"))
                    {
                        grpBoxSection = ctr;
                    }
                    else
                    {
                        foreach (Control c in ctr.Controls)
                        {
                            if (c.GetType().ToString() == "System.Windows.Forms.TextBox")
                            {
                                if (c.Enabled == true)
                                    enableSection = true;

                                break;
                            }
                        }
                    }
                }
                
            }
            

            foreach (Control ctrGrp in grpBoxSection.Controls)
            {
                if (ctrGrp.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ctrGrp.Enabled = enableSection;
                    break;
                }
            }

            btnEnable.ImageIndex = (btnEnable.ImageIndex == 0) ? 1 : 0;
            indexCurrImage = btnEnable.ImageIndex;
        }
    }
}
