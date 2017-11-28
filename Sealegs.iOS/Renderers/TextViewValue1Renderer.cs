using System;

using UIKit;

using FormsToolkit.iOS;

using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

using Sealegs.Clients.UI;
using Sealegs.iOS;

[assembly:ExportRenderer(typeof(TextViewValue1), typeof(TextViewValue1Renderer))]
namespace Sealegs.iOS
{
    public class TextViewValue1Renderer : TextCellRenderer
    {
        public static void Init()
        {
            var test = DateTime.UtcNow;
        }

        public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
        {

            var tvc = reusableCell as CellTableViewCell;
            if (tvc == null) {
                tvc = new CellTableViewCell (UITableViewCellStyle.Value1, item.GetType().FullName);
            }
            tvc.Cell = item;
            var cell = base.GetCell(item, tvc, tv);
            cell.SetDisclosure(item.StyleId);
            return cell;
        }
    }
}

