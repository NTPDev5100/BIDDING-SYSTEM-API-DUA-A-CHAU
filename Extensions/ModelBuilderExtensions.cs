using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Utilities;
using static Utilities.CatalogueEnums;
using Entities.Configuration;

namespace Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Users>().HasData(
                    new tbl_Users()
                    {
                        Id = new Guid("0DEBFF1D-AC80-4E2D-BE24-3151B26F2176"),
                        Username = "admin",
                        FullName = "ACP",
                        Phone = "123 456 7890",
                        Email = "admin@acp.com",
                        Address = "Thành phố Hồ Chí Minh",
                        Status = ((int)userStatus.active),
                        Birthday = 0,
                        Password = SecurityUtilities.HashSHA1("mona@123"),
                        Gender = 0,
                        Created = Timestamp.UtcNow(),
                        IsAdmin = true,
                        CreatedBy = Guid.Empty,
                        Deleted = false,
                        Active = true
                    }
                );

        }
    }
}
