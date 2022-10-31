namespace ControlSAPCSharp
{
    partial class Frm__CtrlSAP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn1_IniciarControl = new System.Windows.Forms.Button();
            this.TblLyt1_ControlSAP = new System.Windows.Forms.TableLayoutPanel();
            this.GrpBx1_Log = new System.Windows.Forms.GroupBox();
            this.TxtBx1_Log = new System.Windows.Forms.TextBox();
            this.Btn2_HacerClic = new System.Windows.Forms.Button();
            this.MskTxtBx1_CoordX = new System.Windows.Forms.MaskedTextBox();
            this.MskTxtBx2_CoordY = new System.Windows.Forms.MaskedTextBox();
            this.Btn3_AbrirAplicacion = new System.Windows.Forms.Button();
            this.TblLyt1_ControlSAP.SuspendLayout();
            this.GrpBx1_Log.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn1_IniciarControl
            // 
            this.Btn1_IniciarControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Btn1_IniciarControl.Location = new System.Drawing.Point(35, 16);
            this.Btn1_IniciarControl.Name = "Btn1_IniciarControl";
            this.Btn1_IniciarControl.Size = new System.Drawing.Size(75, 36);
            this.Btn1_IniciarControl.TabIndex = 1;
            this.Btn1_IniciarControl.Text = "Iniciar Control";
            this.Btn1_IniciarControl.UseVisualStyleBackColor = true;
            this.Btn1_IniciarControl.Click += new System.EventHandler(this.Btn1_IniciarControl_Click);
            // 
            // TblLyt1_ControlSAP
            // 
            this.TblLyt1_ControlSAP.ColumnCount = 3;
            this.TblLyt1_ControlSAP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TblLyt1_ControlSAP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TblLyt1_ControlSAP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TblLyt1_ControlSAP.Controls.Add(this.GrpBx1_Log, 0, 2);
            this.TblLyt1_ControlSAP.Controls.Add(this.Btn1_IniciarControl, 0, 0);
            this.TblLyt1_ControlSAP.Controls.Add(this.Btn2_HacerClic, 0, 1);
            this.TblLyt1_ControlSAP.Controls.Add(this.MskTxtBx1_CoordX, 1, 1);
            this.TblLyt1_ControlSAP.Controls.Add(this.MskTxtBx2_CoordY, 2, 1);
            this.TblLyt1_ControlSAP.Controls.Add(this.Btn3_AbrirAplicacion, 2, 0);
            this.TblLyt1_ControlSAP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TblLyt1_ControlSAP.Location = new System.Drawing.Point(6, 6);
            this.TblLyt1_ControlSAP.Margin = new System.Windows.Forms.Padding(4);
            this.TblLyt1_ControlSAP.Name = "TblLyt1_ControlSAP";
            this.TblLyt1_ControlSAP.Padding = new System.Windows.Forms.Padding(4);
            this.TblLyt1_ControlSAP.RowCount = 3;
            this.TblLyt1_ControlSAP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.77612F));
            this.TblLyt1_ControlSAP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.22388F));
            this.TblLyt1_ControlSAP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.TblLyt1_ControlSAP.Size = new System.Drawing.Size(422, 329);
            this.TblLyt1_ControlSAP.TabIndex = 2;
            // 
            // GrpBx1_Log
            // 
            this.TblLyt1_ControlSAP.SetColumnSpan(this.GrpBx1_Log, 3);
            this.GrpBx1_Log.Controls.Add(this.TxtBx1_Log);
            this.GrpBx1_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpBx1_Log.Location = new System.Drawing.Point(7, 141);
            this.GrpBx1_Log.Name = "GrpBx1_Log";
            this.GrpBx1_Log.Padding = new System.Windows.Forms.Padding(4);
            this.GrpBx1_Log.Size = new System.Drawing.Size(408, 181);
            this.GrpBx1_Log.TabIndex = 1;
            this.GrpBx1_Log.TabStop = false;
            this.GrpBx1_Log.Text = "Log";
            // 
            // TxtBx1_Log
            // 
            this.TxtBx1_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtBx1_Log.Location = new System.Drawing.Point(4, 17);
            this.TxtBx1_Log.Multiline = true;
            this.TxtBx1_Log.Name = "TxtBx1_Log";
            this.TxtBx1_Log.ReadOnly = true;
            this.TxtBx1_Log.Size = new System.Drawing.Size(400, 160);
            this.TxtBx1_Log.TabIndex = 0;
            // 
            // Btn2_HacerClic
            // 
            this.Btn2_HacerClic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Btn2_HacerClic.Location = new System.Drawing.Point(30, 82);
            this.Btn2_HacerClic.Name = "Btn2_HacerClic";
            this.Btn2_HacerClic.Size = new System.Drawing.Size(85, 37);
            this.Btn2_HacerClic.TabIndex = 2;
            this.Btn2_HacerClic.Text = "Hacer Clic";
            this.Btn2_HacerClic.UseVisualStyleBackColor = true;
            this.Btn2_HacerClic.Click += new System.EventHandler(this.Btn2_HacerClic_Click);
            // 
            // MskTxtBx1_CoordX
            // 
            this.MskTxtBx1_CoordX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MskTxtBx1_CoordX.Location = new System.Drawing.Point(161, 91);
            this.MskTxtBx1_CoordX.Mask = "9999";
            this.MskTxtBx1_CoordX.Name = "MskTxtBx1_CoordX";
            this.MskTxtBx1_CoordX.Size = new System.Drawing.Size(100, 20);
            this.MskTxtBx1_CoordX.TabIndex = 3;
            this.MskTxtBx1_CoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MskTxtBx2_CoordY
            // 
            this.MskTxtBx2_CoordY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MskTxtBx2_CoordY.Location = new System.Drawing.Point(299, 91);
            this.MskTxtBx2_CoordY.Mask = "9999";
            this.MskTxtBx2_CoordY.Name = "MskTxtBx2_CoordY";
            this.MskTxtBx2_CoordY.Size = new System.Drawing.Size(100, 20);
            this.MskTxtBx2_CoordY.TabIndex = 4;
            this.MskTxtBx2_CoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Btn3_AbrirAplicacion
            // 
            this.Btn3_AbrirAplicacion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Btn3_AbrirAplicacion.Location = new System.Drawing.Point(311, 22);
            this.Btn3_AbrirAplicacion.Name = "Btn3_AbrirAplicacion";
            this.Btn3_AbrirAplicacion.Size = new System.Drawing.Size(75, 23);
            this.Btn3_AbrirAplicacion.TabIndex = 5;
            this.Btn3_AbrirAplicacion.Text = "Abrir SAP";
            this.Btn3_AbrirAplicacion.UseVisualStyleBackColor = true;
            this.Btn3_AbrirAplicacion.Click += new System.EventHandler(this.Btn3_AbrirAplicacion_Click);
            // 
            // Frm__CtrlSAP
            // 
            this.AcceptButton = this.Btn1_IniciarControl;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 341);
            this.Controls.Add(this.TblLyt1_ControlSAP);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(850, 600);
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "Frm__CtrlSAP";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "Control de SAP";
            this.TblLyt1_ControlSAP.ResumeLayout(false);
            this.TblLyt1_ControlSAP.PerformLayout();
            this.GrpBx1_Log.ResumeLayout(false);
            this.GrpBx1_Log.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn1_IniciarControl;
        private System.Windows.Forms.TableLayoutPanel TblLyt1_ControlSAP;
        private System.Windows.Forms.GroupBox GrpBx1_Log;
        private System.Windows.Forms.TextBox TxtBx1_Log;
        private System.Windows.Forms.Button Btn2_HacerClic;
        private System.Windows.Forms.MaskedTextBox MskTxtBx1_CoordX;
        private System.Windows.Forms.MaskedTextBox MskTxtBx2_CoordY;
        private System.Windows.Forms.Button Btn3_AbrirAplicacion;
    }
}

