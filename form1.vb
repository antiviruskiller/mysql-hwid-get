Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Management
Imports System.Security

Public Class Form1
    Dim login As Integer
    Dim confirm As Integer
    Dim confirmhwid As Integer
    Dim version As Integer
    Dim appPath As String = My.Application.Info.DirectoryPath
    Dim exePath As String = Application.StartupPath
    Dim WebClient As New WebClient()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckHWID()
        ' Generate the HWID
        Dim hw As New clsComputerInfo
        Dim cpu As String
        Dim mb As String
        Dim mac As String
        Dim hwid As String
        cpu = hw.GetProcessorId()
        mb = hw.GetMotherboardID()
        mac = hw.GetMACAddress()
        hwid = cpu + mb + mac
        Dim hwidEncrypted As String = Strings.UCase(hw.getMD5Hash(cpu & mb & mac))
        TextBox1.Text = hwidEncrypted
    End Sub

    Private Sub CheckHWID()
        Dim hwid As String = TextBox1.Text
        Dim url As String = "https://haxcore.net/hwid/check_hwid.php?hwid=" & hwid

        Try
            Dim response As String = WebClient.DownloadString(url)
            If response = "banned" Then
                MessageBox.Show("HWID is banned.")
            ElseIf response = "not_banned" Then
                MessageBox.Show("HWID is not banned.")
            Else
                MessageBox.Show("Error: " & response)
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
    Private Class clsComputerInfo
        'Get processor ID
        Friend Function GetProcessorId() As String
            Dim strProcessorID As String = String.Empty
            Dim query As New SelectQuery("Win32_processor")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strProcessorID = info("processorID").ToString()
            Next
            Return strProcessorID
        End Function
        ' Get MAC Address
        Friend Function GetMACAddress() As String
            Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim moc As ManagementObjectCollection = mc.GetInstances()
            Dim MacAddress As String = String.Empty
            For Each mo As ManagementObject In moc
                If (MacAddress.Equals(String.Empty)) Then
                    If CBool(mo("IPEnabled")) Then MacAddress = mo("MacAddress").ToString()
                    mo.Dispose()
                End If
                MacAddress = MacAddress.Replace(":", String.Empty)
            Next
            Return MacAddress
        End Function
        ' Get Motherboard ID
        Friend Function GetMotherboardID() As String
            Dim strMotherboardID As String = String.Empty
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strMotherboardID = info("product").ToString()
            Next
            Return strMotherboardID
        End Function
        ' Encrypt HWID
        Friend Function getMD5Hash(ByVal strToHash As String) As String
            Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)
            bytesToHash = md5Obj.ComputeHash(bytesToHash)
            Dim strResult As String = ""
            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next
            Return strResult
        End Function
    End Class

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim hwid As String = TextBox1.Text
        Dim url As String = "https://haxcore.net/hwid/insert_hwid.php"
        Dim postData As String = "hwid=" & hwid

        Using client As New WebClient()
            client.Headers(HttpRequestHeader.ContentType) = "application/x-www-form-urlencoded"
            Dim response As String = client.UploadString(url, postData)
            MessageBox.Show(response)

        End Using
        Console.WriteLine("postData: " & postData)
    End Sub

End Class
