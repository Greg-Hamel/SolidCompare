namespace SolidCompare
{
    partial class Launcher
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelAssembly1 = new System.Windows.Forms.Label();
            this.LabelAssembly2 = new System.Windows.Forms.Label();
            this.FieldAssemblyDirectory1 = new System.Windows.Forms.TextBox();
            this.FieldAssemblyDirectory2 = new System.Windows.Forms.TextBox();
            this.CompareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelAssembly1
            // 
            this.LabelAssembly1.AutoSize = true;
            this.LabelAssembly1.Location = new System.Drawing.Point(36, 54);
            this.LabelAssembly1.Name = "LabelAssembly1";
            this.LabelAssembly1.Size = new System.Drawing.Size(67, 13);
            this.LabelAssembly1.TabIndex = 0;
            this.LabelAssembly1.Text = "Assembly #1";
            // 
            // LabelAssembly2
            // 
            this.LabelAssembly2.AutoSize = true;
            this.LabelAssembly2.Location = new System.Drawing.Point(36, 83);
            this.LabelAssembly2.Name = "LabelAssembly2";
            this.LabelAssembly2.Size = new System.Drawing.Size(67, 13);
            this.LabelAssembly2.TabIndex = 1;
            this.LabelAssembly2.Text = "Assembly #2";
            // 
            // FieldAssemblyDirectory1
            // 
            this.FieldAssemblyDirectory1.Location = new System.Drawing.Point(109, 51);
            this.FieldAssemblyDirectory1.Name = "FieldAssemblyDirectory1";
            this.FieldAssemblyDirectory1.Size = new System.Drawing.Size(264, 20);
            this.FieldAssemblyDirectory1.TabIndex = 2;
            // 
            // FieldAssemblyDirectory2
            // 
            this.FieldAssemblyDirectory2.Location = new System.Drawing.Point(109, 80);
            this.FieldAssemblyDirectory2.Name = "FieldAssemblyDirectory2";
            this.FieldAssemblyDirectory2.Size = new System.Drawing.Size(264, 20);
            this.FieldAssemblyDirectory2.TabIndex = 3;
            // 
            // CompareButton
            // 
            this.CompareButton.Location = new System.Drawing.Point(174, 215);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(75, 23);
            this.CompareButton.TabIndex = 4;
            this.CompareButton.Text = "Compare";
            this.CompareButton.UseVisualStyleBackColor = true;
            this.CompareButton.Click += new System.EventHandler(this.CompareButton_Click);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 262);
            this.Controls.Add(this.CompareButton);
            this.Controls.Add(this.FieldAssemblyDirectory2);
            this.Controls.Add(this.FieldAssemblyDirectory1);
            this.Controls.Add(this.LabelAssembly2);
            this.Controls.Add(this.LabelAssembly1);
            this.Name = "Launcher";
            this.Text = "SolidCompare";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelAssembly1;
        private System.Windows.Forms.Label LabelAssembly2;
        private System.Windows.Forms.TextBox FieldAssemblyDirectory1;
        private System.Windows.Forms.TextBox FieldAssemblyDirectory2;
        private System.Windows.Forms.Button CompareButton;
    }
}

