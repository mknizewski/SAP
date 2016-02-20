namespace SAP.BOL.HelperClasses
{
    public class SetAlert
    {
        public string TypeOfAlert { get; set; }
        public string Message { get; set; }
        public string StrongMessage { get; set; }

        public static SetAlert Set(string message, string strongMessage, AlertType typeOfAlert)
        {
            SetAlert alert = new SetAlert();

            alert.Message = message;
            alert.StrongMessage = strongMessage;

            switch (typeOfAlert)
            {
                case AlertType.Success:
                    alert.TypeOfAlert = "alert-success";
                    break;

                case AlertType.Danger:
                    alert.TypeOfAlert = "alert-danger";
                    break;

                case AlertType.Info:
                    alert.TypeOfAlert = "alert-info";
                    break;

                case AlertType.Warning:
                    alert.TypeOfAlert = "alert-warning";
                    break;
            }

            return alert;
        }
    }

    public enum AlertType
    {
        Success,
        Info,
        Danger,
        Warning
    }
}