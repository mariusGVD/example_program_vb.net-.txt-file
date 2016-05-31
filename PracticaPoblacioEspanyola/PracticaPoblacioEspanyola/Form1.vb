Imports System.IO
Imports System.Text
'H.M.C.
Public Class Form1
    Dim sre As IO.StreamReader = Nothing
    Dim linea As String
    Dim arrDades() As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        

        sre = New IO.StreamReader("pobmun13.txt", System.Text.Encoding.Default)

        Do Until sre.Peek = -1
            linea = sre.ReadLine()
            arrDades = Split(linea, vbTab)

            If ComboBox1.Items.Contains(arrDades(1)) = False Then

                ComboBox1.Items.Add(arrDades(1))

            End If
        Loop

        ComboBox2.Items.Add("<")
        ComboBox2.Items.Add("=")
        ComboBox2.Items.Add(">")



    End Sub

   
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        sre = New IO.StreamReader("pobmun13.txt", System.Text.Encoding.Default)
        Refresh()

        Do Until sre.Peek = -1
            linea = sre.ReadLine()
            arrDades = Split(linea, vbTab)

            Dim provincia As String
            provincia = arrDades(1).ToString()
            Dim numPob As Integer

            If ComboBox1.SelectedItem.ToString = provincia Then
                For Each provincia In arrDades(1).ToString
                    numPob = ListView1.Items.Count
                    TextBox2.Text = numPob.ToString()
                    Dim item1 As ListViewItem = ListView1.FindItemWithText(arrDades(3)) ' Busquem es el poble ja existeix en la llista per no repetir-lo
                    If (item1 Is Nothing) Then
                        Dim i As Integer ' index
                        Dim str(4) As String ' creem un array de strings de 4 columnes
                        Dim itm As ListViewItem

                        For i = 0 To 3
                            str(i) = arrDades(i + 3)
                        Next
                        itm = New ListViewItem(str) ' insertem al listview l'array de strings
                        ListView1.Items.Add(itm) ' afegim items linea per linea
                    End If
                Next
            End If
        Loop


    End Sub

    Public Overrides Sub Refresh()
        ListView1.Items.Clear()
    End Sub

    

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ListBox1.Items.Clear()
        sre = New IO.StreamReader("pobmun13.txt", System.Text.Encoding.Default)

        ' Verifiquem si s'han completat els camps de les condicions
        If (String.IsNullOrEmpty(ComboBox2.SelectedItem) OrElse
            String.IsNullOrEmpty(TextBox1.Text) OrElse
            (RadioButton1.Checked = False And RadioButton2.Checked = False)) Then

            MessageBox.Show("Introdueix totes les condicions de busqueda!")
            ' Si els comps no estan complets sortim
            Return
        End If

        ' Variable de la condició <, >, =
        Dim condicio As String
        condicio = ComboBox2.SelectedItem.ToString()

        ' Variable nombre habitants
        Dim nombrehabitants As Integer
        nombrehabitants = TextBox1.Text

        ' Variable sexe. Es guarda am el valor 1 si es home i 2 si es dona
        Dim sex As Integer
        If RadioButton1.Checked = True Then
            sex = 1
        End If
        If RadioButton2.Checked = True Then
            sex = 2
        End If

        Do Until sre.Peek = -1
            linea = sre.ReadLine()
            arrDades = Split(linea, vbTab)
            If (String.IsNullOrEmpty(arrDades(4))) Then Continue Do

            ' Guardo el nr. d'habitants i sexe en un vector amb els indicis 1 i 2 per asociar-lo amb el sexe 1=home, 2=dona
            Dim num(2) As Integer


            'Convertim el num d'habitants que extreiem del array a integer pq esta com a string i reemplaçem el "." per res
            '(alguns nr. tenen "." per agrupar les unitats)
            'Homes
            num(1) = Convert.ToInt32(arrDades(5).Replace(".", ""))
            'Dones
            num(2) = Convert.ToInt32(arrDades(6).Replace(".", ""))

            'Variable poblacio que conte la columna del nom de les poblacions
            Dim poblacio As String
            poblacio = arrDades(3)

            If (condicio = ">") Then ' Mes gran
                If (num(sex) > nombrehabitants) Then
                    ListBox1.Items.Add(poblacio)
                    ListBox1.Sorted = True
                End If
            ElseIf (condicio = "=") Then ' Igual
                If (num(sex) = nombrehabitants) Then
                    ListBox1.Items.Add(poblacio)
                    ListBox1.Sorted = True
                End If
            Else ' Mes petit
                If (num(sex) < nombrehabitants) Then
                    ListBox1.Items.Add(poblacio)
                    ListBox1.Sorted = True
                End If
            End If

        Loop

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        sre = New IO.StreamReader("pobmun13.txt", System.Text.Encoding.Default)

        ' Verifiquem si s'han completat els camps de les condicions
        If (String.IsNullOrEmpty(TextBox3.Text)) Then

            MessageBox.Show("Introdueix la població!")
            ' Si el camp del textbox 3 esta en buit o null sortim
            Return
        End If

        'Variable poble= nom del poble introduit pel usuari
        Dim poble As String
        ' Cambiem les majuscules per minuscules per controlar millor el text introduit
        poble = TextBox3.Text.ToString().ToLower()

        ' Variable que indica si s'ha trobat algun poble
        Dim trobat As Boolean
        trobat = False

        Do Until sre.Peek = -1
            linea = sre.ReadLine()
            arrDades = Split(linea, vbTab)

            'Variable per guardar el nom del poble del fitxer txt
            Dim pobletxt As String
            ' Combio majuscules per minuscules perque l'usuari pugui introduir el nom tal com vol am majuscules o minuscules
            'i que el programa el trobi igualment en el fitxer. Exemple : "Balsa de Ves" que pertany a la provincia Albacete
            'pot ser escrit pel usuari com a : Balsa de ves, Balsa DE VES, etc... el programa convertira les maj en min i trobara igualment el resultat
            pobletxt = arrDades(3).ToString().ToLower()

            If (poble = pobletxt) Then
                ' Daca a fost gasit cel putin un rezultat setezi variabila ca true
                trobat = True
                MessageBox.Show("El poble " + pobletxt.ToUpper() + " forma part de la provincia " + arrDades(1).ToString() + " .", "Informació")
            End If

        Loop
        TextBox3.Text = ""

        ' si es false vol dir que no existeix en el fitx.
        If (Not trobat) Then
            MessageBox.Show("El poble " + poble.ToUpper() + " no pertany a cap provincia o es incorrecte!!! ", "Informació")
        End If


    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        'corverteix el primer caracter a majuscula
        If TextBox3.SelectionStart = 0 Then e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        'valido el textbox perque no es puguin introduir numeros
        If Char.IsLetter(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsControl(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsSeparator(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress

        'valido el textbox perque sol es puguin introduir numeros
        If Char.IsDigit(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsControl(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsSymbol(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsSeparator(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

        'Si en el textbox trobem un espai entre numeros el corregim borran-lo
        Me.TextBox1.Text = Trim(Replace(Me.TextBox1.Text, "  ", " "))
        TextBox1.Select(TextBox1.Text.Length, 0)
    End Sub
End Class
