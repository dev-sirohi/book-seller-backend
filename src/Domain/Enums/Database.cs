namespace BSB.src.Domain.Enums
{
    public class Database
    {
        public enum DBCommandExecutorTypes
        {
            NONQUERY,
            SCALAR,
            READER
        }

        public class UserAuthLinkType
        {
            public static readonly int EMAIL_VERIFICATION = 10;
            public static readonly int PASSWORD_RESET = 20;
        }
    }
}
