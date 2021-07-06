Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render
Imports ZXing
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Text

Class MainWindow
    Dim WithEvents BGWorkerDraw As BackgroundWorker
    Dim branchPath As DirectoryInfo = New DirectoryInfo(My.Application.Info.DirectoryPath & "\Branches")
    Dim strOutputPath, strBranch, strAddress, strCPNo As String
    Dim strQRIDList As New List(Of String)
    Dim noOfCards As Integer

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        BGWorkerDraw = New BackgroundWorker With {
            .WorkerReportsProgress = True
        }
    End Sub

    Public Function GenerateQRCode(ByVal value As String) As Image
        Dim bmp As Bitmap = Nothing
        Try
            Dim encoder As QrEncoder = New QrEncoder(ErrorCorrectionLevel.H)
            Dim qrCode As QrCode = encoder.Encode(value)

            Dim render = New GraphicsRenderer(New FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White)

            Using ms As MemoryStream = New MemoryStream
                render.WriteToStream(qrCode.Matrix, ImageFormat.Bmp, ms, New Point(10, 10))
                bmp = New Bitmap(ms)
                bmp = ResizeImage(bmp, 600, 600)
            End Using

            qrCode = Nothing
            encoder = Nothing

            Return bmp.Clone
        Catch ex As Exception
            Return Nothing
        Finally
            bmp.Dispose()
        End Try
    End Function

    Public Function ResizeImage(ByVal sourceImage As Image, ByVal width As Integer, ByVal height As Integer)
        Dim graphics As Graphics = Nothing
        Try
            Dim b As Bitmap = New Bitmap(width, height)
            graphics = Graphics.FromImage(b)

            graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphics.Clear(Color.Transparent)

            graphics.DrawImage(sourceImage, New Rectangle(0, 0, width, height),
                               New Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                               GraphicsUnit.Pixel)

            Return b
        Catch ex As Exception
            Return Nothing
        Finally
            If graphics IsNot Nothing Then graphics.Dispose()
        End Try
    End Function

    Private Sub NumberVailidationTextBox(ByVal sender As Object, ByVal e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("[^0-9.-]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        txtBranch.Text = ""
        txtAddress.Text = ""
        txtCPNumber.Text = "09675198479 | 0285718622"
        txtOutputPath.Text = ""
        txtCard.Text = 100

        lblBranch.Text = ""
        lblAddress.Text = ""

        gridStatus.Visibility = Visibility.Hidden

        If branchPath.Exists = False Then branchPath.Create()
    End Sub

    Private Sub txtBranch_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtBranch.TextChanged
        lblBranch.Text = txtBranch.Text
    End Sub

    Private Sub txtAddress_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtAddress.TextChanged
        lblAddress.Text = txtAddress.Text
    End Sub

    Private Sub txtCPNumber_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtCPNumber.TextChanged
        lblCPNumber.Text = txtCPNumber.Text
    End Sub

    Private Sub btnBrowseOutputPath_Click(sender As Object, e As RoutedEventArgs) Handles btnBrowseOutputPath.Click
        Dim fPath As New FolderBrowserDialog
        If fPath.ShowDialog = Forms.DialogResult.OK Then
            txtOutputPath.Text = fPath.SelectedPath
        End If
    End Sub

    Private Sub txtCard_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtCard.TextChanged
        If txtCard.Text = "" Then
            txtCard.Text = 1
        ElseIf txtCard.Text = 0 Then
            txtCard.Text = 1
        End If
    End Sub

    Private Sub btnSelectBranch_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectBranch.Click
        branchPath.Refresh()

        If branchPath.GetFiles.Count = 0 Then
            MessageBox.Show("Type the Details Manually, then Click Save button for future use.", "No Records Found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim OpenBranchFileDlg As OpenFileDialog = New OpenFileDialog With {
            .Filter = "Text Files (*.txt)|*.txt",
            .InitialDirectory = branchPath.FullName
        }
            If OpenBranchFileDlg.ShowDialog = Forms.DialogResult.OK Then
                Dim objStreamReader As StreamReader
                Dim strLine() As String

                objStreamReader = New StreamReader(OpenBranchFileDlg.FileName)

                strLine = objStreamReader.ReadToEnd.Split(vbNewLine)

                objStreamReader.Close()
                objStreamReader.Dispose()

                txtBranch.Text = OpenBranchFileDlg.SafeFileName.Replace(".txt", "")
                txtAddress.Text = Regex.Replace(strLine(0), "\n", String.Empty)
                txtCPNumber.Text = Regex.Replace(strLine(1), "\n", String.Empty)
                txtOutputPath.Text = Regex.Replace(strLine(2), "\n", String.Empty)
                txtCard.Text = Regex.Replace(strLine(3), "\n", String.Empty)
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If txtBranch.Text.Trim = "" Then
            MessageBox.Show("Branch Field is Required.", "Failed to Save.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim filePath As FileInfo = New FileInfo(branchPath.FullName & "\" & txtBranch.Text & ".txt")

            If filePath.Exists Then filePath.Delete()

            Using wr As New StreamWriter(filePath.FullName, True)
                wr.WriteLine(txtAddress.Text)
                wr.WriteLine(txtCPNumber.Text)
                wr.WriteLine(txtOutputPath.Text)
                wr.WriteLine(txtCard.Text)
            End Using

            MessageBox.Show("Branch Info is Saved Successfully.", "Saving Successful.", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As RoutedEventArgs) Handles btnGenerate.Click
        If txtBranch.Text.Trim = "" Then
            MessageBox.Show("Branch Field is Required.", "Failed to Generate.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        If txtAddress.Text.Trim = "" Then
            MessageBox.Show("Address Field is Required.", "Failed to Generate.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        If txtCPNumber.Text.Trim = "" Then
            MessageBox.Show("CP Number Field is Required.", "Failed to Generate.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        If txtOutputPath.Text.Trim = "" Then
            MessageBox.Show("Output Path Field is Required.", "Failed to Generate.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        If MessageBox.Show("Please do not Close the App until it finishes generating the Loyalty Card. Click OK to Start.", "Start Generating Loyalty Card.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Forms.DialogResult.OK Then
            Dim DirOutputPath As DirectoryInfo = New DirectoryInfo(txtOutputPath.Text)
            If Not DirOutputPath.Exists Then DirOutputPath.Create()

            strOutputPath = txtOutputPath.Text
            strBranch = txtBranch.Text
            strAddress = txtAddress.Text
            strCPNo = txtCPNumber.Text
            noOfCards = CInt(txtCard.Text)

            btnGenerate.IsEnabled = False
            btnSave.IsEnabled = False
            strQRIDList.Clear()

            gridStatus.Visibility = Visibility.Visible
            lblStatus.Text = "Generating 1 of " & noOfCards & " Loyalty Cards..."

            BGWorkerDraw.RunWorkerAsync()
        End If
    End Sub

    Private Sub BGWorkerDraw_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BGWorkerDraw.DoWork
        Dim strFormat As StringFormat = New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }

        'Get Selected Frame
        Dim ImgFrame As Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Front.png")

        For card As Integer = 0 To noOfCards - 1
            Try
                'Set Output Drawing Details
                Dim FinishedProduct As Bitmap = Nothing
                FinishedProduct = New Bitmap(ImgFrame.Width, ImgFrame.Height,
                                          Imaging.PixelFormat.Format32bppRgb)
                FinishedProduct.SetResolution(300, 300)

                'Get QR Code
                Dim r As New Random
                Dim qrID As String = GenerateQRID(r)

                strQRIDList.Add(qrID)

                Dim QR As Bitmap = Nothing
                QR = GenerateQRCode(qrID)

                'Draw Output
                Using myGraphics As Graphics = Graphics.FromImage(FinishedProduct)
                    myGraphics.FillRectangle(Brushes.White, New Rectangle(0, 0,
                    ImgFrame.Width, ImgFrame.Height))

                    'Draw Frame
                    myGraphics.DrawImage(ImgFrame, 0, 0, ImgFrame.Width, ImgFrame.Height)

                    myGraphics.DrawString(UCase(strBranch),
                                          New Font("Arial", 22,
                                                   System.Drawing.FontStyle.Bold),
                                          Brushes.Black, New RectangleF(550, 600, 585, 90),
                                          strFormat)

                    myGraphics.DrawString(strAddress,
                                          New Font("Arial", 10,
                                                   System.Drawing.FontStyle.Bold),
                                          Brushes.Black, New RectangleF(550, 700, 620, 180),
                                          strFormat)

                    myGraphics.DrawString(strCPNo,
                                          New Font("Arial", 11,
                                                   System.Drawing.FontStyle.Bold),
                                          Brushes.Black, New RectangleF(550, 890, 650, 50),
                                          strFormat)

                    myGraphics.DrawImage(QR, 1230, 270, 600, 600)
                End Using

                'Save Final Output
                Dim myImageCodecInfo As ImageCodecInfo
                Dim myEncoder As Imaging.Encoder
                Dim myEncoderParameter As EncoderParameter
                Dim myEncoderParameters As EncoderParameters

                myImageCodecInfo = GetEncoderInfo("image/jpeg")

                myEncoder = Imaging.Encoder.Quality
                myEncoderParameters = New EncoderParameters(1)

                myEncoderParameter = New EncoderParameter(myEncoder, 100L)
                myEncoderParameters.Param(0) = myEncoderParameter

                'Date Time Stamp as File Name
                Dim finishedProdFileName As String = DateTime.Now.ToString("yyMMddhhmmss")

                'Save to Output Path
                FinishedProduct.Clone.Save(strOutputPath & "\" & finishedProdFileName & ".JPG",
                             myImageCodecInfo, myEncoderParameters)

                If File.Exists(strOutputPath & "\Back.jpg") = False Then
                    File.Copy(My.Application.Info.DirectoryPath & "\Resources\Back.jpg",
                              strOutputPath & "\Back.jpg")
                End If

                'Dispose Frame Image
                myEncoderParameter.Dispose()
                myEncoderParameters.Dispose()

                Threading.Thread.Sleep(1000)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

            BGWorkerDraw.ReportProgress(card + 1)
        Next
    End Sub

    Private Sub BGWorkerDraw_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BGWorkerDraw.ProgressChanged
        lblStatus.Text = "Generating " & e.ProgressPercentage & " of " & noOfCards & " Loyalty Cards..."
    End Sub

    Private Sub BGWorkerDraw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BGWorkerDraw.RunWorkerCompleted
        Dim filePath As FileInfo = New FileInfo(strOutputPath & "\" &
                                                txtBranch.Text & " - " &
                                                Date.Now.ToString("yyyy-MM-dd HHmmss") & ".txt")

        If filePath.Exists Then filePath.Delete()

        Using wr As New StreamWriter(filePath.FullName, True)
            For Each item In strQRIDList
                wr.WriteLine(item.ToString)
            Next
        End Using

        MessageBox.Show("Successfully Generated " & txtCard.Text & " Loyalty Cards for " & txtBranch.Text, "Success!.", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btnGenerate.IsEnabled = True
        btnSave.IsEnabled = True
        gridStatus.Visibility = Visibility.Hidden
    End Sub

    Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        Dim j As Integer
        Dim encoders As ImageCodecInfo()
        encoders = ImageCodecInfo.GetImageEncoders()

        For j = 0 To encoders.Length - 1
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
        Next

        Return Nothing
    End Function

    Private Function GenerateQRID(ByVal r As Random) As String
        Dim result As String = Date.Now.Year().ToString & "-"

        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        For i As Integer = 0 To 1
            Dim idx As Integer = r.Next(0, s.Length)
            result &= s.Substring(idx, 1)
        Next

        result &= r.Next(0, 100000).ToString("D5")

        Return result
    End Function


End Class
