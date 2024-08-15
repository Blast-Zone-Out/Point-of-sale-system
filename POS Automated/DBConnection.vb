Module DBConnection
    Public con As New OleDb.OleDbConnection(My.Settings.POSConnectionString)
    Public cmd As New OleDb.OleDbCommand("", con)
    Public Adapter As New OleDb.OleDbDataAdapter
    Public Table As New DataTable
End Module
