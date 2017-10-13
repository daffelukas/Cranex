/* 2017-10 DL/ERISMA
Extension regulates the quantity of an
article based on the quantity of materials
used. */

using Mobigo.Core.Extend;
using Mobigo.Core;
using Mobigo.Core.Automation;
using Mobigo.Extension.Cranex.ItemMethods;
using Mobigo.Extension.Cranex;
using System.Windows.Forms;

namespace Mobigo.Extension.Cranex.EventManager
{
    // General eventmanager
    public class EventManager : EventListener
    {

        private IMaterialStore _materialStore;
        private ITaskStore _taskStore;
        private IProductStore _productStore;
        private IPlatform _platform;
        private MaterialMethods _materialMethods;
        private ExtensionExtras _functionLib;

        public EventManager(IMaterialStore materialStore, ITaskStore taskStore, IProductStore productStore, IPlatform platform, MaterialMethods materialMethods, ExtensionExtras functionLib)
        {
            _materialStore = materialStore;
            _taskStore = taskStore;
            _productStore = productStore;
            _platform = platform;
            _materialMethods = materialMethods;
            _functionLib = functionLib;
        }

        public override EventResult Material_OnAfterNew(IMaterial material)
        {
            bool callResult = true;

            if (!_materialMethods.CheckIfAllowedMaterial(material))
            {
                _functionLib.ShowMsg(ExtensionSettings.UI_MSG_MATERIALAMOUNTERR);
                return EventResult.Cancel;
            }
            else
            {
                callResult = _materialMethods.UpdateArticleQuantity(material);
            }
            return EventResult.Success;
        }

        public override EventResult Material_OnAfterEdit(IMaterial material)
        {
            bool callResult = true;

            if (!_materialMethods.CheckIfAllowedMaterial(material))
            {
                _functionLib.ShowMsg(ExtensionSettings.UI_MSG_MATERIALAMOUNTERR);
                return EventResult.Cancel;
            }
            else
            {
                callResult = _materialMethods.UpdateArticleQuantity(material);
            }
            return EventResult.Success;
        }
    }

    // ItemEventlistener for deep core operations
    [ItemEventListener(Name = "MaterialChange")]
    class MaterialAmountChange : ItemEventListener<IMaterial>
    {
        private IMaterialStore _materialStore;
        private ITaskStore _taskStore;
        private IProductStore _productStore;
        private MaterialMethods _materialMethods;

        public MaterialAmountChange(IMaterialStore materialStore, ITaskStore taskStore, IProductStore productStore, MaterialMethods materialMethods)
        {
            _materialStore = materialStore;
            _taskStore = taskStore;
            _productStore = productStore;
            _materialMethods = materialMethods;
        }

        public override void OnDeleting(IMaterial material)
        {
            Mobigo.Core.Diagnostics.Console.Log("On Deleting material");
            if (_materialMethods.CheckIfAllowedMaterial(material))
            {
                Mobigo.Core.Diagnostics.Console.Log("delete material allowed");
                _materialMethods.UpdateArticleQuantity(material);
            }
        }

    }

    [ItemEventListener(Name = "TaskChange")]
    class TaskChangeMaterialUpdate : ItemEventListener<ITask>
    {
        private IMaterialStore _materialStore;
        private ITaskStore _taskStore;
        private IProductStore _productStore;
        private MaterialMethods _materialMethods;

        public TaskChangeMaterialUpdate(IMaterialStore materialStore, ITaskStore taskStore, IProductStore productStore, MaterialMethods materialMethods)
        {
            _materialStore = materialStore;
            _taskStore = taskStore;
            _productStore = productStore;
            _materialMethods = materialMethods;
        }

        public override void OnDeleting(ITask task)
        {
            IMaterialList materialList = task.GetMaterials(_materialStore);
            Mobigo.Core.Diagnostics.Console.Log("On Deleting Task");
            foreach (IMaterial mat in materialList)
            {
              
                if (_materialMethods.CheckIfAllowedMaterial(mat))
                {
                    _materialMethods.UpdateArticleQuantity(mat);
                }
            }
            
           
        }
    }
}