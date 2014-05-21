using System.Web;

namespace SdbBackbone.Web
{
    public class HttpContext
    {
        private static HttpContextBase _current;

        public static HttpContextBase Current
        {
            get
            {
                if (_current != null)
                    return _current;

                if(System.Web.HttpContext.Current != null)
                    return _current ??  new HttpContextWrapper(System.Web.HttpContext.Current);
                
                return null;
            }
            set { _current = value; }
        }
    }
}