﻿using System.Net;

namespace FooCommerce.Domain
{
    public interface IEntityRequestLog
    {
        IPAddress IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}