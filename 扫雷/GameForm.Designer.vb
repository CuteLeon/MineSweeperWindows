<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GameForm))
        Me.BtnFinish = New System.Windows.Forms.Button()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.GameTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.EachLineCountsUD = New System.Windows.Forms.NumericUpDown()
        Me.Restart = New System.Windows.Forms.Button()
        Me.LineCountsUD = New System.Windows.Forms.NumericUpDown()
        Me.PicTime = New System.Windows.Forms.PictureBox()
        Me.GroupBox.SuspendLayout()
        CType(Me.EachLineCountsUD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LineCountsUD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnFinish
        '
        Me.BtnFinish.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.BtnFinish, "BtnFinish")
        Me.BtnFinish.Name = "BtnFinish"
        Me.BtnFinish.UseVisualStyleBackColor = False
        '
        'lblTime
        '
        resources.ApplyResources(Me.lblTime, "lblTime")
        Me.lblTime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblTime.Name = "lblTime"
        '
        'GameTimer
        '
        Me.GameTimer.Interval = 1000
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.Label2)
        Me.GroupBox.Controls.Add(Me.Label1)
        Me.GroupBox.Controls.Add(Me.EachLineCountsUD)
        Me.GroupBox.Controls.Add(Me.Restart)
        Me.GroupBox.Controls.Add(Me.LineCountsUD)
        Me.GroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        resources.ApplyResources(Me.GroupBox, "GroupBox")
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.TabStop = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'EachLineCountsUD
        '
        resources.ApplyResources(Me.EachLineCountsUD, "EachLineCountsUD")
        Me.EachLineCountsUD.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.EachLineCountsUD.Name = "EachLineCountsUD"
        Me.EachLineCountsUD.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Restart
        '
        resources.ApplyResources(Me.Restart, "Restart")
        Me.Restart.Name = "Restart"
        Me.Restart.UseVisualStyleBackColor = True
        '
        'LineCountsUD
        '
        resources.ApplyResources(Me.LineCountsUD, "LineCountsUD")
        Me.LineCountsUD.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.LineCountsUD.Name = "LineCountsUD"
        Me.LineCountsUD.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'PicTime
        '
        resources.ApplyResources(Me.PicTime, "PicTime")
        Me.PicTime.Name = "PicTime"
        Me.PicTime.TabStop = False
        '
        'GameForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox)
        Me.Controls.Add(Me.BtnFinish)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.PicTime)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "GameForm"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        CType(Me.EachLineCountsUD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LineCountsUD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnFinish As System.Windows.Forms.Button
    Friend WithEvents PicTime As System.Windows.Forms.PictureBox
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents GameTimer As System.Windows.Forms.Timer
    Friend WithEvents GroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents EachLineCountsUD As System.Windows.Forms.NumericUpDown
    Friend WithEvents Restart As System.Windows.Forms.Button
    Friend WithEvents LineCountsUD As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
