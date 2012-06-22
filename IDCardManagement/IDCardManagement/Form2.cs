using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace IDCardManagement
{
    public partial class Form2 : Form
    {
        public int Y { get; set; }

        public int X { get; set; }
        private IDCard idcard;

        public Form2(IDCard idcard)
        {
            InitializeComponent();
            ControlMover.Init(pictureBox1);
            ControlMover.Init(label1);
            this.idcard = idcard;
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                toolStripComboBox1.Items.Add(font.Name);
            }


        }

        private void Form2_Load(object sender, EventArgs e)
        {

            if (idcard.backgroundImage != null) panel1.BackgroundImage = idcard.backgroundImage;
            label1.Text = idcard.title;
            panel1.Size = new Size(idcard.dimensions.Width * 10, idcard.dimensions.Height * 10);
            foreach (String str in idcard.selectedFields)
            {
                ToolStripItem tmp = contextMenuStrip1.Items.Add(str);
                tmp.Click += tmpToolStripItem_Click;
            }
        }

        private void tmpToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripItem clickedItem = (ToolStripItem)sender;
            Label tmp = new Label();
            tmp.BackColor = Color.Transparent;
            tmp.Text = clickedItem.Text;
            tmp.Left = X;
            tmp.Top = Y;
            tmp.AutoSize = true;
            tmp.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(tmp);
            panel1.Controls.Add(tmp);
        }

        private void tmplbl_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.None;
            if (Control.ModifierKeys != Keys.Control)
                foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }
            Label tmp = sender as Label;
            tmp.BorderStyle = BorderStyle.FixedSingle;
            toolStripComboBox1.Text = tmp.Font.FontFamily.Name;
            toolStripComboBox2.Text = ((int)tmp.Font.Size).ToString();
            toolStripButton1.BackColor = tmp.ForeColor;
            toolStripButton2.BackColor = tmp.BackColor;

        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                X = Cursor.Position.X - panel1.Left;
                Y = Cursor.Position.Y - panel1.Top - 30;
            }

        }


        private void panel1_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }



        private void Form2_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }



        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label) if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle) ((Label)ctl).Font = new Font(toolStripComboBox1.Text, ctl.Font.Size);
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        ((Label)ctl).Font = new Font(ctl.Font.FontFamily.ToString(), Convert.ToInt32(toolStripComboBox2.Text));
                }
            }

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).BackColor = colorDialog1.Color;
                    }
                }

        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        panel1.Controls.Remove(ctl);
                    ctl.Dispose();
                    //((Label)ctl).BackColor = colorDialog1.Color;
                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).ForeColor = colorDialog1.Color;
                    }
                }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                using (XmlWriter wrt = XmlWriter.Create(saveFileDialog1.FileName))
                {
                    wrt.WriteStartDocument();
                    wrt.WriteStartElement("panel");
                    foreach (Control ctl in this.panel1.Controls)
                    {
                        if (ctl is Label)
                        {
                            wrt.WriteStartElement("label");
                            wrt.WriteAttributeString("text", ((Label)ctl).Text);
                            wrt.WriteAttributeString("top", ((Label)ctl).Top.ToString());
                            wrt.WriteAttributeString("left", ((Label)ctl).Left.ToString());
                            wrt.WriteAttributeString("backcolor", ((Label)ctl).BackColor.ToArgb().ToString()); //Color.FromArgb(int);
                            wrt.WriteAttributeString("forecolor", ((Label)ctl).ForeColor.ToArgb().ToString());
                            wrt.WriteAttributeString("font", TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(((Label)ctl).Font)); //Font font = (Font)converter.ConvertFromString(fontString);
                            wrt.WriteEndElement();

                        }
                        if (ctl is PictureBox)
                        {
                            wrt.WriteStartElement("pictureBpx");
                            wrt.WriteAttributeString("left", ((PictureBox)ctl).Left.ToString());
                            wrt.WriteAttributeString("top", ((PictureBox)ctl).Top.ToString());
                            wrt.WriteEndElement();
                        }

                    }

                    wrt.WriteStartElement("idCard");
                    wrt.WriteAttributeString("connectionString", idcard.connectionString);
                    wrt.WriteAttributeString("height", idcard.dimensions.Height.ToString());
                    wrt.WriteAttributeString("width", idcard.dimensions.Width.ToString());
                    wrt.WriteAttributeString("tableName",idcard.tableName);
                    wrt.WriteAttributeString("title",label1.Text);//idcard.title
                    string imagebase64String;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        idcard.backgroundImage.Save(ms, idcard.backgroundImage.RawFormat);
                        byte[] imageBytes = ms.ToArray();
                        imagebase64String = Convert.ToBase64String(imageBytes);
                    }
                    //wrt.WriteAttributeString("backgroundImage", imagebase64String);
                    foreach (string str in idcard.fields)
                    {

                        wrt.WriteElementString("field",str);

 
                    }
                    foreach (string str in idcard.selectedFields)
                    {

                        wrt.WriteElementString("selectedField", str);


                    }
                    wrt.WriteEndElement();//idcard
                    wrt.WriteEndElement();//panel
                    wrt.WriteEndDocument();
                }

        }

    }
}
