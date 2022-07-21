namespace ArquitecturaDemo.Shared.Helpers
{
    public class Const
    {
        /// <summary>
        /// Validation
        /// </summary>
        public const string
            NullOrEmptyMessage = "{PropertyName} es requerido.",
            InvalidMessage = "El valor de {PropertyName} es inválido.",
            EmailExists = "El correo electrónico ya está registrado.";

        public const int MIN_VALUE = 0;

        /// <summary>
        /// Database Section Settings
        /// </summary>
        public const string
            DbUsersConnection = "DbUsers";

        /// <summary>
        /// Helpers
        /// </summary>
        public const string
            StringEmpty = "",
            EmptyParameter = "0",
            OrderByAsc = "asc",
            OrderByDesc = "desc",
            LambdaParameter = "x";

        /// <summary>
        /// Logging responses
        /// </summary>
        public const string
            StartOperationLog = "Iniciando operación...",
            EndOperationLog = "Finalizando operación...",
            ErrorOperationLog = "Error ejecutando la operación -{ErrorLog}-",
            InformationLog = "Ejecutando operación -{Operation}- en entidad -{Entity}-",
            BulkInformationLog = "Ejecutando -{Operation}- masiva de Entidad -{Entity}-";
    }
}