namespace yfs_gui
{
    partial class FormInputKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInputKey));
            inputKey = new TextBox();
            buttonOK = new Button();
            labelInfo = new Label();
            SuspendLayout();
            // 
            // inputKey
            // 
            inputKey.BorderStyle = BorderStyle.FixedSingle;
            inputKey.Location = new Point(12, 72);
            inputKey.Name = "inputKey";
            inputKey.Size = new Size(246, 23);
            inputKey.TabIndex = 0;
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.FlatStyle = FlatStyle.Flat;
            buttonOK.Location = new Point(264, 72);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(81, 23);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "ОК";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Location = new Point(12, 9);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(331, 60);
            labelInfo.TabIndex = 2;
            labelInfo.Text = "Ключ от данного файла не найден в таблице сохранённых\r\nключей (.keytable).\r\n\r\n┌─ Если у вас есть ключ от этого файла, введите в это поле";
            // 
            // FormInputKey
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 107);
            Controls.Add(labelInfo);
            Controls.Add(buttonOK);
            Controls.Add(inputKey);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormInputKey";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public TextBox inputKey;
        public Button buttonOK;
        private Label labelInfo;
    }
}