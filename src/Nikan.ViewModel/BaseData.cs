using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{
    public class BaseDataModel 
    {




        public string Key { get; set; }
        public string Text { get; set; }

        public bool Selected { get; set; }
        public bool Disabled { get; set; }


        public string ParentText { get; set; }

        public string ParentValue { get; set; }

        public string Description { get; set; }


        public string OrganizationId { get; set; }

    }
    public class EnumData
    {
        public string category { get; set; }
        public IEnumerable<BaseDataModel> enumList { get; set; }

    }


    public class formulaDto
    {
        public string formula { get; set; }
       

    }
    public class ConfigModel
    {
        public ConfigModel(bool success, string msg)
        {
            IsSuccess = success;
            Message = msg;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }



    }


}
