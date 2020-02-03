using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FullCalendar.Helper
{

    public class CalendarSetting
    {
        private Properties _properties;

        public Properties Properties
        {
            get
            {
                if (_properties == null)
                    _properties = new Properties();
                return _properties;
            }
            set => _properties = value;
        }
    }

    public class Events
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
    }
    public class Properties
    {

        public List<Events> Events
        {
            get
            {
                if (_events == null)
                    _events = new List<Events>();
                return _events;
            }
            set => _events = value;
        }

        private ClientSideEvents _clientSideEvents;
        private List<Events> _events;

        public ClientSideEvents ClientSideEvents
        {
            get
            {
                if (_clientSideEvents == null)
                    _clientSideEvents = new ClientSideEvents();
                return _clientSideEvents;
            }
            set => _clientSideEvents = value;
        }

        public DateTime DefaultDate { get; set; }
        public bool Selectable { get; set; }

        public List<DateTime?> DisabledDates { get; set; }
    }

    public class ClientSideEvents
    {
        public string Click { get; set; }
        public string DisabledSelected { get; set; }

        public string EndEventDropCallback { get; set; }
    }
    public static class FullCalendar
    {

        public static HtmlString Calendar(this HtmlHelper htmlHelper, Action<CalendarSetting> settings)
        {
            CalendarSetting calendarSetting = new CalendarSetting();
            settings(calendarSetting);

            var res = "<div id='calendar'></div><script>document.addEventListener('DOMContentLoaded', function () {" +
                      "var calendarEl = document.getElementById('calendar');" +
                      "var calendar = new FullCalendar.Calendar(calendarEl, {" +
                      "plugins: ['interaction', 'dayGrid', 'timeGrid', 'list']," +
                      "header: {" +
                      "left: 'prev,next today'," +
                      "center: 'title'," +
                      "right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'}," +
                      "defaultDate: '" + calendarSetting.Properties.DefaultDate.ToString("yyyy-MM-dd") + "'," +
                      "navLinks: true," +
                      "businessHours: true," +
                      "editable: true," +
                      "eventDrop: function(info) {" +
                      calendarSetting.Properties.ClientSideEvents.EndEventDropCallback +
                      ";}," +
                       "selectable: " + calendarSetting.Properties.Selectable.ToString()?.ToLower() + "," +
                       "select: function (arg, end) {" +
                       " console.log(moment(arg.start).format('MM/DD/YYYY'));" +

                       (calendarSetting.Properties.DisabledDates == null ? "" : DisableDates(settings)) +
                       (calendarSetting.Properties.ClientSideEvents?.Click ?? "") +
                       "calendar.unselect()},events: [" + Events(settings) + " ]});calendar.render();});</script>";

            return new HtmlString(res);
        }


        static string Events(Action<CalendarSetting> settings)
        {
            CalendarSetting calendarSetting = new CalendarSetting();
            settings(calendarSetting);
            var res = "";
            if (calendarSetting.Properties.Events != null)
                foreach (var i in calendarSetting.Properties.Events)
                {
                    res += $"{{start: '{i?.Start?.ToString("yyyy-MM-dd")}'," +
                         $"end: '{i?.End?.ToString("yyyy-MM-dd")}'," +
                         $"title: '{i?.Title}'}},";
                }
            return res;
        }
        static string DisableDates(Action<CalendarSetting> settings)
        {
            CalendarSetting calendarSetting = new CalendarSetting();
            settings(calendarSetting);
            var res = "";
            if (calendarSetting.Properties.DisabledDates != null)
                foreach (var i in calendarSetting.Properties.DisabledDates)
                {
                    res += @"if (moment(arg.start).format('MM/DD/YYYY') == '" + i?.ToString("MM/dd/yyyy") + "') {" +
                           calendarSetting.Properties.ClientSideEvents.DisabledSelected + ";" +
                           "return false; }";
                }

            return res;
        }
        static string DisableDates(List<DateTime> dateTimes)
        {
            var res = "";
            if (dateTimes != null)
                foreach (var i in dateTimes)
                {
                    res += @"if (moment(arg.start).format('MM/DD/YYYY') == '" + i.ToString("MM/dd/yyyy") + "') {" +
                           "return false; }";
                }

            return res;
        }
        public static HtmlString Calendar(this HtmlHelper htmlHelper, List<DateTime> dateTimes = null)
        {


            var str = "<script>document.addEventListener('DOMContentLoaded', function () {" +
                      "var calendarEl = document.getElementById('calendar');" +
                      "var calendar = new FullCalendar.Calendar(calendarEl, {" +
                      "plugins: ['interaction', 'dayGrid', 'timeGrid', 'list']," +
                      "header: {" +
                      "left: 'prev,next today'," +
                      "center: 'title'," +
                      "right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'}," +
                      "defaultDate: '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                      "navLinks: true," +
                      "businessHours: true," +
                      "editable: true," +
                      "selectable: true," +
                      "select: function (arg, end) {" +
                      " console.log(moment(arg.start).format('MM/DD/YYYY'));" +

                      (dateTimes == null ? "" : DisableDates(dateTimes)) +
                      ";var title = prompt('Event Title:');" +
                      "if (title) { calendar.addEvent({title: title,start: arg.start, end: arg.end,allDay: arg.allDay})}" +
                      "calendar.unselect()},events: [ ]});calendar.render();});</script>";
            return new HtmlString(str);
        }
    }
}
