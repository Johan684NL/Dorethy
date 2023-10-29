Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Interaction
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports Npgsql


Public Class DRT_Login
    Private Const V1 As String = "Host=localhost;Username=postgres;Password=""4Dor=Access!"";Database=postgres"
    'Private Const V11 As String = "select * from users"
    Private Const V As String = ""
    Private Const V12 As String = "20Dorethy21"

    Public Shared Property GetFromRFID As String
    Public Shared Property ReturnCodeRFID As String
    Public Shared Property UserName As String
    Public Shared Property Auth As String
    Public Shared Property ReturnTo As String

    Private Sub DRT_Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'BtnGetRFID.Visible = False

    End Sub

    Private Sub BtnGetRFID_Click(sender As Object, e As EventArgs) Handles BtnGetRFID.Click
        Actions()
    End Sub

    Private Sub Actions()
        If ReturnCodeRFID = "ok" And ReturnTo = "1" Then
            RFID_scanned.Text = GetFromRFID
            StrtRFIDr.Visible = True

            Dim Conn As NpgsqlConnection
            Dim Adpt As NpgsqlDataAdapter

            'Dim cmd As NpgsqlCommand

            Dim npgsqlConnection As New NpgsqlConnection
            Conn = npgsqlConnection
            Conn.ConnectionString = V1
            Conn.Open()

            Dim sql As String = "Select * from users where users.nfc = '" & RFID_scanned.Text & "'"

            Adpt = New NpgsqlDataAdapter(sql, Conn)
            Dim RS As New DataTable
            Try
                Adpt.Fill(RS)
                Dim ds As New DataSet("DataSetOne")
                ds.Tables.Add(RS)
                DataGridView1.DataSource = RS
                Conn.Close()
            Catch ex As Exception
                MsgBox("Some database error ocurred!",, "Dorethy message")
            End Try

            Conn.Close()

            If DataGridView1.Rows(0).Cells(1).Value <> "" Then
                UserName = DataGridView1.Rows(0).Cells(1).Value
                Auth = DataGridView1.Rows(0).Cells(3).Value

                Dim oForm As Menu
                oForm = New Menu()
                oForm.Show()

                Hide()
            End If
            StrtRFIDr.Visible = True
        End If
    End Sub



    Private Sub StrtRFIDr_Click(sender As Object, e As EventArgs) Handles StrtRFIDr.Click
        StrtRFIDr.Visible = False
        BtnGetRFID.Visible = True

        ReturnTo = "1"
        GetFromRFID = ""

        Dim oForm As FrmMain
        oForm = New FrmMain()
        oForm.Show()

        Hide()
    End Sub

    Private Sub AddNewUser_Click(sender As Object, e As EventArgs) Handles AddNewUser.Click
        RFID_scanned.Text = GetFromRFID
        Dim EnterYourPW As String
        EnterYourPW = InputBox("Dorethy password", "Dorethy payment solution")
        Try
            If EnterYourPW = V12 Then
                RFID_scanned.Text = EnterYourPW
                GetFromRFID = V
                Dim oForm As AddNewUser
                Dim form2 As New AddNewUser()
                oForm = form2
                oForm.Show()
                Hide()
                Return
            ElseIf RFID_scanned.Text <> EnterYourPW Then
                MsgBox("Your entered password in incorrect",, "Dorethy error message")
                Return
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        End
    End Sub
End Class