using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public class EvalTemplateSelector : DataTemplateSelector
    {
        public EvalTemplateSelector()
        {
            // Retain instances!
            this.lockerDataTemplate = new DataTemplate(typeof(LockerCell));
            this.wineDataTemplate = new DataTemplate(typeof(WineCellView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var evalVm = item as EvaluationsViewModel;
            if (evalVm == null)
                return null;

            return evalVm.IsWineCell ? this.lockerDataTemplate : this.wineDataTemplate;
        }

        private readonly DataTemplate lockerDataTemplate;
        private readonly DataTemplate wineDataTemplate;
    }
}
