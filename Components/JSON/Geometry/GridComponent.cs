using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Geometry
{
    // Generates a list of Position JSONs arranged in a grid.
    // Items fill left to right, then top to bottom.
    public class GridComponent : GH_Component
    {
        public GridComponent()
            : base("Grid", "Grid",
                "Generates a list of Position JSONs arranged in a grid. Items fill left to right, then top to bottom.",
                "MiroConnect", "Geometry")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Count", "N", "Total number of items in the grid.", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Items Per Row", "R", "Number of items per row.", GH_ParamAccess.item, 4);
            pManager.AddNumberParameter("Width", "W", "Width of each item in pixels.", GH_ParamAccess.item, 200.0);
            pManager.AddNumberParameter("Height", "H", "Height of each item in pixels.", GH_ParamAccess.item, 200.0);
            pManager.AddNumberParameter("Gap X", "GX", "Horizontal gap between items in pixels.", GH_ParamAccess.item, 20.0);
            pManager.AddNumberParameter("Gap Y", "GY", "Vertical gap between items in pixels.", GH_ParamAccess.item, 20.0);
            pManager.AddNumberParameter("Origin X", "X", "Center X of the first item (top-left of grid).", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Origin Y", "Y", "Center Y of the first item (top-left of grid).", GH_ParamAccess.item, 0.0);
            pManager[3].Optional = true;
            pManager[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Position JSON", "G", "List of Position JSONs, one per grid cell.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int count = 1, itemsPerRow = 4;
            double width = 200, height = 200, gapX = 20, gapY = 20, originX = 0, originY = 0;

            DA.GetData(0, ref count);
            DA.GetData(1, ref itemsPerRow);
            DA.GetData(2, ref width);
            DA.GetData(3, ref height);
            DA.GetData(4, ref gapX);
            DA.GetData(5, ref gapY);
            DA.GetData(6, ref originX);
            DA.GetData(7, ref originY);

            if (itemsPerRow < 1) itemsPerRow = 1;
            if (count < 1) count = 1;

            double stepX = width  + gapX;
            double stepY = height + gapY;

            var positions = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                int col = i % itemsPerRow;
                int row = i / itemsPerRow;

                double x = originX + col * stepX;
                double y = originY + row * stepY;

                positions.Add(SharedBuilders.BuildGeometry(x, y, width, height, 0));
            }

            DA.SetDataList(0, positions);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_Grid;
        public override Guid ComponentGuid => new Guid("e9f0a1b2-c3d4-4e5f-6a7b-8c9d0e1f2a3b");
    }
}
