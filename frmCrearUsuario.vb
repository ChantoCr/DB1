Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Public Class frmCrearUsuario

    Dim cmd As New SqlCommand
    Dim des As New TripleDESCryptoServiceProvider
    Dim MD5 As New MD5CryptoServiceProvider
    Dim usuarioCheck As Integer = 0
    Dim passwordCheck As Integer = 0
    Dim tipoUsuarioCheck As Integer = 0
    Dim IDUser As String = ""
    Dim UserName As String = ""

    Private Sub resetForm()
        txtUser.Text = "Ingrese un Usuario"
        txtPass.Text = ""
        cmbTipoUsuario.Text = "Elija una opcion"
        usuarioCheck = 0
        passwordCheck = 0
        tipoUsuarioCheck = 0
    End Sub

    Private Sub checkReset()
        usuarioCheck = 0
        passwordCheck = 0
        tipoUsuarioCheck = 0
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
    'Final de funciones para encriptar

    'Inicio Codigo que se ejecuta cuando la ventana se abre
    Private Sub frmCrearUsuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarDatos()
    End Sub
    'Final Codigo que se ejecuta cuando la ventana se abre

    'Inicio Suv de carga de datos en SQL
    Sub CargarDatos()
        Try
            Dim dt As New DataTable
            Abrir()
            Using adaptador As New SqlDataAdapter("Select * from vista_usuarios", cnn)
                adaptador.Fill(dt)
            End Using
            dgvUsuarios.DataSource = dt
            'dgvUsuarios.Columns(0).Visible = False
            Cerrar()
        Catch ex As Exception
            MsgBox("No se pudo cargar los datos, favor intentar mas tarde..." + ex.ToString)
        End Try
    End Sub
    'Final Suv de carga de datos en SQL

    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Try
            Abrir()
            cmd = New SqlCommand("insert into usuarios (usuario,password,tipoUsuario) values (@usuario,@password,@tipoUsuario)", cnn)
            If txtUser.Text = "Ingrese un Usuario" Then
                MsgBox("Ingrese un usuario valido")
                usuarioCheck = 1
            ElseIf txtUser.Text = "" Then
                MsgBox("El usuario no puede estar vacio")
                usuarioCheck = 1
            Else
                cmd.Parameters.AddWithValue("@usuario", Me.txtUser.Text)
            End If

            If txtPass.Text = "" Then
                MsgBox("Ingrese un password valido")
                passwordCheck = 1
            ElseIf txtPass.Text = "" Then
                MsgBox("El password no puede estar vacio")
                passwordCheck = 1
            Else
                cmd.Parameters.AddWithValue("@password", Encrypt(Me.txtPass.Text, Key))
            End If

            If cmbTipoUsuario.Text = "Elija una opcion" Then
                MsgBox("Error: Debe elegir una opcion")
                tipoUsuarioCheck = 1
            ElseIf cmbTipoUsuario.Text = "TI" Then
                cmd.Parameters.AddWithValue("@tipoUsuario", 1)
            ElseIf cmbTipoUsuario.Text = "Gerente" Then
                cmd.Parameters.AddWithValue("@tipoUsuario", 2)
            ElseIf cmbTipoUsuario.Text = "Doctor" Then
                cmd.Parameters.AddWithValue("@tipoUsuario", 3)
            ElseIf cmbTipoUsuario.Text = "Enfermera" Then
                cmd.Parameters.AddWithValue("@tipoUsuario", 4)
            ElseIf cmbTipoUsuario.Text = "Paciente" Then
                cmd.Parameters.AddWithValue("@tipoUsuario", 5)
            End If

            If usuarioCheck = 0 And passwordCheck = 0 And tipoUsuarioCheck = 0 Then
                    cmd.ExecuteNonQuery()
                    MsgBox("Usuario Creado.")
                CargarDatos()
                resetForm()
            Else
                MsgBox("Error: Revisar los datos")
                checkReset()
            End If
            Cerrar()
        Catch ex As Exception
            MsgBox("No se pudo crear el Usuario, favor crear mas tarde" + ex.ToString)
        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        resetForm()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Hide()
        frmLogin.Show()
    End Sub

    Private Sub dgvUsuarios_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsuarios.CellClick
        Try
            If e.RowIndex >= 0 Then
                Dim row As DataGridViewRow
                row = Me.dgvUsuarios.Rows(e.RowIndex)
                txtUser.Text = row.Cells(1).Value.ToString
                txtPass.Text = ""
                cmbTipoUsuario.Text = row.Cells(2).Value.ToString
                IDUser = row.Cells(0).Value.ToString
                lblIDUsuario.Text = IDUser
                lblIDUsuario.Visible = True
            End If
        Catch ex As Exception
            MsgBox("No se pudo seleccionar el usuario" + ex.ToString)
        End Try
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Try
            If lblIDUsuario.Text = "ID User" Then
                MsgBox("Error: Debe seleccionar un usuario")
            Else
                If Me.ValidateChildren And txtUser.Text <> String.Empty Then
                    If MessageBox.Show("Esta seguro que desea modificar el usuario " & txtUser.Text & "?", "Favor confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) Then

                    End If
                Else
                        MessageBox.Show("El usuario no puede estar vacío.")
                End If
            End If
        Catch ex As Exception
            MsgBox("No se pudo modificar el usuario" + ex.ToString)
        End Try
    End Sub

End Class