using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn
{
    public static class SessionExt
    {
        public static void SetCurrentUser(this HttpSessionStateBase session, gov_user user )
        {
            session["currentUser"] = user;
        }

        public static gov_user getCurrentUser(this HttpSessionStateBase session)
        {
            return session["currentUser"] as gov_user;
        }
    }
}