Public Class Utils
    'Win Calculations
    Public Shared Function Success(tb1 As String, tb2 As String, tb3 As String, tb4 As String, tb5 As String, tb6 As String, tb7 As String, race As String, win As String, top_five As String, top_ten As String, pole As String, laps As String, laps_led As String, average_start As String, average_finish As String, running_at_finished As String) As Double
        Dim win_chance As Decimal
        Dim calc1 As Decimal = (Convert.ToDecimal(win)) / Convert.ToDecimal(race) * Convert.ToDecimal(tb1)
        Dim calc2 As Decimal = (Convert.ToDecimal(top_five)) / Convert.ToDecimal(race) * Convert.ToDecimal(tb2)
        Dim calc3 As Decimal = (Convert.ToDecimal(top_ten)) / Convert.ToDecimal(race) * Convert.ToDecimal(tb3)
        Dim calc4 As Decimal = ((44 - Convert.ToDecimal(average_start)) / 43) * Convert.ToDecimal(tb4)
        Dim calc5 As Decimal = ((44 - Convert.ToDecimal(average_finish)) / 43) * Convert.ToDecimal(tb5)
        Dim calc6 As Decimal = (Convert.ToDecimal(running_at_finished)) / Convert.ToDecimal(race) * Convert.ToDecimal(tb6)
        Dim calc7 As Decimal = (Convert.ToDecimal(laps_led)) / Convert.ToDecimal(laps) * Convert.ToDecimal(tb7)
        win_chance = calc1 + calc2 + calc3 + calc4 + calc5 + calc6 + calc7
        Return Math.Round(win_chance, 3)
    End Function
End Class
