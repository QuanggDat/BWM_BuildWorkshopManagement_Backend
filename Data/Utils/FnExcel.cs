using Aspose.Cells;
using System.Drawing;

namespace Data.Utils
{
    public static class FnExcel
    {
        public static Style ApplyDefaultStyle(Style style)
        {
            style.Font.IsBold = false;
            style.HorizontalAlignment = TextAlignmentType.Center;
            return style;
        }

        public static Style ApplyFloorStyle(Style style, bool isName = false)
        {
            style = ApplyDefaultStyle(style);
            if (isName)
            {
                style.HorizontalAlignment = TextAlignmentType.Center;
            }
            style.ForegroundColor = Color.FromArgb(250, 188, 140);
            style.Pattern = BackgroundType.Solid;
            style.Font.IsBold = true;
            return style;
        }

        public static Style ApplyAreaStyle(Style style, bool isName = false)
        {
            style = ApplyDefaultStyle(style);
            if (isName)
            {
                style.HorizontalAlignment = TextAlignmentType.Center;
            }
            style.ForegroundColor = Color.FromArgb(184, 220, 232);
            style.Pattern = BackgroundType.Solid;
            style.Font.IsBold = true;
            return style;
        }

        public static Style ApplyWrapTextStyle(Style style, bool isBold = false)
        {
            style = ApplyDefaultStyle(style);
            style.HorizontalAlignment = TextAlignmentType.Left;
            style.IsTextWrapped = true;
            style.Font.IsBold = isBold;
            return style;
        }

        public static void ApplyRangeCommonStyle(Aspose.Cells.Range range, CellsColor borderColor)
        {
            borderColor.Color = Color.Black;
            range.SetInsideBorders(BorderType.Vertical, CellBorderType.Thin, borderColor);
            range.SetInsideBorders(BorderType.Horizontal, CellBorderType.Thin, borderColor);
            range.SetOutlineBorders(CellBorderType.Thin, borderColor);
        }
    }
}
