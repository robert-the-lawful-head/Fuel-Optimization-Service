using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.Test
{
    public static class TestDataHelper
    {
        public static User CreateTestUser()
        {
            User user = new User();
            user.Oid = 1;
            user.Username = "test";
            user.Password = "test";
            user.GroupId = 1;
            user.FboId = 1;
            user.Active = true;
            user.Role = User.UserRoles.Member;

            return user;
        }
    }
}
    