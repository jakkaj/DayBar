namespace Office365Api.Helpers.Office365
{
    public abstract class BaseOffice365Helper
    {
        protected AuthenticationHelper AuthenticationHelper
        {
            get;
            private set;
        }

        public BaseOffice365Helper(AuthenticationHelper authenticationHelper)
        {
            this.AuthenticationHelper = authenticationHelper;
        }
    }
}
