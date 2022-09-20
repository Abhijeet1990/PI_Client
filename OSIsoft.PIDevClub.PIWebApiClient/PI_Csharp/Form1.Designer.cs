namespace PI_Csharp
{
    partial class Form1
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Variables");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("NUGreen");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.plantTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(250, 7);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Root";
            treeNode1.Text = "Variables";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(280, 373);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Location = new System.Drawing.Point(506, 44);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(538, 333);
            this.formsPlot1.TabIndex = 1;
            // 
            // plantTreeView
            // 
            this.plantTreeView.Location = new System.Drawing.Point(3, 7);
            this.plantTreeView.Name = "plantTreeView";
            treeNode2.Name = "NUGreen";
            treeNode2.Text = "NUGreen";
            this.plantTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.plantTreeView.Size = new System.Drawing.Size(241, 373);
            this.plantTreeView.TabIndex = 2;
            this.plantTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.plantTreeView_NodeMouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 392);
            this.Controls.Add(this.plantTreeView);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.formsPlot1);
            this.Name = "Form1";
            this.Text = "PI Live Data";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.TreeView plantTreeView;
    }
}

