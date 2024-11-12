
namespace ProjEncontraPlaca
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictBoxImg = new System.Windows.Forms.PictureBox();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnOTSU = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSegmenta8 = new System.Windows.Forms.Button();
            this.btnReconheDigito = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pictBoxImg
            // 
            this.pictBoxImg.Location = new System.Drawing.Point(16, 15);
            this.pictBoxImg.Margin = new System.Windows.Forms.Padding(4);
            this.pictBoxImg.Name = "pictBoxImg";
            this.pictBoxImg.Size = new System.Drawing.Size(1061, 533);
            this.pictBoxImg.TabIndex = 0;
            this.pictBoxImg.TabStop = false;
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(16, 555);
            this.btnAbrir.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(123, 28);
            this.btnAbrir.TabIndex = 1;
            this.btnAbrir.Text = "Abrir Imagem";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(148, 554);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(4);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(100, 28);
            this.btnLimpar.TabIndex = 2;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnOTSU
            // 
            this.btnOTSU.Location = new System.Drawing.Point(312, 555);
            this.btnOTSU.Margin = new System.Windows.Forms.Padding(4);
            this.btnOTSU.Name = "btnOTSU";
            this.btnOTSU.Size = new System.Drawing.Size(100, 28);
            this.btnOTSU.TabIndex = 4;
            this.btnOTSU.Text = "Teste OTSU";
            this.btnOTSU.UseVisualStyleBackColor = true;
            this.btnOTSU.Click += new System.EventHandler(this.btnOTSU_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(420, 559);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 5;
            // 
            // btnSegmenta8
            // 
            this.btnSegmenta8.Location = new System.Drawing.Point(561, 556);
            this.btnSegmenta8.Margin = new System.Windows.Forms.Padding(4);
            this.btnSegmenta8.Name = "btnSegmenta8";
            this.btnSegmenta8.Size = new System.Drawing.Size(100, 28);
            this.btnSegmenta8.TabIndex = 6;
            this.btnSegmenta8.Text = "Segmenta 8";
            this.btnSegmenta8.UseVisualStyleBackColor = true;
            this.btnSegmenta8.Click += new System.EventHandler(this.btnSegmenta8_Click);
            // 
            // btnReconheDigito
            // 
            this.btnReconheDigito.Location = new System.Drawing.Point(668, 556);
            this.btnReconheDigito.Name = "btnReconheDigito";
            this.btnReconheDigito.Size = new System.Drawing.Size(186, 28);
            this.btnReconheDigito.TabIndex = 8;
            this.btnReconheDigito.Text = "Teste Reconhece Dígito";
            this.btnReconheDigito.UseVisualStyleBackColor = true;
            this.btnReconheDigito.Click += new System.EventHandler(this.btnReconheDigito_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 655);
            this.Controls.Add(this.btnReconheDigito);
            this.Controls.Add(this.btnSegmenta8);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnOTSU);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.btnAbrir);
            this.Controls.Add(this.pictBoxImg);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPrincipal";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBoxImg;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnOTSU;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSegmenta8;
        private System.Windows.Forms.Button btnReconheDigito;
    }
}

