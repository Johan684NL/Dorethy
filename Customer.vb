Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Interaction
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports Npgsql


Public Class Customer
    Private Const V1 As String = "Host=localhost;Username=postgres;Password=""4Dor=Access!"";Database=postgres"
    Private Const V11 As String = "Select * from customer, cmb"
    'Private Const V As String = ""
    Dim V12 As String

    Private Sub BtnScanCrd_Click(sender As Object, e As EventArgs) Handles BtnScanCrd.Click
        BtnGetCardData.Visible = True
        BtnScanCrd.Visible = False

        DRT_Login.ReturnTo = "3"
        DRT_Login.GetFromRFID = ""

        Dim oForm As FrmMain
        oForm = New FrmMain()
        oForm.Show()

        Hide()
    End Sub

    Private Sub Customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LockFields()
    End Sub

    Private Sub BtnGetCardData_Click(sender As Object, e As EventArgs) Handles BtnGetCardData.Click
        NFC2DB.Text = DRT_Login.GetFromRFID
        BtnGetCardData.Visible = False
        BtnScanCrd.Visible = False
        OpenFields()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If V12 = "EDT" Then
            AddNU2DB()
        ElseIf V12 <> "EDT" Then
            Dim Conn As NpgsqlConnection
            Dim Adpt As NpgsqlDataAdapter

            'Dim cmd As NpgsqlCommand

            Dim npgsqlConnection As New NpgsqlConnection
            Conn = npgsqlConnection
            Conn.ConnectionString = V1
            Conn.Open()

            Dim sql As String = "Select * from customer where customer.clocation = '" & CLOC2DB.Text & "'"

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

            If DataGridView1.Rows(0).Cells(1).Value = "" Then
                AddNU2DB()
            ElseIf DataGridView1.Rows(0).Cells(1).Value <> "" Then
                MsgBox("Campinglocation is in use, correct the error",, "Dorethy message")
                DRT_Login.ReturnCodeRFID = "exist"
                V12 = "EDT"
                BtnEdit.Visible = True
            End If

        End If
    End Sub

    Private Sub AddNU2DB()
        If Name2DB.Text <> "" And Address2DB.Text <> "" And PostalC2DB.Text <> "" And City2DB.Text <> "" And PHONE2DB.Text <> "" And NFC2DB.Text <> "" And CLOC2DB.Text <> "" Then
            Dim Conn As NpgsqlConnection
            Dim Adpt As NpgsqlDataAdapter
            Dim cmd As NpgsqlCommand

            Conn = New NpgsqlConnection With {
                .ConnectionString = V1
            }
            Conn.Open()

            Dim sql As String = V11
            Dim Cloc As Integer = Convert.ToInt32(CLOC2DB.Text)
            Dim LBL As Integer = Convert.ToInt32(LBLeft.Text)

            Adpt = New NpgsqlDataAdapter(sql, Conn)

            If V12 <> "EDT" Then
                sql = ("insert into public.customer (name, address, postalcode, city, phone, nfc, clocation, active) values (" & Chr(39) & Name2DB.Text & Chr(39) & ", " & Chr(39) & Address2DB.Text & Chr(39) & "," & Chr(39) &
                   PostalC2DB.Text & Chr(39) & ", " & Chr(39) & City2DB.Text & Chr(39) & "," & Chr(39) &
                   PHONE2DB.Text & Chr(39) & ", " & Chr(39) & NFC2DB.Text & Chr(39) & "," & Chr(39) & Cloc & Chr(39) & "," & Chr(39) & LBL & Chr(39) & ")")
            ElseIf V12 = "EDT" Then
                sql = "update public.customer set name = " & Chr(39) & DataGridView1.Rows(0).Cells(1).Value & Chr(39) & ", address = " & Chr(39) & DataGridView1.Rows(0).Cells(2).Value & Chr(39) & ", postalcode = " & Chr(39) & DataGridView1.Rows(0).Cells(3).Value & Chr(39) &
                   ", city = " & Chr(39) & DataGridView1.Rows(0).Cells(4).Value & Chr(39) & ", phone = " & Chr(39) & DataGridView1.Rows(0).Cells(5).Value & Chr(39) & ", nfc = " & Chr(39) & DataGridView1.Rows(0).Cells(6).Value & Chr(39) & ", clocation = " & Chr(39) & DataGridView1.Rows(0).Cells(7).Value & Chr(39) &
                   ", active = " & Chr(39) & DataGridView1.Rows(0).Cells(8).Value & Chr(39) & " where id=" & Chr(39) & DataGridView1.Rows(0).Cells(0).Value & Chr(39) & " "

            End If


            cmd = New NpgsqlCommand(sql, Conn)
            cmd.ExecuteNonQuery()

            Conn.Close()
            ID_FS.ForeColor.R.ToString()
            ID_FS.Text = "Key successfuly saved"
            V12 = ""

        End If
        If Name2DB.Text = "" Or Address2DB.Text = "" Or PostalC2DB.Text = "" Or City2DB.Text = "" Or PHONE2DB.Text = "" Or CLOC2DB.Text = "" Or LBLeft.Text = "" Then
            MsgBox("All fields are mandatory!",, "Dorethy error message")
        End If
        ClearFields()
    End Sub

    Private Sub BtnReturn_Click(sender As Object, e As EventArgs) Handles BtnReturn.Click
        Dim oForm As Menu
        oForm = New Menu()
        oForm.Show()

        Hide()
    End Sub
    Private Sub ClearFields()
        Name2DB.Text = ""
        Address2DB.Text = ""
        PostalC2DB.Text = ""
        City2DB.Text = ""
        PHONE2DB.Text = ""
        NFC2DB.Text = ""
    End Sub
    Private Sub LockFields()
        Name2DB.Enabled = False
        Address2DB.Enabled = False
        PostalC2DB.Enabled = False
        City2DB.Enabled = False
        PHONE2DB.Enabled = False
    End Sub
    Private Sub OpenFields()
        Name2DB.Enabled = True
        Address2DB.Enabled = True
        PostalC2DB.Enabled = True
        City2DB.Enabled = True
        PHONE2DB.Enabled = True
    End Sub
End Class