using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nikan.DomainClasses.BaseEntity
{
    
    public class AuditSourceValues
    {
        
        public string  HostName { get; set; }

       
        public string  MachineName { get; set; }

        
        public string  RemoteIpAddress { get; set; }

       
        public string LocalIpAddress { get; set; }

        
        public string  UserAgent { get; set; }

        
        public string  ApplicationName { get; set; }

      
        public string  ApplicationVersion { get; set; }

         
        public string  ClientName { get; set; }

       
        public string  ClientVersion { get; set; }

        
        public string  Other { get; set; }
    }



    


    public enum EntityEventType
    {
        Create = 0,
        Update = 1,
        Delete = 2
    }











}
