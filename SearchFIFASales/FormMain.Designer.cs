﻿namespace SearchFIFASales
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.mobileMacroPanel1 = new Module.MobileMacro.MobileMacroPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboCard = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAuth = new System.Windows.Forms.Label();
            this.cboSeason = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtMultiply = new System.Windows.Forms.TextBox();
            this.txtSecondPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFirstPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCompareCard = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboLeague = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 410);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.mobileMacroPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(880, 384);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "선수영입매크로";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // mobileMacroPanel1
            // 
            this.mobileMacroPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mobileMacroPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mobileMacroPanel1.Location = new System.Drawing.Point(3, 3);
            this.mobileMacroPanel1.Name = "mobileMacroPanel1";
            this.mobileMacroPanel1.Size = new System.Drawing.Size(874, 378);
            this.mobileMacroPanel1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(880, 384);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "강장매물검색기";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(874, 378);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboCard);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.lbAuth);
            this.panel1.Controls.Add(this.cboSeason);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtMultiply);
            this.panel1.Controls.Add(this.txtSecondPrice);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtFirstPrice);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboCompareCard);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cboLeague);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 378);
            this.panel1.TabIndex = 0;
            // 
            // cboCard
            // 
            this.cboCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCard.FormattingEnabled = true;
            this.cboCard.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cboCard.Location = new System.Drawing.Point(71, 151);
            this.cboCard.Name = "cboCard";
            this.cboCard.Size = new System.Drawing.Size(158, 20);
            this.cboCard.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "기준카드";
            // 
            // lbAuth
            // 
            this.lbAuth.AutoSize = true;
            this.lbAuth.Location = new System.Drawing.Point(12, 229);
            this.lbAuth.Name = "lbAuth";
            this.lbAuth.Size = new System.Drawing.Size(0, 12);
            this.lbAuth.TabIndex = 9;
            // 
            // cboSeason
            // 
            this.cboSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeason.FormattingEnabled = true;
            this.cboSeason.Location = new System.Drawing.Point(71, 41);
            this.cboSeason.Name = "cboSeason";
            this.cboSeason.Size = new System.Drawing.Size(158, 20);
            this.cboSeason.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "시즌선택";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(154, 213);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtMultiply
            // 
            this.txtMultiply.Location = new System.Drawing.Point(71, 124);
            this.txtMultiply.Name = "txtMultiply";
            this.txtMultiply.Size = new System.Drawing.Size(158, 21);
            this.txtMultiply.TabIndex = 4;
            // 
            // txtSecondPrice
            // 
            this.txtSecondPrice.Location = new System.Drawing.Point(71, 97);
            this.txtSecondPrice.Name = "txtSecondPrice";
            this.txtSecondPrice.Size = new System.Drawing.Size(158, 21);
            this.txtSecondPrice.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "기준배율";
            // 
            // txtFirstPrice
            // 
            this.txtFirstPrice.Location = new System.Drawing.Point(71, 69);
            this.txtFirstPrice.Name = "txtFirstPrice";
            this.txtFirstPrice.Size = new System.Drawing.Size(158, 21);
            this.txtFirstPrice.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "EP최대값";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "EP최소값";
            // 
            // cboCompareCard
            // 
            this.cboCompareCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompareCard.FormattingEnabled = true;
            this.cboCompareCard.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6"});
            this.cboCompareCard.Location = new System.Drawing.Point(71, 179);
            this.cboCompareCard.Name = "cboCompareCard";
            this.cboCompareCard.Size = new System.Drawing.Size(158, 20);
            this.cboCompareCard.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "비교카드";
            // 
            // cboLeague
            // 
            this.cboLeague.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLeague.FormattingEnabled = true;
            this.cboLeague.Location = new System.Drawing.Point(71, 14);
            this.cboLeague.Name = "cboLeague";
            this.cboLeague.Size = new System.Drawing.Size(158, 20);
            this.cboLeague.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "리그선택";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(627, 378);
            this.panel2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(627, 378);
            this.dataGridView1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 410);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.Text = "강장 매물 검색기 (카카오톡 : searchfifa)";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboCard;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbAuth;
        private System.Windows.Forms.ComboBox cboSeason;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtMultiply;
        private System.Windows.Forms.TextBox txtSecondPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFirstPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCompareCard;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboLeague;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage3;
        private Module.MobileMacro.MobileMacroPanel mobileMacroPanel1;

    }
}

