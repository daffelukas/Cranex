using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Mobigo.Core.Extend;
using Mobigo.Core;
using System;

namespace Mobigo.Extension.Cranex.ItemMethods
{
    public class MaterialMethods
    {

        private MaterialMethods _materialMethods;
        private IMaterialStore _materialStore;
        private ITaskStore _taskStore;
        private IProductStore _productStore;

        public MaterialMethods(IMaterialStore materialStore, ITaskStore taskStore,IProductStore productStore)
        {
            _materialStore = materialStore;
            _taskStore = taskStore;
            _productStore = productStore;
        }

       public bool UpdateArticleQuantity(IMaterial item, [CallerMemberName] string callMethod = "")
        {
            ITask materialOwner = _taskStore.TryLoad(item.Owner);
            IProduct product = item.GetProduct(_productStore);
            var orgMaterial = _materialStore.TryLoad(item.Id);
            UserProperty xfieldVal;
      
            string eventType = "";

            /*Mobigo.Core.Diagnostics.Console.Log("CurrentMat", item, "OrgMat", orgMaterial, "MatOwn", materialOwner);
            Mobigo.Core.Diagnostics.Console.Log("ART", product, "callmethod", callMethod);*/

            if (callMethod.IndexOf(ExtensionSettings.EVT_AFTERNEW) >= 0) eventType = ExtensionSettings.EVT_AFTERNEW;
            else if (callMethod.IndexOf(ExtensionSettings.EVT_AFTEREDIT) >= 0) eventType = ExtensionSettings.EVT_AFTEREDIT;
            else if (callMethod.IndexOf(ExtensionSettings.EVT_DELETE, StringComparison.OrdinalIgnoreCase) >= 0) eventType = ExtensionSettings.EVT_DELETE;

            Mobigo.Core.Diagnostics.Console.Log("callmeth is del", callMethod.IndexOf(ExtensionSettings.EVT_DELETE));

            if (product != null) {

                xfieldVal = product.UserProperties.GetByName(ExtensionSettings.XTRAFIELD_ARTICLE_STOREQUANTITY);
                if (xfieldVal.Value == "") xfieldVal.Value = "0";

                switch (eventType)
                {
                    case ExtensionSettings.EVT_AFTERNEW:
                        Mobigo.Core.Diagnostics.Console.Log("afternew");
                        
                        xfieldVal.Value = (Convert.ToDouble(xfieldVal.Value)-item.Quantity.Amount).ToString();
                        _productStore.Save(product);
                        return true;
                    case ExtensionSettings.EVT_AFTEREDIT:
                        if (orgMaterial != null)
                        {
                            Mobigo.Core.Diagnostics.Console.Log("AfterEditOrgmaterial exists");

                            if (orgMaterial.Quantity.Amount != item.Quantity.Amount)
                            {
                                if (item.Quantity.Amount > orgMaterial.Quantity.Amount)
                                {
                                    xfieldVal.Value = ( Convert.ToDouble(xfieldVal.Value) - (item.Quantity.Amount-orgMaterial.Quantity.Amount)).ToString();
                                    //Mobigo.Core.Diagnostics.Console.Log("Amount increased");
                                }
                                else if (item.Quantity.Amount < orgMaterial.Quantity.Amount)
                                {
                                    xfieldVal.Value = (Convert.ToDouble(xfieldVal.Value) + (orgMaterial.Quantity.Amount - item.Quantity.Amount)).ToString();
                                    //Mobigo.Core.Diagnostics.Console.Log("Amount decreased");
                                }
                                _productStore.Save(product);
                            }
                        }
                        return true;
                    case ExtensionSettings.EVT_DELETE:
                        Mobigo.Core.Diagnostics.Console.Log("deleting, returning material");
                        xfieldVal.Value = (Convert.ToDouble(xfieldVal.Value) + item.Quantity.Amount).ToString();
                        _productStore.Save(product);
                        break;
                }
            }

            return true;

        }

        public bool CheckIfAllowedMaterial(IMaterial item)
        {
            ITask materialOwner = _taskStore.TryLoad(item.Owner);
            Mobigo.Core.Diagnostics.Console.Log("Ownercheck",materialOwner,"item",item);
            if (materialOwner == null) return false;
            if (materialOwner.Status == TaskStatus.Archived) return false;
            
            return (item.Quantity.Amount >= 0 && item.InvoiceQuantity.Amount >= 0);
        }
    }
}
