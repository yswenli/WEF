using System.Drawing;
using System.Windows.Forms.Design;

namespace WEF.Standard.DevelopTools.Capture
{
    public class ToolButtonDesigner : ControlDesigner
    {
        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe) {
            Pen p = new Pen(Color.SteelBlue, 1);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            pe.Graphics.DrawRectangle(p, 0, 0, pe.ClipRectangle.Width - 1, 20);
            p.Dispose();
            base.OnPaintAdornments(pe);
        }

        public override SelectionRules SelectionRules {
            get {
                return base.SelectionRules & ~SelectionRules.AllSizeable;
            }
        }
    }
}
