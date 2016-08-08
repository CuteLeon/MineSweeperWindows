Public Class GameForm
    Private Const Mine As Single = 0.16     '地雷出现概率
    Dim MinefieldCounts As Integer                          '雷区内地雷总数
    Dim SquareCounts As Integer                                '雷区内方格总数常量
    Dim ButtonX As Long, ButtonY As Long             '控件的Left和Top
    Dim LineCounts As Integer = 15                          '控件铺设行数
    Dim EachLineCounts As Integer = 15                  '每行铺设控件数
    Dim SquareArray As New ArrayList                     '定义按钮控件数组，填充雷区
    Dim ButtonIndex As Integer, LineIndex As Integer                                  'Index
    Dim MinefieldMap() As Boolean              '雷区地图，记录每一个方格里是否埋雷
    Dim SquareState() As Integer                  '记录方格状态（0：未挖掘、1：已挖掘、2：有雷、3：可疑）
    Dim SquareLeftCount As Integer                          '还剩下未挖掘方格的总数
    Dim GameTime As Long = 0                                       '游戏计时

    Private Sub GameForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load   '程序启动
        SquareCounts = EachLineCounts * LineCounts                          '计算总数
        ReDim MinefieldMap(SquareCounts)     '初始化雷区地图
        ReDim SquareState(SquareCounts)        '初始化方格状态
        MinefieldCounts = LineCounts * EachLineCounts * Mine

        ButtonY = 90 '初始Top

        For Me.ButtonIndex = 1 To LineCounts                          '循环直到绘出所有需要的控件
            ButtonX = 20   '初始Left  
            For Me.LineIndex = 1 To EachLineCounts
                Dim AreaBtn As New Button                                                '定义新的控件

                With AreaBtn                                                                          'With结构设置控件属性
                    .Left = ButtonX             '控件最左边位置
                    .Top = ButtonY              '控件最上边位置
                    .ImageAlign = ContentAlignment.MiddleCenter         '控件图片加载位置：左右上下均居中
                    .ForeColor = Color.Red         '控件字体颜色：红色
                    .Font = New Font("宋体", 24, FontStyle.Bold) 'Font.Size是ReadOnly属性，不可直接修改，所以直接修改Font
                    .Width = 40                   '控件宽度
                    .Height = 40                  '控件高度
                    .Cursor = Cursors.Hand
                    .Tag = ((ButtonIndex - 1) * EachLineCounts) + LineIndex - 1 '给控件设置Index，通过”SquareArray.Item(CType(sender, Button).Tag).“调用
                    .FlatStyle = FlatStyle.Standard                                                '控件外观
                    .Image = My.Resources.PictureRes.未挖掘_32               '从Resx资源文件加载控件的初始图片
                End With

                ButtonX += 40 'ButtonX向右偏移一个控件宽度的值
                Me.Controls.Add(AreaBtn)            '设置属性结束，在窗体上绘出控件
                SquareArray.Add(AreaBtn)            '把同类作用的控件加入控件数组，方便后面统一处理

                '绑定控件事件到自定义Sub过程
                AddHandler AreaBtn.Click, AddressOf PlayerClick
                AddHandler AreaBtn.MouseUp, AddressOf ChangeSquareState
                AddHandler AreaBtn.MouseEnter, AddressOf BtnMouseEnter
                AddHandler AreaBtn.MouseLeave, AddressOf BtnMouseLeave
            Next
            '每行铺满，ButtonY向下偏移一个控件高度的值
            ButtonY += 40
        Next

        BtnFinish.Image = My.Resources.PictureRes.主页_64
        Restart.Image = My.Resources.PictureRes.主页_64

        Me.Width = SquareArray(SquareCounts - 1).Left + SquareArray(SquareCounts - 1).Width + 35
        Me.Height = SquareArray(SquareCounts - 1).Top + SquareArray(SquareCounts - 1).Height + 60
        Me.Left = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2
        Me.Top = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2
        BtnFinish.Left = (Me.Width - BtnFinish.Width) / 2 - 7.5
        BtnFinish.Top = (Me.Height - BtnFinish.Height) / 2 + 35
    End Sub

    Private Sub SetMinefield()   '铺设雷区
        Dim MyRand As New System.Random()
        Dim RandArray(MinefieldCounts) As Integer   '储存随机数的数组
        Dim RandTemp As Integer, CheckIndex As Integer '需要临时变量

        For Me.ButtonIndex = 1 To MinefieldCounts '循环MinefieldCounts次，埋MinefieldCounts个地雷
