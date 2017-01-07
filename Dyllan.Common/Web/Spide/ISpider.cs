using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyllan.Common.Web.Spide
{
    public interface ISpider
    {
        void Restart();
        void Run();
        void Stop();
        event EventHandler<int> OnProceed;
        event EventHandler OnCompleted;
    }
}
