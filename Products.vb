Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Interaction
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports Npgsql


Public Class Products
    Private Const V1 As String = "Host=localhost;Username=postgres;Password=""4Dor=Access!"";Database=postgres"
    Private Const V11 As String = "Select * from products, cmb"
    'Private Const V As String = ""
    Dim V12 As String

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

            Dim sql As String = "Select * from products where products.ean = '" & EAN2DB.Text & "'"

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
                MsgBox("EAN is already stored in the database, select your option below",, "Dorethy message")
                DRT_Login.ReturnCodeRFID = "exist"
                V12 = "EDT"
                BtnEdit.Visible = True
            End If

        End If

    End Sub
    Private Sub AddNU2DB()
        If EAN2DB.Text <> "" And Name2DB.Text <> "" And NETPR2DB.Text <> "" And SALESPR2DB.Text <> "" Then
            Dim Conn As NpgsqlConnection
            Dim Adpt As NpgsqlDataAdapter
            Dim cmd As NpgsqlCommand

            Conn = New NpgsqlConnection With {
                .ConnectionString = V1
            }
            Conn.Open()

            Dim sql As String = V11

            Adpt = New NpgsqlDataAdapter(sql, Conn)

            If V12 <> "EDT" Then
                sql = ("insert into public.products (ean, name, netprice, salesprice) values (" & Chr(39) & EAN2DB.Text & Chr(39) & ", " & Chr(39) & Name2DB.Text & Chr(39) & "," & Chr(39) & NETPR2DB.Text & Chr(39) & ", " & Chr(39) & SALESPR2DB.Text & Chr(39) & ")")
            ElseIf V12 = "EDT" Then
                sql = "update public.products set name = " & Chr(39) & DataGridView1.Rows(0).Cells(2).Value & Chr(39) & ", netprice = " & Chr(39) & DataGridView1.Rows(0).Cells(3).Value & Chr(39) & ", salesprice = " & Chr(39) & DataGridView1.Rows(0).Cells(4).Value & Chr(39) & " where id=" & Chr(39) & DataGridView1.Rows(0).Cells(0).Value & Chr(39) & " "

            End If


            cmd = New NpgsqlCommand(sql, Conn)
            cmd.ExecuteNonQuery()

            Conn.Close()

            ID_FS.ForeColor.R.ToString()
            ID_FS.Text = "Key successfuly saved"
            V12 = ""
            ClearFields()
        End If
        If EAN2DB.Text = "" Or Name2DB.Text = "" Or NETPR2DB.Text = "" Or SALESPR2DB.Text = "" Then
            MsgBox("All fields are mandatory!",, "Dorethy error message")
        End If

    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        V12 = "EDT"
        MsgBox("Edit your data in the datagrid and save your changed by clicking the Add new product button.",, "Dorethy message")
    End Sub

    Private Sub Products_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnEdit.Visible = False
    End Sub

    Private Sub BtnReturn_Click(sender As Object, e As EventArgs) Handles BtnReturn.Click
        Dim oForm As Menu
        oForm = New Menu()
        oForm.Show()

        Hide()
    End Sub
    Private Sub ClearFields()
        EAN2DB.Text = ""
        Name2DB.Text = ""
        NETPR2DB.Text = ""
        SALESPR2DB.Text = ""
    End Sub
End Class