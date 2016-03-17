using System;
using System.Timers;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa zegaru serwerowego do obsługi zdarzeń związanych z truniejami, zegar tyka co 5 min, wczesniej następuje etap synchronizacyjny
    /// Działa na zasadzie singletonu (jedna instancja statyczna na caly serwer)
    /// Inicjalizowana przy starcie w metodzie Application_Start klasy Global.cs
    /// </summary>
    public static class ServerTime
    {
        #region Pola

        private static Timer serverTimer { get; set; }
        private static DateTime serverDateTime { get; set; }

        public static DateTime ServerDataTime
        {
            get { return serverDateTime; }
        }

        #endregion Pola

        #region Metody

        public static void Inicialize()
        {
            serverDateTime = DateTime.Now;
            serverTimer = new Timer();

            serverTimer.Elapsed += IniclaizeEvent;
            serverTimer.Interval = 1000; // 1 sekunda
            serverTimer.Start();

            TodoList.InicializeTodayTasks(); //Pierwsza wstępna inicjalizacja tasków

            //Ustawienia Garbage Collector w celu zapobiegnięcia usunięcia
            GC.KeepAlive(serverTimer);
            GC.KeepAlive(serverDateTime);
            GC.KeepAlive(ServerDataTime);
        }

        //Pierwszy event ktory synchronizuje timer
        private static void IniclaizeEvent(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute % 5 == 0)
            {
                serverDateTime = DateTime.Now;

                serverTimer.Stop();
                serverTimer.Interval = 300000; //5 minut
                serverTimer.Elapsed -= IniclaizeEvent;
                serverTimer.Elapsed += TickEvent;
                serverTimer.Start();
            }
        }

        //Wlasciwy event
        private static void TickEvent(object source, ElapsedEventArgs e)
        {
            serverDateTime = DateTime.Now;

            if (serverDateTime.TimeOfDay == TimeSpan.Zero)
            {
                TodoList.Execute(serverDateTime);
                TodoList.InicializeTodayTasks();
            }
            else
                TodoList.Execute(serverDateTime);
        }

        public static void Dispose()
        {
            //TODO: Opracować dobrą technikę usuwania timera kiedy serwer jest offline
            serverTimer.Stop();
            serverTimer.Dispose();
        }

        #endregion Metody
    }
}