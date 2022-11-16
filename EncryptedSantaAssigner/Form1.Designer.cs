namespace EncryptedSantaAssigner
{
    partial class Form1
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
            this.importParticipantsButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.participantsListLabel = new System.Windows.Forms.Label();
            this.drawResultsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // importParticipantsButton
            // 
            this.importParticipantsButton.Location = new System.Drawing.Point(189, 12);
            this.importParticipantsButton.Name = "importParticipantsButton";
            this.importParticipantsButton.Size = new System.Drawing.Size(430, 42);
            this.importParticipantsButton.TabIndex = 0;
            this.importParticipantsButton.Text = "Import participants";
            this.importParticipantsButton.UseVisualStyleBackColor = true;
            this.importParticipantsButton.Click += new System.EventHandler(this.importParticipantsButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // participantsListLabel
            // 
            this.participantsListLabel.AutoSize = true;
            this.participantsListLabel.Location = new System.Drawing.Point(186, 69);
            this.participantsListLabel.Name = "participantsListLabel";
            this.participantsListLabel.Size = new System.Drawing.Size(65, 13);
            this.participantsListLabel.TabIndex = 1;
            this.participantsListLabel.Text = "Participants:";
            // 
            // drawResultsButton
            // 
            this.drawResultsButton.Location = new System.Drawing.Point(189, 138);
            this.drawResultsButton.Name = "drawResultsButton";
            this.drawResultsButton.Size = new System.Drawing.Size(430, 40);
            this.drawResultsButton.TabIndex = 2;
            this.drawResultsButton.Text = "Go!";
            this.drawResultsButton.UseVisualStyleBackColor = true;
            this.drawResultsButton.Click += new System.EventHandler(this.drawResultsButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 209);
            this.Controls.Add(this.drawResultsButton);
            this.Controls.Add(this.participantsListLabel);
            this.Controls.Add(this.importParticipantsButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button importParticipantsButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label participantsListLabel;
        private System.Windows.Forms.Button drawResultsButton;
    }
}

