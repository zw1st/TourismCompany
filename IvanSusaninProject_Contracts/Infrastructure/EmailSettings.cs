using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Infrastructure;

public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public required string SmtpUsername { get; set; }
    public required string SmtpPassword { get; set; }
    public required string FromEmail { get; set; }
    public required string FromName { get; set; }
}