NewRand:    '设置标签，如果出现重复的数字需要回到这里重新产生
            RandTemp = MyRand.Next(SquareCounts - 1) '产生新的随机数
            For CheckIndex = 1 To Me.ButtonIndex - 1   '内层循环：向前遍历是否出现重复数字
                If RandTemp = RandArray(CheckIndex) Then GoTo NewRand '若重复，回到断点重新产生随机数
            Next
            RandArray(ButtonIndex) = RandTemp   '没有出现重复，则储存结果
            MinefieldMap(RandTemp) = True    '设置以随机数结果为标示的方格为”藏雷“

            '——————————作弊代码：——————————
            'SquareArray(RandArray(ButtonIndex)).text = "X"
        Next
    End Sub

    Private Sub ChangeSquareState(sender As Object, e As MouseEventArgs)
        If BtnFinish.Visible = True Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Right Then   '右键抬起
            Select Case SquareState(CType(sender, Button).Tag)
                Case 0
                    SquareState(CType(sender, Button).Tag) = 2
                    SquareArray.Item(CType(sender, Button).Tag).image = My.Resources.PictureRes.有雷_32
                Case 1
                    Exit Sub
                Case 2
                    SquareState(CType(sender, Button).Tag) = 3
                    SquareArray.Item(CType(sender, Button).Tag).image = My.Resources.PictureRes.可疑_32
                Case 3
                    SquareState(CType(sender, Button).Tag) = 0
                    SquareArray.Item(CType(sender, Button).Tag).image = My.Resources.PictureRes.未挖掘_32
            End Select
        End If
    End Sub

    Private Sub PlayerClick(sender As Object, e As EventArgs)
        If BtnFinish.Visible = True Then Exit Sub
        If SquareState(CType(sender, Button).Tag) > 0 Then Exit Sub

        If MinefieldMap(CType(sender, Button).Tag) Then   '有雷
            SquareArray.Item(CType(sender, Button).Tag).image = My.Resources.PictureRes.有雷_32      '改变控件图片为：有雷_32x32
            BtnFinish.Image = My.Resources.PictureRes.失败_128
            GameTimer.Enabled = False
            BtnFinish.Text = "你踩到地雷了！" & vbCrLf & "(单击重新开始)"

            Dim Center As Integer = CType(sender, Button).Tag
            Dim CenterLine As Integer = Int(Center / EachLineCounts)
            SquareArray(Center).BackColor = BtnFinish.BackColor
            'If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts - 1).BackColor = BtnFinish.BackColor
            'If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts).BackColor = BtnFinish.BackColor
            'If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts + 1).BackColor = BtnFinish.BackColor
            'If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then SquareArray(Center - 1).BackColor = BtnFinish.BackColor
            'If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then SquareArray(Center + 1).BackColor = BtnFinish.BackColor
            'If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts - 1).BackColor = BtnFinish.BackColor
            'If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts).BackColor = BtnFinish.BackColor
            'If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts + 1).BackColor = BtnFinish.BackColor

            BtnFinish.Visible = True
        Else  '无雷
            CleanOne(CType(sender, Button).Tag)
        End If
    End Sub

    Private Sub CleanOne(ByVal SquareIndex As Integer)
        SquareState(SquareIndex) = 1  '改变方格状态
        SquareArray.Item(SquareIndex).Image = Nothing   '清除图片
        Call Me.GetMineCountAround(SquareIndex)   '显示周围地雷数量
        SquareLeftCount -= 1
        If SquareLeftCount = MinefieldCounts Then
            BtnFinish.Image = My.Resources.PictureRes.胜利_128
            GameTimer.Enabled = False
            BtnFinish.Text = "恭喜你过关！" & vbCrLf & "（单击重新开始）"
            Dim Center As Integer = SquareIndex
            Dim CenterLine As Integer = Int(Center / EachLineCounts)
            SquareArray(Center).BackColor = BtnFinish.BackColor
            If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts - 1).BackColor = BtnFinish.BackColor
            If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts).BackColor = BtnFinish.BackColor
            If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts + 1).BackColor = BtnFinish.BackColor
            If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then SquareArray(Center - 1).BackColor = BtnFinish.BackColor
            If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then SquareArray(Center + 1).BackColor = BtnFinish.BackColor
            If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts - 1).BackColor = BtnFinish.BackColor
            If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts).BackColor = BtnFinish.BackColor
            If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts + 1).BackColor = BtnFinish.BackColor

            BtnFinish.Visible = True
        End If
    End Sub

    Private Sub StartNewGame()     '初始化控件和数组，重新开始游戏
        SquareCounts = EachLineCounts * LineCounts                          '计算总数
        ReDim MinefieldMap(SquareCounts)     '初始化雷区地图
        ReDim SquareState(SquareCounts)        '初始化方格状态
        SquareLeftCount = SquareCounts '剩下未挖掘的初始为方格总数
        For Me.ButtonIndex = 1 To SquareCounts
            With SquareArray.Item(Me.ButtonIndex - 1)
                .Image = My.Resources.PictureRes.未挖掘_32  '初始化雷区方格图片
                .Text = ""
            End With
        Next

        SetMinefield()   '重新铺设雷区
    End Sub

    Private Function GetMineCountAround(ByVal Center As Integer) As String    '计算Center周围的地雷个数
        Dim Result As Integer = 0, CenterLine As Integer = Int(Center / EachLineCounts)
        If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then If MinefieldMap(Center - EachLineCounts - 1) Then Result += 1
        If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then If MinefieldMap(Center - EachLineCounts) Then Result += 1
        If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then If MinefieldMap(Center - EachLineCounts + 1) Then Result += 1
        If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then If MinefieldMap(Center - 1) Then Result += 1
        If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then If MinefieldMap(Center + 1) Then Result += 1
        If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then If MinefieldMap(Center + EachLineCounts - 1) Then Result += 1
        If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then If MinefieldMap(Center + EachLineCounts) Then Result += 1
        If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then If MinefieldMap(Center + EachLineCounts + 1) Then Result += 1

        If Result = 0 Then     '若周围无雷就自动清空附近区域
            ClearMineAround(Center)
            SquareArray.Item(Center).Text = ""
            Return ""
        Else   '否则显示该区域附近的地雷数量
            SquareArray.Item(Center).Text = Result
            Return Result
        End If
    End Function

    Private Sub ClearMineAround(ByVal Center As Integer)     '自动清扫附近无雷的区域
        Dim CenterLine As Integer = Int(Center / EachLineCounts)
        If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then If SquareState(Center - EachLineCounts - 1) <> 1 Then CleanOne(Center - EachLineCounts - 1)
        If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then If SquareState(Center - EachLineCounts) <> 1 Then CleanOne(Center - EachLineCounts)
        If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then If SquareState(Center - EachLineCounts + 1) <> 1 Then CleanOne(Center - EachLineCounts + 1)
        If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then If SquareState(Center - 1) <> 1 Then CleanOne(Center - 1)
        If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then If SquareState(Center + 1) <> 1 Then CleanOne(Center + 1)
        If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then If SquareState(Center + EachLineCounts - 1) <> 1 Then CleanOne(Center + EachLineCounts - 1)
        If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then If SquareState(Center + EachLineCounts) <> 1 Then CleanOne(Center + EachLineCounts)
        If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then If SquareState(Center + EachLineCounts + 1) <> 1 Then CleanOne(Center + EachLineCounts + 1)
    End Sub

    Private Sub BtnFinish_Click(sender As Object, e As EventArgs) Handles BtnFinish.Click
        StartNewGame()  '开始新游戏
        BtnFinish.Visible = False
        lblTime.Text = "00:00"
        GameTime = 0
        GameTimer.Enabled = True
    End Sub

    Private Sub GameTimer_Tick(sender As Object, e As EventArgs) Handles GameTimer.Tick
        GameTime += 1
        lblTime.Text = Format(Int(GameTime / 60), "00") & ":" & Format((GameTime Mod 60), "00")
    End Sub

    Private Sub Restart_Click_1(sender As Object, e As EventArgs) Handles Restart.Click
        If MsgBox("确定要重新开始游戏吗？", MsgBoxStyle.YesNo + MsgBoxStyle.ApplicationModal + MsgBoxStyle.Question, "确定重置游戏？") = MsgBoxResult.No Then Exit Sub
        Me.Text = "正在重设界面，请稍后..."
        If Not (LineCounts = LineCountsUD.Value And EachLineCounts = EachLineCountsUD.Value) Then
            SquareCounts = EachLineCountsUD.Value * LineCountsUD.Value                           '期望值
            ReDim MinefieldMap(SquareCounts)     '初始化雷区地图
            ReDim SquareState(SquareCounts)        '初始化方格状态

            '修改雷区大小
            If LineCounts * EachLineCounts > SquareCounts Then RemoveMine() Else AddMine()

            LineCounts = LineCountsUD.Value : EachLineCounts = EachLineCountsUD.Value
            MinefieldCounts = LineCounts * EachLineCounts * Mine
            ButtonY = IIf(LineCountsUD.Value < 9, (480 - 90 - 40 * LineCounts) / 2 + 60, 90) '初始Top
            For Me.ButtonIndex = 1 To LineCounts                          '循环直到绘出所有需要的控件
                '使雷区居中显示
                ButtonX = IIf(EachLineCountsUD.Value < 16, (658 - 40 * EachLineCounts) / 2, 20)

                For Me.LineIndex = 1 To EachLineCounts
                    With SquareArray(((ButtonIndex - 1) * EachLineCounts) + LineIndex - 1)
                        .Left = ButtonX             '控件最左边位置
                        .Top = ButtonY              '控件最上边位置
                    End With

                    ButtonX += 40 'ButtonX向右偏移一个控件宽度的值
                Next
                '每行铺满，ButtonY向下偏移一个控件高度的值
                ButtonY += 40
            Next

            Me.Width = IIf(EachLineCountsUD.Value > 15, SquareArray(SquareCounts - 1).Left + SquareArray(SquareCounts - 1).Width + 35, 658)
            Me.Height = IIf(LineCountsUD.Value > 8, SquareArray(SquareCounts - 1).Top + SquareArray(SquareCounts - 1).Height + 60, 480)
            Me.Left = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2
            Me.Top = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2
            BtnFinish.Left = (Me.Width - BtnFinish.Width) / 2 - 7.5
            BtnFinish.Top = (Me.Height - BtnFinish.Height) / 2 + 35
        End If

        BtnFinish.Image = My.Resources.PictureRes.主页_64
        GameTimer.Enabled = False
        BtnFinish.Text = "  单击开始游戏"
        BtnFinish.Visible = True
        StartNewGame()
        lblTime.Text = "00:00"
        Me.Text = "扫雷：(左击挖掘，右击标记)"
    End Sub

    Private Sub AddMine()    '修改雷区大小  新增方格
        For Me.ButtonIndex = SquareArray.Count To SquareCounts - 1 '循环直到绘出所有需要的控件
            Dim AreaBtn As New Button                                                '定义新的控件

            With AreaBtn                                                                          'With结构设置控件属性
                .ImageAlign = ContentAlignment.MiddleCenter         '控件图片加载位置：左右上下均居中
                .ForeColor = Color.Red         '控件字体颜色：红色
                .Font = New Font("宋体", 24, FontStyle.Bold) 'Font.Size是ReadOnly属性，不可直接修改，所以直接修改Font
                .Width = 40                   '控件宽度
                .Height = 40                  '控件高度
                .Tag = ButtonIndex  '给控件设置Index
                .Image = My.Resources.PictureRes.未挖掘_32               '从Resx资源文件加载控件的初始图片
            End With

            Me.Controls.Add(AreaBtn)            '设置属性结束，在窗体上绘出控件
            SquareArray.Add(AreaBtn)            '把同类作用的控件加入控件数组，方便后面统一处理

            '绑定控件事件到自定义Sub过程
            AddHandler AreaBtn.Click, AddressOf PlayerClick
            AddHandler AreaBtn.MouseUp, AddressOf ChangeSquareState
            AddHandler AreaBtn.MouseEnter, AddressOf BtnMouseEnter
            AddHandler AreaBtn.MouseLeave, AddressOf BtnMouseLeave
        Next
    End Sub

    Private Sub RemoveMine()            '修改雷区大小  移除方格
        For Me.ButtonIndex = SquareCounts To SquareArray.Count - 1
            Me.Controls.Remove(SquareArray(ButtonIndex))
        Next
        SquareArray.RemoveRange(SquareCounts, SquareArray.Count - SquareCounts)
    End Sub

    '周围方格高亮：鼠标移入 加黑周围方格
    Private Sub BtnMouseEnter(sender As Object, e As EventArgs)
        If BtnFinish.Visible = True Then Exit Sub

        Dim Center As Integer = CType(sender, Button).Tag
        Dim CenterLine As Integer = Int(Center / EachLineCounts)
        SquareArray(Center).BackColor = Color.Gray
        If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts - 1).BackColor = Color.Gray
        If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts).BackColor = Color.Gray
        If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts + 1).BackColor = Color.Gray
        If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then SquareArray(Center - 1).BackColor = Color.Gray
        If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then SquareArray(Center + 1).BackColor = Color.Gray
        If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts - 1).BackColor = Color.Gray
        If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts).BackColor = Color.Gray
        If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts + 1).BackColor = Color.Gray
    End Sub

    '周围方格高亮：鼠标移出 恢复周围方格
    Private Sub BtnMouseLeave(sender As Object, e As EventArgs)
        If BtnFinish.Visible = True Then Exit Sub

        Dim Center As Integer = CType(sender, Button).Tag
        Dim CenterLine As Integer = Int(Center / EachLineCounts)
        SquareArray(Center).BackColor = BtnFinish.BackColor
        If (Center - EachLineCounts - 1 >= 0 And Int((Center - EachLineCounts - 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts - 1).BackColor = BtnFinish.BackColor
        If (Center - EachLineCounts >= 0 And Int((Center - EachLineCounts) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts).BackColor = BtnFinish.BackColor
        If (Center - EachLineCounts + 1 >= 0 And Int((Center - EachLineCounts + 1) / EachLineCounts) = CenterLine - 1) Then SquareArray(Center - EachLineCounts + 1).BackColor = BtnFinish.BackColor
        If (Center - 1 >= 0 And Int((Center - 1) / EachLineCounts) = CenterLine) Then SquareArray(Center - 1).BackColor = BtnFinish.BackColor
        If (Center + 1 < SquareCounts And Int((Center + 1) / EachLineCounts) = CenterLine) Then SquareArray(Center + 1).BackColor = BtnFinish.BackColor
        If (Center + EachLineCounts - 1 < SquareCounts And Int((Center + EachLineCounts - 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts - 1).BackColor = BtnFinish.BackColor
        If (Center + EachLineCounts < SquareCounts And Int((Center + EachLineCounts) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts).BackColor = BtnFinish.BackColor
        If (Center + EachLineCounts + 1 < SquareCounts And Int((Center + EachLineCounts + 1) / EachLineCounts) = CenterLine + 1) Then SquareArray(Center + EachLineCounts + 1).BackColor = BtnFinish.BackColor
    End Sub
End Class