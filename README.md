
<h3>For VB.net </h3>
<span>
 @Html.Calendar(Function(CalendarSettings As CalendarSetting)
                        CalendarSettings.Properties.DisabledDates = list
                        CalendarSettings.Properties.DefaultDate = DateTime.Now
                        CalendarSettings.Properties.Selectable = True
                        CalendarSettings.Properties.ClientSideEvents.Click = "showModal(arg.start);"
                        CalendarSettings.Properties.ClientSideEvents.DisabledSelected = "alert('this is disabled');"
                        CalendarSettings.Properties.ClientSideEvents.EndEventDropCallback = "console.log(moment(info.oldEvent.start).format('MM/DD/YYYY'))"
                        For Each i In New MISEntities().Holidays
                            Dim events = New Events
                            events.Title = i.Holiday
                            events.Start = i.DateOfHoliday
                            events.End = i.DateOfHoliday
                            CalendarSettings.Properties.Events.Add(events)
                        Next
                        Return CalendarSettings
                    End Function)
 </span>
