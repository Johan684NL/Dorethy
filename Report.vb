Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Interaction
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports Npgsql
Public Class Report
    Private Const V1 As String = "Host=localhost;Username=postgres;Password=""4Dor=Access!"";Database=postgres"
    'Private Const V11 As String = "Select * from customer, cmb"
    'Private Const V As String = ""
    'Dim V12 As String
    Dim A1 As String


    Private Sub BtnShow_Click(sender As Object, e As EventArgs) Handles BtnShow.Click
        Dim Conn As NpgsqlConnection
        Dim Adpt As NpgsqlDataAdapter

        'Dim cmd As NpgsqlCommand

        Dim npgsqlConnection As New NpgsqlConnection
        Conn = npgsqlConnection
        Conn.ConnectionString = V1
        Conn.Open()
        Dim sql As String
        If LBTable.Text = "Customer" Then
            A1 = "Select * from customer"
        End If
        If LBTable.Text = "Products" Then
            A1 = "Select * from products"
        End If
        If LBTable.Text = "Card" Then
            A1 = "Select * from sales"
        End If

        sql = A1

        Adpt = New NpgsqlDataAdapter(sql, Conn)

        Dim RS As New DataTable

        Adpt.Fill(RS)
            Dim ds As New DataSet("DataSetOne")
            ds.Tables.Add(RS)
            DataGridView1.DataSource = RS
            Conn.Close()
    End Sub

    Private Sub BtnReturn_Click(sender As Object, e As EventArgs) Handles BtnReturn.Click
        Dim oForm As Menu
        oForm = New Menu()
        oForm.Show()

        Hide()
    End Sub
End Class