using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Utilities;
namespace Request.DomainRequests
{
    public class DomainCreate
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [JsonIgnore]
        public double Created 
        {
            get
            {
                return Timestamp.UtcNow();
            } 
        }
        /// <summary>
        /// Cờ xóa
        /// </summary>
        [JsonIgnore]
        public bool Deleted
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Cờ active
        /// </summary>
        [JsonIgnore]
        public bool Active
        {
            get
            {
                return true;
            }
        }
    }
}
