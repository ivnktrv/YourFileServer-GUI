namespace yfs_gui
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonConnect = new Button();
            buttonDownload = new Button();
            buttonUpload = new Button();
            buttonDelete = new Button();
            inputIP = new TextBox();
            inputPort = new TextBox();
            listFiles = new ListBox();
            windowFileInfo = new RichTextBox();
            inputUser = new TextBox();
            inputPass = new TextBox();
            buttonDisconnect = new Button();
            selectSavePath = new FolderBrowserDialog();
            selectUploadFile = new OpenFileDialog();
            progressBar = new ProgressBar();
            labelEncryptingFile = new Label();
            SuspendLayout();
            // 
            // buttonConnect
            // 
            buttonConnect.Font = new Font("Segoe UI", 7F);
            buttonConnect.Location = new Point(12, 135);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(100, 59);
            buttonConnect.TabIndex = 0;
            buttonConnect.Text = "ПОДКЛЮЧИТЬСЯ";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // buttonDownload
            // 
            buttonDownload.Enabled = false;
            buttonDownload.Location = new Point(619, 200);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(169, 55);
            buttonDownload.TabIndex = 1;
            buttonDownload.Text = "СКАЧАТЬ";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // buttonUpload
            // 
            buttonUpload.Enabled = false;
            buttonUpload.Location = new Point(619, 261);
            buttonUpload.Name = "buttonUpload";
            buttonUpload.Size = new Size(169, 55);
            buttonUpload.TabIndex = 2;
            buttonUpload.Text = "ЗАГРУЗИТЬ";
            buttonUpload.UseVisualStyleBackColor = true;
            buttonUpload.Click += buttonUpload_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Enabled = false;
            buttonDelete.Location = new Point(619, 322);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(169, 53);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "УДАЛИТЬ";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // inputIP
            // 
            inputIP.Location = new Point(12, 12);
            inputIP.Name = "inputIP";
            inputIP.Size = new Size(100, 23);
            inputIP.TabIndex = 4;
            inputIP.Text = "Введите IP";
            inputIP.Click += inputIP_Click;
            // 
            // inputPort
            // 
            inputPort.Location = new Point(12, 41);
            inputPort.Name = "inputPort";
            inputPort.Size = new Size(100, 23);
            inputPort.TabIndex = 5;
            inputPort.Text = "Введите порт";
            inputPort.Click += inputPort_Click;
            // 
            // listFiles
            // 
            listFiles.BorderStyle = BorderStyle.None;
            listFiles.Font = new Font("Cascadia Mono", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            listFiles.FormattingEnabled = true;
            listFiles.ItemHeight = 17;
            listFiles.Location = new Point(118, 12);
            listFiles.Name = "listFiles";
            listFiles.Size = new Size(495, 323);
            listFiles.TabIndex = 6;
            listFiles.SelectedIndexChanged += listFiles_SelectedIndexChanged;
            // 
            // windowFileInfo
            // 
            windowFileInfo.BorderStyle = BorderStyle.None;
            windowFileInfo.Enabled = false;
            windowFileInfo.Location = new Point(619, 12);
            windowFileInfo.Name = "windowFileInfo";
            windowFileInfo.ReadOnly = true;
            windowFileInfo.Size = new Size(169, 182);
            windowFileInfo.TabIndex = 7;
            windowFileInfo.Text = "";
            // 
            // inputUser
            // 
            inputUser.Location = new Point(12, 70);
            inputUser.Name = "inputUser";
            inputUser.Size = new Size(100, 23);
            inputUser.TabIndex = 8;
            inputUser.Text = "Логин";
            inputUser.Click += inputUser_Click;
            // 
            // inputPass
            // 
            inputPass.Location = new Point(12, 99);
            inputPass.Name = "inputPass";
            inputPass.Size = new Size(100, 23);
            inputPass.TabIndex = 9;
            inputPass.Text = "Пароль";
            inputPass.Click += inputPass_Click;
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Enabled = false;
            buttonDisconnect.Font = new Font("Segoe UI", 7F);
            buttonDisconnect.Location = new Point(12, 200);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(100, 59);
            buttonDisconnect.TabIndex = 10;
            buttonDisconnect.Text = "ОТКЛЮЧИТЬСЯ";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(118, 342);
            progressBar.MarqueeAnimationSpeed = 10;
            progressBar.Maximum = 400;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(495, 33);
            progressBar.Step = 1;
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 11;
            // 
            // labelEncryptingFile
            // 
            labelEncryptingFile.AutoSize = true;
            labelEncryptingFile.BackColor = SystemColors.ControlLight;
            labelEncryptingFile.Location = new Point(288, 351);
            labelEncryptingFile.Name = "labelEncryptingFile";
            labelEncryptingFile.Size = new Size(158, 15);
            labelEncryptingFile.TabIndex = 12;
            labelEncryptingFile.Text = "Файл расшифровывается...";
            labelEncryptingFile.Visible = false;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 387);
            Controls.Add(labelEncryptingFile);
            Controls.Add(progressBar);
            Controls.Add(buttonDisconnect);
            Controls.Add(inputPass);
            Controls.Add(inputUser);
            Controls.Add(windowFileInfo);
            Controls.Add(listFiles);
            Controls.Add(inputPort);
            Controls.Add(inputIP);
            Controls.Add(buttonDelete);
            Controls.Add(buttonUpload);
            Controls.Add(buttonDownload);
            Controls.Add(buttonConnect);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMain";
            Text = "YourFileServer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonConnect;
        private Button buttonDownload;
        private Button buttonUpload;
        private Button buttonDelete;
        private TextBox inputIP;
        private TextBox inputPort;
        private ListBox listFiles;
        private RichTextBox windowFileInfo;
        private TextBox inputUser;
        private TextBox inputPass;
        private Button buttonDisconnect;
        private FolderBrowserDialog selectSavePath;
        private OpenFileDialog selectUploadFile;
        private ProgressBar progressBar;
        private Label labelEncryptingFile;
    }
}
