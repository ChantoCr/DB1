Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Public Class frmLogin

    Dim cmd As New SqlCommand
    Dim des As New TripleDESCryptoServiceProvider
    Dim MD5 As New MD5CryptoServiceProvider

    Private Sub Borrar()
        txtUser.Text = "Ingrese un usuario"
        txtPass.Text = ""
    End Sub

    'Inicio funciones para desencriptar
    Function MD5Hash(ByVal value As String) As Byte()
        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

    Function Encrypt(ByVal Stringinput As String, ByVal Key As String) As String
        des.Key = MD5Hash(Key)
        des.Mode = CipherMode.ECB
        Dim buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(Stringinput)
        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
    End Function
    'Final de funciones para desencriptar


    Function ConsultarTipoUsuario(ByVal Usuarios As String) As Integer
        Dim resultado As Integer
        Try

            Abrir()
            enunciado = New SqlCommand("Select tipoUsuario from usuarios where usuario='" & Usuarios & "'", cnn)
            respuesta = enunciado.ExecuteReader

            If respuesta.Read Then
                resultado = CInt(respuesta.Item("tipoUsuario"))
            End If
            respuesta.Close()
        Catch ex As Exception
            MsgBox("No se puede leer el tipo de Usuario " + ex.ToString)
        End Try
        Return resultado
    End Function


    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            If txtUser.Text = "Ingrese un usuario" Then
                MsgBox("Debe ingresar un usuario valido")
            ElseIf txtUser.Text = "" Then
                MsgBox("Debe ingresar un usuario valido")
            ElseIf txtPass.Text = "" Then
                MsgBox("El campo Password no puede estar vacío")
            Else
                Abrir()
                Dim query As String = "Select * from usuarios where usuario=@usuario and password=@password"
                cmd = New SqlCommand(query, cnn)
                cmd.Parameters.AddWithValue("@usuario", txtUser.Text)
                cmd.Parameters.AddWithValue("@password", Encrypt(txtPass.Text, Key))
                cmd.ExecuteNonQuery()
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader
                While dr.Read
                    If ConsultarTipoUsuario(txtUser.Text) = 1 Then
                        MsgBox("El Usuario es de TI")
                    ElseIf ConsultarTipoUsuario(txtUser.Text) = 2 Then
                        MsgBox("El Usuario es de Gerente")
                    ElseIf ConsultarTipoUsuario(txtUser.Text) = 3 Then
                        MsgBox("El Usuario es de Doctor ó un Doctor")
                    ElseIf ConsultarTipoUsuario(txtUser.Text) = 4 Then
                        MsgBox("El Usuario es de Enfermero ó Enfermera")
                    ElseIf ConsultarTipoUsuario(txtUser.Text) = 5 Then
                        MsgBox("El Usuario es de un o una Paciente")
                    Else
                        MsgBox("El Usuario no pertenece a ningun grupo")
                    End If
                    Me.Hide()
                    Borrar()
                    Exit Sub
                End While
                MsgBox("Login Incorrecto")
            End If
        Catch ex As Exception
            MsgBox("No puede leer el usuario " + ex.ToString)
        End Try
    End Sub


    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub



    'Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '   Abrir()
    '   Cerrar()
    '   MsgBox("Conexion y desconexion satisfactoria")
    'End Sub

End Class
