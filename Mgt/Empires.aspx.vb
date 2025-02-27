Imports System.Data.SqlClient
Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim Con As String = System.Configuration.ConfigurationManager.ConnectionStrings("UBConnectionString").ConnectionString
    Dim Conn As New SqlConnection(Con)

    Public Result As String = ""

    Sub GetResult()

        Dim str As String = ""
        Dim rlt_th As String = ""
        Dim rlt_td As String = ""

        Conn.Open()

        Try


            If T_input.Text <> "" Then

                Dim ADT As New SqlDataAdapter(T_input.Text, Conn)
                Dim DT As New DataTable
                ADT.Fill(DT)

                For c As Integer = 0 To DT.Columns.Count - 1
                    rlt_th &= " <th>" & DT.Columns(c).ColumnName & "</th>"
                Next

                For r As Integer = 0 To DT.Rows.Count - 1
                    rlt_td &= "<tr>"
                    For i As Integer = 0 To DT.Columns.Count - 1
                        rlt_td &= "<td>" & DT.Rows(r).Item(i) & "</td>"
                    Next
                    rlt_td &= "</tr>"
                Next

                str = "<div class='alert alert-success'>"
                str &= "    <strong>對戰成功。</strong><br/>"
                str &= "</div>"

                str &= "<table style='width:100%'>"
                str &= "    <thead><tr>" & rlt_th & "</tr></thead>"
                str &= "    <tbody>" & rlt_td & "</tbody>"
                str &= "</table>"
            Else
                str = "<div class='alert alert-danger'>"
                str &= "    <strong>請提供對戰指令。</strong>"
                str &= "</div>"
            End If

        Catch ex As Exception
            str = "<div class='alert alert-danger'>"
            str &= "    <strong>對戰失敗。</strong><br/>"
            str &= "    " & ex.Message
            str &= "</div>"
        End Try

        Conn.Close()

        Result = str

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        GetResult()
    End Sub



End Class
