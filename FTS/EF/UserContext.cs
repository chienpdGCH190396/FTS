using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTS.Models;

namespace FTS.EF
{
    public class UserContext : IdentityDbContext<UserEntity>
    {
        public UserContext() : base("TrainingSystem")
        {

        }

    }
}