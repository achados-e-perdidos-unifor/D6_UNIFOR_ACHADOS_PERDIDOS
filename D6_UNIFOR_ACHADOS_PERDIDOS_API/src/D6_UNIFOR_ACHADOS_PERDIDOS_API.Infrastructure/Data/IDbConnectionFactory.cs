using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
