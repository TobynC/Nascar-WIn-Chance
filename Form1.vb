Imports System.IO
Imports System.Data.OleDb
Imports System.Threading
Public Class Form1
    'Creates RaceCarDriver
    Public Class RaceCarDriver
        Public Property track_name As String
        Public Property driver_name As String
        Public Property race As String
        Public Property win As String
        Public Property top_five As String
        Public Property top_ten As String
        Public Property pole As String
        Public Property laps As String
        Public Property laps_led As String
        Public Property average_start As String
        Public Property average_finish As String
        Public Property running_at_finished As String
        Public Property win_chance As Decimal
    End Class
    Dim trd1 As New Thread(AddressOf combobox_fill)
    Sub combobox_fill(dt As DataTable)
        'Makes combobox visible and creates a list of tracks and sets the combobox information to it
        ComboBox1.Visible = True
        Dim list_of_tracks As New ArrayList()
        list_of_tracks.Add("Select Track")

        'Fills the list for the combobox
        For rowCount = 0 To dt.Rows.Count - 1
            If Not list_of_tracks.Contains(dt.Rows(rowCount)(1)) Then
                list_of_tracks.Add(dt.Rows(rowCount)(1).ToString)
            End If
        Next
        'Sets the binding source for the combobox to the list of tracks
        Form1BindingSource1.DataSource = list_of_tracks

        'Sets Default Values and reveals textboxes and labels
        Label1.Visible = True
        TextBox1.Visible = True
        TextBox1.Text = "5"
        Label2.Visible = True
        TextBox2.Visible = True
        TextBox2.Text = "1.5"
        Label3.Visible = True
        TextBox3.Visible = True
        TextBox3.Text = "0.75"
        Label4.Visible = True
        TextBox4.Visible = True
        TextBox4.Text = "0.5"
        Label5.Visible = True
        TextBox5.Visible = True
        TextBox5.Text = "0.75"
        Label6.Visible = True
        TextBox6.Visible = True
        TextBox6.Text = "0.25"
        Label7.Visible = True
        TextBox7.Visible = True
        TextBox7.Text = "2"
    End Sub
    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Open Dialogue Box
        Dim OpenFileDialog As OpenFileDialog = New OpenFileDialog()
        Dim load As load = New load()
        'Objects being called and File being opened
        Dim strFileName As String = load.GetFileName(OpenFileDialog)
        Dim dt As DataTable = load.OpenDirectory(strFileName)
        combobox_fill(dt)
        'Sets datatable to the opendirectory file
        DataGridView1.DataSource = load.OpenDirectory(strFileName)
    End Sub
    Public Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        'Sets TextBox Values
        Dim tb1 = TextBox1.Text
        Dim tb2 = TextBox2.Text
        Dim tb3 = TextBox3.Text
        Dim tb4 = TextBox4.Text
        Dim tb5 = TextBox5.Text
        Dim tb6 = TextBox6.Text
        Dim tb7 = TextBox7.Text

        'Sets trackmodule to the selected combobox item
        TrackModule.SelectedTrack = ComboBox1.SelectedItem().ToString
        'opens datatable to get information
        Dim load As load = New load()
        Dim dt As DataTable = load.OpenDirectory(FileNameModule.selectedFileName)
        'Creates a list of drivers
        Dim driver_list As New List(Of RaceCarDriver)
        'fills the driver objects in list of drivers
        Dim counter As Integer = 0
        For Each r As DataRow In dt.Rows
            Dim d As New RaceCarDriver()
            If counter > 0 Then
                d.driver_name = r(0)
                d.track_name = r("Track")
                d.race = r("Race")
                d.win = r("Win")
                d.top_five = r("T5")
                d.top_ten = r("T10")
                d.pole = r("Pole")
                d.laps = r("Laps")
                d.laps_led = r("Laps Led")
                Try
                    d.average_start = r("Avg# St")
                Catch ex As Exception
                    d.average_start = r("Avg. St")
                End Try
                Try
                    d.average_finish = r("Avg# Fn")
                Catch ex As Exception
                    d.average_finish = r("Avg. Fn")
                End Try
                d.running_at_finished = r("Running At Finish")
                driver_list.Add(d)
            End If
            counter += 1
        Next
        'sets the win chance and shows the 
        Dim dr As List(Of RaceCarDriver) = (From x In driver_list Where x.track_name = TrackModule.SelectedTrack Select x).ToList
        If Not IsNothing(dr) Then
            For Each drv As RaceCarDriver In dr
                drv.win_chance = Utils.Success(tb1, tb2, tb3, tb4, tb5, tb6, tb7, drv.race, drv.win, drv.top_five, drv.top_ten, drv.pole, drv.laps, drv.laps_led, drv.average_start, drv.average_finish, drv.running_at_finished)
            Next
        End If
        'Manually sets the column information for the ouptut at each track
        Dim f2 As Form2 = New Form2
        f2.Visible = True
        f2.DataGridView1.AutoGenerateColumns = False
        f2.DataGridView1.ColumnCount = 3
        f2.DataGridView1.Columns(0).Name = "Track"
        f2.DataGridView1.Columns(0).HeaderText = "Track"
        f2.DataGridView1.Columns(0).DataPropertyName = "track_name"
        f2.DataGridView1.Columns(1).Name = "Name"
        f2.DataGridView1.Columns(1).HeaderText = "Name"
        f2.DataGridView1.Columns(1).DataPropertyName = "driver_name"
        f2.DataGridView1.Columns(2).Name = "Win Chance"
        f2.DataGridView1.Columns(2).HeaderText = "Win Chance"
        f2.DataGridView1.Columns(2).DataPropertyName = "win_chance"
        f2.DataGridView1.DataSource = dr.OrderByDescending(Function(x) x.win_chance).ToList
    End Sub
End Class
Public Class load
    'loads the file using the given file name
    Public Function GetFileName(ofd As OpenFileDialog) As String
        Dim strFileName As String
        If ofd.ShowDialog() = DialogResult.OK Then
            strFileName = ofd.FileName
        Else
            strFileName = "The file did not load correctly."
            MsgBox(strFileName)
        End If
        FileNameModule.selectedFileName = strFileName
        Return strFileName
    End Function
    'Enables the use of Excel in the program
    Private Excel03ConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;'"
    Private Excel07ConString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;'"
    Public Function OpenDirectory(strFileName As String) As DataTable
        'Gets the file name
        Dim extension As String = Path.GetExtension(strFileName)
        Dim conStr As String, sheetName As String
        'Opens the file and fills a datatable with the information
        conStr = String.Empty
        Select Case extension
            Case ".xls"
                'Excel 97-03
                conStr = String.Format(Excel03ConString, strFileName)
                Exit Select

            Case ".xlsx"
                'Excel 07
                conStr = String.Format(Excel07ConString, strFileName)
                Exit Select
        End Select
        'Get the name of the First Sheet.
        Using con As New OleDbConnection(conStr)
            Using cmd As New OleDbCommand()
                cmd.Connection = con
                con.Open()
                Dim dtExcelSchema As DataTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                sheetName = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
                con.Close()
            End Using
        End Using
        'Read Data from the First Sheet.
        Using con As New OleDbConnection(conStr)
            Using cmd As New OleDbCommand()
                Using oda As New OleDbDataAdapter()
                    Dim dt As New DataTable()
                    cmd.CommandText = (Convert.ToString("SELECT * From [") & sheetName) + "]"
                    cmd.Connection = con
                    con.Open()
                    oda.SelectCommand = cmd
                    oda.Fill(dt)
                    con.Close()
                    Return dt
                End Using
            End Using
        End Using
    End Function
End Class