using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib;
using System.Drawing.Imaging;

namespace Project.Core.Helpers
{
    public class BarcodeHelper
    {


        public static  byte[] GetBarcodeAsByte(string sku)
        {
            var barcode = new Barcode
            {
                IncludeLabel = true
            };

            var img = barcode.Encode(TYPE.CODE128, sku, System.Drawing.Color.Black,
                System.Drawing.Color.White, 320, 75);

            var memoryStream = new MemoryStream();

            img.Save(memoryStream, ImageFormat.Png);

            var imgBytes = memoryStream.ToArray();
            img.Dispose();

            return imgBytes;

        }

        public static string GetBarcodeAsBase64String(string sku)
        {
            var barcode = new Barcode
            {
                IncludeLabel = true,
                AlternateLabel = sku,
                EncodedType = TYPE.CODE128,
                ImageFormat = ImageFormat.Png,
                LabelPosition = LabelPositions.BOTTOMCENTER
            };

            var img = barcode.Encode(TYPE.CODE128, sku, System.Drawing.Color.Black,
                System.Drawing.Color.White, 320, 75);

            var memoryStream = new MemoryStream();

            img.Save(memoryStream, ImageFormat.Png);

            var imgBytes = memoryStream.ToArray();

            var barcodePath = Convert.ToBase64String(imgBytes);

            img.Dispose();

            return barcodePath;
        }

    }
}
