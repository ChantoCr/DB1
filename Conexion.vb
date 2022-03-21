Imports System.Data.SqlClient

Module Conexion
    Public cnn As SqlConnection
    Public Key As String = "abc"
    Public enunciado As SqlCommand
    Public respuesta As SqlDataReader

    Sub Abrir()
        Try
            cnn = New SqlConnection("Data Source=DESKTOP-NHQJ965\SQLEXPRESS;Initial Catalog=Encrypt;Integrated Security=True")
            cnn.Open()
            'MsgBox("Conexion Establecida")
        Catch ex As Exception
            MsgBox("No se pudo conectar, favor intentar mas tarde... " + ex.ToString)
        End Try
    End Sub

    Sub Cerrar()
        Try
            cnn.Close()
        Catch ex As Exception
            MsgBox("No se pudo cerrar, favor intentar mas tarde... " + ex.ToString)
        End Try
    End Sub
End Module
