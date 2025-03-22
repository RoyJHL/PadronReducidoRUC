namespace RJHL.Models
{
    public class MessageViewModel
    {

        public class Error
        {
            public const string ControlledException = "-99998";
            public const string Exception = "-99999";
        }

        public class Status
        {
            public static string Ok { get { return "SWA-API-DAT-GEN-0001"; } }
            public static string Exception { get { return "SWA-API-DAT-GEN-0002"; } }
            public static string ControlledException { get { return "SWA-API-DAT-GEN-0003"; } }
            public static string DataIsEmpthy { get { return "SWA-API-DAT-GEN-0004"; } }

            #region Functions

            /* DatabaseCommandExecution */
            public static string DatabaseCommandExecutionException { get { return "SWA-API-FUN-DCE-0002"; } }

            /* ResponseView */
            public static string ResponseViewException { get { return "SWA-API-FUN-RPV-0002"; } }

            #endregion

        }

        public static class StatusText
        {
            public static string DatabaseCommandExecutionException { get { return "Unhandled error executing command on database."; } }
            
        }

    }
}
