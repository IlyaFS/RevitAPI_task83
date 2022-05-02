using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_task83
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            ViewPlan viewPlan = new FilteredElementCollector(doc)
                                .OfClass(typeof(ViewPlan))
                                .Cast<ViewPlan>()
                                .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan &&
                                                    v.Name.Equals("Level 1"));

            IList<ElementId> imageExportList = new List<ElementId>();
            imageExportList.Add(viewPlan.Id);

            var imageOption = new ImageExportOptions
            {
                ZoomType = ZoomFitType.Zoom,
                PixelSize = 1024,
                FilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{viewPlan.Name}",
                FitDirection = FitDirectionType.Horizontal,
                HLRandWFViewsFileType = ImageFileType.JPEGLossless,
                ImageResolution = ImageResolution.DPI_150,
                ExportRange = ExportRange.SetOfViews
            };

            imageOption.SetViewsAndSheets(imageExportList);
            doc.ExportImage(imageOption);

            return Result.Succeeded;
        }
    }
}
