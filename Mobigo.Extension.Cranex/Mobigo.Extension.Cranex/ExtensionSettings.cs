using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobigo.Extension.Cranex
{
    public static class ExtensionSettings
    {
        // Eventtype codes
        public const string EVT_AFTERNEW = "OnAfterNew";
        public const string EVT_AFTEREDIT = "OnAfterEdit";
        public const string EVT_DELETE = "OnDeleting"; //to catch these: OnDeleted,OnDeleting

        // Extrafield names, currently only quantity is used
        public const string XTRAFIELD_ARTICLE_STOREQUANTITY = "Lagersaldo";
        public const string XTRAFIELD_ARTICLE_LEVARTNR = "Leverantörens artikelnr";
        public const string XTRAFIELD_ARTICLE_LEVNAME = "Leverantörens namn";
        public const string XTRAFIELD_ARTICLE_PURCHASEPRICE = "Inköpspris";

        // Output msg
        public const string UI_MSG_MATERIALAMOUNTERR = "Antal eller faktureras antal får inte vara mindre än 0";
    }
}
